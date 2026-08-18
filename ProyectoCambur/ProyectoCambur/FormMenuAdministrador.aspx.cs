using BE;
using SERVICIOS;
using System;

public partial class FormMenuAdministrador : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        if (psicologoActual.RolPermiso != "Administrador")
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            lblNombreProfesional.Text = psicologoActual.Nombre + " " + psicologoActual.Apellido;
            lblIniciales.Text = ObtenerIniciales(psicologoActual.Nombre, psicologoActual.Apellido);
        }
    }

    private string ObtenerIniciales(string nombre, string apellido)
    {
        string inicialNombre = string.IsNullOrEmpty(nombre) ? "" : nombre.Substring(0, 1);
        string inicialApellido = string.IsNullOrEmpty(apellido) ? "" : apellido.Substring(0, 1);
        return (inicialNombre + inicialApellido).ToUpper();
    }
}