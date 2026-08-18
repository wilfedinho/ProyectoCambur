using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class InformeDerivacion
    {
        public int IdInforme { get; set; }
        public int IdPaciente { get; set; }
        public int IdProfesional { get; set; }
        public string Contenido { get; set; }
        public EstadoInforme Estado { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public DateTime? FechaAuditoria { get; set; }
        public string DigitoVerificador { get; set; }

        public InformeDerivacion()
        {
        }

        public InformeDerivacion(int nIdInforme, int nIdPaciente, int nIdProfesional, string nContenido, EstadoInforme nEstado, DateTime nFechaGeneracion, DateTime? nFechaAuditoria = null, string nDigitoVerificador = null)
        {
            IdInforme = nIdInforme;
            IdPaciente = nIdPaciente;
            IdProfesional = nIdProfesional;
            Contenido = nContenido;
            Estado = nEstado;
            FechaGeneracion = nFechaGeneracion;
            FechaAuditoria = nFechaAuditoria;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}