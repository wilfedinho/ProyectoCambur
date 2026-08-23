using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

public partial class FormAuditoriaBitacora : GUI.PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;


        if (psicologoActual.RolPermiso != "Web Master")
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            CargarCombosDeFiltros();
            CargarGrilla();

            new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_ADMINISTRACION, EventosBitacora.DESC_CONSULTA_BITACORA, EventosBitacora.CRIT_CONSULTA_BITACORA);
        }
    }

    private void AplicarTraducciones()
    {
        lblTaglineSidebar.Text = Traducir("tagline_panel_tecnico");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");

        lblHeaderSeccion.Text = Traducir("header_web_master");
        lblHeaderPagina.Text = Traducir("menu_bitacora");

        lblTituloFiltros.Text = Traducir("titulo_filtros_bitacora");
        lblSubtituloFiltros.Text = Traducir("subtitulo_filtros_bitacora");
        lblEtiquetaFechaInicio.Text = Traducir("lbl_fecha_inicio");
        lblEtiquetaFechaFin.Text = Traducir("lbl_fecha_fin");
        lblEtiquetaModulo.Text = Traducir("lbl_modulo");
        lblEtiquetaUsuario.Text = Traducir("lbl_usuario_email");
        lblEtiquetaCriticidad.Text = Traducir("lbl_criticidad");
        btnLimpiarFiltros.Text = Traducir("btn_limpiar_filtros");
        btnFiltrar.Text = Traducir("btn_filtrar");

        lblTituloEventos.Text = Traducir("titulo_eventos_registrados");

        gvBitacora.Columns[0].HeaderText = Traducir("lbl_usuario_email");
        gvBitacora.Columns[1].HeaderText = Traducir("lbl_modulo");
        gvBitacora.Columns[2].HeaderText = Traducir("col_descripcion");
        gvBitacora.Columns[3].HeaderText = Traducir("lbl_criticidad");
        gvBitacora.Columns[4].HeaderText = Traducir("col_fecha_evento");
        gvBitacora.EmptyDataText = Traducir("empty_bitacora");

        lblTituloDetalle.Text = Traducir("titulo_detalle_profesional");
        lblDetEtiquetaNombre.Text = Traducir("col_nombre_completo");
        lblDetEtiquetaDni.Text = Traducir("lbl_dni");
        lblDetEtiquetaEmail.Text = Traducir("lbl_correo");
        lblDetEtiquetaRol.Text = Traducir("lbl_rol_plan");
        lblDetEtiquetaEstado.Text = Traducir("col_estado");
        lblDetalleNoEncontrado.Text = Traducir("msg_profesional_no_encontrado_por_email");
    }


    private void CargarCombosDeFiltros()
    {
        GestorBitacora gestorBitacora = new GestorBitacora();

        ddlModulo.Items.Clear();
        ddlModulo.Items.Add(new ListItem(Traducir("opt_todos"), ""));
        foreach (string modulo in gestorBitacora.ObtenerModulosRegistrados())
        {
            ddlModulo.Items.Add(new ListItem(modulo, modulo));
        }

        ddlUsuario.Items.Clear();
        ddlUsuario.Items.Add(new ListItem(Traducir("opt_todos"), ""));
        foreach (string usuario in gestorBitacora.ObtenerUsuariosRegistrados())
        {
            ddlUsuario.Items.Add(new ListItem(usuario, usuario));
        }

        ddlCriticidad.Items.Clear();
        ddlCriticidad.Items.Add(new ListItem(Traducir("opt_todos"), ""));
        foreach (int criticidad in gestorBitacora.ObtenerCriticidadesRegistradas())
        {
            ddlCriticidad.Items.Add(new ListItem(criticidad + " - " + TextoCriticidad(criticidad), criticidad.ToString()));
        }
    }

    protected void btnFiltrar_Click(object sender, EventArgs e)
    {
        gvBitacora.PageIndex = 0;
        pnlDetalle.Visible = false;
        CargarGrilla();
    }

    protected void btnLimpiarFiltros_Click(object sender, EventArgs e)
    {
        txtFechaInicio.Text = string.Empty;
        txtFechaFin.Text = string.Empty;
        ddlModulo.SelectedValue = "";
        ddlUsuario.SelectedValue = "";
        ddlCriticidad.SelectedValue = "";
        gvBitacora.PageIndex = 0;
        pnlDetalle.Visible = false;
        CargarGrilla();
    }

    private void CargarGrilla()
    {
        DateTime? fechaInicio = null;
        DateTime? fechaFin = null;
        DateTime parseado;

        if (DateTime.TryParse(txtFechaInicio.Text, out parseado)) fechaInicio = parseado;
        if (DateTime.TryParse(txtFechaFin.Text, out parseado)) fechaFin = parseado;

        string modulo = ddlModulo.SelectedValue;
        string usuario = ddlUsuario.SelectedValue;
        int? criticidad = string.IsNullOrEmpty(ddlCriticidad.SelectedValue) ? (int?)null : Convert.ToInt32(ddlCriticidad.SelectedValue);

        GestorBitacora gestorBitacora = new GestorBitacora();
        List<Bitacora> eventos = gestorBitacora.ObtenerPorFiltros(fechaInicio, fechaFin, modulo, usuario, criticidad);

        gvBitacora.DataSource = eventos;
        gvBitacora.DataBind();

        bool sinFiltrosAplicados = fechaInicio == null && fechaFin == null &&
            string.IsNullOrEmpty(modulo) && string.IsNullOrEmpty(usuario) && criticidad == null;

        lblCantidadResultados.Text = eventos.Count + (sinFiltrosAplicados ? " (" + Traducir("hint_ultimos_dias") + ")" : "");
    }

    protected void gvBitacora_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvBitacora.PageIndex = e.NewPageIndex;
        CargarGrilla();
    }

    protected void gvBitacora_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        Bitacora evento = e.Row.DataItem as Bitacora;
        if (evento == null) return;

        Label lblCriticidad = e.Row.FindControl("lblCriticidad") as Label;
        if (lblCriticidad != null)
        {
            lblCriticidad.Text = evento.Criticidad + " - " + TextoCriticidad(evento.Criticidad);
            lblCriticidad.CssClass = "badge-criticidad badge-criticidad-" + evento.Criticidad;
        }

        LinkButton lbVerDetalle = e.Row.FindControl("lbVerDetalle") as LinkButton;
        if (lbVerDetalle != null) lbVerDetalle.Text = "🔍 " + Traducir("btn_ver_detalle");
    }

    protected void gvBitacora_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "VerDetalle") return;

        string email = e.CommandArgument.ToString();
        pnlDetalle.Visible = true;

        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        Psicologo psicologo = gestorPsicologo.BuscarPorEmail(email);

        if (psicologo == null)
        {
            pnlDetalleEncontrado.Visible = false;
            pnlDetalleNoEncontrado.Visible = true;
            return;
        }

        pnlDetalleEncontrado.Visible = true;
        pnlDetalleNoEncontrado.Visible = false;

        lblDetNombre.Text = psicologo.Nombre + " " + psicologo.Apellido;
        lblDetDni.Text = psicologo.Dni;
        lblDetEmail.Text = psicologo.Email;
        lblDetRol.Text = psicologo.RolPermiso;

        lblDetActivo.Text = psicologo.Activo ? Traducir("estado_disponible") : Traducir("estado_desactivado");
        lblDetActivo.CssClass = psicologo.Activo ? "badge-estado activo" : "badge-estado inactivo";

        lblDetHabilitado.Text = psicologo.IsHabilitado ? Traducir("btn_habilitar") : Traducir("btn_deshabilitar");
        lblDetHabilitado.CssClass = psicologo.IsHabilitado ? "badge-estado activo" : "badge-estado inactivo";

        lblDetBloqueado.Visible = psicologo.IsBloqueado;
        lblDetBloqueado.Text = Traducir("estado_bloqueado");
    }


    private string TextoCriticidad(int criticidad)
    {
        switch (criticidad)
        {
            case 1: return Traducir("criticidad_alta");
            case 2: return Traducir("criticidad_media");
            case 3: return Traducir("criticidad_baja");
            default: return criticidad.ToString();
        }
    }
}