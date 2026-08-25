using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using GUI;
public partial class FormMenu : PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }
        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        AplicarTraducciones(psicologoActual);
        if (!IsPostBack)
        {
            GestorPermiso gestorPermiso = new GestorPermiso();
            PermisoCompuesto perfil = gestorPermiso.LeerPerfilConEstructura(psicologoActual.RolPermiso);
            if (perfil == null || perfil.ObtenerHijos().Count == 0)
            {
                pnlSinPermisos.Visible = true;
                return;
            }
            RenderizarNodo(perfil);
        }
    }

    private void AplicarTraducciones(Psicologo psicologoActual)
    {
        lblTaglineSidebar.Text = Traducir("tagline_gestion_clinica");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("header_inicio");
        lblHeaderPagina.Text = Traducir("header_menu_principal");
        lblBienvenida.Text = string.Format(Traducir("msg_bienvenida"), psicologoActual.Nombre);
        lblBannerSub.Text = psicologoActual.RolPermiso;
        lblSinPermisos.Text = Traducir("msg_sin_permisos_asignados");
    }
    private void RenderizarNodo(PermisoCompuesto nodo)
    {
        System.Collections.Generic.List<PermisoSimple> sueltos = new System.Collections.Generic.List<PermisoSimple>();

        foreach (Permiso hijo in nodo.ObtenerHijos())
        {
            if (hijo is PermisoCompuesto familia)
            {
                AgregarSeccionConTiles(familia.ObtenerNombre(), familia);
            }
            else if (hijo is PermisoSimple simple)
            {
                sueltos.Add(simple);
            }
        }
        if (sueltos.Count > 0)
        {
            AgregarSeccionSuelta(sueltos);
        }
    }

    private void AgregarSeccionConTiles(string nombreFamilia, PermisoCompuesto familia)
    {
        string claveSeccion = CatalogoNavegacion.ObtenerClaveSeccion(nombreFamilia);
        string titulo = claveSeccion != null ? Traducir(claveSeccion) : nombreFamilia;
        HtmlGenericControl divSeccion = new HtmlGenericControl("div");
        divSeccion.Attributes["class"] = "content-card mt-24";
        HtmlGenericControl header = new HtmlGenericControl("div");
        header.Attributes["class"] = "card-header";
        HtmlGenericControl h2 = new HtmlGenericControl("h2");
        h2.Attributes["class"] = "card-title";
        h2.InnerText = titulo;
        header.Controls.Add(h2);
        divSeccion.Controls.Add(header);
        HtmlGenericControl grid = new HtmlGenericControl("div");
        grid.Attributes["class"] = "menu-tile-grid";
        bool huboTiles = AgregarTilesDeSimples(grid, familia);
        divSeccion.Controls.Add(grid);
        if (huboTiles)
        {
            phSecciones.Controls.Add(divSeccion);
        }
        foreach (Permiso hijo in familia.ObtenerHijos())
        {
            if (hijo is PermisoCompuesto subFamilia)
            {
                AgregarSeccionConTiles(subFamilia.ObtenerNombre(), subFamilia);
            }
        }
    }

    private void AgregarSeccionSuelta(List<PermisoSimple> sueltos)
    {
        HtmlGenericControl divSeccion = new HtmlGenericControl("div");
        divSeccion.Attributes["class"] = "content-card mt-24";
        HtmlGenericControl grid = new HtmlGenericControl("div");
        grid.Attributes["class"] = "menu-tile-grid";
        bool huboTiles = false;
        foreach (PermisoSimple simple in sueltos)
        {
            if (AgregarTile(grid, simple))
            {
                huboTiles = true;
            }
        }
        divSeccion.Controls.Add(grid);
        if (huboTiles)
        {
            phSecciones.Controls.Add(divSeccion);
        }
    }

    private bool AgregarTilesDeSimples(HtmlGenericControl grid, PermisoCompuesto familia)
    {
        bool huboTiles = false;
        foreach (Permiso hijo in familia.ObtenerHijos())
        {
            if (hijo is PermisoSimple simple)
            {
                if (AgregarTile(grid, simple))
                {
                    huboTiles = true;
                }
            }
        }
        return huboTiles;
    }

    private bool AgregarTile(HtmlGenericControl grid, PermisoSimple simple)
    {
        GUI.ItemMenu info = GUI.CatalogoNavegacion.ObtenerInfo(simple.ObtenerNombre());
        if (info == null) return false;
        HtmlGenericControl tile = new HtmlGenericControl("a");
        tile.Attributes["class"] = "menu-tile";
        tile.Attributes["href"] = info.Url;
        HtmlGenericControl icono = new HtmlGenericControl("div");
        icono.Attributes["class"] = "menu-tile-icono";
        icono.InnerText = info.Icono;
        HtmlGenericControl titulo = new HtmlGenericControl("div");
        titulo.Attributes["class"] = "menu-tile-titulo";
        titulo.InnerText = Traducir(info.ClaveTitulo);
        HtmlGenericControl desc = new HtmlGenericControl("div");
        desc.Attributes["class"] = "menu-tile-desc";
        desc.InnerText = Traducir(info.ClaveDescripcion);
        tile.Controls.Add(icono);
        tile.Controls.Add(titulo);
        tile.Controls.Add(desc);
        grid.Controls.Add(tile);
        return true;
    }
}