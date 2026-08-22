using BE;
using SERVICIOS;
using System;

public partial class HeaderUsuario : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado) return;

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        lblNombreProfesional.Text = psicologoActual.Nombre + " " + psicologoActual.Apellido;
        lblRolActual.Text = psicologoActual.RolPermiso;
        lblIniciales.Text = ObtenerIniciales(psicologoActual.Nombre, psicologoActual.Apellido);
        GUI.PaginaBase paginaBase = Page as GUI.PaginaBase;
        if (paginaBase != null)
        {
            lblMenuCambiarIdioma.Text = paginaBase.Traducir("menu_cambiar_idioma");
            lblMenuCambiarClave.Text = paginaBase.Traducir("menu_cambiar_clave");
            lblMenuCerrarSesion.Text = paginaBase.Traducir("menu_cerrar_sesion");
        }
    }

    private string ObtenerIniciales(string nombre, string apellido)
    {
        string inicialNombre = string.IsNullOrEmpty(nombre) ? "" : nombre.Substring(0, 1);
        string inicialApellido = string.IsNullOrEmpty(apellido) ? "" : apellido.Substring(0, 1);
        return (inicialNombre + inicialApellido).ToUpper();
    }
}