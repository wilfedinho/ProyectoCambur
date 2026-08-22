using BE;
using SERVICIOS;
using System;

public partial class FormMenuAdministrador : GUI.PaginaBase
{
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
        lblHeaderPagina.Text = Traducir("header_menu_principal");

        lblBannerTitulo.Text = Traducir("banner_admin_titulo");
        lblBannerSub.Text = Traducir("banner_admin_sub");

        lblTileProfesionalesTitulo.Text = Traducir("menu_profesionales");
        lblTileProfesionalesDesc.Text = Traducir("tile_profesionales_desc");
        lblTileIdiomasTitulo.Text = Traducir("menu_idiomas");
        lblTileIdiomasDesc.Text = Traducir("tile_idiomas_desc");
        lblTileBitacoraTitulo.Text = Traducir("menu_bitacora");
        lblTileBitacoraDesc.Text = Traducir("tile_bitacora_desc");
    }
}