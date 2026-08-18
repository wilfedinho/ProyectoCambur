<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormMenuProfesional.aspx.cs" Inherits="FormMenuProfesional" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Menú</title>
    <link href="EstilosPaginas/Shared.css"    rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/MenuRoles.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Gestión Clínica</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormMenuProfesional.aspx"   class="nav-item active">🏠 Menú</a>
                <a href="FormDashboard.aspx"         class="nav-item">📊 Dashboard</a>
                <a href="FormRegistrarPaciente.aspx" class="nav-item">👤 Pacientes</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormSuscripcion.aspx" class="nav-item">💳 Mi Suscripción</a>
                <a href="FormLogout.aspx"      class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Inicio</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Menú principal</span>
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server" CssClass="user-name" Text="" />
                        <asp:Label ID="lblPlanActual" runat="server" CssClass="user-role" Text="" />
                    </div>
                </div>
            </header>

            <div class="page-content">

                <div class="rol-banner">
                    <div>
                        <asp:Label ID="lblBienvenida" runat="server" CssClass="rol-banner-titulo" Text="" />
                        <div class="rol-banner-sub">Tu espacio de trabajo clínico</div>
                    </div>
                </div>

                <div class="menu-tile-grid">

                    <a class="menu-tile" href="FormDashboard.aspx">
                        <div class="menu-tile-icono">📊</div>
                        <div class="menu-tile-titulo">Dashboard</div>
                        <div class="menu-tile-desc">Vista general de tu actividad clínica: pacientes, consultas y métricas.</div>
                    </a>

                    <a class="menu-tile" href="FormRegistrarPaciente.aspx">
                        <div class="menu-tile-icono">👤</div>
                        <div class="menu-tile-titulo">Pacientes</div>
                        <div class="menu-tile-desc">Registrar y gestionar los pacientes de tu entorno clínico.</div>
                    </a>

                    <a class="menu-tile" href="FormSuscripcion.aspx">
                        <div class="menu-tile-icono">💳</div>
                        <div class="menu-tile-titulo">Mi Suscripción</div>
                        <div class="menu-tile-desc">Ver o cambiar tu plan actual.</div>
                    </a>

                </div>

            </div>
        </div>

    </form>
</body>
</html>
