<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormRealizarConsulta.aspx.cs" Inherits="FormRealizarConsulta" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Realizar Consulta</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"       rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"   rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormRealizarConsulta.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_realizar_consulta" />
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

                <asp:Label ID="lblMensaje" runat="server" Visible="false" />

                <div class="consulta-layout">

                    <div class="consulta-form-col">

                        <div class="content-card">
                            <div class="card-header">
                                <h2 class="card-title"><asp:Label ID="lblFormTitulo" runat="server" Text="" /></h2>
                                <p class="card-subtitle"><asp:Label ID="lblFormSubtitulo" runat="server" Text="" /></p>
                            </div>

                            <asp:Label ID="lblSeccionPacienteFecha" runat="server" CssClass="section-sep" Text="" />

                            <div class="grid-3">
                                <div class="field">
                                    <asp:Label ID="lblEtiquetaPaciente" runat="server" AssociatedControlID="ddlPaciente" Text="" />
                                    <asp:DropDownList ID="ddlPaciente" runat="server"
                                        ClientIDMode="Static"
                                        AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPaciente_SelectedIndexChanged" />
                                    <asp:RequiredFieldValidator ID="rfvPaciente" runat="server"
                                        ControlToValidate="ddlPaciente" InitialValue=""
                                        ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaFecha" runat="server" AssociatedControlID="txtFechaConsulta" Text="" />
                                    <asp:TextBox ID="txtFechaConsulta" runat="server" TextMode="Date" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvFecha" runat="server"
                                        ControlToValidate="txtFechaConsulta" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaDuracion" runat="server" AssociatedControlID="txtDuracion" Text="" />
                                    <asp:TextBox ID="txtDuracion" runat="server" TextMode="Number" MaxLength="3" placeholder="50" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvDuracion" runat="server"
                                        ControlToValidate="txtDuracion" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                                    <asp:CompareValidator ID="cvDuracion" runat="server"
                                        ControlToValidate="txtDuracion"
                                        Operator="GreaterThan" ValueToCompare="0" Type="Integer"
                                        ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                                </div>
                            </div>

                            <asp:Label ID="lblSeccionContenidoClinico" runat="server" CssClass="section-sep" Text="" />
                            <p class="aviso-encriptado">🔒 <asp:Label ID="lblAvisoEncriptado" runat="server" Text="" /></p>

                            <div class="grid-1">
                                <div class="field">
                                    <asp:Label ID="lblEtiquetaObjetivos" runat="server" AssociatedControlID="txtObjetivos" Text="" />
                                    <asp:TextBox ID="txtObjetivos" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvObjetivos" runat="server"
                                        ControlToValidate="txtObjetivos" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaObservaciones" runat="server" AssociatedControlID="txtObservaciones" Text="" />
                                    <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Rows="4" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvObservaciones" runat="server"
                                        ControlToValidate="txtObservaciones" ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaHipotesis" runat="server" AssociatedControlID="txtHipotesis" Text="" />
                                    <asp:TextBox ID="txtHipotesis" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaIntervenciones" runat="server" AssociatedControlID="txtIntervenciones" Text="" />
                                    <asp:TextBox ID="txtIntervenciones" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaEvolucion" runat="server" AssociatedControlID="txtEvolucion" Text="" />
                                    <asp:TextBox ID="txtEvolucion" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                </div>
                            </div>

                            <asp:Label ID="lblSeccionCierreClinico" runat="server" CssClass="section-sep" Text="" />

                            <div class="grid-2">
                                <div class="field">
                                    <asp:Label ID="lblEtiquetaDiagnostico" runat="server" AssociatedControlID="txtDiagnostico" Text="" />
                                    <asp:TextBox ID="txtDiagnostico" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaTratamiento" runat="server" AssociatedControlID="txtTratamiento" Text="" />
                                    <asp:TextBox ID="txtTratamiento" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                                </div>
                            </div>

                            <div class="form-actions">
                                <a href="FormDashboard.aspx" class="btn-secondary"><asp:Label ID="lblBtnCancelar" runat="server" Text="" /></a>
                                <asp:Button ID="btnRegistrar" runat="server"
                                    Text=""
                                    CssClass="btn-primary"
                                    ValidationGroup="vgConsulta"
                                    OnClick="btnRegistrar_Click" />
                            </div>

                        </div>
                    </div>

                    <div class="consulta-info-col">

                        <div class="content-card paciente-card">
                            <div class="paciente-avatar-grande">
                                <asp:Label ID="lblPacienteIniciales" runat="server" Text="--" CssClass="avatar-circulo" />
                            </div>
                            <asp:Label ID="lblPacienteNombre"    runat="server" CssClass="paciente-card-nombre" Text="" />
                            <asp:Label ID="lblPacienteEdad"      runat="server" CssClass="paciente-card-dato"   Text="" />
                            <asp:Label ID="lblPacienteOcupacion" runat="server" CssClass="paciente-card-dato"   Text="" />
                            <asp:Label ID="lblPacienteEstado"    runat="server" CssClass="paciente-card-dato"   Text="" />

                            <div class="paciente-card-sep"></div>

                            <div class="paciente-stat-row">
                                <div class="paciente-stat">
                                    <asp:Label ID="lblTotalConsultas" runat="server" CssClass="stat-num" Text="--" />
                                    <span class="stat-label"><asp:Label ID="lblEtiquetaTotalConsultas" runat="server" Text="" /></span>
                                </div>
                                <div class="paciente-stat">
                                    <asp:Label ID="lblUltimaConsulta" runat="server" CssClass="stat-fecha" Text="--" />
                                    <span class="stat-label"><asp:Label ID="lblEtiquetaUltimaSesion" runat="server" Text="" /></span>
                                </div>
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoCardTexto" runat="server" Text="" /></p>
                        </div>

                        <div class="content-card historial-card">
                            <p class="historial-titulo"><asp:Label ID="lblTituloUltimasConsultas" runat="server" Text="" /></p>
                            <asp:Repeater ID="rptUltimasConsultas" runat="server">
                                <ItemTemplate>
                                    <div class="historial-item">
                                        <span class="historial-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></span>
                                        <span class="historial-resumen"><%# Eval("Resumen") %></span>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Label ID="lblSinConsultas" runat="server"
                                CssClass="historial-vacio"
                                Text=""
                                Visible="false" />
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </form>
</body>
</html>