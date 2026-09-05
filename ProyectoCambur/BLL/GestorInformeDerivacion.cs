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
    public class GestorInformeDerivacion
    {
        private const string TABLA = "InformeDerivacion";

        private readonly IClienteIA clienteIA;
        public GestorInformeDerivacion() : this(new ClienteOpenRouter())
        {
        }

        public GestorInformeDerivacion(IClienteIA clienteIA)
        {
            this.clienteIA = clienteIA;
        }

        #region CUN08 - Generar Informe de Derivación Clínico

        public int Generar(int idPsicologo, int idPaciente, string especialidad, string profesionalDestino, string institucion, string motivo)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null || paciente.IdPsicologo != idPsicologo)
            {
                throw new ExcepcionTraducible("error_paciente_no_propio");
            }

            if (string.IsNullOrWhiteSpace(especialidad) || string.IsNullOrWhiteSpace(profesionalDestino) || string.IsNullOrWhiteSpace(motivo))
            {
                throw new ExcepcionTraducible("error_informe_campos_incompletos");
            }
            GestorHistorialClinico gestorHistorial = new GestorHistorialClinico();
            HistorialClinico historial = gestorHistorial.BuscarPorPaciente(idPaciente);

            GestorConsulta gestorConsulta = new GestorConsulta();
            List<Consulta> consultas = gestorConsulta.ObtenerPorPaciente(idPaciente).OrderBy(c => c.FechaConsulta).ToList();

            if (historial == null && consultas.Count == 0)
            {
                throw new ExcepcionTraducible("error_informe_sin_informacion_clinica");
            }

            string informacionClinica = ArmarInformacionClinica(paciente, historial, consultas);

            SeccionesInformeDerivacion secciones;
            try
            {
                secciones = clienteIA.GenerarInformeDerivacion(informacionClinica);
            }
            catch (ExcepcionTraducible)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_ia_comunicacion");
            }
            secciones.EspecialidadDerivacion = especialidad;
            secciones.ProfesionalDestinatario = profesionalDestino;
            secciones.Institucion = institucion;
            secciones.MotivoDerivacion = motivo;
            secciones.Firma = null;

            string contenidoEncriptado = EncriptarSecciones(secciones);

            InformeDerivacion informe = new InformeDerivacion();
            informe.IdPaciente = idPaciente;
            informe.IdProfesional = idPsicologo;
            informe.Contenido = contenidoEncriptado;
            informe.Estado = EstadoInforme.Borrador;
            informe.FechaGeneracion = DateTime.Now;
            informe.FechaAuditoria = null;

            InformeDerivacionDAL informeDAL = new InformeDerivacionDAL();
            int idGenerado = informeDAL.Alta(informe);

            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(informe, TABLA);

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_MODULO_IA, EventosBitacora.DESC_INFORME_DERIVACION, EventosBitacora.CRIT_INFORME_DERIVACION);

            return idGenerado;
        }

        #endregion

        #region CUN09 - Auditoría de Informe de Derivación
        public void Auditar(int idPsicologo, int idInforme, string sintesisDiagnostica, string andamiajes, string objetivos, string modalidadTrabajo, string motivoDerivacion, string firma)
        {
            InformeDerivacion informe = ObtenerPropioOFallar(idPsicologo, idInforme);

            if (string.IsNullOrWhiteSpace(sintesisDiagnostica) && string.IsNullOrWhiteSpace(andamiajes))
            {
                throw new ExcepcionTraducible("error_informe_campos_incompletos");
            }

            if (string.IsNullOrWhiteSpace(firma))
            {
                throw new ExcepcionTraducible("error_informe_firma_obligatoria");
            }

            SeccionesInformeDerivacion secciones = DesencriptarSecciones(informe.Contenido);
            secciones.SintesisDiagnostica = sintesisDiagnostica;
            secciones.Andamiajes = andamiajes;
            secciones.Objetivos = objetivos;
            secciones.ModalidadTrabajo = modalidadTrabajo;
            secciones.MotivoDerivacion = motivoDerivacion;
            secciones.Firma = firma.Trim();

            informe.Contenido = EncriptarSecciones(secciones);
            informe.Estado = EstadoInforme.Auditado;
            informe.FechaAuditoria = DateTime.Now;

            InformeDerivacionDAL informeDAL = new InformeDerivacionDAL();
            informeDAL.ActualizarContenidoYEstado(informe.IdInforme, informe.Contenido, informe.Estado, informe.FechaAuditoria);

            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(informe, TABLA);

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_MODULO_IA, EventosBitacora.DESC_AUDITORIA_INFORME, EventosBitacora.CRIT_AUDITORIA_INFORME);
        }
        public void GuardarBorrador(int idPsicologo, int idInforme, string sintesisDiagnostica, string andamiajes, string objetivos, string modalidadTrabajo, string motivoDerivacion)
        {
            InformeDerivacion informe = ObtenerPropioOFallar(idPsicologo, idInforme);

            SeccionesInformeDerivacion secciones = DesencriptarSecciones(informe.Contenido);
            secciones.SintesisDiagnostica = sintesisDiagnostica;
            secciones.Andamiajes = andamiajes;
            secciones.Objetivos = objetivos;
            secciones.ModalidadTrabajo = modalidadTrabajo;
            secciones.MotivoDerivacion = motivoDerivacion;

            informe.Contenido = EncriptarSecciones(secciones);

            InformeDerivacionDAL informeDAL = new InformeDerivacionDAL();
            informeDAL.ActualizarContenidoYEstado(informe.IdInforme, informe.Contenido, EstadoInforme.Borrador, null);

            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(informe, TABLA);
        }
        public void Descartar(int idPsicologo, int idInforme)
        {
            InformeDerivacion informe = ObtenerPropioOFallar(idPsicologo, idInforme);

            InformeDerivacionDAL informeDAL = new InformeDerivacionDAL();
            informeDAL.Baja(informe.IdInforme);

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_MODULO_IA, EventosBitacora.DESC_DESCARTE_INFORME, EventosBitacora.CRIT_DESCARTE_INFORME);
        }

        #endregion

        #region Busquedas InformeDerivacion

        public InformeDerivacion BuscarPorId(int idInforme)
        {
            InformeDerivacionDAL informeDAL = new InformeDerivacionDAL();
            InformeDerivacion informe = informeDAL.BuscarPorId(idInforme);
            if (informe == null) return null;

            informe.Contenido = Cifrador.GestorCifrador.DesencriptarReversible(informe.Contenido);
            return informe;
        }

        public List<InformeDerivacion> ObtenerPorPaciente(int idPaciente)
        {
            InformeDerivacionDAL informeDAL = new InformeDerivacionDAL();
            List<InformeDerivacion> lista = informeDAL.ObtenerPorPaciente(idPaciente);

            foreach (InformeDerivacion informe in lista)
            {
                informe.Contenido = Cifrador.GestorCifrador.DesencriptarReversible(informe.Contenido);
            }

            return lista;
        }

        public List<InformeDerivacion> ObtenerPorPsicologo(int idPsicologo)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            List<InformeDerivacion> todos = new List<InformeDerivacion>();

            foreach (Paciente paciente in gestorPaciente.ObtenerPorPsicologo(idPsicologo, soloActivos: false))
            {
                todos.AddRange(ObtenerPorPaciente(paciente.IdPaciente));
            }

            return todos;
        }

        public SeccionesInformeDerivacion ObtenerSecciones(InformeDerivacion informe)
        {
            if (informe == null || string.IsNullOrWhiteSpace(informe.Contenido)) return null;

            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Deserialize<SeccionesInformeDerivacion>(informe.Contenido);
        }

        #endregion

        #region Privados

        private InformeDerivacion ObtenerPropioOFallar(int idPsicologo, int idInforme)
        {
            InformeDerivacion informe = BuscarPorId(idInforme);
            if (informe == null || informe.IdProfesional != idPsicologo)
            {
                throw new ExcepcionTraducible("error_informe_no_propio");
            }

            return informe;
        }

        private string EncriptarSecciones(SeccionesInformeDerivacion secciones)
        {
            JavaScriptSerializer serializador = new JavaScriptSerializer();
            string contenidoJson = serializador.Serialize(secciones);
            try
            {
                return Cifrador.GestorCifrador.EncriptarReversible(contenidoJson);
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_informe_encriptacion");
            }
        }

        private SeccionesInformeDerivacion DesencriptarSecciones(string contenidoEncriptado)
        {
            string contenidoJson = Cifrador.GestorCifrador.DesencriptarReversible(contenidoEncriptado);
            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Deserialize<SeccionesInformeDerivacion>(contenidoJson) ?? new SeccionesInformeDerivacion();
        }

        private string ArmarInformacionClinica(Paciente paciente, HistorialClinico historial, List<Consulta> consultas)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Información clínica del paciente para redactar un informe de derivación.");
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