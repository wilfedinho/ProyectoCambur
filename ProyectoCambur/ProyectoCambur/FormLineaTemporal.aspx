<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormLineaTemporal.aspx.cs" Inherits="FormLineaTemporal" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Línea Temporal Clínica</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormLineaTemporal.css"   rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

      
        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Gestión Clínica</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormDashboard.aspx"         class="nav-item">🏠 Dashboard</a>
                <a href="FormRegistroPaciente.aspx"  class="nav-item">👤 Pacientes</a>
                <a href="FormRealizarConsulta.aspx"  class="nav-item">🗒️ Consultas</a>
                <a href="FormHistorialClinico.aspx"  class="nav-item">📋 Historial Clínico</a>
                <a href="FormResumenIA.aspx"         class="nav-item">🤖 Resumen IA</a>
                <a href="FormLineaTemporal.aspx"     class="nav-item active">📅 Línea Temporal</a>
                <a href="FormInformeDerivacion.aspx" class="nav-item">📤 Derivaciones</a>
                <a href="FormPerfilPaciente.aspx"    class="nav-item">🧠 Perfilación</a>
                <a href="FormExportarReporte.aspx"   class="nav-item">💾 Exportar</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormSuscripcion.aspx" class="nav-item">💳 Mi Suscripción</a>
                <a href="FormLogin.aspx"       class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

    
        <div class="main-wrap">

        
            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Pacientes</span>
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderPaciente" runat="server"
                        CssClass="header-page" Text="Línea Temporal" />
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="LM" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server"
                            CssClass="user-name" Text="" />
                        <span class="user-role">Psicólogo/a</span>
                    </div>
                </div>
            </header>

     
            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server"
                    Visible="false" CssClass="server-error" />

             
                <div class="temporal-layout">

                   
                    <div class="temporal-main">

                    
                        <div class="content-card">
                            <div class="paciente-header">
                                <div class="paciente-header-avatar">
                                    <asp:Label ID="lblPacienteIniciales"
                                        runat="server" Text="MG" />
                                </div>
                                <div class="paciente-header-info">
                                    <asp:Label ID="lblPacienteNombre" runat="server"
                                        CssClass="paciente-header-nombre" Text="" />
                                    <div class="paciente-header-meta">
                                        <asp:Label ID="lblPacienteEdad"
                                            runat="server" CssClass="meta-item" Text="" />
                                        <span class="meta-sep">·</span>
                                        <asp:Label ID="lblPacienteEstado"
                                            runat="server" CssClass="meta-item" Text="" />
                                        <span class="meta-sep">·</span>
                                        <asp:Label ID="lblPacienteOcup"
                                            runat="server" CssClass="meta-item" Text="" />
                                    </div>
                                </div>
                            </div>

                     
                            <div class="filtros-row">
                                <div class="filtros-tipos">
                                    <span class="filtro-label">Tipo:</span>
                                    <asp:Button ID="btnFiltroTodos" runat="server"
                                        Text="Todos" CssClass="filtro-btn active"
                                        OnClick="btnFiltro_Click"
                                        CommandArgument="TODOS"
                                        CausesValidation="false" />
                                    <asp:Button ID="btnFiltroConsulta" runat="server"
                                        Text="🗒️ Consulta" CssClass="filtro-btn"
                                        OnClick="btnFiltro_Click"
                                        CommandArgument="CONSULTA"
                                        CausesValidation="false" />
                                    <asp:Button ID="btnFiltroHistorial" runat="server"
                                        Text="📋 Historial" CssClass="filtro-btn"
                                        OnClick="btnFiltro_Click"
                                        CommandArgument="HISTORIAL"
                                        CausesValidation="false" />
                                    <asp:Button ID="btnFiltroEvento" runat="server"
                                        Text="⚡ Evento" CssClass="filtro-btn"
                                        OnClick="btnFiltro_Click"
                                        CommandArgument="EVENTO"
                                        CausesValidation="false" />
                                </div>
                                <div class="filtros-fechas">
                                    <asp:TextBox ID="txtDesde" runat="server"
                                        TextMode="Date" CssClass="filtro-input"
                                        ClientIDMode="Static" />
                                    <span class="filtro-sep">→</span>
                                    <asp:TextBox ID="txtHasta" runat="server"
                                        TextMode="Date" CssClass="filtro-input"
                                        ClientIDMode="Static" />
                                    <asp:Button ID="btnAplicarFecha" runat="server"
                                        Text="Filtrar" CssClass="btn-secondary btn-sm"
                                        OnClick="btnAplicarFecha_Click"
                                        CausesValidation="false" />
                                </div>
                            </div>

                            <div class="filtros-stats">
                                <asp:Label ID="lblTotalEventos" runat="server"
                                    CssClass="stats-total" Text="" />
                                <div class="stats-leyenda">
                                    <span class="leyenda-item consulta">● Consulta</span>
                                    <span class="leyenda-item historial">● Historial</span>
                                    <span class="leyenda-item evento">● Evento</span>
                                </div>
                            </div>
                        </div>

                     
                        <div class="content-card mt-24">

                
                            <asp:Label ID="lblSinRegistros" runat="server"
                                CssClass="timeline-vacio" Visible="false"
                                Text="No se encontraron registros clínicos para los filtros seleccionados." />

                       
                            <div class="timeline-wrap">
                                <div class="timeline-linea"></div>

                                <asp:Repeater ID="rptTimeline" runat="server"
                                    OnItemCommand="rptTimeline_ItemCommand">
                                    <ItemTemplate>

                                        <div class='<%# "timeline-item " + Eval("LadoCss") %>'>

                                   
                                            <div class='<%# "timeline-punto " + Eval("TipoCss") %>'>
                                                <span class="timeline-punto-icono">
                                                    <%# Eval("Icono") %>
                                                </span>
                                            </div>

                                      
                                            <div class='<%# "timeline-card " + Eval("TipoCss") + "-card" %>'>
                                                <div class="timeline-card-header">
                                                    <div class="tc-header-left">
                                                        <span class='<%# "tc-tipo-badge " + Eval("TipoCss") + "-badge" %>'>
                                                            <%# Eval("TipoLabel") %>
                                                        </span>
                                                        <span class="tc-fecha">
                                                            <%# Eval("Fecha", "{0:dd/MM/yyyy}") %>
                                                        </span>
                                                    </div>
                                                    <asp:LinkButton
                                                        ID="lbVerDetalle" runat="server"
                                                        CommandName="VerDetalle"
                                                        CommandArgument='<%# Eval("IdEvento") + "|" + Eval("Tipo") %>'
                                                        CssClass="tc-btn-ver"
                                                        Text="Ver detalle" />
                                                </div>
                                                <p class="tc-resumen"><%# Eval("Resumen") %></p>

                                             
                                                <div class="tc-detalle" id='<%# "detalle_" + Eval("IdEvento") %>'
                                                     style="display:none;">
                                                    <div class="tc-detalle-sep"></div>
                                                    <p class="tc-detalle-texto">
                                                        <%# Eval("Detalle") %>
                                                    </p>
                                                 
                                                    <asp:Panel ID="pnlDatosConsulta" runat="server"
                                                        Visible='<%# Eval("Tipo").ToString() == "CONSULTA" %>'
                                                        CssClass="tc-detalle-extra">
                                                        <span>⏱ <%# Eval("Duracion") %> min</span>
                                                        <span>·</span>
                                                        <span><%# Eval("Modalidad") %></span>
                                                    </asp:Panel>
                                                </div>
                                            </div>

                                        </div>

                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>

                        </div>
                    </div>

                   
                    <div class="temporal-aside">

                     
                        <div class="content-card stats-card">
                            <p class="accesos-titulo">Resumen del tratamiento</p>
                            <div class="stat-item">
                                <asp:Label ID="lblStatConsultas" runat="server"
                                    CssClass="stat-big" Text="0" />
                                <span class="stat-desc">Consultas totales</span>
                            </div>
                            <div class="stat-item">
                                <asp:Label ID="lblStatMeses" runat="server"
                                    CssClass="stat-big" Text="0" />
                                <span class="stat-desc">Meses en tratamiento</span>
                            </div>
                            <div class="stat-item">
                                <asp:Label ID="lblStatInicio" runat="server"
                                    CssClass="stat-fecha-val" Text="" />
                                <span class="stat-desc">Primera sesión</span>
                            </div>
                            <div class="stat-item" style="border-bottom:none;">
                                <asp:Label ID="lblStatUltima" runat="server"
                                    CssClass="stat-fecha-val" Text="" />
                                <span class="stat-desc">Última sesión</span>
                            </div>
                        </div>

                   
                        <div class="content-card accesos-card">
                            <p class="accesos-titulo">Acciones relacionadas</p>
                            <a href="FormRealizarConsulta.aspx"   class="acceso-item">🗒️ <span>Nueva consulta</span></a>
                            <a href="FormResumenIA.aspx"          class="acceso-item">🤖 <span>Resumen IA</span></a>
                            <a href="FormInformeDerivacion.aspx"  class="acceso-item">📤 <span>Generar derivación</span></a>
                            <a href="FormExportarReporte.aspx"    class="acceso-item">💾 <span>Exportar reporte</span></a>
                        </div>

                    </div>
                </div>
            </div>
        </div>

    </form>

    <script type="text/javascript">
       
        function toggleDetalle(idEvento, btn) {
            var panel = document.getElementById('detalle_' + idEvento);
            if (!panel) return;
            var abierto = panel.style.display !== 'none';
            panel.style.display = abierto ? 'none' : 'block';
            btn.textContent     = abierto ? 'Ver detalle' : 'Ocultar';
        }

      
        window.addEventListener('DOMContentLoaded', function () {
            document.querySelectorAll('.tc-btn-ver').forEach(function (btn) {
                btn.addEventListener('click', function (e) {
                    e.preventDefault();
                    var card    = btn.closest('.timeline-card');
                    var detalle = card ? card.querySelector('.tc-detalle') : null;
                    if (!detalle) return;
                    var abierto = detalle.style.display !== 'none';
                    detalle.style.display = abierto ? 'none' : 'block';
                    btn.textContent       = abierto ? 'Ver detalle' : 'Ocultar';
                });
            });
        });
    </script>
</body>
</html>
