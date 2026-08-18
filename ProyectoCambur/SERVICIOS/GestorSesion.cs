using BE;
using System.Web;

namespace SERVICIOS
{
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