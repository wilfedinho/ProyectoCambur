using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Consulta
    {
        public int IdConsulta { get; set; }
        public int IdPaciente { get; set; }
        public int IdPsicologo { get; set; }
        public DateTime FechaConsulta { get; set; }
        public int TiempoConsulta { get; set; }
        public string Objetivos { get; set; }
        public string Observaciones { get; set; }
        public string Hipotesis { get; set; }
        public string Intervenciones { get; set; }
        public string EvolucionObservada { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DigitoVerificador { get; set; }

        public Consulta()
        {
        }

        public Consulta(int nIdConsulta, int nIdPaciente, int nIdPsicologo, DateTime nFechaConsulta, int nTiempoConsulta, string nObjetivos, string nObservaciones, string nHipotesis, string nIntervenciones, string nEvolucionObservada, string nDiagnostico, string nTratamiento, DateTime nFechaRegistro, DateTime? nFechaModificacion = null, string nDigitoVerificador = null)
        {
            IdConsulta = nIdConsulta;
            IdPaciente = nIdPaciente;
            IdPsicologo = nIdPsicologo;
            FechaConsulta = nFechaConsulta;
            TiempoConsulta = nTiempoConsulta;
            Objetivos = nObjetivos;
            Observaciones = nObservaciones;
            Hipotesis = nHipotesis;
            Intervenciones = nIntervenciones;
            EvolucionObservada = nEvolucionObservada;
            Diagnostico = nDiagnostico;
            Tratamiento = nTratamiento;
            FechaRegistro = nFechaRegistro;
            FechaModificacion = nFechaModificacion;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}