using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public static class CatalogoPlanes
    {
        public static readonly List<InfoPlan> Planes = new List<InfoPlan>
        {
            new InfoPlan(1, PlanSuscripcion.Free,        "Free",        "Básico",      4.99m),
            new InfoPlan(2, PlanSuscripcion.Profesional, "Profesional", "Profesional", 14.99m),
            new InfoPlan(3, PlanSuscripcion.Premium,     "Premium",     "Premium",     21.99m),
        };

        public static InfoPlan ObtenerPorId(int idPlan)
        {
            return Planes.FirstOrDefault(p => p.IdPlan == idPlan);
        }

        public static InfoPlan ObtenerPorRolPermiso(string rolPermiso)
        {
            return Planes.FirstOrDefault(p => p.RolPermiso == rolPermiso);
        }
    }
}