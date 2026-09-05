using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace BLL
{
    public class GestorPerfilPaciente
    {
        private const string TABLA = "PerfilPaciente";
        private static readonly Dictionary<string, int> CODIGOS_MODELO = new Dictionary<string, int>
        {
            { "BIGFIVE", 1 },
            { "COPE", 2 },
            { "AUTOEFICACIA", 3 },
            { "APEGO", 4 },
            { "VALORES", 5 }
        };

        private readonly IClienteIA clienteIA;
        public GestorPerfilPaciente() : this(new ClienteOpenRouter())
        {
        }

        public GestorPerfilPaciente(IClienteIA clienteIA)
        {
            this.clienteIA = clienteIA;
        }

        #region CUN10 - Perfilación del Paciente

        public int Generar(int idPsicologo, int idPaciente, string codigoModelo)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null || paciente.IdPsicologo != idPsicologo)
            {
                throw new ExcepcionTraducible("error_paciente_no_propio");
            }

            if (string.IsNullOrWhiteSpace(codigoModelo) || !CODIGOS_MODELO.ContainsKey(codigoModelo))
            {
                throw new ExcepcionTraducible("error_perfil_modelo_invalido");
            }

            int idModelo = CODIGOS_MODELO[codigoModelo];
            ModeloEvaluacionDAL modeloDAL = new ModeloEvaluacionDAL();
            ModeloEvaluacion modelo = modeloDAL.BuscarPorId(idModelo);
            if (modelo == null)
            {
                throw new ExcepcionTraducible("error_perfil_modelo_invalido");
            }
            GestorHistorialClinico gestorHistorial = new GestorHistorialClinico();
            HistorialClinico historial = gestorHistorial.BuscarPorPaciente(idPaciente);

            GestorConsulta gestorConsulta = new GestorConsulta();
            List<Consulta> consultas = gestorConsulta.ObtenerPorPaciente(idPaciente).OrderBy(c => c.FechaConsulta).ToList();

            if (historial == null && consultas.Count == 0)
            {
                throw new ExcepcionTraducible("error_perfil_sin_informacion_clinica");
            }

            string informacionClinica = ArmarInformacionClinica(modelo, historial, consultas);

            SeccionesPerfilPaciente secciones;
            try
            {
                secciones = clienteIA.GenerarPerfilPaciente(informacionClinica);
            }
            catch (ExcepcionTraducible)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_ia_comunicacion");
            }

            secciones.NombreModelo = modelo.Nombre;

            JavaScriptSerializer serializador = new JavaScriptSerializer();
            string contenidoJson = serializador.Serialize(secciones);
            string contenidoEncriptado;
            try
            {
                contenidoEncriptado = Cifrador.GestorCifrador.EncriptarReversible(contenidoJson);
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_perfil_encriptacion");
            }

            PerfilPaciente perfil = new PerfilPaciente();
            perfil.IdPaciente = idPaciente;
            perfil.IdProfesional = idPsicologo;
            perfil.IdModelo = idModelo;
            perfil.Resultado = contenidoEncriptado;
            perfil.FechaGeneracion = DateTime.Now;

            PerfilPacienteDAL perfilDAL = new PerfilPacienteDAL();
            int idGenerado = perfilDAL.Alta(perfil);

            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(perfil, TABLA);

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_MODULO_IA, EventosBitacora.DESC_PERFIL_EVOLUTIVO, EventosBitacora.CRIT_PERFIL_EVOLUTIVO);

            return idGenerado;
        }

        #endregion

        #region Busquedas PerfilPaciente

        public PerfilPaciente BuscarPorId(int idPerfil)
        {
            PerfilPacienteDAL perfilDAL = new PerfilPacienteDAL();
            PerfilPaciente perfil = perfilDAL.BuscarPorId(idPerfil);
            if (perfil == null) return null;

            perfil.Resultado = Cifrador.GestorCifrador.DesencriptarReversible(perfil.Resultado);
            return perfil;
        }

        public List<PerfilPaciente> ObtenerPorPaciente(int idPaciente)
        {
            PerfilPacienteDAL perfilDAL = new PerfilPacienteDAL();
            List<PerfilPaciente> lista = perfilDAL.ObtenerPorPaciente(idPaciente);

            foreach (PerfilPaciente perfil in lista)
            {
                perfil.Resultado = Cifrador.GestorCifrador.DesencriptarReversible(perfil.Resultado);
            }

            return lista;
        }

        public List<PerfilPaciente> ObtenerPorPsicologo(int idPsicologo)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            List<PerfilPaciente> todos = new List<PerfilPaciente>();

            foreach (Paciente paciente in gestorPaciente.ObtenerPorPsicologo(idPsicologo, soloActivos: false))
            {
                todos.AddRange(ObtenerPorPaciente(paciente.IdPaciente));
            }

            return todos;
        }

        public SeccionesPerfilPaciente ObtenerSecciones(PerfilPaciente perfil)
        {
            if (perfil == null || string.IsNullOrWhiteSpace(perfil.Resultado)) return null;

            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Deserialize<SeccionesPerfilPaciente>(perfil.Resultado);
        }

        public List<ModeloEvaluacion> ObtenerModelosDisponibles()
        {
            ModeloEvaluacionDAL modeloDAL = new ModeloEvaluacionDAL();
            return modeloDAL.ObtenerTodos();
        }

        #endregion

        #region Armado del prompt

        private string ArmarInformacionClinica(ModeloEvaluacion modelo, HistorialClinico historial, List<Consulta> consultas)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Modelo de evaluación solicitado: " + modelo.Nombre + ".");
            if (!string.IsNullOrWhiteSpace(modelo.Descripcion))
            {
                sb.AppendLine("Descripción del modelo: " + modelo.Descripcion);
            }
            sb.AppendLine();
            sb.AppendLine("Información clínica del paciente para elaborar el perfil según ese modelo.");
            sb.AppendLine("Cantidad de consultas registradas: " + consultas.Count + ".");
            sb.AppendLine();

            if (historial != null)
            {
                sb.AppendLine("--- Historial clínico ---");
                AgregarCampo(sb, "Hábitos nocivos", historial.HabitosNocivos);
                AgregarCampo(sb, "Contexto familiar", historial.ContextoFamiliar);
                AgregarCampo(sb, "Antecedentes familiares", historial.AntecedentesFamiliares);
                AgregarCampo(sb, "Antecedentes médicos", historial.AntecedentesMedicos);
                AgregarCampo(sb, "Situación laboral", historial.SituacionLaboral);
                AgregarCampo(sb, "Eventos traumáticos", historial.EventosTraumaticos);
                sb.AppendLine();
            }

            foreach (Consulta consulta in consultas)
            {
                sb.AppendLine("--- Consulta del " + consulta.FechaConsulta.ToString("dd/MM/yyyy") + " (" + consulta.TiempoConsulta + " min) ---");
                AgregarCampo(sb, "Objetivos", consulta.Objetivos);
                AgregarCampo(sb, "Observaciones", consulta.Observaciones);
                AgregarCampo(sb, "Hipótesis", consulta.Hipotesis);
                AgregarCampo(sb, "Intervenciones", consulta.Intervenciones);
                AgregarCampo(sb, "Evolución observada", consulta.EvolucionObservada);
                AgregarCampo(sb, "Diagnóstico", consulta.Diagnostico);
                AgregarCampo(sb, "Tratamiento", consulta.Tratamiento);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        private void AgregarCampo(StringBuilder sb, string etiqueta, string valor)
        {
            if (!string.IsNullOrWhiteSpace(valor))
            {
                sb.AppendLine(etiqueta + ": " + valor);
            }
        }

        #endregion
    }
}