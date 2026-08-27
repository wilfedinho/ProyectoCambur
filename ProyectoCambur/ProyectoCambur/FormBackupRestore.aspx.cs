using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

public partial class FormBackupRestore : System.Web.UI.Page
{
    private const string CARPETA_BACKUPS = "BackupsSQL";
    private const string NOMBRE_BD = "CamburDB";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarAdminDemo();
            CargarNombreArchivoPreview();
            CargarListaArchivosDemo();
            CargarHistorialDemo();
            lblUltimoBackup.Text = "19/05/2026 08:30:00";
        }
    }
    private void CargarAdminDemo()
    {
        lblNombreAdmin.Text = "Web Master";
        lblIniciales.Text = "WB";
    }
    private void CargarNombreArchivoPreview()
    {
        string nombre = "Backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
        lblNombreArchivo.Text = CARPETA_BACKUPS + "\\" + nombre;
    }
    private void CargarListaArchivosDemo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("NombreArchivo", typeof(string));
        dt.Columns.Add("Tamanio", typeof(string));
        dt.Columns.Add("Fecha", typeof(DateTime));
        dt.Columns.Add("Seleccionado", typeof(bool));

        dt.Rows.Add("Backup_20260521_083000.bak", "248 MB", new DateTime(2026, 5, 21, 8, 30, 0), false);
        dt.Rows.Add("Backup_20260519_083000.bak", "245 MB", new DateTime(2026, 5, 19, 8, 30, 0), false);
        dt.Rows.Add("Backup_20260517_091512.bak", "241 MB", new DateTime(2026, 5, 17, 9, 15, 12), false);
        dt.Rows.Add("Backup_20260515_080000.bak", "238 MB", new DateTime(2026, 5, 15, 8, 0, 0), false);
        dt.Rows.Add("Backup_20260510_084520.bak", "230 MB", new DateTime(2026, 5, 10, 8, 45, 20), false);

        rptArchivos.DataSource = dt;
        rptArchivos.DataBind();
    }
    private void CargarHistorialDemo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Fecha", typeof(DateTime));
        dt.Columns.Add("Tipo", typeof(string));
        dt.Columns.Add("Archivo", typeof(string));
        dt.Columns.Add("Resultado", typeof(string));

        dt.Rows.Add(new DateTime(2026, 5, 21, 8, 30, 0), "BACKUP", "Backup_20260521_083000.bak", "Completado correctamente.");

        gvHistorial.DataSource = dt;
        gvHistorial.DataBind();
    }
    protected void btnGenerarBackup_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlResultadoBackup.Visible = false;
        pnlResultadoRestore.Visible = false;

        string nombreArchivo = "Backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
        string rutaCompleta = CARPETA_BACKUPS + "\\" + nombreArchivo;

        try
        {
            bool exitoso = SimularBackupDemo(nombreArchivo);
            if (!exitoso)
                throw new Exception("Fallo simulado en la generación del backup.");
            CargarNombreArchivoPreview();
            lblUltimoBackup.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            CargarListaArchivosDemo();
            CargarHistorialDemo();

            pnlResultadoBackup.Visible = true;
            lblResultadoBackup.Text = rutaCompleta;
        }
        catch (Exception ex)
        {
            MostrarError("No fue posible completar el backup por un error interno. " +
                         "Verificá los permisos de escritura en la carpeta BackupsSQL y reintentá. " +
                         "Detalle: " + ex.Message);
        }
    }
    protected void btnIniciarRestore_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlResultadoRestore.Visible = false;

        string archivo = hfArchivoSeleccionado.Value;

        if (string.IsNullOrEmpty(archivo))
        {
            MostrarError("Seleccioná un archivo de backup de la lista antes de continuar.");
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
            MostrarError("No se pudo identificar el archivo a restaurar. Seleccionalo nuevamente.");
            return;
        }

        try
        {
            bool exitoso = SimularRestoreDemo(archivo);

            if (!exitoso)
                throw new Exception("Fallo simulado en la restauración.");

            CargarHistorialDemo();
            pnlResultadoRestore.Visible = true;
            lblResultadoRestore.Text = "Restaurado desde: " + archivo +
                                          " · " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            hfArchivoSeleccionado.Value = string.Empty;
            CargarListaArchivosDemo();
        }
        catch (Exception ex)
        {

            MostrarError("No fue posible completar la restauración. " +
                         "Se intentó restablecer el modo multiusuario. " +
                         "Contactá al soporte técnico si el sistema no responde. " +
                         "Detalle: " + ex.Message);
        }
    }
    private bool SimularBackupDemo(string nombreArchivo)
    {
        return true;
    }

    private bool SimularRestoreDemo(string archivo)
    {
        return true;
    }
    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }
}
