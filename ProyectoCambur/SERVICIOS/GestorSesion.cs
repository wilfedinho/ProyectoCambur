using BE;
using System.Web;

namespace SERVICIOS
{
    // NO es un singleton. Es una clase estatica que simplemente envuelve el objeto Session
    // que ASP.NET ya aisla por usuario (via cookie de SessionID). No guarda estado propio:
    // cada Get/Set lee y escribe directamente sobre el Session del usuario que hizo el request.
    public static class GestorSesion
    {
        private const string ClavePsicologoActual = "PsicologoActual";

        public static Psicologo PsicologoActual
        {
            get { return HttpContext.Current.Session[ClavePsicologoActual] as Psicologo; }
            set { HttpContext.Current.Session[ClavePsicologoActual] = value; }
        }

        public static bool EstaAutenticado
        {
            get { return PsicologoActual != null; }
        }

        public static void Login(Psicologo psicologoLogueado)
        {
            HttpContext.Current.Session[ClavePsicologoActual] = psicologoLogueado;
        }

        public static void Logout()
        {
            HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
        }
    }
}