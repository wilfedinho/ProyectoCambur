<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormMenuAdministrador.aspx.cs" Inherits="FormMenuAdministrador" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Menú Administrador</title>
    <link href="EstilosPaginas/Shared.css"    rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/MenuRoles.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Panel de Gestión</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormMenuAdministrador.aspx"  class="nav-item active">🏠 Menú</a>
                <a href="FormMaestroProfesional.aspx" class="nav-item">👥 Profesionales</a>
                <a href="FormAuditoriaBitacora.aspx"  class="nav-item">📜 Bitácora</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Administrador</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Menú principal</span>
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server" CssClass="user-name" Text="" />
                        <span class="user-role">Administrador</span>
                    </div>
                </div>
            </header>

            <div class="page-content">

                <div class="rol-banner">
                    <div>
                        <div class="rol-banner-titulo">Panel de administración</div>
                        <div class="rol-banner-sub">Gestión de profesionales y auditoría del sistema</div>
                    </div>
                </div>

                <div class="menu-tile-grid">

                    <a class="menu-tile" href="FormMaestroProfesional.aspx">
                        <div class="menu-tile-icono">👥</div>
                        <div class="menu-tile-titulo">Profesionales</div>
                        <div class="menu-tile-desc">Alta, baja, modificación, habilitación y desbloqueo de cuentas de profesionales.</div>
                    </a>

                    <a class="menu-tile" href="FormAuditoriaBitacora.aspx">
                        <div class="menu-tile-icono">📜</div>
                        <div class="menu-tile-titulo">Bitácora</div>
                        <div class="menu-tile-desc">Ver el registro de auditoría de eventos del sistema.</div>
                    </a>

                </div>

            </div>
        </div>

    </form>
</body>
</html>
