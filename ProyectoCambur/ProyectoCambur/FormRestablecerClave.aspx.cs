using BE;
using BLL;
using SERVICIOS;
using System;
using System.Web.UI;
using GUI;

public partial class FormRestablecerClave : PaginaBase
{
    private string TokenActual
    {
        get { return Request.QueryString["token"]; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            ValidarTokenYMostrarFormulario();
        }
    }

    private void ValidarTokenYMostrarFormulario()
    {
        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        Psicologo psicologoDelToken;
        bool tokenValido = gestorPsicologo.ValidarTokenRecuperacion(TokenActual, out psicologoDelToken);

        pnlFormulario.Visible = tokenValido;
        pnlTokenInvalido.Visible = !tokenValido;
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        if (!Page.IsValid) return;

        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        try
        {
            gestorPsicologo.RestablecerClave(TokenActual, txtClaveNueva.Text);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
            return;
        }

        Response.Redirect("FormLogin.aspx?clave_restablecida=ok");
    }

    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }
}