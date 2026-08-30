<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormLineaTemporal.aspx.cs" Inherits="FormLineaTemporal" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Línea Temporal Clínica</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"       rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"   rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormLineaTemporal.css"   rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Gestión Clínica</div>
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_linea_temporal" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
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
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server"
                    Visible="false" CssClass="server-error" />

                <asp:Panel ID="pnlSeleccionPaciente" runat="server" CssClass="content-card">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblTituloSeleccion" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblSubtituloSeleccion" runat="server" Text="" /></p>
                    </div>
                    <div class="grid-2" style="max-width:420px;">
                        <div class="field full">
                            <asp:Label ID="lblEtiquetaPacienteSel" runat="server" AssociatedControlID="ddlPacienteSeleccion" Text="" />
                            <asp:DropDownList ID="ddlPacienteSeleccion" runat="server" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvPacienteSel" runat="server"
                                ControlToValidate="ddlPacienteSeleccion" InitialValue=""
                                ErrorMessage="." CssClass="field-error" Display="Dynamic" />
                        </div>
                    </div>
                    <div class="form-actions">
                        <asp:Button ID="btnContinuar" runat="server" Text="Continuar"
                            CssClass="btn-primary" OnClick="btnContinuar_Click" />
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlTimeline" runat="server" Visible="false">

                    <div class="temporal-layout">

                        <div class="temporal-main">

                            <div class="content-card">
                                <div class="paciente-header">
                                    <div class="paciente-header-avatar">
                                        <asp:Label ID="lblPacienteIniciales" runat="server" Text="" />
                                    </div>
                                    <div class="paciente-header-info">
                                        <asp:Label ID="lblPacienteNombre" runat="server"
                                            CssClass="paciente-header-nombre" Text="" />
                                        <div class="paciente-header-meta">
                                            <asp:Label ID="lblPacienteEdad" runat="server" CssClass="meta-item" Text="" />
                                            <span class="meta-sep">·</span>
                                            <asp:Label ID="lblPacienteEstado" runat="server" CssClass="meta-item" Text="" />
                                            <span class="meta-sep">·</span>
                                            <asp:Label ID="lblPacienteOcup" runat="server" CssClass="meta-item" Text="" />
                                        </div>
                                    </div>
                                </div>

                                <div class="filtros-row">
                                    <div class="filtros-tipos">
                                        <asp:Label ID="lblFiltroTipoEtiqueta" runat="server" CssClass="filtro-label" Text="" />
                                        <asp:Button ID="btnFiltroTodos" runat="server"
                                            Text="Todos" CssClass="filtro-btn active"
                                            OnClick="btnFiltro_Click" CommandArgument="TODOS"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnFiltroConsulta" runat="server"
                                            Text="Consulta" CssClass="filtro-btn"
                                            OnClick="btnFiltro_Click" CommandArgument="CONSULTA"
                                            CausesValidation="false" />
                                        <asp:Button ID="btnFiltroHistorial" runat="server"
                                            Text="Historial" CssClass="filtro-btn"
                                            OnClick="btnFiltro_Click" CommandArgument="HISTORIAL"
                                            CausesValidation="false" />
                                    </div>
                                    <div class="filtros-fechas">
                                        <asp:TextBox ID="txtDesde" runat="server"
                                            TextMode="Date" CssClass="filtro-input" ClientIDMode="Static" />
                                        <span class="filtro-sep">→</span>
                                        <asp:TextBox ID="txtHasta" runat="server"
                                            TextMode="Date" CssClass="filtro-input" ClientIDMode="Static" />
                                        <asp:Button ID="btnAplicarFecha" runat="server"
                                            Text="Filtrar" CssClass="btn-secondary btn-sm"
                                            OnClick="btnAplicarFecha_Click" CausesValidation="false" />
                                    </div>
                                </div>

                                <div class="filtros-stats">
                                    <asp:Label ID="lblTotalEventos" runat="server" CssClass="stats-total" Text="" />
                                    <div class="stats-leyenda">
                                        <asp:Label ID="lblLeyendaConsulta" runat="server" CssClass="leyenda-item consulta" Text="" />
                                        <asp:Label ID="lblLeyendaHistorial" runat="server" CssClass="leyenda-item historial" Text="" />
                                    </div>
                                </div>
                            </div>

                            <div class="content-card mt-24">

                                <asp:Label ID="lblSinRegistros" runat="server"
                                    CssClass="timeline-vacio" Visible="false" Text="" />

                                <div class="timeline-wrap">
                                    <div class="timeline-linea"></div>

                                    <asp:Repeater ID="rptTimeline" runat="server">
                                        <ItemTemplate>

                                            <div class='<%# "timeline-item " + Eval("LadoCss") %>'>

                                                <div class='<%# "timeline-punto " + Eval("TipoCss") %>'>
                                                    <span class="timeline-punto-icono"><%# Eval("Icono") %></span>
                                                </div>

                                                <div class='<%# "timeline-card " + Eval("TipoCss") + "-card" %>'>
                                                    <div class="timeline-card-header">
                                                        <div class="tc-header-left">
                                                            <span class='<%# "tc-tipo-badge " + Eval("TipoCss") + "-badge" %>'>
                                                                <%# Eval("TipoLabel") %>
                                                            </span>
                                                            <span class="tc-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></span>
                                                        </div>
                                                        <button type="button" class="tc-btn-ver"
                                                            onclick='<%# "return toggleDetalle(\"" + Eval("Tipo") + "_" + Eval("IdEvento") + "\", this);" %>'>
                                                            Ver detalle
                                                        </button>
                                                    </div>
                                                    <p class="tc-resumen"><%# Eval("Resumen") %></p>

                                                    <div class="tc-detalle" id='<%# "detalle_" + Eval("Tipo") + "_" + Eval("IdEvento") %>' style="display:none;">
                                                        <div class="tc-detalle-sep"></div>
                                                        <p class="tc-detalle-texto" style="white-space:pre-line;"><%# Eval("Detalle") %></p>
                                                        <asp:Panel ID="pnlDatosConsulta" runat="server"
                                                            Visible='<%# Eval("Tipo").ToString() == "CONSULTA" %>'
                                                            CssClass="tc-detalle-extra">
                                                            <span>⏱ <%# Eval("Duracion") %> min</span>
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
                                <p class="accesos-titulo"><asp:Label ID="lblResumenTratamientoTitulo" runat="server" Text="" /></p>
                                <div class="stat-item">
                                    <asp:Label ID="lblStatConsultas" runat="server" CssClass="stat-big" Text="0" />
                                    <asp:Label ID="lblDescConsultasTotales" runat="server" CssClass="stat-desc" Text="" />
                                </div>
                                <div class="stat-item">
                                    <asp:Label ID="lblStatMeses" runat="server" CssClass="stat-big" Text="0" />
                                    <asp:Label ID="lblDescMesesTratamiento" runat="server" CssClass="stat-desc" Text="" />
                                </div>
                                <div class="stat-item">
                                    <asp:Label ID="lblStatInicio" runat="server" CssClass="stat-fecha-val" Text="" />
                                    <asp:Label ID="lblDescPrimeraSesion" runat="server" CssClass="stat-desc" Text="" />
                                </div>
                                <div class="stat-item" style="border-bottom:none;">
                                    <asp:Label ID="lblStatUltima" runat="server" CssClass="stat-fecha-val" Text="" />
                                    <asp:Label ID="lblDescUltimaSesion" runat="server" CssClass="stat-desc" Text="" />
                                </div>
                            </div>

                            <div class="content-card accesos-card">
                                <p class="accesos-titulo"><asp:Label ID="lblAccesosTitulo" runat="server" Text="" /></p>
                                <a href="FormRealizarConsulta.aspx" class="acceso-item">🗒️ <span>Nueva consulta</span></a>
                                <a href="FormResumenIA.aspx" class="acceso-item">🤖 <span>Resumen IA</span></a>
                                <a href="FormExportarReporte.aspx" class="acceso-item">💾 <span>Exportar reporte</span></a>
                            </div>

                        </div>
                    </div>
                </asp:Panel>
            </div>
        </div>

    </form>

    <script type="text/javascript">
        function toggleDetalle(clave, btn) {
            var panel = document.getElementById('detalle_' + clave);
            if (!panel) return false;
            var abierto = panel.style.display !== 'none';
            panel.style.display = abierto ? 'none' : 'block';
            btn.textContent = abierto ? 'Ver detalle' : 'Ocultar';
            return false;
        }
    </script>
</body>
</html>