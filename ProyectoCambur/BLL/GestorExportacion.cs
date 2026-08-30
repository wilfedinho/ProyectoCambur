using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace BLL
{

    public class GestorExportacion
    {
        public const string TIPO_RESUMEN = "RESUMEN";
        public const string TIPO_DERIVACION = "DERIVACION";
        public const string TIPO_PERFIL = "PERFIL";

        public byte[] Generar(int idPsicologo, int idPaciente, string tipoDocumento, out string nombreArchivoSugerido)
        {
            GestorPaciente gestorPaciente = new GestorPaciente();
            Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
            if (paciente == null || paciente.IdPsicologo != idPsicologo)
            {
 
                throw new ExcepcionTraducible("error_paciente_no_propio");
            }

            byte[] pdf;
            string tituloDocumento;

            switch (tipoDocumento)
            {
                case TIPO_RESUMEN:
                    pdf = GenerarResumenClinicoPdf(idPsicologo, paciente, out tituloDocumento);
                    break;
                case TIPO_DERIVACION:
                case TIPO_PERFIL:
                    throw new ExcepcionTraducible("error_documento_no_disponible");
                default:
                    throw new ExcepcionTraducible("error_documento_no_disponible");
            }

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_REPORTES, EventosBitacora.DESC_EXPORTAR_PDF, EventosBitacora.CRIT_EXPORTAR_PDF);

            nombreArchivoSugerido = ArmarNombreArchivo(paciente, tituloDocumento);
            return pdf;
        }

        public bool DocumentoDisponible(int idPsicologo, int idPaciente, string tipoDocumento)
        {
            if (tipoDocumento != TIPO_RESUMEN) return false;

            GestorResumenClinico gestorResumen = new GestorResumenClinico();
            return gestorResumen.ObtenerPorPaciente(idPaciente).Count > 0;
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

        #region Generación PDF - Resumen Clínico IA

        private byte[] GenerarResumenClinicoPdf(int idPsicologo, Paciente paciente, out string tituloDocumento)
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

            ResumenClinico resumen = resumenes[0];
            SeccionesResumenClinico secciones = gestorResumen.ObtenerSecciones(resumen);

            string nombrePsicologo = "";
            if (GestorSesion.EstaAutenticado)
            {
                nombrePsicologo = "Prof. " + GestorSesion.PsicologoActual.Nombre + " " + GestorSesion.PsicologoActual.Apellido;
            }

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

        private double EscribirSeccion(ref XGraphics gfx, PdfDocument documento, ref PdfPage pagina, string titulo, string contenido, double margen, double y, double anchoUtil, XFont fuenteSeccion, XFont fuenteTexto)
        {
            if (string.IsNullOrWhiteSpace(contenido)) return y;

            double altoPagina = pagina.Height;

            if (y > altoPagina - 100)
            {
                pagina = documento.AddPage();
                gfx = XGraphics.FromPdfPage(pagina);
                y = margen;
                altoPagina = pagina.Height;
            }

            gfx.DrawString(titulo, fuenteSeccion, XBrushes.DarkSlateBlue, Rect(margen, y, anchoUtil, 18), XStringFormats.TopLeft);
            y += 20;

            foreach (string linea in DividirEnLineas(contenido, fuenteTexto, gfx, anchoUtil))
            {
                if (y > altoPagina - 60)
                {
                    pagina = documento.AddPage();
                    gfx = XGraphics.FromPdfPage(pagina);
                    y = margen;
                    altoPagina = pagina.Height;
                }
                gfx.DrawString(linea, fuenteTexto, XBrushes.Black, Rect(margen, y, anchoUtil, 14), XStringFormats.TopLeft);
                y += 14;
            }

            y += 12;
            return y;
        }

        private List<string> DividirEnLineas(string texto, XFont fuente, XGraphics gfx, double anchoMaximo)
        {
            List<string> lineas = new List<string>();
            foreach (string parrafo in texto.Replace("\r\n", "\n").Split('\n'))
            {
                string lineaActual = "";
                foreach (string palabra in parrafo.Split(' '))
                {
                    string prueba = lineaActual.Length == 0 ? palabra : lineaActual + " " + palabra;
                    if (gfx.MeasureString(prueba, fuente).Width > anchoMaximo && lineaActual.Length > 0)
                    {
                        lineas.Add(lineaActual);
                        lineaActual = palabra;
                    }
                    else
                    {
                        lineaActual = prueba;
                    }
                }
                lineas.Add(lineaActual);
            }
            return lineas;
        }
        private XRect Rect(double x, double y, double ancho, double alto)
        {
            return new XRect(XUnit.FromPoint(x), XUnit.FromPoint(y), XUnit.FromPoint(ancho), XUnit.FromPoint(alto));
        }

        private string ArmarNombreArchivo(Paciente paciente, string tituloDocumento)
        {
            string baseNombre = tituloDocumento.Replace(" ", "_") + "_" + paciente.Apellido + "_" + paciente.Nombre;
            baseNombre = string.Join("", baseNombre.Split(System.IO.Path.GetInvalidFileNameChars()));
            return baseNombre + "_" + DateTime.Today.ToString("yyyyMMdd") + ".pdf";
        }

        #endregion
    }
}