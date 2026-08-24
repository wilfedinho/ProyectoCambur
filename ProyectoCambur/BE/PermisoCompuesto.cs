using System.Collections.Generic;
using System.Linq;

namespace BE
{
    public class PermisoCompuesto : Permiso
    {
        private readonly List<Permiso> hijos = new List<Permiso>();

        public PermisoCompuesto(string nNombre) : base(nNombre)
        {
        }

        public override void Agregar(Permiso permiso)
        {
            hijos.Add(permiso);
        }

        public override void Quitar(Permiso permiso)
        {
            hijos.Remove(permiso);
        }

        public List<Permiso> ObtenerHijos()
        {
            return hijos;
        }
        public bool Contiene(string nombrePermisoSimple)
        {
            foreach (Permiso hijo in hijos)
            {
                if (hijo.ObtenerNombre() == nombrePermisoSimple)
                {
                    return true;
                }

                if (hijo is PermisoCompuesto compuesto && compuesto.Contiene(nombrePermisoSimple))
                {
                    return true;
                }
            }

            return false;
        }
        public bool ContieneFamilia(string nombreFamilia)
        {
            foreach (Permiso hijo in hijos)
            {
                if (hijo is PermisoCompuesto compuesto)
                {
                    if (compuesto.ObtenerNombre() == nombreFamilia)
                    {
                        return true;
                    }

                    if (compuesto.ContieneFamilia(nombreFamilia))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        public List<PermisoSimple> ObtenerTodosLosPermisosSimples()
        {
            List<PermisoSimple> resultado = new List<PermisoSimple>();

            foreach (Permiso hijo in hijos)
            {
                if (hijo is PermisoSimple simple)
                {
                    resultado.Add(simple);
                }
                else if (hijo is PermisoCompuesto compuesto)
                {
                    resultado.AddRange(compuesto.ObtenerTodosLosPermisosSimples());
                }
            }

            return resultado.Distinct().ToList();
        }
    }
}