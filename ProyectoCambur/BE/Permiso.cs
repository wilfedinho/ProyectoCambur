using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
 
    public abstract class Permiso
    {
        private readonly string nombre;

        protected Permiso(string nNombre)
        {
            nombre = nNombre;
        }

        public virtual void Agregar(Permiso permiso)
        {
        }

        public virtual void Quitar(Permiso permiso)
        {
        }

        public string ObtenerNombre()
        {
            return nombre;
        }
    }
}