using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIOS
{
    public class Bitacora
    {
        public int IdBitacora { get; set; }
        public int IdProfesional { get; set; }
        public string Accion { get; set; }
        public string Detalle { get; set; }
        public DateTime Fecha { get; set; }

        public Bitacora()
        {
        }

        public Bitacora(int nIdBitacora, int nIdProfesional, string nAccion, string nDetalle, DateTime nFecha)
        {
            IdBitacora = nIdBitacora;
            IdProfesional = nIdProfesional;
            Accion = nAccion;
            Detalle = nDetalle;
            Fecha = nFecha;
        }
    }
}