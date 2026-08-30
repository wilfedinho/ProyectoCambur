using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Traduccion
    {
        public int IdTraduccion { get; set; }
        public string Idioma { get; set; }
        public string Clave { get; set; }
        public string Texto { get; set; }
        public bool Pendiente { get; set; }
        public string DigitoVerificador { get; set; }

        public Traduccion()
        {
        }

        public Traduccion(int nIdTraduccion, string nIdioma, string nClave, string nTexto, bool nPendiente = false, string nDigitoVerificador = null)
        {
            IdTraduccion = nIdTraduccion;
            Idioma = nIdioma;
            Clave = nClave;
            Texto = nTexto;
            Pendiente = nPendiente;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}