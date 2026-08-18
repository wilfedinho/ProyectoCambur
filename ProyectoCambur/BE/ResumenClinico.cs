using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class ResumenClinico
    {
        public int IdResumen { get; set; }
        public int IdPaciente { get; set; }
        public int IdProfesional { get; set; }
        public string Contenido { get; set; }
        public DateTime RangoDesde { get; set; }
        public DateTime RangoHasta { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string DigitoVerificador { get; set; }

        public ResumenClinico()
        {
        }

        public ResumenClinico(int nIdResumen, int nIdPaciente, int nIdProfesional, string nContenido, DateTime nRangoDesde, DateTime nRangoHasta, DateTime nFechaGeneracion, string nDigitoVerificador = null)
        {
            IdResumen = nIdResumen;
            IdPaciente = nIdPaciente;
            IdProfesional = nIdProfesional;
            Contenido = nContenido;
            RangoDesde = nRangoDesde;
            RangoHasta = nRangoHasta;
            FechaGeneracion = nFechaGeneracion;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}