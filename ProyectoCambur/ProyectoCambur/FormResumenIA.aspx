<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormResumenIA.aspx.cs" Inherits="FormResumenIA" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Resumen Clínico IA</title>
    <link href="EstilosPaginas/Shared.css"            rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"     rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css" rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormResumenIA.css"     rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_resumen_ia" />
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

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <asp:Panel ID="pnlFiltros" runat="server" CssClass="content-card">

                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblFormTitulo" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblFormSubtitulo" runat="server" Text="" /></p>
                    </div>

                    <div class="ia-badge-aviso">
                        🤖 <asp:Label ID="lblAvisoIA" runat="server" Text="" />
                    </div>

                    <asp:Label ID="lblSeccionFiltros" runat="server" CssClass="section-sep" Text="" />

                    <div class="grid-3">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaPaciente" runat="server" AssociatedControlID="ddlPaciente" Text="" />
                            <asp:DropDownList ID="ddlPaciente" runat="server" ClientIDMode="Static"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlPaciente_SelectedIndexChanged" />
                            <asp:RequiredFieldValidator ID="rfvPaciente" runat="server"
                                ControlToValidate="ddlPaciente" InitialValue=""
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgFiltro" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaFechaDesde" runat="server" AssociatedControlID="txtFechaDesde" Text="" />
                            <asp:TextBox ID="txtFechaDesde" runat="server"
                                TextMode="Date" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvDesde" runat="server"
                                ControlToValidate="txtFechaDesde"
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgFiltro" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaFechaHasta" runat="server" AssociatedControlID="txtFechaHasta" Text="" />
                            <asp:TextBox ID="txtFechaHasta" runat="server"
                                TextMode="Date" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvHasta" runat="server"
                                ControlToValidate="txtFechaHasta"
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgFiltro" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <asp:Button ID="btnBuscar" runat="server"
                            Text=""
                            CssClass="btn-secondary"
                            ValidationGroup="vgFiltro"
                            OnClick="btnBuscar_Click" />
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnlResumenesAnteriores" runat="server" CssClass="content-card mt-24" Visible="false">

                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblTituloResumenesAnteriores" runat="server" Text="" /></h2>
                    </div>

                    <asp:Repeater ID="rptResumenesAnteriores" runat="server" OnItemCommand="rptResumenesAnteriores_ItemCommand">
                        <ItemTemplate>
                            <div class="resumen-anterior-item">
                                <div class="ra-info">
                                    <span class="ra-periodo"><%# Eval("Periodo") %></span>
                                    <span class="ra-fecha"><%# Eval("FechaGeneracion", "{0:dd/MM/yyyy HH:mm}") %></span>
                                </div>
                                <asp:LinkButton runat="server" CssClass="btn-secondary"
                                    CommandName="VerResumen" CommandArgument='<%# Eval("IdResumen") %>'
                                    Text="Ver" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="lblSinResumenesAnteriores" runat="server"
                        CssClass="sin-perfiles-txt" Text="" Visible="false" />

                </asp:Panel>

                <asp:Panel ID="pnlConsultas" runat="server" CssClass="content-card mt-24" Visible="false">

                    <div class="card-header-row">
                        <div class="card-header-left">
                            <h2 class="card-title"><asp:Label ID="lblTituloConsultasEncontradas" runat="server" Text="" /></h2>
                            <asp:Label ID="lblCantConsultas" runat="server" CssClass="badge-activos" Text="" />
                        </div>
                        <asp:Label ID="lblRangoBusqueda" runat="server" CssClass="rango-label" Text="" />
                    </div>

                    <p class="hint-text" style="margin-top:6px;">
                        <asp:Label ID="lblHintSeleccion" runat="server" Text="" />
                    </p>

                    <div class="table-wrap">
                        <table class="data-table" id="tblConsultas">
                            <thead>
                                <tr>
                                    <th class="th-check">
                                        <input type="checkbox" id="chkTodas" onclick="toggleTodas(this)" title="Seleccionar todas" />
                                    </th>
                                    <th class="th-left"><asp:Label ID="lblThFecha" runat="server" Text="" /></th>
                                    <th class="th-left"><asp:Label ID="lblThDuracion" runat="server" Text="" /></th>
                                    <th class="th-left"><asp:Label ID="lblThResumenObjetivos" runat="server" Text="" /></th>
                                </tr>
                            </thead>
                        </table>

                        <table class="data-table" style="margin-top:-1px; border-top:none;">
                            <asp:Repeater ID="rptConsultas" runat="server">
                                <ItemTemplate>
                                    <tr class="table-row">
                                        <td class="td-check">
                                            <asp:CheckBox ID="chkConsulta" runat="server"
                                                CssClass="chk-consulta"
                                                Checked="true" />
                                            <asp:HiddenField ID="hfIdConsulta" runat="server"
                                                Value='<%# Eval("IdConsulta") %>' />
                                        </td>
                                        <td class="td-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></td>
                                        <td class="td-dur"><%# Eval("Duracion") %> min</td>
                                        <td class="td-resumen"><%# Eval("ResumenObjetivos") %></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </table>
                    </div>

                    <div class="form-actions">
                        <asp:Button ID="btnVolver" runat="server"
                            Text=""
                            CssClass="btn-secondary"
                            OnClick="btnVolver_Click"
                            CausesValidation="false" />
                        <asp:Button ID="btnGenerar" runat="server"
                            Text=""
                            CssClass="btn-primary btn-ia"
                            OnClick="btnGenerar_Click"
                            CausesValidation="false"
                            OnClientClick="mostrarCarga(); return true;" />
                    </div>

                </asp:Panel>

                <div class="carga-overlay" id="cargaOverlay" style="display:none;">
                    <div class="carga-card">
                        <div class="carga-spinner"></div>
                        <p class="carga-titulo"><asp:Label ID="lblCargaTitulo" runat="server" Text="" /></p>
                        <p class="carga-subtitulo"><asp:Label ID="lblCargaSubtitulo" runat="server" Text="" /></p>
                    </div>
                </div>

                <asp:Panel ID="pnlResumen" runat="server" CssClass="resumen-layout" Visible="false">

                    <div class="resumen-main">
                        <div class="content-card">

                            <div class="resumen-header">
                                <div>
                                    <h2 class="card-title"><asp:Label ID="lblTituloResumenGenerado" runat="server" Text="" /></h2>
                                    <asp:Label ID="lblResumenMeta" runat="server" CssClass="card-subtitle" Text="" />
                                </div>
                                <div class="resumen-header-actions">
                                    <asp:Button ID="btnNuevoResumen" runat="server"
                                        Text=""
                                        CssClass="btn-secondary"
                                        OnClick="btnNuevoResumen_Click"
                                        CausesValidation="false" />
                                </div>
                            </div>

                            <div class="ia-badge-resultado">
                                🤖 <asp:Label ID="lblAvisoResultado" runat="server" Text="" />
                            </div>

                            <div class="resumen-seccion">
                                <div class="resumen-seccion-titulo">
                                    <span class="resumen-seccion-icono">📌</span> <asp:Label ID="lblTituloContexto" runat="server" Text="" />
                                </div>
                                <asp:Label ID="lblContextoGeneral" runat="server"
                                    CssClass="resumen-seccion-texto" Text="" />
                            </div>

                            <div class="resumen-seccion">
                                <div class="resumen-seccion-titulo">
                                    <span class="resumen-seccion-icono">📈</span> <asp:Label ID="lblTituloEvolucion" runat="server" Text="" />
                                </div>
                                <asp:Label ID="lblEvolucion" runat="server"
                                    CssClass="resumen-seccion-texto" Text="" />
                            </div>

                            <div class="resumen-seccion">
                                <div class="resumen-seccion-titulo">
                                    <span class="resumen-seccion-icono">🔁</span> <asp:Label ID="lblTituloTemas" runat="server" Text="" />
                                </div>
                                <asp:Label ID="lblTemasRecurrentes" runat="server"
                                    CssClass="resumen-seccion-texto" Text="" />
                            </div>

                            <div class="resumen-seccion">
                                <div class="resumen-seccion-titulo">
                                    <span class="resumen-seccion-icono">🛠️</span> <asp:Label ID="lblTituloIntervenciones" runat="server" Text="" />
                                </div>
                                <asp:Label ID="lblIntervenciones" runat="server"
                                    CssClass="resumen-seccion-texto" Text="" />
                            </div>

                            <div class="resumen-seccion" style="border-bottom:none; margin-bottom:0; padding-bottom:0;">
                                <div class="resumen-seccion-titulo">
                                    <span class="resumen-seccion-icono">💡</span> <asp:Label ID="lblTituloObservaciones" runat="server" Text="" />
                                </div>
                                <asp:Label ID="lblObservaciones" runat="server"
                                    CssClass="resumen-seccion-texto" Text="" />
                            </div>

                        </div>
                    </div>

                    <div class="resumen-aside">

                        <div class="content-card resumen-meta-card">
                            <p class="accesos-titulo"><asp:Label ID="lblAccesosTitulo" runat="server" Text="" /></p>
                            <div class="meta-fila">
                                <asp:Label ID="lblMetaLabelPaciente" runat="server" CssClass="meta-label" Text="" />
                                <asp:Label ID="lblMetaPaciente" runat="server" CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <asp:Label ID="lblMetaLabelPeriodo" runat="server" CssClass="meta-label" Text="" />
                                <asp:Label ID="lblMetaPeriodo"  runat="server" CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <asp:Label ID="lblMetaLabelConsultas" runat="server" CssClass="meta-label" Text="" />
                                <asp:Label ID="lblMetaConsultas" runat="server" CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <asp:Label ID="lblMetaLabelFecha" runat="server" CssClass="meta-label" Text="" />
                                <asp:Label ID="lblMetaFecha" runat="server" CssClass="meta-valor" Text="" />
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoEncriptadoTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoEncriptadoTexto" runat="server" Text="" /></p>
                        </div>

                        <div class="content-card accesos-card">
                            <a href="FormExportarReporte.aspx" class="acceso-item">💾 <span>Exportar en PDF</span></a>
                            <a href="FormInformeDerivacion.aspx" class="acceso-item">📤 <span>Generar derivación</span></a>
                            <a href="FormLineaTemporal.aspx" class="acceso-item">📅 <span>Ver línea temporal</span></a>
                        </div>

                    </div>

                </asp:Panel>

            </div>
        </div>

    </form>

    <script type="text/javascript">
        function toggleTodas(chkTodas) {
            var checks = document.querySelectorAll('.chk-consulta input[type="checkbox"]');
            checks.forEach(function (c) { c.checked = chkTodas.checked; });
        }

        function mostrarCarga() {
            var overlay = document.getElementById('cargaOverlay');
            if (overlay) overlay.style.display = 'flex';
            return true;
        }

        window.addEventListener('DOMContentLoaded', function () {
            var overlay = document.getElementById('cargaOverlay');
            if (overlay) overlay.style.display = 'none';
        });
    </script>
</body>
</html>