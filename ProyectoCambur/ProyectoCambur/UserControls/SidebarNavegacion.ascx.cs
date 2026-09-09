using BE;
using GUI;
using SERVICIOS;
using System;
using System.Text;
using System.Web;
public partial class SidebarNavegacion : System.Web.UI.UserControl
{
    public string PaginaActual { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        RenderizarNavegacion();
    }
    public void RenderizarNavegacion()
    {
        if (!GestorSesion.EstaAutenticado) return;
        PaginaBase paginaBase = Page as PaginaBase;
        string rolPermiso = GestorSesion.PsicologoActual.RolPermiso;
        GestorPermiso gestorPermiso = new GestorPermiso();
        PermisoCompuesto perfil = gestorPermiso.LeerPerfilConEstructura(rolPermiso);
        StringBuilder html = new StringBuilder();
        AgregarItem(html, paginaBase, "menu_inicio", "🏠", "FormMenu.aspx", "inicio");
        if (perfil != null)
        {
            RenderizarHijos(html, paginaBase, perfil);
        }
        litNav.Text = html.ToString();
    }

    private void RenderizarHijos(StringBuilder html, PaginaBase paginaBase, PermisoCompuesto nodo)
    {
        foreach (Permiso hijo in nodo.ObtenerHijos())
        {
            if (hijo is PermisoCompuesto familia)
            {
                string claveSeccion = GUI.CatalogoNavegacion.ObtenerClaveSeccion(familia.ObtenerNombre());
                string tituloSeccion = claveSeccion != null && paginaBase != null
                    ? paginaBase.Traducir(claveSeccion)
                    : familia.ObtenerNombre();
                html.Append("<div class=\"nav-seccion-titulo\">").Append(HttpUtility.HtmlEncode(tituloSeccion)).Append("</div>");
                RenderizarHijos(html, paginaBase, familia);
            }
            else if (hijo is PermisoSimple simple)
            {
                GUI.ItemMenu info = GUI.CatalogoNavegacion.ObtenerInfo(simple.ObtenerNombre());
                if (info != null)
                {
                    AgregarItem(html, paginaBase, info.ClaveTitulo, info.Icono, info.Url, simple.ObtenerNombre());
                }
            }
        }
    }

    private void AgregarItem(StringBuilder html, GUI.PaginaBase paginaBase, string clave, string icono, string url, string clavePagina)
    {
        string texto = paginaBase != null ? paginaBase.Traducir(clave) : clave;
        string cssActivo = (PaginaActual == clavePagina) ? " active" : "";
        html.Append("<a href=\"").Append(HttpUtility.HtmlEncode(url)).Append("\" class=\"nav-item").Append(cssActivo).Append("\">")
            .Append("<span>").Append(icono).Append("</span> ")
            .Append(HttpUtility.HtmlEncode(texto))
            .Append("</a>");
    }
}