<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormMenuWebMaster.aspx.cs" Inherits="FormMenuWebMaster" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Menú Web Master</title>
    <link href="EstilosPaginas/Shared.css"    rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/MenuRoles.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Panel Técnico</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormMenuWebMaster.aspx"      class="nav-item active">🏠 Menú</a>
                <a href="FormMaestroProfesional.aspx" class="nav-item">👥 Profesionales</a>
                <a href="FormGestionIdiomas.aspx"      class="nav-item">🌐 Idiomas</a>
                <a href="FormDigitoVerificador.aspx"   class="nav-item">🔐 Integridad</a>
                <a href="FormBackupRestore.aspx"       class="nav-item">💾 Backup / Restore</a>
                <a href="FormAuditoriaBitacora.aspx"   class="nav-item">📜 Bitácora</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Web Master</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Menú principal</span>
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server" CssClass="user-name" Text="" />
                        <span class="user-role">Web Master</span>
                    </div>
                </div>
            </header>

            <div class="page-content">

                <div class="rol-banner">
                    <div>
                        <div class="rol-banner-titulo">Panel técnico del sistema</div>
                        <div class="rol-banner-sub">Acceso completo a integridad, respaldo y auditoría</div>
                    </div>
                </div>

                <div class="menu-tile-grid">

                    <a class="menu-tile" href="FormMaestroProfesional.aspx">
                        <div class="menu-tile-icono">👥</div>
                        <div class="menu-tile-titulo">Profesionales</div>
                        <div class="menu-tile-desc">Alta, baja, modificación, habilitación y desbloqueo de cuentas de profesionales.</div>
                    </a>

                    <a class="menu-tile" href="FormGestionIdiomas.aspx">
                        <div class="menu-tile-icono">🌐</div>
                        <div class="menu-tile-titulo">Idiomas</div>
                        <div class="menu-tile-desc">Administrar los idiomas disponibles en el sistema.</div>
                    </a>

                    <a class="menu-tile" href="FormDigitoVerificador.aspx">
                        <div class="menu-tile-icono">🔐</div>
                        <div class="menu-tile-titulo">Integridad del sistema</div>
                        <div class="menu-tile-desc">Verificar el dígito verificador de las tablas críticas.</div>
                    </a>

                    <a class="menu-tile" href="FormBackupRestore.aspx">
                        <div class="menu-tile-icono">💾</div>
                        <div class="menu-tile-titulo">Backup / Restore</div>
                        <div class="menu-tile-desc">Respaldar o restaurar la base de datos del sistema.</div>
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
