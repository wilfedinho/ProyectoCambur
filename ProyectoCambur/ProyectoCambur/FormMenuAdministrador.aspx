<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormMenuAdministrador.aspx.cs" Inherits="FormMenuAdministrador" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Menú Administrador</title>
    <link href="EstilosPaginas/Shared.css"            rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"     rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css" rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/MenuRoles.css"         rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="inicio" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <asp:Label ID="lblHeaderSeccion" runat="server" CssClass="header-section" Text="" />
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderPagina" runat="server" CssClass="header-page" Text="" />
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <div class="rol-banner">
                    <div>
                        <asp:Label ID="lblBannerTitulo" runat="server" CssClass="rol-banner-titulo" Text="" />
                        <asp:Label ID="lblBannerSub" runat="server" CssClass="rol-banner-sub" Text="" />
                    </div>
                </div>

                <div class="menu-tile-grid">

                    <a class="menu-tile" href="FormMaestroProfesional.aspx">
                        <div class="menu-tile-icono">👥</div>
                        <asp:Label ID="lblTileProfesionalesTitulo" runat="server" CssClass="menu-tile-titulo" Text="" />
                        <asp:Label ID="lblTileProfesionalesDesc" runat="server" CssClass="menu-tile-desc" Text="" />
                    </a>

                    <a class="menu-tile" href="FormGestionIdiomas.aspx">
                        <div class="menu-tile-icono">🌐</div>
                        <asp:Label ID="lblTileIdiomasTitulo" runat="server" CssClass="menu-tile-titulo" Text="" />
                        <asp:Label ID="lblTileIdiomasDesc" runat="server" CssClass="menu-tile-desc" Text="" />
                    </a>

                </div>

            </div>
        </div>

    </form>
</body>
</html>
