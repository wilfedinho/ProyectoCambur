namespace SERVICIOS
{
    public class InconsistenciaDetectada
    {
        public string Clave { get; }
        public object[] Parametros { get; }

        public InconsistenciaDetectada(string clave, params object[] parametros)
        {
            Clave = clave;
            Parametros = parametros ?? new object[0];
        }
    }
}