using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormGestionPermisos : GUI.PaginaBase
{
    private GestorPermiso GestorPermiso
    {
        get { return new GestorPermiso(); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }
        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        if (!GestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_gestionar_permisos"))
        {
            DenegarAcceso();
            return;
        }
        AplicarTraducciones();
        if (!IsPostBack)
        {
            lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
            CargarComboTipoYElemento();
        }
    }

    private void AplicarTraducciones()
    {
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("header_administrador");
        lblHeaderPagina.Text = Traducir("nav_gestionar_permisos");
        lblTituloAltas.Text = Traducir("titulo_altas_permisos");
        lblSubtituloAltas.Text = Traducir("subtitulo_altas_permisos");
        lblEtiquetaNuevaFamilia.Text = Traducir("lbl_nueva_familia");
        btnAltaFamilia.Text = Traducir("btn_agregar");
        lblEtiquetaNuevoPerfil.Text = Traducir("lbl_nuevo_perfil");
        btnAltaPerfil.Text = Traducir("btn_agregar");
        lblTituloEstructura.Text = Traducir("titulo_gestionar_estructura");
        lblSubtituloEstructura.Text = Traducir("subtitulo_gestionar_estructura");
        lblEtiquetaTipo.Text = Traducir("lbl_tipo");
        lblEtiquetaElemento.Text = Traducir("lbl_elemento");
        ddlTipoElemento.Items.FindByValue("Perfil").Text = Traducir("opt_perfil_rol");
        ddlTipoElemento.Items.FindByValue("Familia").Text = Traducir("opt_familia");
        btnBorrarElementoSeleccionado.Text = "🗑️ " + Traducir("btn_borrar_seleccionado");
        lblTituloHijosDirectos.Text = Traducir("titulo_hijos_directos");
        lblSubtituloHijosDirectos.Text = Traducir("subtitulo_hijos_directos");
        gvHijosDirectos.Columns[0].HeaderText = Traducir("col_nombre");
        gvHijosDirectos.Columns[1].HeaderText = Traducir("col_tipo");
        gvHijosDirectos.EmptyDataText = Traducir("empty_hijos_directos");
        lblEtiquetaAgregar.Text = Traducir("lbl_agregar_elemento");
        btnAgregarElemento.Text = Traducir("btn_agregar");
        lblTituloArbolCompleto.Text = Traducir("titulo_arbol_completo");
        lblSubtituloArbolCompleto.Text = Traducir("subtitulo_arbol_completo");
    }
    protected void btnAltaFamilia_Click(object sender, EventArgs e)
    {
        try
        {
            GestorPermiso.AltaFamilia(txtNuevaFamilia.Text.Trim());
            MostrarExito(string.Format(Traducir("msg_familia_creada"), txtNuevaFamilia.Text.Trim()));
            txtNuevaFamilia.Text = string.Empty;
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }

        CargarComboTipoYElemento();
        RecargarEstructuraSeleccionada();
    }

    protected void btnAltaPerfil_Click(object sender, EventArgs e)
    {
        try
        {
            GestorPermiso.AltaPerfil(txtNuevoPerfil.Text.Trim());
            MostrarExito(string.Format(Traducir("msg_perfil_creado"), txtNuevoPerfil.Text.Trim()));
            txtNuevoPerfil.Text = string.Empty;
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }

        CargarComboTipoYElemento();
        RecargarEstructuraSeleccionada();
    }
    private void CargarComboTipoYElemento()
    {
        CargarComboElemento();
        RecargarEstructuraSeleccionada();
    }

    private void CargarComboElemento()
    {
        ddlElemento.Items.Clear();

        List<string> nombres = ddlTipoElemento.SelectedValue == "Perfil"
            ? GestorPermiso.ObtenerNombresPerfiles()
            : GestorPermiso.ObtenerNombresFamilias();

        foreach (string nombre in nombres.OrderBy(n => n))
        {
            ddlElemento.Items.Add(new ListItem(nombre, nombre));
        }
    }

    protected void ddlTipoElemento_SelectedIndexChanged(object sender, EventArgs e)
    {
        CargarComboElemento();
        RecargarEstructuraSeleccionada();
    }

    protected void ddlElemento_SelectedIndexChanged(object sender, EventArgs e)
    {
        RecargarEstructuraSeleccionada();
    }
    private void RecargarEstructuraSeleccionada()
    {
        if (ddlElemento.Items.Count == 0)
        {
            pnlEstructura.Visible = false;
            return;
        }

        bool esPerfil = ddlTipoElemento.SelectedValue == "Perfil";
        string nombreSeleccionado = ddlElemento.SelectedValue;

        PermisoCompuesto raiz = esPerfil
            ? GestorPermiso.LeerPerfilConEstructura(nombreSeleccionado)
            : GestorPermiso.LeerFamiliaConEstructura(nombreSeleccionado);

        if (raiz == null)
        {
            pnlEstructura.Visible = false;
            return;
        }

        pnlEstructura.Visible = true;
        lblNombreSeleccionado.Text = nombreSeleccionado;

        CargarGrillaHijosDirectos(raiz);
        CargarComboElementoParaAgregar(raiz, esPerfil);
        CargarArbolCompleto(raiz);
    }

    private class FilaHijo
    {
        public string Nombre { get; set; }
        public bool EsFamilia { get; set; }
    }

    private void CargarGrillaHijosDirectos(PermisoCompuesto raiz)
    {
        List<FilaHijo> filas = raiz.ObtenerHijos()
            .Select(h => new FilaHijo { Nombre = h.ObtenerNombre(), EsFamilia = h is PermisoCompuesto })
            .OrderBy(f => f.Nombre)
            .ToList();

        gvHijosDirectos.DataSource = filas;
        gvHijosDirectos.DataBind();
    }

    protected void gvHijosDirectos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        FilaHijo fila = e.Row.DataItem as FilaHijo;
        if (fila == null) return;

        Label lblTipoHijo = e.Row.FindControl("lblTipoHijo") as Label;
        if (lblTipoHijo != null)
        {
            lblTipoHijo.Text = fila.EsFamilia ? Traducir("opt_familia") : Traducir("lbl_permiso_simple");
        }

        LinkButton lbQuitar = e.Row.FindControl("lbQuitar") as LinkButton;
        if (lbQuitar != null)
        {
            lbQuitar.Text = "✕ " + Traducir("btn_quitar");
        }
    }
    private void CargarComboElementoParaAgregar(PermisoCompuesto raiz, bool esPerfil)
    {
        HashSet<string> yaIncluidosDirecto = new HashSet<string>(raiz.ObtenerHijos().Select(h => h.ObtenerNombre()));
        HashSet<string> permisosYaPresentes = new HashSet<string>(raiz.ObtenerTodosLosPermisosSimples().Select(p => p.ObtenerNombre()));

        ddlElementoParaAgregar.Items.Clear();

        foreach (string nombreSimple in GestorPermiso.ObtenerNombresPermisosSimples().OrderBy(n => n))
        {
            if (!permisosYaPresentes.Contains(nombreSimple))
            {
                ddlElementoParaAgregar.Items.Add(new ListItem(nombreSimple, nombreSimple));
            }
        }
        List<PermisoCompuesto> todasLasFamilias = GestorPermiso.ObtenerTodasLasFamilias();
        foreach (PermisoCompuesto familiaCandidata in todasLasFamilias.OrderBy(f => f.ObtenerNombre()))
        {
            if (familiaCandidata.ObtenerNombre() == raiz.ObtenerNombre()) continue;
            if (yaIncluidosDirecto.Contains(familiaCandidata.ObtenerNombre())) continue;

            bool generariaDuplicado = familiaCandidata.ObtenerTodosLosPermisosSimples()
                .Any(p => permisosYaPresentes.Contains(p.ObtenerNombre()));

            if (!generariaDuplicado)
            {
                ddlElementoParaAgregar.Items.Add(new ListItem(familiaCandidata.ObtenerNombre(), familiaCandidata.ObtenerNombre()));
            }
        }
    }

    private void CargarArbolCompleto(PermisoCompuesto raiz)
    {
        tvEstructuraCompleta.Nodes.Clear();
        tvEstructuraCompleta.Nodes.Add(ConstruirNodo(raiz));
        tvEstructuraCompleta.ExpandAll();
    }

    private TreeNode ConstruirNodo(Permiso permiso)
    {
        bool esCompuesto = permiso is PermisoCompuesto;
        string prefijo = esCompuesto ? "▣ " : "◇ ";
        TreeNode nodo = new TreeNode(prefijo + permiso.ObtenerNombre());
        nodo.SelectAction = TreeNodeSelectAction.None;

        if (esCompuesto)
        {
            foreach (Permiso hijo in ((PermisoCompuesto)permiso).ObtenerHijos())
            {
                nodo.ChildNodes.Add(ConstruirNodo(hijo));
            }
        }

        return nodo;
    }
    protected void btnAgregarElemento_Click(object sender, EventArgs e)
    {
        if (ddlElementoParaAgregar.Items.Count == 0) return;

        bool esPerfil = ddlTipoElemento.SelectedValue == "Perfil";
        string nombreSeleccionado = ddlElemento.SelectedValue;
        string nombreElemento = ddlElementoParaAgregar.SelectedValue;

        try
        {
            if (esPerfil)
            {
                GestorPermiso.AgregarElementoAPerfil(nombreSeleccionado, nombreElemento);
            }
            else
            {
                GestorPermiso.AgregarElementoAFamilia(nombreSeleccionado, nombreElemento);
            }

            MostrarExito(string.Format(Traducir("msg_elemento_agregado"), nombreElemento, nombreSeleccionado));
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }

        RecargarEstructuraSeleccionada();
    }

    protected void gvHijosDirectos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Quitar") return;

        string[] partes = e.CommandArgument.ToString().Split('|');
        string nombreElemento = partes[0];
        bool elementoEsFamilia = partes[1] == "True";

        bool esPerfil = ddlTipoElemento.SelectedValue == "Perfil";
        string nombreSeleccionado = ddlElemento.SelectedValue;

        if (esPerfil)
        {
            GestorPermiso.QuitarElementoDePerfil(nombreSeleccionado, nombreElemento, elementoEsFamilia);
        }
        else
        {
            GestorPermiso.QuitarElementoDeFamilia(nombreSeleccionado, nombreElemento, elementoEsFamilia);
        }

        MostrarExito(string.Format(Traducir("msg_elemento_quitado"), nombreElemento, nombreSeleccionado));
        RecargarEstructuraSeleccionada();
    }
    protected void btnBorrarElementoSeleccionado_Click(object sender, EventArgs e)
    {
        if (ddlElemento.Items.Count == 0) return;

        bool esPerfil = ddlTipoElemento.SelectedValue == "Perfil";
        string nombreSeleccionado = ddlElemento.SelectedValue;

        try
        {
            if (esPerfil)
            {
                GestorPermiso.BorrarPerfil(nombreSeleccionado);
            }
            else
            {
                GestorPermiso.BorrarFamilia(nombreSeleccionado);
            }

            MostrarExito(string.Format(Traducir("msg_elemento_borrado"), nombreSeleccionado));
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }

        CargarComboTipoYElemento();
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