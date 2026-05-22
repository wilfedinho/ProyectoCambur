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
    // =========================================================
    // CONSTANTES
    // =========================================================
    private const string CARPETA_BACKUPS = "BackupsSQL";
    private const string NOMBRE_BD = "CamburDB";

    // =========================================================
    // PAGE LOAD
    // =========================================================
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

    // =========================================================
    // ADMIN (demo)
    // TODO: reemplazar por Session["Administrador"]
    // =========================================================
    private void CargarAdminDemo()
    {
        lblNombreAdmin.Text = "Web Master";
        lblIniciales.Text = "WB";
    }

    // =========================================================
    // PREVIEW DEL NOMBRE DEL ARCHIVO A GENERAR
    // =========================================================
    private void CargarNombreArchivoPreview()
    {
        string nombre = "Backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
        lblNombreArchivo.Text = CARPETA_BACKUPS + "\\" + nombre;
    }

    // =========================================================
    // LISTA DE ARCHIVOS DISPONIBLES (demo)
    // TODO: reemplazar por:
    //   string carpeta = Server.MapPath("~/" + CARPETA_BACKUPS);
    //   string[] archivos = Directory.GetFiles(carpeta, "*.bak");
    // =========================================================
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

    // =========================================================
    // HISTORIAL DE OPERACIONES (demo)
    // TODO: reemplazar por BLL.BitacoraBLL.ObtenerPorModulo("Administración")
    // =========================================================
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

    // =========================================================
    // EVENTO: GENERAR BACKUP (escenario principal CUS06)
    // =========================================================
    protected void btnGenerarBackup_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlResultadoBackup.Visible = false;
        pnlResultadoRestore.Visible = false;

        string nombreArchivo = "Backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
        string rutaCompleta = CARPETA_BACKUPS + "\\" + nombreArchivo;

        try
        {
            // TODO: reemplazar por:
            //   string rutaFisica = Server.MapPath("~/" + CARPETA_BACKUPS + "/" + nombreArchivo);
            //   string sql = "BACKUP DATABASE [" + NOMBRE_BD + "] TO DISK = N'" + rutaFisica + "' WITH FORMAT, INIT, COMPRESSION";
            //   BLL.BackupBLL.EjecutarBackup(sql);
            //   BLL.BitacoraBLL.Registrar(idAdmin, "Administración",
            //       "Backup generado: " + nombreArchivo, criticidad: 1);

            // DEMO: simular backup exitoso
            bool exitoso = SimularBackupDemo(nombreArchivo);

            if (!exitoso)
                throw new Exception("Fallo simulado en la generación del backup.");

            // Actualizar preview y resultado
            CargarNombreArchivoPreview();
            lblUltimoBackup.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            CargarListaArchivosDemo();
            CargarHistorialDemo();

            pnlResultadoBackup.Visible = true;
            lblResultadoBackup.Text = rutaCompleta;
        }
        catch (Exception ex)
        {
            // ── FLUJO ALTERNATIVO 4.1: Error en la generación ─────
            MostrarError("No fue posible completar el backup por un error interno. " +
                         "Verificá los permisos de escritura en la carpeta BackupsSQL y reintentá. " +
                         "Detalle: " + ex.Message);
        }
    }

    // =========================================================
    // EVENTO: INICIAR RESTORE — mostrar panel de confirmación
    // =========================================================
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

    // =========================================================
    // EVENTO: CANCELAR RESTORE
    // =========================================================
    protected void btnCancelarRestore_Click(object sender, EventArgs e)
    {
        pnlConfirmRestore.Visible = false;
        btnIniciarRestore.Visible = true;
    }

    // =========================================================
    // EVENTO: CONFIRMAR RESTORE (flujo 3.1 CUS06)
    // =========================================================
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
            // TODO: reemplazar por:
            //   string rutaFisica = Server.MapPath("~/" + CARPETA_BACKUPS + "/" + archivo);
            //
            //   // Validar integridad del archivo (flujo 3.1.1)
            //   if (!BLL.BackupBLL.ValidarArchivo(rutaFisica))
            //   { MostrarError("El archivo seleccionado está corrupto o en formato inválido."); return; }
            //
            //   // Verificar si la BD existe
            //   bool bdExiste = BLL.BackupBLL.ExisteBaseDatos(NOMBRE_BD);
            //   string sql;
            //   if (bdExiste)
            //   {
            //       // Modo usuario único → restaurar → modo multiusuario
            //       sql = "ALTER DATABASE [" + NOMBRE_BD + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
            //             "RESTORE DATABASE [" + NOMBRE_BD + "] FROM DISK = N'" + rutaFisica + "' WITH REPLACE; " +
            //             "ALTER DATABASE [" + NOMBRE_BD + "] SET MULTI_USER;";
            //   }
            //   else
            //   {
            //       sql = "RESTORE DATABASE [" + NOMBRE_BD + "] FROM DISK = N'" + rutaFisica + "';";
            //   }
            //   BLL.BackupBLL.EjecutarRestore(sql);
            //   BLL.BitacoraBLL.Registrar(idAdmin, "Administración",
            //       "Restore desde: " + archivo, criticidad: 1);

            // DEMO: simular restore exitoso
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
            // ── FLUJO ALTERNATIVO 3.1.2: Error en el proceso ──────
            // Intentar restablecer modo multiusuario si corresponde
            // TODO: BLL.BackupBLL.IntentarRestablecerMultiUsuario(NOMBRE_BD);

            MostrarError("No fue posible completar la restauración. " +
                         "Se intentó restablecer el modo multiusuario. " +
                         "Contactá al soporte técnico si el sistema no responde. " +
                         "Detalle: " + ex.Message);
        }
    }

    // =========================================================
    // DEMOS
    // =========================================================
    private bool SimularBackupDemo(string nombreArchivo)
    {
        // Siempre exitoso en demo
        return true;
    }

    private bool SimularRestoreDemo(string archivo)
    {
        // Siempre exitoso en demo
        return true;
    }

    // =========================================================
    // HELPERS
    // =========================================================
    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }
}
