using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Idioma
    {
        public string NombreIdioma { get; set; }
        public string CodigoIso { get; set; }
        public bool IsDisponible { get; set; }
        public bool IsOcupado { get; set; }
        public string DigitoVerificador { get; set; }

        public Idioma()
        {
        }

        public Idioma(string nNombreIdioma, string nCodigoIso, bool nIsDisponible, bool nIsOcupado, string nDigitoVerificador = null)
        {
            NombreIdioma = nNombreIdioma;
            CodigoIso = nCodigoIso;
            IsDisponible = nIsDisponible;
            IsOcupado = nIsOcupado;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}