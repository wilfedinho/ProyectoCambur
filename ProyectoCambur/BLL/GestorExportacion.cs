using BE;
using SERVICIOS;
using System.Collections.Generic;
using System.Linq;

namespace BLL
{

    public class GestorExportacion
    {
        public const string TIPO_RESUMEN = "RESUMEN";
        public const string TIPO_DERIVACION = "DERIVACION";
        public const string TIPO_PERFIL = "PERFIL";

        static GestorExportacion()
        {
            ResolvedorFuentesPdf.Registrar();
        }
        private IExportadorDocumento ObtenerExportador(string tipoDocumento)
        {
            switch (tipoDocumento)
            {
                case TIPO_RESUMEN:
                    return new ExportadorResumenClinico();
                case TIPO_DERIVACION:
                    return new ExportadorInformeDerivacion();
                case TIPO_PERFIL:
                    return new ExportadorPerfilPaciente();
                default:
                    return null;
            }
        }

        public byte[] Generar(int idPsicologo, int idPaciente, string tipoDocumento, int? idDocumento, out string nombreArchivoSugerido)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null || paciente.IdPsicologo != idPsicologo)
            {
                throw new ExcepcionTraducible("error_paciente_no_propio");
            }

            IExportadorDocumento exportador = ObtenerExportador(tipoDocumento);
            if (exportador == null)
            {
                throw new ExcepcionTraducible("error_documento_no_disponible");
            }

            string tituloDocumento;
            byte[] pdf = exportador.GenerarPdf(idPsicologo, paciente, idDocumento, out tituloDocumento);

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_REPORTES, EventosBitacora.DESC_EXPORTAR_PDF, EventosBitacora.CRIT_EXPORTAR_PDF);

            nombreArchivoSugerido = ArmarNombreArchivo(paciente, tituloDocumento);
            return pdf;
        }

        public bool DocumentoDisponible(int idPsicologo, int idPaciente, string tipoDocumento)
        {
            IExportadorDocumento exportador = ObtenerExportador(tipoDocumento);
            return exportador != null && exportador.EstaDisponible(idPaciente);
        }

        public bool DocumentoPendienteAuditoria(int idPaciente)
        {
            GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
            return gestorInforme.ObtenerPorPaciente(idPaciente).Any(i => i.Estado == EstadoInforme.Borrador);
        }

        public List<DocumentoExportable> ObtenerDocumentosDisponibles(int idPsicologo, int idPaciente, string tipoDocumento)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null || paciente.IdPsicologo != idPsicologo)
            {
                throw new ExcepcionTraducible("error_paciente_no_propio");
            }

            IExportadorDocumento exportador = ObtenerExportador(tipoDocumento);
            return exportador != null ? exportador.ObtenerDisponibles(idPaciente) : new List<DocumentoExportable>();
        }

        public List<Bitacora> ObtenerExportacionesRecientes(int idPsicologo, int cantidad = 6)
        {
            if (!GestorSesion.EstaAutenticado) return new List<Bitacora>();

            GestorBitacora gestorBitacora = new GestorBitacora();
            string email = GestorSesion.PsicologoActual.Email;
            List<Bitacora> eventos = gestorBitacora.ObtenerPorFiltros(null, null, EventosBitacora.MOD_REPORTES, email, null);

            return eventos
                .Where(e => e.Descripcion == EventosBitacora.DESC_EXPORTAR_PDF)
                .OrderByDescending(e => e.FechaEvento)
                .Take(cantidad)
                .ToList();
        }

        private string ArmarNombreArchivo(Paciente paciente, string tituloDocumento)
        {
            string baseNombre = tituloDocumento.Replace(" ", "_") + "_" + paciente.Apellido + "_" + paciente.Nombre;
            baseNombre = string.Join("", baseNombre.Split(System.IO.Path.GetInvalidFileNameChars()));
            return baseNombre + "_" + System.DateTime.Today.ToString("yyyyMMdd") + ".pdf";
        }
    }
}