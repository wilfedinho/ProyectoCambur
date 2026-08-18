using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class HistorialClinico
    {
        public int IdHistorial { get; set; }
        public int IdPaciente { get; set; }
        public string HabitosNocivos { get; set; }
        public string ContextoFamiliar { get; set; }
        public string AntecedentesFamiliares { get; set; }
        public string AntecedentesMedicos { get; set; }
        public string SituacionLaboral { get; set; }
        public string EventosTraumaticos { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DigitoVerificador { get; set; }

        public HistorialClinico()
        {
        }

        public HistorialClinico(int nIdHistorial, int nIdPaciente, string nHabitosNocivos, string nContextoFamiliar, string nAntecedentesFamiliares, string nAntecedentesMedicos, string nSituacionLaboral, string nEventosTraumaticos, DateTime nFechaRegistro, string nDigitoVerificador = null)
        {
            IdHistorial = nIdHistorial;
            IdPaciente = nIdPaciente;
            HabitosNocivos = nHabitosNocivos;
            ContextoFamiliar = nContextoFamiliar;
            AntecedentesFamiliares = nAntecedentesFamiliares;
            AntecedentesMedicos = nAntecedentesMedicos;
            SituacionLaboral = nSituacionLaboral;
            EventosTraumaticos = nEventosTraumaticos;
            FechaRegistro = nFechaRegistro;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}