using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace BLL
{
    public class ExportadorResumenClinico : ExportadorDocumentoBase, IExportadorDocumento
    {
        public bool EstaDisponible(int idPaciente)
        {
            GestorResumenClinico gestorResumen = new GestorResumenClinico();
            return gestorResumen.ObtenerPorPaciente(idPaciente).Count > 0;
        }

        public List<DocumentoExportable> ObtenerDisponibles(int idPaciente)
        {
            List<DocumentoExportable> documentos = new List<DocumentoExportable>();

            GestorResumenClinico gestorResumen = new GestorResumenClinico();
            foreach (ResumenClinico r in gestorResumen.ObtenerPorPaciente(idPaciente).OrderByDescending(r => r.FechaGeneracion))
            {
                documentos.Add(new DocumentoExportable
                {
                    IdDocumento = r.IdResumen,
                    Fecha = r.FechaGeneracion,
                    Detalle = "Rango " + r.RangoDesde.ToString("dd/MM/yyyy") + " al " + r.RangoHasta.ToString("dd/MM/yyyy")
                });
            }

            return documentos;
        }

        public byte[] GenerarPdf(int idPsicologo, Paciente paciente, int? idDocumento, out string tituloDocumento)
        {
            tituloDocumento = "Resumen Clínico IA";

            GestorResumenClinico gestorResumen = new GestorResumenClinico();
            List<ResumenClinico> resumenes = gestorResumen.ObtenerPorPaciente(paciente.IdPaciente)
                .OrderByDescending(r => r.FechaGeneracion)
                .ToList();

            if (resumenes.Count == 0)
            {
                throw new ExcepcionTraducible("error_documento_no_disponible");
            }

            ResumenClinico resumen = idDocumento.HasValue
                ? resumenes.FirstOrDefault(r => r.IdResumen == idDocumento.Value)
                : resumenes[0];

            if (resumen == null)
            {
                throw new ExcepcionTraducible("error_documento_no_disponible");
            }
            SeccionesResumenClinico secciones = gestorResumen.ObtenerSecciones(resumen);

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
                gfx.DrawString(DateTime.Today.ToString("dd/MM/yyyy"), fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 30), XStringFormats.TopRight);
                y += 32;
                gfx.DrawString(nombrePsicologo, fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 22;
                gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                y += 16;

                gfx.DrawString(tituloDocumento, fuenteSeccion, XBrushes.Black, Rect(margen, y, anchoUtil, 20), XStringFormats.TopLeft);
                y += 24;
                gfx.DrawString(paciente.Nombre + " " + paciente.Apellido, fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 16;
                gfx.DrawString("DNI: " + paciente.DNI + "   ·   Rango: " + resumen.RangoDesde.ToString("dd/MM/yyyy") + " al " + resumen.RangoHasta.ToString("dd/MM/yyyy"), fuenteTexto, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 24;
                gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                y += 16;

                if (secciones != null)
                {
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Contexto General", secciones.ContextoGeneral, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Evolución", secciones.Evolucion, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Temas Recurrentes", secciones.TemasRecurrentes, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Intervenciones", secciones.Intervenciones, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Observaciones", secciones.Observaciones, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                }
                else
                {
                    gfx.DrawString("No se pudo recuperar el contenido del resumen.", fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                    y += 20;
                }

                altoPagina = pagina.Height;
                anchoPagina = pagina.Width;
                if (y > altoPagina - 60)
                {
                    pagina = documento.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    y = margen;
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