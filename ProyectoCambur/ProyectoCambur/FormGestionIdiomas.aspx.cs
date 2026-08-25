using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using GUI;

public partial class FormGestionIdiomas : PaginaBase
{
    private class IdiomaCandidato
    {
        public string Nombre;
        public string CodigoIso;
        public IdiomaCandidato(string nombre, string codigoIso) { Nombre = nombre; CodigoIso = codigoIso; }
    }

    private static readonly List<IdiomaCandidato> CatalogoIdiomas = new List<IdiomaCandidato>
    {
        new IdiomaCandidato("English", "en"),
        new IdiomaCandidato("Français", "fr"),
        new IdiomaCandidato("Português", "pt"),
        new IdiomaCandidato("Deutsch (Alemán)", "de"),
        new IdiomaCandidato("Italiano", "it"),
        new IdiomaCandidato("中文 (Chino)", "zh"),
        new IdiomaCandidato("日本語 (Japonés)", "ja"),
        new IdiomaCandidato("Русский (Ruso)", "ru"),
        new IdiomaCandidato("العربية (Árabe)", "ar"),
    };

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
        if (!new GestorPermiso().TienePermiso(psicologoActual.RolPermiso, "acceder_gestionar_idiomas"))
        {
            DenegarAcceso();
            return;
        }
        AplicarTraducciones();
        if (!IsPostBack)
        {
            CargarComboIdiomasCandidatos();
            CargarIdiomas();
        }
    }

    private void AplicarTraducciones()
    {
        lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("header_administrador");
        lblHeaderPagina.Text = Traducir("menu_idiomas");
        lblTituloNuevoIdioma.Text = Traducir("titulo_nuevo_idioma");
        lblSubtituloNuevoIdioma.Text = Traducir("subtitulo_nuevo_idioma");
        lblEtiquetaNuevoIdioma.Text = Traducir("lbl_nuevo_idioma");
        rfvNuevoIdioma.ErrorMessage = Traducir("error_idioma_obligatorio_alta");
        lblHintNuevoIdioma.Text = Traducir("hint_nuevo_idioma");
        btnAltaIdioma.Text = Traducir("btn_generar_idioma");
        lblOverlayTitulo.Text = Traducir("overlay_generando_titulo");
        lblOverlaySub.Text = Traducir("overlay_generando_sub");
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

    private void CargarComboIdiomasCandidatos()
    {
        GestorIdioma gestorIdioma = new GestorIdioma();
        List<string> nombresYaRegistrados = gestorIdioma.ObtenerTodos().Select(i => i.NombreIdioma).ToList();
        ddlNuevoIdioma.Items.Clear();
        ddlNuevoIdioma.Items.Add(new ListItem(Traducir("opt_seleccionar"), ""));
        foreach (IdiomaCandidato candidato in CatalogoIdiomas)
        {
            if (!nombresYaRegistrados.Contains(candidato.Nombre))
            {
                ddlNuevoIdioma.Items.Add(new ListItem(
                    candidato.Nombre + " — " + candidato.CodigoIso.ToUpper(),
                    candidato.Nombre + "|" + candidato.CodigoIso
                ));
            }
        }
    }

    protected void btnAltaIdioma_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        string[] partes = ddlNuevoIdioma.SelectedValue.Split('|');
        Idioma nuevoIdioma = new Idioma(partes[0], partes[1], true, false);

        GestorIdioma gestorIdioma = new GestorIdioma();
        try
        {
            gestorIdioma.Alta(nuevoIdioma);
            MostrarExito(string.Format(Traducir("msg_idioma_generado"), nuevoIdioma.NombreIdioma));
            CargarComboIdiomasCandidatos();
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
                gvTraducciones.PageIndex = 0;
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
            RefrescarTraducciones();
            AplicarTraducciones();
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

    protected void gvTraducciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvTraducciones.PageIndex = e.NewPageIndex;
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