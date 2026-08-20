<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormDigitoVerificador.aspx.cs" Inherits="FormDigitoVerificador" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Integridad del sistema</title>
    <link href="EstilosPaginas/Shared.css"               rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormDigitoVerificador.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Panel Técnico</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormMenuWebMaster.aspx"      class="nav-item">🏠 Menú</a>
                <a href="FormMaestroProfesional.aspx" class="nav-item">👥 Profesionales</a>
                <a href="FormGestionIdiomas.aspx"      class="nav-item">🌐 Idiomas</a>
                <a href="FormDigitoVerificador.aspx"   class="nav-item active">🔐 Integridad</a>
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
                    <span class="header-page">Integridad del sistema</span>
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

                <asp:Label ID="lblMensaje" runat="server" Visible="false" />

                <asp:Panel ID="pnlSinInconsistencias" runat="server" CssClass="estado-card estado-ok" Visible="false">
                    <span class="estado-icono">✅</span>
                    <div>
                        <div class="estado-titulo">Sin inconsistencias detectadas</div>
                        <div class="estado-sub">Todos los dígitos verificadores (DVH/DVV) y conteos de registros coinciden con lo esperado.</div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlConInconsistencias" runat="server" CssClass="content-card" Visible="false">
                    <div class="card-header">
                        <h2 class="card-title">⚠️ Inconsistencias detectadas</h2>
                        <p class="card-subtitle">Se detectaron cambios en la base de datos que no coinciden con lo registrado por el sistema.</p>
                    </div>

                    <ul class="lista-inconsistencias">
                        <asp:Repeater ID="rptInconsistencias" runat="server">
                            <ItemTemplate>
                                <li><%# Container.DataItem %></li>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ul>
                </asp:Panel>

                <div class="content-card mt-24">
                    <div class="card-header">
                        <h2 class="card-title">Acciones disponibles</h2>
                        <p class="card-subtitle">Elegí cómo proceder ante lo detectado (o como chequeo de rutina, aunque no haya inconsistencias).</p>
                    </div>

                    <div class="acciones-grid">

                        <a class="accion-tile" href="FormBackupRestore.aspx">
                            <div class="accion-icono">💾</div>
                            <div class="accion-titulo">Realizar backup</div>
                            <div class="accion-desc">Genera un respaldo del estado actual de la base de datos.</div>
                        </a>

                        <a class="accion-tile" href="FormBackupRestore.aspx">
                            <div class="accion-icono">♻️</div>
                            <div class="accion-titulo">Restaurar backup</div>
                            <div class="accion-desc">Revierte la base de datos a un respaldo anterior conocido como válido.</div>
                        </a>

                        <div class="accion-tile accion-tile-peligro">
                            <div class="accion-icono">🔁</div>
                            <div class="accion-titulo">Recalcular dígitos verificadores</div>
                            <div class="accion-desc">Asume el estado actual de la base como válido y recalcula todos los DVH/DVV/CR desde cero. Usar solo después de confirmar que los cambios detectados son legítimos.</div>
                            <asp:Button ID="btnRecalcular" runat="server"
                                Text="Recalcular y aceptar estado actual"
                                CssClass="btn-secondary btn-peligro"
                                OnClick="btnRecalcular_Click"
                                OnClientClick="return confirm('Esto va a asumir el estado ACTUAL de la base como correcto, incluyendo cualquier inconsistencia detectada. ¿Confirmás que ya investigaste el origen del cambio?');" />
                        </div>

                    </div>
                </div>

            </div>
        </div>

    </form>
</body>
</html>
