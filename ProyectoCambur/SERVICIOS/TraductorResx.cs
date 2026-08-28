using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace SERVICIOS
{
    public class TraductorResx : ITraductorAutomatico
    {
        private const string NOMBRE_BASE_RECURSOS = "SERVICIOS.Recursos.Traducciones";

        public Dictionary<string, string> Traducir(List<string> claves, string idiomaDestinoIso)
        {
            Dictionary<string, string> resultado = new Dictionary<string, string>();

            CultureInfo cultura;
            try
            {
                cultura = CultureInfo.GetCultureInfo(idiomaDestinoIso);
            }
            catch (CultureNotFoundException)
            {
                foreach (string clave in claves)
                {
                    resultado[clave] = null;
                }
                return resultado;
            }

            ResourceManager gestorRecursos = new ResourceManager(NOMBRE_BASE_RECURSOS, Assembly.GetExecutingAssembly());

            foreach (string clave in claves)
            {
                string texto;
                try
                {
                    texto = gestorRecursos.GetString(clave, cultura);
                }
                catch (MissingManifestResourceException)
                {
                    texto = null;
                }
                resultado[clave] = texto;
            }

            return resultado;
        }
    }
}