using BE;
using BLL;
using SERVICIOS;
using System;
using System.Web.UI;

public partial class FormCambiarClave : GUI.PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        AplicarTraducciones();

        if (!IsPostBack)
        {
            lnkCancelar.NavigateUrl = DestinoSegunRol(psicologoActual.RolPermiso);
        }
    }

    private void AplicarTraducciones()
    {
        lblTaglineSidebar.Text = Traducir("tagline_configuracion");
        lblMenuCerrarSesion.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("header_configuracion");
        lblHeaderPagina.Text = Traducir("header_cambiar_clave");

        lblTituloCard.Text = Traducir("titulo_cambiar_contrasena");
        lblSubtituloCard.Text = Traducir("subtitulo_cambiar_contrasena");
        lblSeccionVerificacion.Text = Traducir("seccion_verificacion_identidad");
        lblEtiquetaClaveActual.Text = Traducir("lbl_contrasena_actual");
        rfvClaveActual.ErrorMessage = Traducir("error_contrasena_actual_obligatoria");

        lblSeccionNueva.Text = Traducir("seccion_nueva_contrasena");
        lblEtiquetaClaveNueva.Text = Traducir("lbl_contrasena_nueva");
        rfvClaveNueva.ErrorMessage = Traducir("error_contrasena_nueva_obligatoria");

        lblEtiquetaConfirmacion.Text = Traducir("lbl_confirmar_contrasena");
        rfvConfirmacion.ErrorMessage = Traducir("error_confirmacion_obligatoria");
        cvClaves.ErrorMessage = Traducir("error_confirmacion_no_coincide");

        lnkCancelar.Text = Traducir("btn_cancelar");
        btnConfirmar.Text = Traducir("btn_confirmar_cambio");

        lblTituloPolitica.Text = Traducir("titulo_politica_contrasena");
        lblPolLongitud.Text = Traducir("pol_longitud");
        lblPolMayuscula.Text = Traducir("pol_mayuscula");
        lblPolNumero.Text = Traducir("pol_numero");
        lblPolDistinta.Text = Traducir("pol_distinta_actual");

        lblAvisoSeguroTitulo.Text = Traducir("aviso_seguro_titulo");
        lblAvisoSeguroTexto.Text = Traducir("aviso_seguro_texto");
        lblAvisoSesionTitulo.Text = Traducir("aviso_sesion_titulo");
        lblAvisoSesionTexto.Text = Traducir("aviso_sesion_texto");
    }

    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        GestorPsicologo gestorPsicologo = new GestorPsicologo();

        try
        {
            gestorPsicologo.CambiarContrasena(idPsicologo, txtClaveActual.Text, txtClaveNueva.Text);

            LimpiarCampos();
            MostrarExito(Traducir("msg_contrasena_actualizada"));
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
            LimpiarCampos();
        }
    }

    private void LimpiarCampos()
    {
        txtClaveActual.Text = string.Empty;
        txtClaveNueva.Text = string.Empty;
        txtClaveConfirmacion.Text = string.Empty;
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