<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormDigitoVerificador.aspx.cs" Inherits="FormDigitoVerificador" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Dígito Verificador</title>
    <link href="EstilosPaginas/Shared.css"                  rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormDigitoVerificador.css"   rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

     
        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline-admin">Panel Web Master</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormDashboard.aspx"          class="nav-item">🏠 Dashboard</a>
                <a href="FormAuditoriaBitacora.aspx"  class="nav-item">📜 Bitácora</a>
                <a href="FormBackupRestore.aspx"      class="nav-item">💾 Backup / Restore</a>
                <a href="FormDigitoVerificador.aspx"  class="nav-item active">🔢 Dígito Verificador</a>
                <a href="FormGestionIdiomas.aspx"     class="nav-item">🌐 Gestionar Idiomas</a>
                <a href="FormABMProfesionales.aspx"   class="nav-item">👤 ABM Profesionales</a>
                <a href="FormABMPacientes.aspx"       class="nav-item">🧑‍⚕️ ABM Pacientes</a>
                <a href="FormABMConsultas.aspx"       class="nav-item">🗒️ ABM Consultas</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>


        <div class="main-wrap">

       
            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Administración</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Dígito Verificador</span>
                </div>
                <div class="header-user">
                    <div class="user-avatar user-avatar-admin">
                        <asp:Label ID="lblIniciales" runat="server" Text="AD" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreAdmin" runat="server" CssClass="user-name" Text="" />
                        <span class="user-role admin-role">Web Master</span>
                    </div>
                </div>
            </header>

     
            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <div class="dv-layout">

                
                    <div class="dv-main">

                
                        <asp:Panel ID="pnlInicial" runat="server" CssClass="content-card">

                            <div class="card-header">
                                <h2 class="card-title">Verificar integridad de datos</h2>
                                <p class="card-subtitle">
                                    Recorre todas las tablas del sistema, recalcula los dígitos verificadores
                                    horizontales y verticales, y reporta cualquier inconsistencia detectada.
                                </p>
                            </div>

                            <div class="section-sep">Tablas a verificar</div>

                            <div class="tablas-grid">
                                <asp:Repeater ID="rptTablas" runat="server">
                                    <ItemTemplate>
                                        <div class="tabla-chip">
                                            <span class="tabla-icono">🗄</span>
                                            <span class="tabla-nombre"><%# Eval("Nombre") %></span>
                                            <span class="tabla-registros"><%# Eval("Registros") %> reg.</span>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                            <div class="dv-aviso">
                                <span class="dv-aviso-icono">⏱️</span>
                                <p class="dv-aviso-texto">
                                    El proceso recorre todas las tablas de forma secuencial. En bases de datos con
                                    gran volumen puede tardar varios segundos. No cerrés ni recargues la página
                                    mientras el proceso está en ejecución.
                                </p>
                            </div>

                            <div class="form-actions">
                                <asp:Button ID="btnRecalcular" runat="server"
                                    Text="🔢 Iniciar verificación"
                                    CssClass="btn-primary btn-dv"
                                    OnClick="btnRecalcular_Click"
                                    CausesValidation="false"
                                    OnClientClick="mostrarProcesando(); return true;" />
                            </div>

                        </asp:Panel>

                     
                        <div class="content-card procesando-card" id="procesandoCard" style="display:none;">
                            <div class="procesando-spinner"></div>
                            <p class="procesando-titulo">Verificando integridad...</p>
                            <p class="procesando-sub">Recorriendo tablas y recalculando dígitos verificadores</p>
                        </div>

                 
                        <asp:Panel ID="pnlResultadoOk" runat="server"
                            CssClass="content-card resultado-ok-card" Visible="false">
                            <div class="resultado-ok-icono">✓</div>
                            <h2 class="resultado-ok-titulo">Verificación completada</h2>
                            <p class="resultado-ok-sub">No se detectaron inconsistencias en ninguna tabla del sistema.</p>
                            <div class="resultado-ok-stats">
                                <div class="res-stat">
                                    <asp:Label ID="lblStatTablas" runat="server" CssClass="res-stat-num" Text="0" />
                                    <span class="res-stat-label">Tablas verificadas</span>
                                </div>
                                <div class="res-stat">
                                    <asp:Label ID="lblStatRegistros" runat="server" CssClass="res-stat-num" Text="0" />
                                    <span class="res-stat-label">Registros analizados</span>
                                </div>
                                <div class="res-stat">
                                    <asp:Label ID="lblStatTiempo" runat="server" CssClass="res-stat-num" Text="0s" />
                                    <span class="res-stat-label">Tiempo de ejecución</span>
                                </div>
                            </div>
                            <asp:Button ID="btnNuevaVerif" runat="server"
                                Text="Realizar nueva verificación"
                                CssClass="btn-secondary"
                                OnClick="btnNuevaVerificacion_Click"
                                CausesValidation="false" />
                        </asp:Panel>

                
                        <asp:Panel ID="pnlResultadoError" runat="server"
                            CssClass="content-card resultado-error-card" Visible="false">

                            <div class="resultado-error-header">
                                <div class="resultado-error-icono">⚠️</div>
                                <div>
                                    <h2 class="resultado-error-titulo">Inconsistencias detectadas</h2>
                                    <asp:Label ID="lblResumenError" runat="server"
                                        CssClass="resultado-error-sub" Text="" />
                                </div>
                            </div>

                            <div class="section-sep">Detalle de inconsistencias</div>

                            <div class="table-wrap">
                                <asp:GridView ID="gvInconsistencias" runat="server"
                                    CssClass="data-table"
                                    AutoGenerateColumns="false"
                                    GridLines="None">
                                    <HeaderStyle CssClass="table-header" />
                                    <RowStyle    CssClass="table-row" />
                                    <AlternatingRowStyle CssClass="table-row table-row-alt" />
                                    <Columns>
                                        <asp:BoundField DataField="Tabla"
                                            HeaderText="Tabla"
                                            HeaderStyle-CssClass="th-left" />
                                        <asp:TemplateField HeaderText="Tipo"
                                            HeaderStyle-CssClass="th-centro"
                                            ItemStyle-CssClass="td-centro">
                                            <ItemTemplate>
                                                <span class='<%# "badge-tipo-inc tipo-" + Eval("TipoCss") %>'>
                                                    <%# Eval("Tipo") %>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IdRegistro"
                                            HeaderText="ID Registro"
                                            HeaderStyle-CssClass="th-centro"
                                            ItemStyle-CssClass="td-centro" />
                                        <asp:BoundField DataField="Detalle"
                                            HeaderText="Detalle"
                                            HeaderStyle-CssClass="th-left" />
                                    </Columns>
                                </asp:GridView>
                            </div>

                            <div class="error-actions">
                                <asp:Button ID="btnNuevaVerif2" runat="server"
                                    Text="Repetir verificación"
                                    CssClass="btn-secondary"
                                    OnClick="btnNuevaVerificacion_Click"
                                    CausesValidation="false" />
                                <a href="FormAuditoriaBitacora.aspx" class="btn-primary">
                                    Ver en bitácora
                                </a>
                            </div>

                        </asp:Panel>

                    </div>

            
                    <div class="dv-aside">

                        <div class="content-card dv-info-card">
                            <p class="accesos-titulo">¿Qué verifica?</p>
                            <div class="dv-info-item">
                                <span class="dv-info-icono">↔</span>
                                <div>
                                    <div class="dv-info-titulo">Dígito horizontal</div>
                                    <div class="dv-info-desc">Verifica que cada registro no fue modificado fuera del sistema.</div>
                                </div>
                            </div>
                            <div class="dv-info-item">
                                <span class="dv-info-icono">↕</span>
                                <div>
                                    <div class="dv-info-titulo">Dígito vertical</div>
                                    <div class="dv-info-desc">Detecta inserciones o eliminaciones no autorizadas a nivel de tabla.</div>
                                </div>
                            </div>
                        </div>

                        <div class="content-card dv-historial-card">
                            <p class="accesos-titulo">Últimas verificaciones</p>
                            <asp:Repeater ID="rptHistorialDV" runat="server">
                                <ItemTemplate>
                                    <div class="dv-hist-item">
                                        <span class='<%# "dv-hist-icono " + Eval("ResultadoCss") %>'>
                                            <%# Eval("ResultadoIcono") %>
                                        </span>
                                        <div class="dv-hist-info">
                                            <span class="dv-hist-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy HH:mm}") %></span>
                                            <span class="dv-hist-res"><%# Eval("Resultado") %></span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo">Algoritmo SHA-256</p>
                            <p class="aviso-texto">Los dígitos verificadores se calculan con SHA-256 sobre los campos de cada registro, garantizando detección de cualquier alteración.</p>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </form>

    <script type="text/javascript">
        function mostrarProcesando() {
            var card = document.getElementById('procesandoCard');
            if (card) card.style.display = 'flex';
        }
        window.addEventListener('DOMContentLoaded', function () {
            var card = document.getElementById('procesandoCard');
            if (card) card.style.display = 'none';
        });
    </script>
</body>
</html>
