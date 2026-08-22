using System.Collections.Generic;

namespace SERVICIOS
{
    public interface IObservadorIdioma
    {
        void ActualizarIdioma(Dictionary<string, string> traducciones);
    }
}