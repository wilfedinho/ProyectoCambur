using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class FormDigitoVerificador : GUI.PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        GestorPermiso gestorPermiso = new GestorPermiso();
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_digito_verificador"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            CargarInconsistencias();
        }
    }

    private void AplicarTraducciones()
    {
        lblTaglineSidebar.Text = Traducir("tagline_panel_tecnico");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");

        lblTituloSinInconsistencias.Text = Traducir("titulo_sin_inconsistencias");
        lblSubtituloSinInconsistencias.Text = Traducir("subtitulo_sin_inconsistencias");
        lblTituloConInconsistencias.Text = "⚠️ " + Traducir("titulo_inconsistencias_detectadas");
        lblSubtituloConInconsistencias.Text = Traducir("subtitulo_inconsistencias_detectadas");
        lblTituloAcciones.Text = Traducir("titulo_acciones_disponibles");
        lblSubtituloAcciones.Text = Traducir("subtitulo_acciones_disponibles");
        lblAccionBackupTitulo.Text = Traducir("accion_backup_titulo");
        lblAccionBackupDesc.Text = Traducir("accion_backup_desc");
        lblAccionRestoreTitulo.Text = Traducir("accion_restore_titulo");
        lblAccionRestoreDesc.Text = Traducir("accion_restore_desc");
        lblAccionRecalcularTitulo.Text = Traducir("accion_recalcular_titulo");
        lblAccionRecalcularDesc.Text = Traducir("accion_recalcular_desc");
        btnRecalcular.Text = Traducir("btn_recalcular_digitos");
    }

    private void CargarInconsistencias()
    {
        DigitoVerificador digitoVerificador = new DigitoVerificador();
        List<InconsistenciaDetectada> inconsistencias = digitoVerificador.VerificarIntegridadTodasLasTablas();

        if (inconsistencias.Count == 0)
        {
            pnlSinInconsistencias.Visible = true;
            pnlConInconsistencias.Visible = false;
        }
        else
        {
            pnlSinInconsistencias.Visible = false;
            pnlConInconsistencias.Visible = true;


            List<string> mensajesTraducidos = inconsistencias
                .Select(inc =>
                {
                    string plantilla = Traducir(inc.Clave);
                    return inc.Parametros.Length > 0 ? string.Format(plantilla, inc.Parametros) : plantilla;
                })
                .ToList();

            rptInconsistencias.DataSource = mensajesTraducidos;
            rptInconsistencias.DataBind();
        }
    }

    protected void btnRecalcular_Click(object sender, EventArgs e)
    {
        DigitoVerificador digitoVerificador = new DigitoVerificador();
        digitoVerificador.RecalcularTodo();

        MostrarExito(Traducir("msg_digitos_recalculados"));
        CargarInconsistencias();
    }

    private void MostrarExito(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-success";
        lblMensaje.Visible = true;
    }
}