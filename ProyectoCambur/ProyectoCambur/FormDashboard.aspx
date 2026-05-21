<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormDashboard.aspx.cs" Inherits="FormDashboard" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Dashboard</title>
    <link href="EstilosPaginas/Shared.css"          rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormDashboard.css"   rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <!-- SIDEBAR -->
        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Gestión Clínica</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormDashboard.aspx"         class="nav-item active">🏠 Dashboard</a>
                <a href="FormRegistroPaciente.aspx"  class="nav-item">👤 Pacientes</a>
                <a href="FormRealizarConsulta.aspx"  class="nav-item">🗒️ Consultas</a>
                <a href="FormHistorialClinico.aspx"  class="nav-item">📋 Historial Clínico</a>
                <a href="FormResumenIA.aspx"         class="nav-item">🤖 Resumen IA</a>
                <a href="FormLineaTemporal.aspx"     class="nav-item">📅 Línea Temporal</a>
                <a href="FormInformeDerivacion.aspx" class="nav-item">📤 Derivaciones</a>
                <a href="FormPerfilPaciente.aspx"    class="nav-item">🧠 Perfilación</a>
                <a href="FormExportarReporte.aspx"   class="nav-item">💾 Exportar</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormSuscripcion.aspx" class="nav-item">💳 Mi Suscripción</a>
                <a href="FormLogin.aspx"       class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

        <!-- ÁREA PRINCIPAL -->
        <div class="main-wrap">

            <!-- HEADER -->
            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Inicio</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Dashboard operativo</span>
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="LM" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server" CssClass="user-name" Text="" />
                        <span class="user-role">Psicólogo/a</span>
                    </div>
                </div>
            </header>

            <!-- CONTENIDO -->
            <div class="page-content">

                <!-- BIENVENIDA + SELECTOR DE PERÍODO -->
                <div class="dash-top-row">
                    <div class="dash-bienvenida">
                        <asp:Label ID="lblBienvenida" runat="server"
                            CssClass="bienvenida-titulo" Text="" />
                        <asp:Label ID="lblFechaHoy" runat="server"
                            CssClass="bienvenida-fecha" Text="" />
                    </div>
                    <div class="dash-periodo-selector">
                        <asp:Button ID="btnSemana" runat="server" Text="Esta semana"
                            CssClass="periodo-btn"
                            CommandArgument="SEMANA"
                            OnClick="btnPeriodo_Click"
                            CausesValidation="false" />
                        <asp:Button ID="btnMes" runat="server" Text="Este mes"
                            CssClass="periodo-btn active"
                            CommandArgument="MES"
                            OnClick="btnPeriodo_Click"
                            CausesValidation="false" />
                        <asp:Button ID="btnTrimestre" runat="server" Text="Este trimestre"
                            CssClass="periodo-btn"
                            CommandArgument="TRIMESTRE"
                            OnClick="btnPeriodo_Click"
                            CausesValidation="false" />
                        <asp:Button ID="btnAnio" runat="server" Text="Este año"
                            CssClass="periodo-btn"
                            CommandArgument="ANIO"
                            OnClick="btnPeriodo_Click"
                            CausesValidation="false" />
                    </div>
                </div>

                <!-- FILA 1: KPIs principales -->
                <div class="kpi-grid kpi-grid-4">

                    <div class="kpi-card">
                        <div class="kpi-icono kpi-azul">👤</div>
                        <asp:Label ID="lblKpiTotalPacientes" runat="server"
                            CssClass="kpi-numero" Text="0" />
                        <span class="kpi-label">Total pacientes</span>
                        <asp:Label ID="lblKpiDeltaPacientes" runat="server"
                            CssClass="kpi-delta positivo" Text="" />
                    </div>

                    <div class="kpi-card">
                        <div class="kpi-icono kpi-verde">➕</div>
                        <asp:Label ID="lblKpiNuevosPacientes" runat="server"
                            CssClass="kpi-numero" Text="0" />
                        <span class="kpi-label">Nuevos en el período</span>
                        <asp:Label ID="lblKpiDeltaNuevos" runat="server"
                            CssClass="kpi-delta positivo" Text="" />
                    </div>

                    <div class="kpi-card">
                        <div class="kpi-icono kpi-acento">🗒️</div>
                        <asp:Label ID="lblKpiConsultas" runat="server"
                            CssClass="kpi-numero" Text="0" />
                        <span class="kpi-label">Consultas realizadas</span>
                        <asp:Label ID="lblKpiDeltaConsultas" runat="server"
                            CssClass="kpi-delta positivo" Text="" />
                    </div>

                    <div class="kpi-card">
                        <div class="kpi-icono kpi-gris">📤</div>
                        <asp:Label ID="lblKpiDerivaciones" runat="server"
                            CssClass="kpi-numero" Text="0" />
                        <span class="kpi-label">Derivaciones</span>
                        <asp:Label ID="lblKpiDeltaDeriv" runat="server"
                            CssClass="kpi-delta neutro" Text="" />
                    </div>

                </div>

                <!-- FILA 2: KPIs IA -->
                <div class="kpi-grid kpi-grid-3" style="margin-top:12px;">

                    <div class="kpi-card kpi-card-ia">
                        <div class="kpi-icono-ia">🤖</div>
                        <asp:Label ID="lblKpiResumenes" runat="server"
                            CssClass="kpi-numero-sm" Text="0" />
                        <span class="kpi-label">Resúmenes IA generados</span>
                    </div>

                    <div class="kpi-card kpi-card-ia">
                        <div class="kpi-icono-ia">🧠</div>
                        <asp:Label ID="lblKpiPerfilaciones" runat="server"
                            CssClass="kpi-numero-sm" Text="0" />
                        <span class="kpi-label">Perfilaciones</span>
                    </div>

                    <div class="kpi-card kpi-card-ia">
                        <div class="kpi-icono-ia">💾</div>
                        <asp:Label ID="lblKpiExportaciones" runat="server"
                            CssClass="kpi-numero-sm" Text="0" />
                        <span class="kpi-label">Informes exportados</span>
                    </div>

                </div>

                <!-- FILA 3: Gráfico + Últimas consultas -->
                <div class="dash-main-row">

                    <!-- Gráfico de actividad mensual -->
                    <div class="content-card grafico-card">
                        <div class="grafico-header">
                            <h3 class="grafico-titulo">Actividad mensual</h3>
                            <asp:Label ID="lblGraficoSubtitulo" runat="server"
                                CssClass="grafico-subtitulo" Text="" />
                        </div>
                        <div class="grafico-wrap">
                            <div class="grafico-barras" id="graficoBars">
                                <!-- Las barras se generan desde el code-behind via Repeater -->
                                <asp:Repeater ID="rptGrafico" runat="server">
                                    <ItemTemplate>
                                        <div class="barra-col">
                                            <div class="barra-valor"><%# Eval("Valor") %></div>
                                            <div class="barra-wrap">
                                                <div class="barra-fill"
                                                     style='height: <%# Eval("PctAltura") %>%;'
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

                    <!-- Últimas consultas -->
                    <div class="content-card ultimas-card">
                        <div class="card-header-row">
                            <div class="card-header-left">
                                <h3 class="grafico-titulo">Últimas consultas</h3>
                            </div>
                            <a href="FormRealizarConsulta.aspx" class="ver-todas-link">Ver todas</a>
                        </div>

                        <div class="table-wrap" style="margin-top:14px;">
                            <asp:GridView ID="gvUltimasConsultas" runat="server"
                                CssClass="data-table"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                EmptyDataText="No hay consultas registradas aún.">
                                <EmptyDataRowStyle CssClass="empty-row" />
                                <HeaderStyle      CssClass="table-header" />
                                <RowStyle         CssClass="table-row" />
                                <AlternatingRowStyle CssClass="table-row table-row-alt" />
                                <Columns>
                                    <asp:BoundField DataField="Paciente"
                                        HeaderText="Paciente"
                                        HeaderStyle-CssClass="th-left" />
                                    <asp:BoundField DataField="Fecha"
                                        HeaderText="Fecha"
                                        DataFormatString="{0:dd/MM/yyyy}"
                                        HeaderStyle-CssClass="th-centro"
                                        ItemStyle-CssClass="td-centro" />
                                    <asp:BoundField DataField="Duracion"
                                        HeaderText="Min"
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

                <!-- FILA 4: Pacientes activos recientes -->
                <div class="content-card" style="margin-top:20px;">
                    <div class="card-header-row">
                        <div class="card-header-left">
                            <h3 class="grafico-titulo">Pacientes activos</h3>
                            <asp:Label ID="lblBadgePacientesActivos" runat="server"
                                CssClass="badge-activos" Text="" />
                        </div>
                        <a href="FormRegistroPaciente.aspx" class="ver-todas-link">Ver todos</a>
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
                                    <a href='FormHistorialClinico.aspx?id=<%# Eval("IdPaciente") %>'
                                       class="chip-btn">Ver</a>
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
