using BE;
using BLL;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Script.Serialization;
using GUI;

public partial class FormBackupRestore : PaginaBase
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
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_backup_restore"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            CargarCarpetaDestino();
            CargarNombreArchivoPreview();
            CargarListaArchivos();
            CargarHistorial();
            string archivoRestaurado = Request.QueryString["restaurado"];
            if (!string.IsNullOrEmpty(archivoRestaurado))
            {
                pnlResultadoRestore.Visible = true;
                lblResultadoRestore.Text = archivoRestaurado;
            }
        }
    }

    private void AplicarTraducciones()
    {
        lblTaglineSidebar.Text = Traducir("tagline_panel_tecnico");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("header_administrador");
        lblHeaderTitulo.Text = Traducir("nav_backup_restore");

        lblAvisoCriticoTitulo.Text = Traducir("aviso_critico_titulo");
        lblAvisoCriticoTexto.Text = Traducir("aviso_critico_texto");

        lblTituloGenerarBackup.Text = Traducir("titulo_generar_backup");
        lblSubtituloGenerarBackup.Text = Traducir("subtitulo_generar_backup");
        lblConfigBackup.Text = Traducir("lbl_config_backup");
        lblEtiquetaCarpetaDestino.Text = Traducir("lbl_carpeta_destino");
        lblEtiquetaFormatoArchivo.Text = Traducir("lbl_formato_archivo");
        lblEtiquetaTipoBackup.Text = Traducir("lbl_tipo_backup");
        lblValorTipoBackup.Text = Traducir("valor_tipo_backup_completo");
        lblEtiquetaUltimoBackup.Text = Traducir("lbl_ultimo_backup");
        lblEtiquetaArchivoAGenerar.Text = Traducir("lbl_archivo_a_generar");
        btnGenerarBackup.Text = Traducir("btn_generar_backup");
        lblResultadoTituloBackup.Text = Traducir("msg_backup_generado");

        lblTituloRestaurarBackup.Text = Traducir("titulo_restaurar_backup");
        lblSubtituloRestaurarBackup.Text = Traducir("subtitulo_restaurar_backup");
        lblArchivosDisponibles.Text = Traducir("lbl_archivos_disponibles");
        lblSinBackupsDisponibles.Text = Traducir("lbl_sin_backups_disponibles");
        lblAvisoRestoreTexto.Text = Traducir("aviso_restore_texto");
        lblEtiquetaArchivoSeleccionado.Text = Traducir("lbl_archivo_seleccionado");
        btnCancelarRestore.Text = Traducir("btn_cancelar");
        btnConfirmarRestore.Text = Traducir("btn_confirmar_restore");
        btnIniciarRestore.Text = Traducir("btn_seleccionar_y_restaurar");
        lblResultadoTituloRestore.Text = Traducir("msg_backup_restaurado");

        lblCargaTitulo.Text = Traducir("carga_titulo_backup_restore");
        lblCargaSubtitulo.Text = Traducir("carga_subtitulo_backup_restore");

        lblTituloHistorial.Text = Traducir("titulo_historial_operaciones");
        gvHistorial.EmptyDataText = Traducir("msg_sin_operaciones");
        if (gvHistorial.Columns.Count >= 4)
        {
            gvHistorial.Columns[0].HeaderText = Traducir("th_fecha_hora");
            gvHistorial.Columns[1].HeaderText = Traducir("th_tipo");
            gvHistorial.Columns[2].HeaderText = Traducir("th_archivo");
            gvHistorial.Columns[3].HeaderText = Traducir("th_resultado");
        }
    }

    public string JsonConfirmarGenerarBackup
    {
        get
        {
            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Serialize(Traducir("confirm_generar_backup"));
        }
    }

    private void CargarCarpetaDestino()
    {
        GestorBackup gestorBackup = new GestorBackup();
        try
        {
            string carpeta = gestorBackup.ObtenerCarpetaDestino();
            lblCarpetaDestino.Text = string.IsNullOrWhiteSpace(carpeta) ? "-" : carpeta;
        }
        catch (Exception)
        {
            lblCarpetaDestino.Text = "-";
        }
    }

    private void CargarNombreArchivoPreview()
    {
        lblNombreArchivo.Text = "SistemaCambur_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
    }

    private void CargarListaArchivos()
    {
        GestorBackup gestorBackup = new GestorBackup();
        List<BackupDisponible> disponibles;

        try
        {
            disponibles = gestorBackup.ListarDisponibles();
        }
        catch (ExcepcionTraducible ex)
        {
            disponibles = new List<BackupDisponible>();
            MostrarError(TraducirExcepcion(ex));
        }

        lblUltimoBackup.Text = disponibles.Count > 0
            ? disponibles[0].Fecha.ToString("dd/MM/yyyy HH:mm:ss")
            : Traducir("lbl_sin_backups");

        string seleccionado = hfArchivoSeleccionado.Value;

        DataTable dt = new DataTable();
        dt.Columns.Add("NombreArchivo", typeof(string));
        dt.Columns.Add("Tamanio", typeof(string));
        dt.Columns.Add("Fecha", typeof(DateTime));
        dt.Columns.Add("Seleccionado", typeof(bool));

        foreach (BackupDisponible b in disponibles)
        {
            dt.Rows.Add(b.NombreArchivo, FormatearTamanio(b.TamanioBytes), b.Fecha, b.NombreArchivo == seleccionado);
        }

        rptArchivos.DataSource = dt;
        rptArchivos.DataBind();

        lblSinBackupsDisponibles.Visible = disponibles.Count == 0;
    }

    private string FormatearTamanio(long bytes)
    {
        double mb = bytes / (1024.0 * 1024.0);
        return mb.ToString("0.#") + " MB";
    }

    private void CargarHistorial()
    {
        GestorBackup gestorBackup = new GestorBackup();
        List<OperacionBackupRestore> historial = gestorBackup.ObtenerHistorial(10);

        DataTable dt = new DataTable();
        dt.Columns.Add("Fecha", typeof(DateTime));
        dt.Columns.Add("Tipo", typeof(string));
        dt.Columns.Add("Archivo", typeof(string));
        dt.Columns.Add("Resultado", typeof(string));

        foreach (OperacionBackupRestore op in historial)
        {
            dt.Rows.Add(op.FechaOperacion, op.TipoOperacion, op.NombreArchivo, op.Resultado);
        }

        gvHistorial.DataSource = dt;
        gvHistorial.DataBind();
    }

    protected void btnGenerarBackup_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlResultadoBackup.Visible = false;
        pnlResultadoRestore.Visible = false;

        GestorBackup gestorBackup = new GestorBackup();

        try
        {
            string nombreArchivo = gestorBackup.GenerarBackup();

            CargarNombreArchivoPreview();
            CargarListaArchivos();
            CargarHistorial();

            pnlResultadoBackup.Visible = true;
            lblResultadoBackup.Text = nombreArchivo;
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
        catch (Exception)
        {
            MostrarError(Traducir("error_backup_inesperado"));
        }
    }

    protected void btnIniciarRestore_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlResultadoRestore.Visible = false;

        string archivo = hfArchivoSeleccionado.Value;

        if (string.IsNullOrEmpty(archivo))
        {
            MostrarError(Traducir("error_seleccionar_archivo"));
            return;
        }

        lblArchivoARestaurar.Text = archivo;
        pnlConfirmRestore.Visible = true;
        btnIniciarRestore.Visible = false;
    }

    protected void btnCancelarRestore_Click(object sender, EventArgs e)
    {
        pnlConfirmRestore.Visible = false;
        btnIniciarRestore.Visible = true;
    }

    protected void btnConfirmarRestore_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlConfirmRestore.Visible = false;
        btnIniciarRestore.Visible = true;

        string archivo = hfArchivoSeleccionado.Value;

        if (string.IsNullOrEmpty(archivo))
        {
            MostrarError(Traducir("error_seleccionar_archivo"));
            return;
        }

        GestorBackup gestorBackup = new GestorBackup();

        try
        {
            gestorBackup.RestaurarBackup(archivo);
            RefrescarSesionPostRestore(archivo);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
        catch (Exception)
        {
            MostrarError(Traducir("error_restore_inesperado"));
        }
    }
    private void RefrescarSesionPostRestore(string archivoRestaurado)
    {
        PsicologoDAL psicologoDAL = new PsicologoDAL();
        Psicologo actualizado = psicologoDAL.BuscarPorId(GestorSesion.PsicologoActual.IdPsicologo);

        if (actualizado == null)
        {
            GestorSesion.Logout();
            Response.Redirect("FormLogin.aspx?restore=ok");
            return;
        }

        GestorSesion.Login(actualizado);
        Response.Redirect("FormBackupRestore.aspx?restaurado=" + Server.UrlEncode(archivoRestaurado));
    }

    private void MostrarError(string mensaje)
    {
        lblMensaje.Text = mensaje;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }
}