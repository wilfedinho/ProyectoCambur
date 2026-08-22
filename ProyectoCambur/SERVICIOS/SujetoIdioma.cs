using DAL;
using System.Collections.Generic;
using System.Linq;

namespace SERVICIOS
{
    
    public class SujetoIdioma
    {
        private readonly List<IObservadorIdioma> observadores = new List<IObservadorIdioma>();

        public void Suscribir(IObservadorIdioma observador)
        {
            if (!observadores.Contains(observador))
            {
                observadores.Add(observador);
            }
        }

        public void Desuscribir(IObservadorIdioma observador)
        {
            observadores.Remove(observador);
        }

        public void NotificarCambioIdioma(string nombreIdioma)
        {
            TraduccionDAL traduccionDAL = new TraduccionDAL();
            Dictionary<string, string> traducciones = traduccionDAL.ObtenerTraduccionesDeIdioma(nombreIdioma);
            foreach (IObservadorIdioma observador in observadores.ToList())
            {
                observador.ActualizarIdioma(traducciones);
            }
        }
    }
}