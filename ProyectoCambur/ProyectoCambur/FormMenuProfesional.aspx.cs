using BE;
using SERVICIOS;
using System;

public partial class FormMenuProfesional : GUI.PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

     
        if (psicologoActual.RolPermiso == "Administrador" || psicologoActual.RolPermiso == "Web Master")
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        AplicarTraducciones(psicologoActual);
    }

    private void AplicarTraducciones(Psicologo psicologoActual)
    {
        lblTaglineSidebar.Text = Traducir("tagline_gestion_clinica");
        lblMenuInicio.Text = Traducir("menu_inicio");
        lblMenuDashboard.Text = Traducir("menu_dashboard");
        lblMenuPacientes.Text = Traducir("menu_pacientes");
        lblMenuSuscripcion.Text = Traducir("menu_mi_suscripcion");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");

        lblHeaderSeccion.Text = Traducir("header_inicio");
        lblHeaderPagina.Text = Traducir("header_menu_principal");

        lblBienvenida.Text = string.Format(Traducir("msg_bienvenida"), psicologoActual.Nombre);
        lblBannerSub.Text = Traducir("banner_profesional_sub");

        lblTileDashboardTitulo.Text = Traducir("menu_dashboard");
        lblTileDashboardDesc.Text = Traducir("tile_dashboard_desc");
        lblTilePacientesTitulo.Text = Traducir("menu_pacientes");
        lblTilePacientesDesc.Text = Traducir("tile_pacientes_desc");
        lblTileSuscripcionTitulo.Text = Traducir("menu_mi_suscripcion");
        lblTileSuscripcionDesc.Text = Traducir("tile_suscripcion_desc");
    }
}