using BE;
using BLL;
using SERVICIOS;
using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using GUI;
public partial class FormRegistroProfesional : PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
    }
    protected string ObtenerPublicKeyStripe()
    {
        string publicKey = ConfigurationManager.AppSettings["StripePublicKey"];
        return publicKey ?? string.Empty;
    }

    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        string tokenTarjeta = hfTokenTarjeta.Value;
        if (string.IsNullOrWhiteSpace(tokenTarjeta))
        {
            MostrarError("No pudimos validar los datos de la tarjeta. Recargá la página e intentá nuevamente.");
            return;
        }

        int idPlan;
        if (!int.TryParse(hfPlanSeleccionado.Value, out idPlan))
        {
            idPlan = 2;
        }

        Psicologo nuevoPsicologo = new Psicologo
        {
            Nombre = txtNombre.Text.Trim(),
            Apellido = txtApellido.Text.Trim(),
            Dni = txtDNI.Text.Trim(),
            Email = txtEmail.Text.Trim().ToLower(),
            Idioma = "Español"
        };

        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        try
        {
            gestorPsicologo.RegistrarProfesionalConSuscripcion(nuevoPsicologo, txtPassword.Text, idPlan, tokenTarjeta, string.Empty);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
            return;
        }
        catch (Exception)
        {
            MostrarError("No fue posible completar el registro. Verificá los datos ingresados e intentá nuevamente.");
            return;
        }
        Response.Redirect("FormLogin.aspx?registro=ok");
    }

    private void MostrarError(string mensaje)
    {
        lblMensaje.Text = mensaje;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }
}