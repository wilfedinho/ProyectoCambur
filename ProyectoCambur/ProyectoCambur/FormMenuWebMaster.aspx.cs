using BE;
using SERVICIOS;
using System;

public partial class FormMenuWebMaster : GUI.PaginaBase
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
    }

    private void AplicarTraducciones()
    {
        lblTaglineSidebar.Text = Traducir("tagline_panel_tecnico");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");

        lblHeaderSeccion.Text = Traducir("header_web_master");
        lblHeaderPagina.Text = Traducir("header_menu_principal");

        lblBannerTitulo.Text = Traducir("banner_webmaster_titulo");
        lblBannerSub.Text = Traducir("banner_webmaster_sub");

        lblTileIntegridadTitulo.Text = Traducir("menu_integridad");
        lblTileIntegridadDesc.Text = Traducir("tile_integridad_desc");
        lblTileBackupTitulo.Text = Traducir("menu_backup_restore");
        lblTileBackupDesc.Text = Traducir("tile_backup_desc");
        lblTileBitacoraTitulo.Text = Traducir("menu_bitacora");
        lblTileBitacoraDesc.Text = Traducir("tile_bitacora_desc");
    }
}