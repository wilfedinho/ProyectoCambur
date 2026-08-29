using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class InfoPlan
    {
        public int IdPlan { get; set; }
        public PlanSuscripcion Plan { get; set; }
        public string RolPermiso { get; set; }
        public string NombreComercial { get; set; }
        public decimal Precio { get; set; }

        public InfoPlan(int idPlan, PlanSuscripcion plan, string rolPermiso, string nombreComercial, decimal precio)
        {
            IdPlan = idPlan;
            Plan = plan;
            RolPermiso = rolPermiso;
            NombreComercial = nombreComercial;
            Precio = precio;
        }
    }
}