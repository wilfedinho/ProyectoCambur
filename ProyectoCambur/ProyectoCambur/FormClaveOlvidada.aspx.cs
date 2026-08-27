using BLL;
using SERVICIOS;
using System;
using System.Web.UI;
using GUI;

public partial class FormClaveOlvidada : PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack && Request.QueryString["enviado"] == "ok")
        {
            MostrarPanelEnviado();
        }
    }

    protected void btnEnviar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        if (!Page.IsValid) return;
        string email = txtEmail.Text.Trim().ToLower();
        string urlBase = Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/");

        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        try
        {
            gestorPsicologo.SolicitarRecuperacionClave(email, urlBase);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
            return;
        }
        catch (Exception)
        {
            MostrarError(Traducir("error_envio_correo_recuperacion"));
            return;
        }
        Response.Redirect("FormClaveOlvidada.aspx?enviado=ok");
    }

    private void MostrarPanelEnviado()
    {
        pnlFormulario.Visible = false;
        pnlEnviado.Visible = true;
    }

    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }
}