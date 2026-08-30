<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormDashboard.aspx.cs" Inherits="FormDashboard" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Dashboard</title>
    <link href="EstilosPaginas/Shared.css"          rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"   rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css" rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormDashboard.css"   rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormDashboard_CU11_Extra.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Gestión Clínica</div>
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_dashboard" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Inicio</span>
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderPagina" runat="server" CssClass="header-page" Text="Dashboard operativo" />
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <div class="dash-top-row">
                    <div class="dash-bienvenida">
                        <asp:Label ID="lblBienvenida" runat="server" CssClass="bienvenida-titulo" Text="" />
                        <asp:Label ID="lblFechaHoy" runat="server" CssClass="bienvenida-fecha" Text="" />
                    </div>
                    <div class="dash-periodo-selector">
                        <asp:Button ID="btnSemana" runat="server" Text="Esta semana"
                            CssClass="periodo-btn" CommandArgument="SEMANA"
                            OnClick="btnPeriodo_Click" CausesValidation="false" />
                        <asp:Button ID="btnMes" runat="server" Text="Este mes"
                            CssClass="periodo-btn active" CommandArgument="MES"
                            OnClick="btnPeriodo_Click" CausesValidation="false" />
                        <asp:Button ID="btnTrimestre" runat="server" Text="Este trimestre"
                            CssClass="periodo-btn" CommandArgument="TRIMESTRE"
                            OnClick="btnPeriodo_Click" CausesValidation="false" />
                        <asp:Button ID="btnAnio" runat="server" Text="Este año"
                            CssClass="periodo-btn" CommandArgument="ANIO"
                            OnClick="btnPeriodo_Click" CausesValidation="false" />
                    </div>
                </div>

                <div class="kpi-grid kpi-grid-4">

                    <div class="kpi-card">
                        <div class="kpi-icono kpi-azul">👤</div>
                        <asp:Label ID="lblKpiTotalPacientes" runat="server" CssClass="kpi-numero" Text="0" />
                        <asp:Label ID="lblLabelTotalPacientes" runat="server" CssClass="kpi-label" Text="" />
                        <asp:Label ID="lblKpiDeltaPacientes" runat="server" CssClass="kpi-delta positivo" Text="" />
                    </div>

                    <div class="kpi-card">
                        <div class="kpi-icono kpi-verde">➕</div>
                        <asp:Label ID="lblKpiNuevosPacientes" runat="server" CssClass="kpi-numero" Text="0" />
                        <asp:Label ID="lblLabelNuevosPacientes" runat="server" CssClass="kpi-label" Text="" />
                        <asp:Label ID="lblKpiDeltaNuevos" runat="server" CssClass="kpi-delta positivo" Text="" />
                    </div>

                    <div class="kpi-card">
                        <div class="kpi-icono kpi-acento">🗒️</div>
                        <asp:Label ID="lblKpiConsultas" runat="server" CssClass="kpi-numero" Text="0" />
                        <asp:Label ID="lblLabelConsultas" runat="server" CssClass="kpi-label" Text="" />
                        <asp:Label ID="lblKpiDeltaConsultas" runat="server" CssClass="kpi-delta positivo" Text="" />
                    </div>

                    <div class="kpi-card">
                        <div class="kpi-icono kpi-gris">📤</div>
                        <asp:Label ID="lblKpiDerivaciones" runat="server" CssClass="kpi-numero" Text="0" />
                        <asp:Label ID="lblLabelDerivaciones" runat="server" CssClass="kpi-label" Text="" />
                        <asp:Label ID="lblKpiDeltaDeriv" runat="server" CssClass="kpi-delta neutro" Text="" />
                    </div>

                </div>

                <div class="kpi-grid kpi-grid-3" style="margin-top:12px;">

                    <div class="kpi-card kpi-card-ia">
                        <div class="kpi-icono-ia">🤖</div>
                        <asp:Label ID="lblKpiResumenes" runat="server" CssClass="kpi-numero-sm" Text="0" />
                        <asp:Label ID="lblLabelResumenes" runat="server" CssClass="kpi-label" Text="" />
                    </div>

                    <div class="kpi-card kpi-card-ia">
                        <div class="kpi-icono-ia">🧠</div>
                        <asp:Label ID="lblKpiPerfilaciones" runat="server" CssClass="kpi-numero-sm" Text="0" />
                        <asp:Label ID="lblLabelPerfilaciones" runat="server" CssClass="kpi-label" Text="" />
                        <asp:Label ID="lblNotaPerfilaciones" runat="server" CssClass="kpi-nota" Visible="false" Text="" />
                    </div>

                    <div class="kpi-card kpi-card-ia">
                        <div class="kpi-icono-ia">💾</div>
                        <asp:Label ID="lblKpiExportaciones" runat="server" CssClass="kpi-numero-sm" Text="0" />
                        <asp:Label ID="lblLabelExportaciones" runat="server" CssClass="kpi-label" Text="" />
                    </div>

                </div>

                <div class="dash-main-row">

                    <div class="content-card grafico-card">
                        <div class="grafico-header">
                            <h3 class="grafico-titulo"><asp:Label ID="lblGraficoTitulo" runat="server" Text="" /></h3>
                            <asp:Label ID="lblGraficoSubtitulo" runat="server" CssClass="grafico-subtitulo" Text="" />
                        </div>
                        <div class="grafico-wrap">
                            <div class="grafico-barras" id="graficoBars">
                                <asp:Repeater ID="rptGrafico" runat="server">
                                    <ItemTemplate>
                                        <div class="barra-col">
                                            <div class="barra-valor"><%# Eval("Valor") %></div>
                                            <div class="barra-wrap">
                                                <div class="barra-fill"
                                                     style='<%# string.Format("height:{0}%;", Eval("PctAltura")) %>'
                                                     title='<%# Eval("Mes") %>: <%# Eval("Valor") %> consultas'>
                                                </div>
                                            </div>
                                            <div class="barra-mes"><%# Eval("MesCorto") %></div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div>
                    </div>

                    <div class="content-card ultimas-card">
                        <div class="card-header-row">
                            <div class="card-header-left">
                                <h3 class="grafico-titulo"><asp:Label ID="lblUltimasTitulo" runat="server" Text="" /></h3>
                            </div>
                            <asp:HyperLink ID="lnkVerTodasConsultas" runat="server"
                                NavigateUrl="~/FormRealizarConsulta.aspx" CssClass="ver-todas-link" Text="" />
                        </div>

                        <div class="table-wrap" style="margin-top:14px;">
                            <asp:GridView ID="gvUltimasConsultas" runat="server"
                                CssClass="data-table"
                                AutoGenerateColumns="false"
                                GridLines="None">
                                <EmptyDataRowStyle CssClass="empty-row" />
                                <HeaderStyle      CssClass="table-header" />
                                <RowStyle         CssClass="table-row" />
                                <AlternatingRowStyle CssClass="table-row table-row-alt" />
                                <Columns>
                                    <asp:BoundField DataField="Paciente"
                                        HeaderStyle-CssClass="th-left" />
                                    <asp:BoundField DataField="Fecha"
                                        DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-CssClass="th-centro"
                                        ItemStyle-CssClass="td-centro" />
                                    <asp:BoundField DataField="Duracion"
                                        HeaderStyle-CssClass="th-centro"
                                        ItemStyle-CssClass="td-centro" />
                                    <asp:TemplateField HeaderText=""
                                        HeaderStyle-CssClass="th-centro"
                                        ItemStyle-CssClass="td-acciones">
                                        <ItemTemplate>
                                            <a href='FormModificarConsulta.aspx?id=<%# Eval("IdConsulta") %>'
                                               class="tbl-btn tbl-btn-ver">Ver</a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </div>

                <div class="content-card" style="margin-top:20px;">
                    <div class="card-header-row">
                        <div class="card-header-left">
                            <h3 class="grafico-titulo"><asp:Label ID="lblPacientesActivosTitulo" runat="server" Text="" /></h3>
                            <asp:Label ID="lblBadgePacientesActivos" runat="server" CssClass="badge-activos" Text="" />
                        </div>
                        <asp:HyperLink ID="lnkVerTodosPacientes" runat="server"
                            NavigateUrl="~/FormRegistroPaciente.aspx" CssClass="ver-todas-link" Text="" />
                    </div>

                    <div class="pacientes-activos-grid" style="margin-top:16px;">
                        <asp:Repeater ID="rptPacientesActivos" runat="server">
                            <ItemTemplate>
                                <div class="paciente-chip">
                                    <div class="chip-avatar"><%# Eval("Iniciales") %></div>
                                    <div class="chip-info">
                                        <span class="chip-nombre"><%# Eval("Nombre") %></span>
                                        <span class="chip-meta"><%# Eval("UltimaConsulta") %></span>
                                    </div>
                                    <a href='FormLineaTemporal.aspx?id=<%# Eval("IdPaciente") %>' class="chip-btn">Ver</a>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>

            </div>
        </div>

    </form>
</body>
</html>