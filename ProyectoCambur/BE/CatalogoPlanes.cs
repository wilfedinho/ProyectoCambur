using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public static class CatalogoPlanes
    {
        public static readonly List<InfoPlan> Planes = new List<InfoPlan>
        {
            new InfoPlan(1, PlanSuscripcion.Free,        "Free",        "Básico",      4990m),
            new InfoPlan(2, PlanSuscripcion.Profesional, "Profesional", "Profesional", 9990m),
            new InfoPlan(3, PlanSuscripcion.Premium,     "Premium",     "Premium",     14990m),
        };

        public static InfoPlan ObtenerPorId(int idPlan)
        {
            return Planes.FirstOrDefault(p => p.IdPlan == idPlan);
        }
    }
}