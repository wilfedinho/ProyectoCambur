using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;

namespace GUI
{
   
    public class PaginaBase : System.Web.UI.Page, IObservadorIdioma
    {
        protected Dictionary<string, string> Traducciones { get; private set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;

            SujetoIdioma sujetoIdioma = new SujetoIdioma();
            sujetoIdioma.Suscribir(this);
            sujetoIdioma.NotificarCambioIdioma(ObtenerIdiomaActual());
        }

        public void ActualizarIdioma(Dictionary<string, string> traducciones)
        {
            Traducciones = traducciones;
        }
        public void RefrescarTraducciones()
        {
            SujetoIdioma sujetoIdioma = new SujetoIdioma();
            sujetoIdioma.Suscribir(this);
            sujetoIdioma.NotificarCambioIdioma(ObtenerIdiomaActual());
        }
        public string Traducir(string clave)
        {
            if (Traducciones != null && Traducciones.ContainsKey(clave))
            {
                return Traducciones[clave];
            }
            return clave;
        }

        protected string TraducirExcepcion(ExcepcionTraducible ex)
        {
            string plantilla = Traducir(ex.Clave);
            return ex.Parametros != null && ex.Parametros.Length > 0
                ? string.Format(plantilla, ex.Parametros)
                : plantilla;
        }

        private string ObtenerIdiomaActual()
        {
            if (GestorSesion.EstaAutenticado)
            {
                Psicologo psicologoActual = GestorSesion.PsicologoActual;
                if (!string.IsNullOrEmpty(psicologoActual.Idioma))
                {
                    return psicologoActual.Idioma;
                }
            }
            return "Español";
        }
    }
}