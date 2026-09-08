using SERVICIOS;
using System.Collections.Generic;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace BLL
{
    public abstract class ExportadorDocumentoBase
    {
        protected string ObtenerNombrePsicologoActual()
        {
            if (!GestorSesion.EstaAutenticado) return "";
            return "Prof. " + GestorSesion.PsicologoActual.Nombre + " " + GestorSesion.PsicologoActual.Apellido;
        }

        protected double EscribirSeccion(ref XGraphics gfx, PdfDocument documento, ref PdfPage pagina, string titulo, string contenido, double margen, double y, double anchoUtil, XFont fuenteSeccion, XFont fuenteTexto)
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

        protected List<string> DividirEnLineas(string texto, XFont fuente, XGraphics gfx, double anchoMaximo)
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

        protected XRect Rect(double x, double y, double ancho, double alto)
        {
            return new XRect(XUnit.FromPoint(x), XUnit.FromPoint(y), XUnit.FromPoint(ancho), XUnit.FromPoint(alto));
        }
    }
}