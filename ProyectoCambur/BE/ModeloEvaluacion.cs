using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class ModeloEvaluacion
    {
        public int IdModelo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string DigitoVerificador { get; set; }

        public ModeloEvaluacion()
        {
        }

        public ModeloEvaluacion(int nIdModelo, string nNombre, string nDescripcion, string nDigitoVerificador = null)
        {
            IdModelo = nIdModelo;
            Nombre = nNombre;
            Descripcion = nDescripcion;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}