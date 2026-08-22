using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormGestionIdiomas : GUI.PaginaBase
{
    private string IdiomaSeleccionado
    {
        get { return ViewState["IdiomaSeleccionado"] as string; }
        set { ViewState["IdiomaSeleccionado"] = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

       
        if (psicologoActual.RolPermiso != "Administrador")
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
           
            CargarIdiomas();
        }
    }

    private void AplicarTraducciones()
    {
        lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
        lblMenuInicio.Text = Traducir("menu_inicio");
        lblMenuProfesionales.Text = Traducir("menu_profesionales");
        lblMenuIdiomas.Text = Traducir("menu_idiomas");
        lblMenuBitacora.Text = Traducir("menu_bitacora");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");

        lblHeaderSeccion.Text = Traducir("header_administrador");
        lblHeaderPagina.Text = Traducir("menu_idiomas");

        lblTituloNuevoIdioma.Text = Traducir("titulo_nuevo_idioma");
        lblSubtituloNuevoIdioma.Text = Traducir("subtitulo_nuevo_idioma");
        lblEtiquetaNombreIdioma.Text = Traducir("lbl_nombre_idioma");
        lblEtiquetaCodigoIso.Text = Traducir("lbl_codigo_iso");
        lblHintCodigoIso.Text = Traducir("hint_codigo_iso");
        rfvNombreIdioma.ErrorMessage = Traducir("error_nombre_idioma_obligatorio");
        rfvCodigoIso.ErrorMessage = Traducir("error_codigo_iso_obligatorio");
        btnAltaIdioma.Text = Traducir("btn_generar_idioma");
        btnAltaIdioma.OnClientClick = "return confirm('" + Traducir("confirm_alta_idioma").Replace("'", "\\'") + "');";

        lblTituloIdiomasSistema.Text = Traducir("titulo_idiomas_sistema");
        gvIdiomas.EmptyDataText = Traducir("empty_idiomas");
        gvIdiomas.Columns[0].HeaderText = Traducir("col_idioma");
        gvIdiomas.Columns[1].HeaderText = Traducir("col_codigo_iso");
        gvIdiomas.Columns[2].HeaderText = Traducir("col_estado");
        gvIdiomas.Columns[3].HeaderText = Traducir("col_acciones");

        lblTituloTraducciones.Text = Traducir("titulo_traducciones");
        lblSubtituloTraducciones.Text = Traducir("subtitulo_traducciones");
        gvTraducciones.EmptyDataText = Traducir("empty_traducciones");
        gvTraducciones.Columns[0].HeaderText = Traducir("col_clave");
        gvTraducciones.Columns[1].HeaderText = Traducir("col_texto");
        gvTraducciones.Columns[2].HeaderText = Traducir("col_estado");
    }

    protected void btnAltaIdioma_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        Idioma nuevoIdioma = new Idioma(
            txtNombreIdioma.Text.Trim(),
            txtCodigoIso.Text.Trim().ToLower(),
            true,
            false
        );

        GestorIdioma gestorIdioma = new GestorIdioma();

        try
        {
            gestorIdioma.Alta(nuevoIdioma);
            MostrarExito(string.Format(Traducir("msg_idioma_generado"), nuevoIdioma.NombreIdioma));

            txtNombreIdioma.Text = string.Empty;
            txtCodigoIso.Text = string.Empty;
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }

        CargarIdiomas();
    }

    protected void gvIdiomas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string nombreIdioma = e.CommandArgument.ToString();
        GestorIdioma gestorIdioma = new GestorIdioma();

        switch (e.CommandName)
        {
            case "Activar":
                gestorIdioma.Activar(nombreIdioma);
                MostrarExito(string.Format(Traducir("msg_idioma_activado"), nombreIdioma));
                CargarIdiomas();
                return;

            case "Desactivar":
                try
                {
                    gestorIdioma.Desactivar(nombreIdioma);
                    MostrarExito(string.Format(Traducir("msg_idioma_desactivado"), nombreIdioma));
                }
                catch (ExcepcionTraducible ex)
                {
                    MostrarError(TraducirExcepcion(ex));
                }
                CargarIdiomas();
                return;

            case "VerPendientes":
                IdiomaSeleccionado = nombreIdioma;
                lblIdiomaSeleccionado.Text = nombreIdioma;
                pnlTraducciones.Visible = true;
                CargarTraducciones(nombreIdioma);
                return;
        }
    }

    protected void gvIdiomas_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        Idioma idioma = e.Row.DataItem as Idioma;
        if (idioma == null) return;

        Label lblEstadoIdioma = e.Row.FindControl("lblEstadoIdioma") as Label;
        if (lblEstadoIdioma != null)
        {
            lblEstadoIdioma.Text = idioma.IsDisponible ? Traducir("estado_disponible") : Traducir("estado_desactivado");
            lblEstadoIdioma.CssClass = idioma.IsDisponible ? "badge-estado activo" : "badge-estado inactivo";
        }

        Label lblEnUso = e.Row.FindControl("lblEnUso") as Label;
        if (lblEnUso != null)
        {
            lblEnUso.Visible = idioma.IsOcupado;
            lblEnUso.Text = Traducir("badge_en_uso");
        }

        LinkButton lbDesactivar = e.Row.FindControl("lbDesactivar") as LinkButton;
        if (lbDesactivar != null) lbDesactivar.Text = "🚫 " + Traducir("btn_desactivar");

        LinkButton lbActivar = e.Row.FindControl("lbActivar") as LinkButton;
        if (lbActivar != null) lbActivar.Text = "✅ " + Traducir("btn_activar");

        LinkButton lbVerPendientes = e.Row.FindControl("lbVerPendientes") as LinkButton;
        if (lbVerPendientes != null) lbVerPendientes.Text = "✏️ " + Traducir("btn_traducciones");
    }

    protected void gvTraducciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "GuardarTraduccion") return;

        int idTraduccion = Convert.ToInt32(e.CommandArgument);

        GridViewRow fila = ((Control)e.CommandSource).NamingContainer as GridViewRow;
        TextBox txtTexto = (TextBox)fila.FindControl("txtTexto");

        GestorIdioma gestorIdioma = new GestorIdioma();

        try
        {
            gestorIdioma.ModificarTraduccion(idTraduccion, txtTexto.Text);
            MostrarExito(Traducir("msg_traduccion_actualizada"));
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }

        pnlTraducciones.Visible = true;
        lblIdiomaSeleccionado.Text = IdiomaSeleccionado;
        CargarTraducciones(IdiomaSeleccionado);
    }

    protected void gvTraducciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        Traduccion traduccion = e.Row.DataItem as Traduccion;
        if (traduccion == null) return;

        Label lblEstadoTraduccion = e.Row.FindControl("lblEstadoTraduccion") as Label;
        if (lblEstadoTraduccion != null)
        {
            lblEstadoTraduccion.Text = traduccion.Pendiente ? Traducir("estado_pendiente") : Traducir("estado_revisado");
            lblEstadoTraduccion.CssClass = traduccion.Pendiente ? "badge-estado bloqueado" : "badge-estado activo";
        }

        LinkButton lbGuardarTraduccion = e.Row.FindControl("lbGuardarTraduccion") as LinkButton;
        if (lbGuardarTraduccion != null) lbGuardarTraduccion.Text = "💾 " + Traducir("btn_guardar");
    }

    private void CargarIdiomas()
    {
        GestorIdioma gestorIdioma = new GestorIdioma();
        List<Idioma> idiomas = gestorIdioma.ObtenerTodos();

        gvIdiomas.DataSource = idiomas;
        gvIdiomas.DataBind();
    }

    private void CargarTraducciones(string nombreIdioma)
    {
        if (string.IsNullOrEmpty(nombreIdioma)) return;

        GestorIdioma gestorIdioma = new GestorIdioma();
        List<Traduccion> traducciones = gestorIdioma.ObtenerTraduccionesDe(nombreIdioma);

        gvTraducciones.DataSource = traducciones;
        gvTraducciones.DataBind();
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