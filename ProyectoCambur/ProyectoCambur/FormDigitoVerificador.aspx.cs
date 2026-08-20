using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;

public partial class FormDigitoVerificador : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        if (psicologoActual.RolPermiso != "Web Master")
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            lblNombreProfesional.Text = psicologoActual.Nombre + " " + psicologoActual.Apellido;
            lblIniciales.Text = ObtenerIniciales(psicologoActual.Nombre, psicologoActual.Apellido);

            CargarInconsistencias();
        }
    }

    private void CargarInconsistencias()
    {
        DigitoVerificador digitoVerificador = new DigitoVerificador();
        List<string> inconsistencias = digitoVerificador.VerificarIntegridadTodasLasTablas();

        if (inconsistencias.Count == 0)
        {
            pnlSinInconsistencias.Visible = true;
            pnlConInconsistencias.Visible = false;
        }
        else
        {
            pnlSinInconsistencias.Visible = false;
            pnlConInconsistencias.Visible = true;
            rptInconsistencias.DataSource = inconsistencias;
            rptInconsistencias.DataBind();
        }
    }

    protected void btnRecalcular_Click(object sender, EventArgs e)
    {
        DigitoVerificador digitoVerificador = new DigitoVerificador();
        digitoVerificador.RecalcularTodo();

        MostrarExito("Dígitos verificadores recalculados. El estado actual de la base quedó registrado como válido.");
        CargarInconsistencias();
    }

    private string ObtenerIniciales(string nombre, string apellido)
    {
        string inicialNombre = string.IsNullOrEmpty(nombre) ? "" : nombre.Substring(0, 1);
        string inicialApellido = string.IsNullOrEmpty(apellido) ? "" : apellido.Substring(0, 1);
        return (inicialNombre + inicialApellido).ToUpper();
    }

    private void MostrarExito(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-success";
        lblMensaje.Visible = true;
    }
}