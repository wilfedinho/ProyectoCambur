using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Web.UI;
using GUI;
public partial class FormLogin : PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            if (Request.QueryString["logout"] == "ok")
            {
                MostrarExito(Traducir("login_sesion_cerrada"));
            }
            if (Request.QueryString["registro"] == "ok")
            {
                MostrarExito(Traducir("login_cuenta_creada"));
            }
            if (Request.QueryString["acceso_denegado"] == "ok")
            {
                MostrarError(Traducir("login_sesion_cerrada_por_acceso_denegado"));
            }
            if (Request.QueryString["clave_restablecida"] == "ok")
            {
                MostrarExito(Traducir("login_clave_restablecida"));
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
                MostrarPanelBloqueado(Traducir("login_cuenta_bloqueada"));
                return;

            case ResultadoLogin.CuentaDeshabilitada:
                MostrarError(Traducir("login_cuenta_deshabilitada"));
                return;

            case ResultadoLogin.CuentaInactiva:
                MostrarError(Traducir("login_cuenta_inactiva"));
                return;

            case ResultadoLogin.CredencialesInvalidas:
            default:
                MostrarError(Traducir("login_credenciales_invalidas"));
                return;
        }
    }
    private void ProcesarLoginExitoso(Psicologo psicologoLogueado)
    {
        DigitoVerificador digitoVerificador = new DigitoVerificador();
        List<InconsistenciaDetectada> inconsistencias = digitoVerificador.VerificarIntegridadTodasLasTablas();
        if (inconsistencias.Count > 0)
        {
            GestorPermiso gestorPermiso = new GestorPermiso();
            BE.PermisoCompuesto perfil = gestorPermiso.LeerPerfilConEstructura(psicologoLogueado.RolPermiso);

            if (perfil != null && perfil.ContieneFamilia("HerramientasSistema"))
            {
                GestorSesion.Login(psicologoLogueado);
                Response.Redirect("FormDigitoVerificador.aspx");
                return;
            }

            if (perfil != null && perfil.ContieneFamilia("GestionSistema"))
            {
                Response.Redirect("FormError.aspx?codigo=inconsistencia_bd");
                return;
            }

            Response.Redirect("FormError.aspx?codigo=no_disponible");
            return;
        }
        GestorSesion.Login(psicologoLogueado);
        new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_AUTENTICACION, EventosBitacora.DESC_INICIO_SESION, EventosBitacora.CRIT_INICIO_SESION);
        Response.Redirect("FormMenu.aspx");
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

}