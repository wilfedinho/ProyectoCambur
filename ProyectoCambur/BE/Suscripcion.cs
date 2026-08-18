using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Suscripcion
    {
        public int IdSuscripcion { get; set; }
        public int IdProfesional { get; set; }
        public PlanSuscripcion Plan { get; set; }
        public EstadoSuscripcion Estado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string DigitoVerificador { get; set; }

        public Suscripcion()
        {
        }

        public Suscripcion(int nIdSuscripcion, int nIdProfesional, PlanSuscripcion nPlan, EstadoSuscripcion nEstado, DateTime nFechaInicio, DateTime? nFechaFin = null, string nDigitoVerificador = null)
        {
            IdSuscripcion = nIdSuscripcion;
            IdProfesional = nIdProfesional;
            Plan = nPlan;
            Estado = nEstado;
            FechaInicio = nFechaInicio;
            FechaFin = nFechaFin;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}