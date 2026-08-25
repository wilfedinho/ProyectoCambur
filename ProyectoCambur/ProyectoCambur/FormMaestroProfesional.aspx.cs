using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using GUI;
public partial class FormMaestroProfesional : PaginaBase
{
    private class FilaProfesional
    {
        public int IdPsicologo { get; set; }
        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Idioma { get; set; }
        public string RolPermiso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
        public bool IsHabilitado { get; set; }
        public bool IsBloqueado { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }
        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        if (!new GestorPermiso().TienePermiso(psicologoActual.RolPermiso, "acceder_abm_profesionales"))
        {
            DenegarAcceso();
            return;
        }
        AplicarTraducciones();
        if (!IsPostBack)
        {
            lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
            ModoAlta();
            CargarGrilla();
        }
    }
    private void AplicarTraducciones()
    {
        ddlFiltroEstado.Items.FindByValue("TODOS").Text = Traducir("opt_todos");
        ddlFiltroEstado.Items.FindByValue("ACTIVOS").Text = Traducir("opt_activos");
        ddlFiltroEstado.Items.FindByValue("INACTIVOS").Text = Traducir("opt_desactivados");
        lblTituloListado.Text = Traducir("titulo_profesionales_registrados");
        lblEtiquetaMostrar.Text = Traducir("lbl_mostrar");
        gvProfesionales.Columns[0].HeaderText = Traducir("col_profesional");
        gvProfesionales.Columns[1].HeaderText = Traducir("col_dni");
        gvProfesionales.Columns[2].HeaderText = Traducir("col_email");
        gvProfesionales.Columns[3].HeaderText = Traducir("col_idioma");
        gvProfesionales.Columns[4].HeaderText = Traducir("lbl_rol_plan");
        gvProfesionales.Columns[5].HeaderText = Traducir("col_registrado");
        gvProfesionales.Columns[6].HeaderText = Traducir("col_estado");
        gvProfesionales.Columns[7].HeaderText = Traducir("col_acciones");
        gvProfesionales.EmptyDataText = Traducir("empty_profesionales");
        ddlRol.Items.FindByValue("").Text = Traducir("opt_seleccionar");
        ddlRol.Items.FindByValue("Free").Text = Traducir("rol_psicologo_free");
        ddlRol.Items.FindByValue("Profesional").Text = Traducir("rol_psicologo_profesional");
        ddlRol.Items.FindByValue("Premium").Text = Traducir("rol_psicologo_premium");
    }

    private void CargarGrilla()
    {
        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        int idPropio = GestorSesion.PsicologoActual.IdPsicologo;
        List<Psicologo> todos = gestorPsicologo.ObtenerTodos().Where(p => p.IdPsicologo != idPropio).ToList();
        string filtro = ddlFiltroEstado.SelectedValue;
        if (filtro == "ACTIVOS")
        {
            todos = todos.Where(p => p.Activo).ToList();
        }
        else if (filtro == "INACTIVOS")
        {
            todos = todos.Where(p => !p.Activo).ToList();
        }
        List<FilaProfesional> filas = todos.Select(p => new FilaProfesional
        {
            IdPsicologo = p.IdPsicologo,
            NombreCompleto = p.Nombre + " " + p.Apellido,
            Dni = p.Dni,
            Email = p.Email,
            Idioma = p.Idioma,
            RolPermiso = p.RolPermiso,
            FechaRegistro = p.FechaRegistro,
            Activo = p.Activo,
            IsHabilitado = p.IsHabilitado,
            IsBloqueado = p.IsBloqueado
        }).ToList();
        gvProfesionales.DataSource = filas;
        gvProfesionales.DataBind();
        TraducirFilasGrilla();
        List<Psicologo> universoCompleto = gestorPsicologo.ObtenerTodos().Where(p => p.IdPsicologo != idPropio).ToList();
        lblBadgeActivos.Text = universoCompleto.Count(p => p.Activo) + " " + Traducir("badge_activos_sufijo");
        lblBadgeActivos.Visible = true;
        lblBadgeInactivos.Text = universoCompleto.Count(p => !p.Activo) + " " + Traducir("badge_inactivos_sufijo");
        lblBadgeInactivos.Visible = true;
    }

    private void TraducirFilasGrilla()
    {
        foreach (GridViewRow fila in gvProfesionales.Rows)
        {
            if (fila.RowType != DataControlRowType.DataRow) continue;
            LinkButton lbModificar = fila.FindControl("lbModificar") as LinkButton;
            if (lbModificar != null) lbModificar.Text = "✏️ " + Traducir("btn_modificar");
            LinkButton lbBaja = fila.FindControl("lbBaja") as LinkButton;
            if (lbBaja != null) lbBaja.Text = "🚫 " + Traducir("btn_dar_baja");
            LinkButton lbReactivar = fila.FindControl("lbReactivar") as LinkButton;
            if (lbReactivar != null) lbReactivar.Text = "✅ " + Traducir("btn_reactivar");
            LinkButton lbDeshabilitar = fila.FindControl("lbDeshabilitar") as LinkButton;
            if (lbDeshabilitar != null) lbDeshabilitar.Text = "⛔ " + Traducir("btn_deshabilitar");
            LinkButton lbHabilitar = fila.FindControl("lbHabilitar") as LinkButton;
            if (lbHabilitar != null) lbHabilitar.Text = "✅ " + Traducir("btn_habilitar");
            LinkButton lbDesbloquear = fila.FindControl("lbDesbloquear") as LinkButton;
            if (lbDesbloquear != null) lbDesbloquear.Text = "🔓 " + Traducir("btn_desbloquear");
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        bool esAlta = hdnIdPsicologo.Value == "0";
        if (!Page.IsValid) return;
        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        try
        {
            if (esAlta)
            {
                Psicologo nuevoPsicologo = new Psicologo();
                nuevoPsicologo.Nombre = txtNombre.Text.Trim();
                nuevoPsicologo.Apellido = txtApellido.Text.Trim();
                nuevoPsicologo.Dni = txtDni.Text.Trim();
                nuevoPsicologo.Email = txtEmail.Text.Trim().ToLower();
                nuevoPsicologo.Contrasena = nuevoPsicologo.Dni + nuevoPsicologo.Email;
                nuevoPsicologo.Idioma = ddlIdioma.SelectedValue;
                nuevoPsicologo.RolPermiso = ddlRol.SelectedValue;
                gestorPsicologo.Alta(nuevoPsicologo);
                MostrarExito(string.Format(Traducir("msg_profesional_registrado"), nuevoPsicologo.Nombre + " " + nuevoPsicologo.Apellido));
            }
            else
            {
                int idPsicologo = Convert.ToInt32(hdnIdPsicologo.Value);
                Psicologo psicologoModificado = gestorPsicologo.BuscarPorId(idPsicologo);
                if (psicologoModificado == null)
                {
                    MostrarError(Traducir("msg_profesional_no_existe"));
                    ModoAlta();
                    CargarGrilla();
                    return;
                }
                psicologoModificado.Nombre = txtNombre.Text.Trim();
                psicologoModificado.Apellido = txtApellido.Text.Trim();
                psicologoModificado.Dni = txtDni.Text.Trim();
                psicologoModificado.Email = txtEmail.Text.Trim().ToLower();
                psicologoModificado.Idioma = ddlIdioma.SelectedValue;
                psicologoModificado.RolPermiso = ddlRol.SelectedValue;
                gestorPsicologo.Modificar(psicologoModificado);
                MostrarExito(string.Format(Traducir("msg_profesional_modificado"), psicologoModificado.Nombre + " " + psicologoModificado.Apellido));
            }
            ModoAlta();
            CargarGrilla();
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void btnCancelarEdicion_Click(object sender, EventArgs e)
    {
        ModoAlta();
    }

    protected void ddlFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        CargarGrilla();
    }

    protected void gvProfesionales_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int idPsicologo = Convert.ToInt32(e.CommandArgument);
        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        try
        {
            switch (e.CommandName)
            {
                case "Modificar":
                    CargarFormularioParaEdicion(idPsicologo);
                    return;

                case "DarBaja":
                    gestorPsicologo.Baja(idPsicologo);
                    MostrarExito(Traducir("msg_profesional_baja"));
                    break;

                case "Reactivar":
                    gestorPsicologo.Activar(idPsicologo);
                    MostrarExito(Traducir("msg_profesional_reactivado"));
                    break;

                case "Deshabilitar":
                    gestorPsicologo.Deshabilitar(idPsicologo);
                    MostrarExito(Traducir("msg_profesional_deshabilitado"));
                    break;

                case "Habilitar":
                    gestorPsicologo.Habilitar(idPsicologo);
                    MostrarExito(Traducir("msg_profesional_habilitado"));
                    break;

                case "Desbloquear":
                    gestorPsicologo.Desbloquear(idPsicologo);
                    MostrarExito(Traducir("msg_profesional_desbloqueado"));
                    break;
            }
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
        CargarGrilla();
    }

    private void ModoAlta()
    {
        hdnIdPsicologo.Value = "0";
        lblFormTitulo.Text = Traducir("titulo_nuevo_profesional");
        btnGuardar.Text = Traducir("btn_registrar_profesional");
        btnCancelarEdicion.Visible = false;
        pnlAvisoContrasena.Visible = true;
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtDni.Text = string.Empty;
        txtEmail.Text = string.Empty;
        ddlIdioma.SelectedIndex = 0;
        ddlRol.SelectedIndex = 0;
    }

    private void CargarFormularioParaEdicion(int idPsicologo)
    {
        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        Psicologo psicologo = gestorPsicologo.BuscarPorId(idPsicologo);
        if (psicologo == null)
        {
            MostrarError(Traducir("msg_profesional_no_existe_grilla"));
            CargarGrilla();
            return;
        }
        hdnIdPsicologo.Value = psicologo.IdPsicologo.ToString();
        lblFormTitulo.Text = Traducir("titulo_modificar_profesional");
        btnGuardar.Text = Traducir("btn_guardar_cambios");
        btnCancelarEdicion.Visible = true;
        pnlAvisoContrasena.Visible = false;
        txtNombre.Text = psicologo.Nombre;
        txtApellido.Text = psicologo.Apellido;
        txtDni.Text = psicologo.Dni;
        txtEmail.Text = psicologo.Email;
        ddlIdioma.SelectedValue = psicologo.Idioma;
        ddlRol.SelectedValue = psicologo.RolPermiso;
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
}