using System;
using System.Collections.Generic;
using System.IO;
using PdfSharp.Fonts;

namespace BLL
{
    public class ResolvedorFuentesPdf : IFontResolver
    {
        public const string NombreFamilia = "Verdana";

        private const string CLAVE_REGULAR = "Verdana-Regular";
        private const string CLAVE_BOLD = "Verdana-Bold";
        private const string CLAVE_ITALIC = "Verdana-Italic";
        private const string CLAVE_BOLD_ITALIC = "Verdana-BoldItalic";

        private static readonly object candado = new object();
        private static readonly Dictionary<string, byte[]> cacheFuentes = new Dictionary<string, byte[]>();

        public string DefaultFontName
        {
            get { return NombreFamilia; }
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            string clave;

            if (isBold && isItalic) clave = CLAVE_BOLD_ITALIC;
            else if (isBold) clave = CLAVE_BOLD;
            else if (isItalic) clave = CLAVE_ITALIC;
            else clave = CLAVE_REGULAR;

            return new FontResolverInfo(clave);
        }

        public byte[] GetFont(string faceName)
        {
            lock (candado)
            {
                byte[] datosCacheados;
                if (cacheFuentes.TryGetValue(faceName, out datosCacheados))
                {
                    return datosCacheados;
                }

                string ruta = ObtenerRutaArchivo(faceName);
                byte[] datos = File.ReadAllBytes(ruta);
                cacheFuentes[faceName] = datos;
                return datos;
            }
        }

        private string ObtenerRutaArchivo(string faceName)
        {
            string carpetaFuentes = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            string nombreArchivo;

            switch (faceName)
            {
                case CLAVE_BOLD:
                    nombreArchivo = "verdanab.ttf";
                    break;
                case CLAVE_ITALIC:
                    nombreArchivo = "verdanai.ttf";
                    break;
                case CLAVE_BOLD_ITALIC:
                    nombreArchivo = "verdanaz.ttf";
                    break;
                default:
                    nombreArchivo = "verdana.ttf";
                    break;
            }

            string ruta = Path.Combine(carpetaFuentes, nombreArchivo);
            if (!File.Exists(ruta))
            {
                ruta = Path.Combine(carpetaFuentes, "verdana.ttf");
            }

            return ruta;
        }
        public static void Registrar()
        {
            if (GlobalFontSettings.FontResolver == null)
            {
                GlobalFontSettings.FontResolver = new ResolvedorFuentesPdf();
            }
        }
    }
}