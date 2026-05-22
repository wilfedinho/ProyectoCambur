using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

public partial class FormAuditoriaBitacora : System.Web.UI.Page
{
    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarAdminDemo();
            CargarFiltroUsuarios();
            CargarBitacora(null, null, null, null, null);
        }
    }

    // =========================================================
    // ADMINISTRADOR (demo)
    // TODO: reemplazar por Session["Administrador"]
    // =========================================================
    private void CargarAdminDemo()
    {
        lblNombreAdmin.Text = "Web Master";
        lblIniciales.Text = "WM";
    }

    // =========================================================
    // DROPDOWN USUARIOS (demo)
    // TODO: reemplazar por BLL.ProfesionalBLL.ObtenerTodos()
    // =========================================================
    private void CargarFiltroUsuarios()
    {
        ddlFiltroUsuario.Items.Clear();
        ddlFiltroUsuario.Items.Add(new ListItem("Todos los usuarios", ""));
        ddlFiltroUsuario.Items.Add(new ListItem("Lucía Martínez", "lucia@consultorio.com"));
        ddlFiltroUsuario.Items.Add(new ListItem("Carlos Rodríguez", "carlos@consultorio.com"));
        ddlFiltroUsuario.Items.Add(new ListItem("Admin Sistema", "admin@cambur.com"));
    }

    // =========================================================
    // CARGA DE BITÁCORA CON FILTROS
    // TODO: reemplazar por BLL.BitacoraBLL.ObtenerFiltrado(usuario, modulo, criticidad, desde, hasta)
    // =========================================================
    private void CargarBitacora(string usuario, string modulo,
                                 string criticidad, DateTime? desde, DateTime? hasta)
    {
        var todos = ObtenerRegistrosDemo();
        var filtrados = new List<RegistroBitacora>();

        foreach (var r in todos)
        {
            bool pasaUsuario = string.IsNullOrEmpty(usuario) || r.Usuario.Contains(usuario);
            bool pasaModulo = string.IsNullOrEmpty(modulo) || r.Modulo == modulo;
            bool pasaCriticidad = string.IsNullOrEmpty(criticidad) || r.Criticidad.ToString() == criticidad;
            bool pasaDesde = !desde.HasValue || r.FechaEvento >= desde.Value;
            bool pasaHasta = !hasta.HasValue || r.FechaEvento <= hasta.Value;

            if (pasaUsuario && pasaModulo && pasaCriticidad && pasaDesde && pasaHasta)
                filtrados.Add(r);
        }

        if (filtrados.Count == 0)
        {
            gvBitacora.Visible = false;
            lblVacio.Visible = true;
            lblTotalRegistros.Visible = false;
            lblFiltroActivo.Visible = false;
            return;
        }

        gvBitacora.Visible = true;
        lblVacio.Visible = false;

        // Convertir a DataTable
        DataTable dt = new DataTable();
        dt.Columns.Add("IdBitacora", typeof(int));
        dt.Columns.Add("FechaEvento", typeof(DateTime));
        dt.Columns.Add("Usuario", typeof(string));
        dt.Columns.Add("Modulo", typeof(string));
        dt.Columns.Add("Criticidad", typeof(int));
        dt.Columns.Add("CriticidadLabel", typeof(string));
        dt.Columns.Add("Descripcion", typeof(string));

        foreach (var r in filtrados)
            dt.Rows.Add(r.IdBitacora, r.FechaEvento, r.Usuario,
                        r.Modulo, r.Criticidad, r.CriticidadLabel, r.Descripcion);

        gvBitacora.DataSource = dt;
        gvBitacora.DataBind();

        // Badges de conteo
        lblTotalRegistros.Text = filtrados.Count + " registros";
        lblTotalRegistros.Visible = true;

        bool hayFiltro = !string.IsNullOrEmpty(usuario) || !string.IsNullOrEmpty(modulo) ||
                         !string.IsNullOrEmpty(criticidad) || desde.HasValue || hasta.HasValue;
        lblFiltroActivo.Text = hayFiltro ? "Filtro activo" : "";
        lblFiltroActivo.Visible = hayFiltro;
    }

    // =========================================================
    // EVENTO: APLICAR FILTROS
    // =========================================================
    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlDetalle.Visible = false;
        gvBitacora.PageIndex = 0;

        DateTime? desde = null, hasta = null;
        DateTime d, h;
        if (DateTime.TryParse(txtFechaDesde.Text, out d)) desde = d;
        if (DateTime.TryParse(txtFechaHasta.Text, out h)) hasta = h.AddDays(1).AddSeconds(-1);

        if (desde.HasValue && hasta.HasValue && desde.Value > hasta.Value)
        {
            MostrarError("La fecha de inicio debe ser anterior a la fecha de fin.");
            return;
        }

        CargarBitacora(
            ddlFiltroUsuario.SelectedValue,
            ddlFiltroModulo.SelectedValue,
            ddlFiltroCriticidad.SelectedValue,
            desde, hasta);
    }

    // =========================================================
    // EVENTO: LIMPIAR FILTROS
    // =========================================================
    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlDetalle.Visible = false;
        gvBitacora.PageIndex = 0;

        ddlFiltroUsuario.SelectedIndex = 0;
        ddlFiltroModulo.SelectedIndex = 0;
        ddlFiltroCriticidad.SelectedIndex = 0;
        txtFechaDesde.Text = string.Empty;
        txtFechaHasta.Text = string.Empty;

        CargarBitacora(null, null, null, null, null);
    }

    // =========================================================
    // EVENTO: PAGINACIÓN
    // =========================================================
    protected void gvBitacora_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBitacora.PageIndex = e.NewPageIndex;
        btnFiltrar_Click(sender, EventArgs.Empty);
    }

    // =========================================================
    // EVENTO: VER DETALLE DE REGISTRO
    // =========================================================
    protected void gvBitacora_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow fila = gvBitacora.SelectedRow;
        if (fila == null) return;

        lblDetId.Text = fila.Cells[0].Text; // no visible directo, usamos DataKeys
        lblDetFecha.Text = fila.Cells[0].Text;
        lblDetUsuario.Text = fila.Cells[1].Text;
        lblDetModulo.Text = fila.Cells[2].Text;
        lblDetDescripcion.Text = fila.Cells[4].Text;

        // Criticidad la buscamos del registro demo
        int idReg = 0;
        var lbDet = (LinkButton)fila.FindControl("lbDetalle");
        if (lbDet != null) int.TryParse(lbDet.CommandArgument, out idReg);

        var reg = ObtenerRegistrosDemo().Find(r => r.IdBitacora == idReg);
        if (reg != null)
        {
            lblDetId.Text = reg.IdBitacora.ToString();
            lblDetFecha.Text = reg.FechaEvento.ToString("dd/MM/yyyy HH:mm:ss");
            lblDetUsuario.Text = reg.Usuario;
            lblDetModulo.Text = reg.Modulo;
            lblDetCriticidad.Text = reg.Criticidad + " — " + reg.CriticidadLabel;
            lblDetDescripcion.Text = reg.Descripcion;
        }

        pnlDetalle.Visible = true;
    }

    // =========================================================
    // EVENTO: CERRAR DETALLE
    // =========================================================
    protected void btnCerrarDetalle_Click(object sender, EventArgs e)
    {
        pnlDetalle.Visible = false;
    }

    // =========================================================
    // DATOS DEMO
    // TODO: reemplazar por BLL.BitacoraBLL.ObtenerTodos()
    // =========================================================
    private List<RegistroBitacora> ObtenerRegistrosDemo()
    {
        return new List<RegistroBitacora>
        {
            new RegistroBitacora(11, new DateTime(2026,5,20,14,25,03),"carlos@consultorio.com", "Pacientes",      3, "Baja",  "Registro de nuevo paciente."),
            new RegistroBitacora(12, new DateTime(2026,5,20,14,40,29),"carlos@consultorio.com", "Consultas",      3, "Baja",  "Registro de consulta clínica."),
            new RegistroBitacora(13, new DateTime(2026,5,20,15,10,44),"carlos@consultorio.com", "IA Asistiva",    2, "Media", "Perfilación de paciente con modelo Big Five (BFI)."),
            new RegistroBitacora(14, new DateTime(2026,5,20,15,55,12),"carlos@consultorio.com", "Logout",         1, "Alta",  "Cierre de sesión."),
            new RegistroBitacora(15, new DateTime(2026,5,19,8,30,00), "admin@cambur.com",        "Administración", 1, "Alta",  "Backup completo de base de datos generado: Backup_20260519_083000.bak"),
            new RegistroBitacora(16, new DateTime(2026,5,19,8,35,22), "admin@cambur.com",        "Administración", 1, "Alta",  "Recálculo de dígitos verificadores completado."),
        };
    }

    // =========================================================
    // CLASE AUXILIAR
    // =========================================================
    private class RegistroBitacora
    {
        public int IdBitacora { get; set; }
        public DateTime FechaEvento { get; set; }
        public string Usuario { get; set; }
        public string Modulo { get; set; }
        public int Criticidad { get; set; }
        public string CriticidadLabel { get; set; }
        public string Descripcion { get; set; }

        public RegistroBitacora(int id, DateTime fecha, string usuario,
            string modulo, int crit, string critLabel, string desc)
        {
            IdBitacora = id; FechaEvento = fecha; Usuario = usuario;
            Modulo = modulo; Criticidad = crit; CriticidadLabel = critLabel; Descripcion = desc;
        }
    }

    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }
}
