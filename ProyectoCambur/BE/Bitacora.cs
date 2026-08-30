using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Bitacora
    {
        public int IdBitacora { get; set; }
        public string Usuario { get; set; }
        public string Modulo { get; set; }
        public string Descripcion { get; set; }

        public int Criticidad { get; set; }
        public DateTime FechaEvento { get; set; }
        public string DigitoVerificador { get; set; }

        public Bitacora()
        {
        }

        public Bitacora(int nIdBitacora, string nUsuario, string nModulo, string nDescripcion, int nCriticidad, DateTime nFechaEvento, string nDigitoVerificador = null)
        {
            IdBitacora = nIdBitacora;
            Usuario = nUsuario;
            Modulo = nModulo;
            Descripcion = nDescripcion;
            Criticidad = nCriticidad;
            FechaEvento = nFechaEvento;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}