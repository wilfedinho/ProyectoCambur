using BE;
using SERVICIOS;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace BLL
{
    public class ExportadorInformeDerivacion : ExportadorDocumentoBase, IExportadorDocumento
    {
        public bool EstaDisponible(int idPaciente)
        {
            GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
            return gestorInforme.ObtenerPorPaciente(idPaciente).Any(i => i.Estado == EstadoInforme.Auditado);
        }

        public List<DocumentoExportable> ObtenerDisponibles(int idPaciente)
        {
            List<DocumentoExportable> documentos = new List<DocumentoExportable>();

            GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
            foreach (InformeDerivacion i in gestorInforme.ObtenerPorPaciente(idPaciente)
                .Where(i => i.Estado == EstadoInforme.Auditado)
                .OrderByDescending(i => i.FechaAuditoria ?? i.FechaGeneracion))
            {
                SeccionesInformeDerivacion secciones = gestorInforme.ObtenerSecciones(i);
                documentos.Add(new DocumentoExportable
                {
                    IdDocumento = i.IdInforme,
                    Fecha = i.FechaAuditoria ?? i.FechaGeneracion,
                    Detalle = secciones != null && !string.IsNullOrWhiteSpace(secciones.EspecialidadDerivacion)
                        ? "Especialidad: " + secciones.EspecialidadDerivacion
                        : ""
                });
            }

            return documentos;
        }

        public byte[] GenerarPdf(int idPsicologo, Paciente paciente, int? idDocumento, out string tituloDocumento)
        {
            tituloDocumento = "Informe de Derivación";

            GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
            List<InformeDerivacion> informes = gestorInforme.ObtenerPorPaciente(paciente.IdPaciente)
                .Where(i => i.Estado == EstadoInforme.Auditado)
                .OrderByDescending(i => i.FechaAuditoria ?? i.FechaGeneracion)
                .ToList();

            InformeDerivacion informe = idDocumento.HasValue
                ? informes.FirstOrDefault(i => i.IdInforme == idDocumento.Value)
                : informes.FirstOrDefault();

            if (informe == null)
            {
                throw new ExcepcionTraducible("error_documento_no_disponible");
            }

            SeccionesInformeDerivacion secciones = gestorInforme.ObtenerSecciones(informe);

            string nombrePsicologo = ObtenerNombrePsicologoActual();

            using (PdfDocument documento = new PdfDocument())
            {
                documento.Info.Title = tituloDocumento + " - " + paciente.Nombre + " " + paciente.Apellido;
                documento.Info.Author = nombrePsicologo;

                PdfPage pagina = documento.AddPage();
                pagina.Size = PdfSharp.PageSize.A4;
                XGraphics gfx = XGraphics.FromPdfPage(pagina);

                XFont fuenteTitulo = new XFont("Verdana", 18, XFontStyleEx.Bold);
                XFont fuenteSubtitulo = new XFont("Verdana", 10, XFontStyleEx.Regular);
                XFont fuenteSeccion = new XFont("Verdana", 12, XFontStyleEx.Bold);
                XFont fuenteTexto = new XFont("Verdana", 10, XFontStyleEx.Regular);
                XFont fuenteAviso = new XFont("Verdana", 8, XFontStyleEx.Italic);

                double margen = 40;
                double y = margen;
                double anchoPagina = pagina.Width;
                double altoPagina = pagina.Height;
                double anchoUtil = anchoPagina - (margen * 2);

                gfx.DrawString("CAMBUR", fuenteTitulo, XBrushes.DarkSlateBlue, Rect(margen, y, anchoUtil, 30), XStringFormats.TopLeft);
                gfx.DrawString((informe.FechaAuditoria ?? informe.FechaGeneracion).ToString("dd/MM/yyyy"), fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 30), XStringFormats.TopRight);
                y += 32;
                gfx.DrawString(nombrePsicologo, fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 22;
                gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                y += 16;

                gfx.DrawString(tituloDocumento, fuenteSeccion, XBrushes.Black, Rect(margen, y, anchoUtil, 20), XStringFormats.TopLeft);
                y += 24;
                gfx.DrawString(paciente.Nombre + " " + paciente.Apellido, fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 16;

                if (secciones != null)
                {
                    string metaLinea = "DNI: " + paciente.DNI;
                    if (!string.IsNullOrWhiteSpace(secciones.EspecialidadDerivacion))
                        metaLinea += "   ·   Especialidad destino: " + secciones.EspecialidadDerivacion;
                    if (!string.IsNullOrWhiteSpace(secciones.ProfesionalDestinatario))
                        metaLinea += "   ·   Destinatario: " + secciones.ProfesionalDestinatario;
                    gfx.DrawString(metaLinea, fuenteTexto, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                    y += 24;
                    gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                    y += 16;

                    if (!string.IsNullOrWhiteSpace(secciones.Institucion))
                    {
                        y = EscribirSeccion(ref gfx, documento, ref pagina, "Institución", secciones.Institucion, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    }
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Motivo de Derivación", secciones.MotivoDerivacion, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Síntesis Diagnóstica", secciones.SintesisDiagnostica, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Andamiajes Implementados", secciones.Andamiajes, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Objetivos Terapéuticos", secciones.Objetivos, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Modalidad de Trabajo", secciones.ModalidadTrabajo, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);

                    if (!string.IsNullOrWhiteSpace(secciones.Firma))
                    {
                        y += 8;
                        y = EscribirSeccion(ref gfx, documento, ref pagina, "Firma Profesional", secciones.Firma, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    }
                }
                else
                {
                    gfx.DrawString("No se pudo recuperar el contenido del informe.", fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                    y += 20;
                }

                altoPagina = pagina.Height;
                anchoPagina = pagina.Width;
                if (y > altoPagina - 60)
                {
                    pagina = documento.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    altoPagina = pagina.Height;
                    anchoPagina = pagina.Width;
                }

                gfx.DrawLine(XPens.LightGray, margen, altoPagina - 50, anchoPagina - margen, altoPagina - 50);
                gfx.DrawString("Documento generado por Cambur · Contenido clínico confidencial, uso exclusivo del profesional autenticado.", fuenteAviso, XBrushes.Gray, Rect(margen, altoPagina - 42, anchoUtil, 20), XStringFormats.TopLeft);

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    documento.Save(ms, false);
                    return ms.ToArray();
                }
            }
        }
    }
}