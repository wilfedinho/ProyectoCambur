using BE;
using SERVICIOS;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace BLL
{
    public class ExportadorPerfilPaciente : ExportadorDocumentoBase, IExportadorDocumento
    {
        public bool EstaDisponible(int idPaciente)
        {
            GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
            return gestorPerfil.ObtenerPorPaciente(idPaciente).Count > 0;
        }

        public List<DocumentoExportable> ObtenerDisponibles(int idPaciente)
        {
            List<DocumentoExportable> documentos = new List<DocumentoExportable>();

            GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
            foreach (PerfilPaciente p in gestorPerfil.ObtenerPorPaciente(idPaciente).OrderByDescending(p => p.FechaGeneracion))
            {
                SeccionesPerfilPaciente secciones = gestorPerfil.ObtenerSecciones(p);
                documentos.Add(new DocumentoExportable
                {
                    IdDocumento = p.IdPerfil,
                    Fecha = p.FechaGeneracion,
                    Detalle = secciones != null && !string.IsNullOrWhiteSpace(secciones.NombreModelo)
                        ? "Modelo: " + secciones.NombreModelo
                        : ""
                });
            }

            return documentos;
        }

        public byte[] GenerarPdf(int idPsicologo, Paciente paciente, int? idDocumento, out string tituloDocumento)
        {
            tituloDocumento = "Perfil Evolutivo del Paciente";

            GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
            List<PerfilPaciente> perfiles = gestorPerfil.ObtenerPorPaciente(paciente.IdPaciente)
                .OrderByDescending(p => p.FechaGeneracion)
                .ToList();

            PerfilPaciente perfil = idDocumento.HasValue
                ? perfiles.FirstOrDefault(p => p.IdPerfil == idDocumento.Value)
                : perfiles.FirstOrDefault();

            if (perfil == null)
            {
                throw new ExcepcionTraducible("error_documento_no_disponible");
            }

            SeccionesPerfilPaciente secciones = gestorPerfil.ObtenerSecciones(perfil);

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
                gfx.DrawString(perfil.FechaGeneracion.ToString("dd/MM/yyyy"), fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 30), XStringFormats.TopRight);
                y += 32;
                gfx.DrawString(nombrePsicologo, fuenteSubtitulo, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 22;
                gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                y += 16;

                gfx.DrawString(tituloDocumento, fuenteSeccion, XBrushes.Black, Rect(margen, y, anchoUtil, 20), XStringFormats.TopLeft);
                y += 24;
                gfx.DrawString(paciente.Nombre + " " + paciente.Apellido, fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 16;
                string modeloTexto = "DNI: " + paciente.DNI;
                if (secciones != null && !string.IsNullOrWhiteSpace(secciones.NombreModelo))
                {
                    modeloTexto += "   ·   Modelo de evaluación: " + secciones.NombreModelo;
                }
                gfx.DrawString(modeloTexto, fuenteTexto, XBrushes.Gray, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
                y += 24;
                gfx.DrawLine(XPens.LightGray, margen, y, anchoPagina - margen, y);
                y += 16;

                if (secciones != null)
                {
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Descripción", secciones.Descripcion, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Dimensiones Evaluadas", secciones.Dimensiones, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Patrones Observados", secciones.Patrones, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                    y = EscribirSeccion(ref gfx, documento, ref pagina, "Consideraciones", secciones.Consideraciones, margen, y, anchoUtil, fuenteSeccion, fuenteTexto);
                }
                else
                {
                    gfx.DrawString("No se pudo recuperar el contenido del perfil.", fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 16), XStringFormats.TopLeft);
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