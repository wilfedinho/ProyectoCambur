using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Web.UI;

public partial class FormLogin : System.Web.UI.Page
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            if (Request.QueryString["logout"] == "ok")
            {
                MostrarExito("Sesión cerrada correctamente. ¡Hasta la próxima!");
            }

            if (Request.QueryString["registro"] == "ok")
            {
                MostrarExito("Cuenta creada correctamente. Podés iniciar sesión.");
            }
        }
    }

    
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlBloqueado.Visible = false;
        pnlIntentos.Visible = false;

        if (!Page.IsValid) return;

        string email = txtEmail.Text.Trim().ToLower();
        string password = txtPassword.Text;

        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        Psicologo psicologoLogueado;
        ResultadoLogin resultado = gestorPsicologo.ValidarCredenciales(email, password, out psicologoLogueado);

        switch (resultado)
        {
            case ResultadoLogin.Ok:
                ProcesarLoginExitoso(psicologoLogueado);
                return;

            case ResultadoLogin.CuentaBloqueada:
                MostrarPanelBloqueado(
                    "Tu cuenta fue bloqueada por exceso de intentos fallidos. " +
                    "Contactá al administrador para desbloquearla.");
                return;

            case ResultadoLogin.CuentaDeshabilitada:
                MostrarError("Tu cuenta fue deshabilitada por un administrador. Contactalo para más información.");
                return;

            case ResultadoLogin.CuentaInactiva:
                MostrarError("Tu cuenta se encuentra inactiva. Contactá al administrador.");
                return;

            case ResultadoLogin.CredencialesInvalidas:
            default:
                MostrarError("El correo o la contraseña ingresados son incorrectos.");
                return;
        }
    }

    
    private void ProcesarLoginExitoso(Psicologo psicologoLogueado)
    {
        DigitoVerificador digitoVerificador = new DigitoVerificador();
        List<string> inconsistencias = digitoVerificador.VerificarIntegridadTodasLasTablas();

        if (inconsistencias.Count > 0)
        {
            switch (psicologoLogueado.RolPermiso)
            {
                case "Web Master":
                    
                    GestorSesion.Login(psicologoLogueado);
                    Response.Redirect("FormDigitoVerificador.aspx");
                    return;

                case "Administrador":
                    
                    Response.Redirect("FormError.aspx?codigo=inconsistencia_bd");
                    return;

                default:
                    
                    Response.Redirect("FormError.aspx?codigo=no_disponible");
                    return;
            }
        }

       
        GestorSesion.Login(psicologoLogueado);
        
        Response.Redirect(DestinoSegunRol(psicologoLogueado.RolPermiso));
    }

    
    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }

    private void MostrarExito(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-success";
        lblMensaje.Visible = true;
    }

    private void MostrarPanelBloqueado(string msg)
    {
        pnlBloqueado.Visible = true;
        lblMensajeBloqueado.Text = msg;
        
    }

    
    private string DestinoSegunRol(string rolPermiso)
    {
        switch (rolPermiso)
        {
            case "Administrador":
                return "FormMenuAdministrador.aspx";
            case "Web Master":
                return "FormMenuWebMaster.aspx";
            default:
                
                return "FormMenuProfesional.aspx";
        }
    }
}