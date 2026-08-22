using System;

namespace SERVICIOS
{
    public class ExcepcionTraducible : Exception
    {
        public string Clave { get; }
        public object[] Parametros { get; }

        public ExcepcionTraducible(string clave, params object[] parametros) : base(clave)
        {
            Clave = clave;
            Parametros = parametros ?? new object[0];
        }
    }
}