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
    public class GestorResumenClinico
    {
        private const string TABLA = "ResumenClinico";

        private readonly IClienteIA clienteIA;
        public GestorResumenClinico() : this(new ClienteOpenRouter())
        {
        }

        public GestorResumenClinico(IClienteIA clienteIA)
        {
            this.clienteIA = clienteIA;
        }

        #region CUN05 - Generar Resumen Clínico Asistido por IA
        public int Generar(int idPsicologo, int idPaciente, DateTime rangoDesde, DateTime rangoHasta, List<int> idsConsultasIncluir = null)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null || paciente.IdPsicologo != idPsicologo)
            {
                throw new ExcepcionTraducible("error_paciente_no_propio");
            }
            GestorConsulta gestorConsulta = new GestorConsulta();
            List<Consulta> consultas = gestorConsulta.ObtenerPorPaciente(idPaciente).Where(c => c.FechaConsulta.Date >= rangoDesde.Date && c.FechaConsulta.Date <= rangoHasta.Date).ToList();
            if (idsConsultasIncluir != null)
            {
                consultas = consultas.Where(c => idsConsultasIncluir.Contains(c.IdConsulta)).ToList();
            }
            if (consultas.Count == 0)
            {
                throw new ExcepcionTraducible("error_resumen_sin_consultas");
            }
            string informacionClinica = ArmarInformacionClinica(paciente, consultas, rangoDesde, rangoHasta);
            SeccionesResumenClinico secciones;
            try
            {
                secciones = clienteIA.GenerarResumenClinico(informacionClinica);
            }
            catch (ExcepcionTraducible)
            {
                throw;
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_ia_comunicacion");
            }

            JavaScriptSerializer serializador = new JavaScriptSerializer();
            string contenidoJson = serializador.Serialize(secciones);
            string contenidoEncriptado;
            try
            {
                contenidoEncriptado = Cifrador.GestorCifrador.EncriptarReversible(contenidoJson);
            }
            catch (Exception)
            {
                throw new ExcepcionTraducible("error_resumen_encriptacion");
            }

            ResumenClinico resumen = new ResumenClinico();
            resumen.IdPaciente = idPaciente;
            resumen.IdProfesional = idPsicologo;
            resumen.Contenido = contenidoEncriptado;
            resumen.RangoDesde = rangoDesde.Date;
            resumen.RangoHasta = rangoHasta.Date;
            resumen.FechaGeneracion = DateTime.Now;
            ResumenClinicoDAL resumenDAL = new ResumenClinicoDAL();
            int idGenerado = resumenDAL.Alta(resumen);
            DigitoVerificador digitoVerificador = new DigitoVerificador();
            digitoVerificador.ActualizarDVH(resumen, TABLA);

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_MODULO_IA, EventosBitacora.DESC_RESUMEN_IA, EventosBitacora.CRIT_RESUMEN_IA);

            return idGenerado;
        }

        #endregion

        #region Busquedas ResumenClinico
        public ResumenClinico BuscarPorId(int idResumen)
        {
            ResumenClinicoDAL resumenDAL = new ResumenClinicoDAL();
            ResumenClinico resumen = resumenDAL.BuscarPorId(idResumen);
            if (resumen == null) return null;

            resumen.Contenido = Cifrador.GestorCifrador.DesencriptarReversible(resumen.Contenido);
            return resumen;
        }

        public List<ResumenClinico> ObtenerPorPaciente(int idPaciente)
        {
            ResumenClinicoDAL resumenDAL = new ResumenClinicoDAL();
            List<ResumenClinico> lista = resumenDAL.ObtenerPorPaciente(idPaciente);

            foreach (ResumenClinico resumen in lista)
            {
                resumen.Contenido = Cifrador.GestorCifrador.DesencriptarReversible(resumen.Contenido);
            }

            return lista;
        }
        public SeccionesResumenClinico ObtenerSecciones(ResumenClinico resumen)
        {
            if (resumen == null || string.IsNullOrWhiteSpace(resumen.Contenido)) return null;

            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Deserialize<SeccionesResumenClinico>(resumen.Contenido);
        }
        public List<ResumenClinico> ObtenerPorPsicologo(int idPsicologo)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            List<ResumenClinico> todos = new List<ResumenClinico>();

            foreach (Paciente paciente in gestorPaciente.ObtenerPorPsicologo(idPsicologo, soloActivos: false))
            {
                todos.AddRange(ObtenerPorPaciente(paciente.IdPaciente));
            }

            return todos;
        }

        #endregion

        #region Armado del prompt

        private string ArmarInformacionClinica(Paciente paciente, List<Consulta> consultas, DateTime rangoDesde, DateTime rangoHasta)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Información clínica del paciente para sintetizar.");
            sb.AppendLine("Rango analizado: " + rangoDesde.ToString("dd/MM/yyyy") + " al " + rangoHasta.ToString("dd/MM/yyyy") + ".");
            sb.AppendLine("Cantidad de consultas incluidas: " + consultas.Count + ".");
            sb.AppendLine();

            foreach (Consulta consulta in consultas.OrderBy(c => c.FechaConsulta))
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