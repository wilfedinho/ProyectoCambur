<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormModificarConsulta.aspx.cs" Inherits="FormModificarConsulta" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Modificar Consulta</title>
    <link href="EstilosPaginas/Shared.css"                 rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"          rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"      rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormModificarConsulta.css"  rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_modificar_consulta" />
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

                <asp:HiddenField ID="hdnIdConsulta" runat="server" Value="0" />

                <asp:Panel ID="pnlSeleccion" runat="server" CssClass="content-card">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblFormTituloSeleccion" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblFormSubtituloSeleccion" runat="server" Text="" /></p>
                    </div>

                    <div class="grid-1-col">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaPacienteSeleccion" runat="server" AssociatedControlID="ddlPacienteSeleccion" Text="" />
                            <asp:DropDownList ID="ddlPacienteSeleccion" runat="server" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvPacienteSeleccion" runat="server"
                                ControlToValidate="ddlPacienteSeleccion" InitialValue=""
                                ErrorMessage="." CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgBuscar" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <asp:Button ID="btnBuscarConsultas" runat="server"
                            Text="" CssClass="btn-primary"
                            OnClick="btnBuscarConsultas_Click"
                            ValidationGroup="vgBuscar" />
                    </div>

                    <asp:Panel ID="pnlListaConsultas" runat="server" Visible="false" CssClass="lista-consultas-wrap">
                        <div class="section-sep" style="margin-top:20px;">
                            <asp:Label ID="lblTituloConsultasEncontradas" runat="server" Text="" />
                            <asp:Label ID="lblCantConsultas" runat="server" CssClass="badge-activos" Visible="false" Text="" />
                        </div>
                        <p class="hint-text"><asp:Label ID="lblHintConsultas" runat="server" Text="" /></p>

                        <div class="table-wrap">
                            <table class="data-table">
                                <thead>
                                    <tr>
                                        <th class="th-left"><asp:Label ID="lblThFecha" runat="server" Text="" /></th>
                                        <th class="th-left"><asp:Label ID="lblThDuracion" runat="server" Text="" /></th>
                                        <th class="th-left"><asp:Label ID="lblThResumenObjetivos" runat="server" Text="" /></th>
                                        <th class="th-left"><asp:Label ID="lblThPlazo" runat="server" Text="" /></th>
                                        <th class="th-centro"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <asp:Repeater ID="rptConsultas" runat="server" OnItemCommand="rptConsultas_ItemCommand">
                                        <ItemTemplate>
                                            <tr class="table-row">
                                                <td><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></td>
                                                <td><%# Eval("Duracion") %> min</td>
                                                <td><%# Eval("ResumenObjetivos") %></td>
                                                <td><%# Eval("DiasRestantes") %> días</td>
                                                <td class="td-acciones">
                                                    <asp:LinkButton ID="lbModificar" runat="server"
                                                        CommandName="Modificar"
                                                        CommandArgument='<%# Eval("IdConsulta") %>'>✏️</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </div>
                    </asp:Panel>
                </asp:Panel>

                <asp:Panel ID="pnlBloqueado" runat="server" CssClass="content-card plazo-vencido-card" Visible="false">
                    <div class="plazo-icono">🔒</div>
                    <div class="plazo-info">
                        <p class="plazo-titulo"><asp:Label ID="lblPlazoVencidoTitulo" runat="server" Text="" /></p>
                        <asp:Label ID="lblMensajeBloqueado" runat="server"
                            CssClass="plazo-subtitulo" Text="" />
                    </div>
                    <asp:Button ID="btnVolverDesdeBloqueado" runat="server"
                        Text="" CssClass="btn-secondary"
                        OnClick="btnVolverDesdeBloqueado_Click"
                        CausesValidation="false" />
                </asp:Panel>

                <asp:Panel ID="pnlFormulario" runat="server" CssClass="modificar-layout" Visible="false">

                    <div class="modificar-form-col">
                        <div class="content-card">

                            <div class="consulta-readonly-header">
                                <div class="consulta-readonly-paciente">
                                    <div class="cr-avatar">
                                        <asp:Label ID="lblPacienteIniciales" runat="server" Text="" />
                                    </div>
                                    <div class="cr-info">
                                        <asp:Label ID="lblPacienteNombre" runat="server"
                                            CssClass="cr-nombre" Text="" />
                                        <div class="cr-meta">
                                            <span class="cr-meta-item">📅</span>
                                            <asp:Label ID="lblFechaConsulta" runat="server"
                                                CssClass="cr-meta-item cr-fecha" Text="" />
                                            <span class="cr-meta-sep">·</span>
                                            <asp:Label ID="lblDuracionConsulta" runat="server"
                                                CssClass="cr-meta-item" Text="" />
                                        </div>
                                    </div>
                                </div>
                                <asp:Label ID="lblBadgePlazo" runat="server"
                                    CssClass="badge-plazo-ok" Text="" />
                            </div>

                            <div class="aviso-readonly">
                                ✏️ <asp:Label ID="lblAvisoReadonly" runat="server" Text="" />
                            </div>

                            <asp:Label ID="lblSeccionEditables" runat="server" CssClass="section-sep" Text="" style="display:block;" />

                            <div class="grid-1-col">

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaObjetivos" runat="server" AssociatedControlID="txtObjetivos" Text="" />
                                    <asp:TextBox ID="txtObjetivos" runat="server"
                                        TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvObjetivos" runat="server"
                                        ControlToValidate="txtObjetivos" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgModificar" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaObservaciones" runat="server" AssociatedControlID="txtObservaciones" Text="" />
                                    <asp:TextBox ID="txtObservaciones" runat="server"
                                        TextMode="MultiLine" Rows="4" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvObservaciones" runat="server"
                                        ControlToValidate="txtObservaciones" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgModificar" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaHipotesis" runat="server" AssociatedControlID="txtHipotesis" Text="" />
                                    <asp:TextBox ID="txtHipotesis" runat="server"
                                        TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvHipotesis" runat="server"
                                        ControlToValidate="txtHipotesis" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgModificar" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaIntervenciones" runat="server" AssociatedControlID="txtIntervenciones" Text="" />
                                    <asp:TextBox ID="txtIntervenciones" runat="server"
                                        TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvIntervenciones" runat="server"
                                        ControlToValidate="txtIntervenciones" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgModificar" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaEvolucion" runat="server" AssociatedControlID="txtEvolucion" Text="" />
                                    <asp:TextBox ID="txtEvolucion" runat="server"
                                        TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvEvolucion" runat="server"
                                        ControlToValidate="txtEvolucion" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgModificar" />
                                </div>

                            </div>

                            <asp:Label ID="lblSeccionCierre" runat="server" CssClass="section-sep" Text="" style="display:block;" />

                            <div class="grid-2">
                                <div class="field">
                                    <asp:Label ID="lblEtiquetaDiagnostico" runat="server" AssociatedControlID="txtDiagnostico" Text="" />
                                    <asp:TextBox ID="txtDiagnostico" runat="server"
                                        TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvDiagnostico" runat="server"
                                        ControlToValidate="txtDiagnostico" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgModificar" />
                                </div>
                                <div class="field">
                                    <asp:Label ID="lblEtiquetaTratamiento" runat="server" AssociatedControlID="txtTratamiento" Text="" />
                                    <asp:TextBox ID="txtTratamiento" runat="server"
                                        TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvTratamiento" runat="server"
                                        ControlToValidate="txtTratamiento" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgModificar" />
                                </div>
                            </div>

                            <div class="form-actions">
                                <asp:Button ID="btnVolverFormulario" runat="server"
                                    Text="" CssClass="btn-secondary"
                                    OnClick="btnVolverFormulario_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnGuardar" runat="server"
                                    Text="" CssClass="btn-primary"
                                    OnClick="btnGuardar_Click"
                                    ValidationGroup="vgModificar" />
                            </div>

                        </div>
                    </div>

                    <div class="modificar-info-col">

                        <div class="content-card plazo-card">
                            <p class="plazo-card-titulo"><asp:Label ID="lblPlazoCardTitulo" runat="server" Text="" /></p>
                            <div class="plazo-dias-wrap">
                                <asp:Label ID="lblDiasRestantes" runat="server"
                                    CssClass="plazo-dias-num" Text="" />
                                <span class="plazo-dias-label"><asp:Label ID="lblDiasRestantesLabel" runat="server" Text="" /></span>
                            </div>
                            <div class="plazo-barra-wrap">
                                <div class="plazo-barra">
                                    <asp:Label ID="lblPlazoFill" runat="server"
                                        CssClass="plazo-fill" Text="" />
                                </div>
                            </div>
                            <asp:Label ID="lblFechaLimite" runat="server"
                                CssClass="plazo-fecha-limite" Text="" />
                        </div>

                        <div class="content-card ultima-mod-card">
                            <p class="accesos-titulo"><asp:Label ID="lblHistorialCambiosTitulo" runat="server" Text="" /></p>
                            <div class="mod-item">
                                <span class="mod-icono">📝</span>
                                <div class="mod-info">
                                    <span class="mod-label"><asp:Label ID="lblCreadaLabel" runat="server" Text="" /></span>
                                    <asp:Label ID="lblFechaCreacion" runat="server"
                                        CssClass="mod-valor" Text="" />
                                </div>
                            </div>
                            <div class="mod-item">
                                <span class="mod-icono">✏️</span>
                                <div class="mod-info">
                                    <span class="mod-label"><asp:Label ID="lblUltimaModLabel" runat="server" Text="" /></span>
                                    <asp:Label ID="lblUltimaModificacion" runat="server"
                                        CssClass="mod-valor" Text="" />
                                </div>
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoEncriptadoTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoEncriptadoTexto" runat="server" Text="" /></p>
                        </div>

                    </div>

                </asp:Panel>

            </div>
        </div>

    </form>
</body>
</html>