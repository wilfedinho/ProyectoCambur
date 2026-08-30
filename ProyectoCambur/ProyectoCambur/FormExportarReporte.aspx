<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormExportarReporte.aspx.cs" Inherits="FormExportarReporte" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Exportar Reporte</title>
    <link href="EstilosPaginas/Shared.css"               rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"        rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"    rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormExportarReporte.css"  rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormExportarReporte_CU12_Extra.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Gestión Clínica</div>
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_exportar_reporte" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Documentación</span>
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderTitulo" runat="server" CssClass="header-page" Text="Exportar reporte" />
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <div class="exportar-layout">

                    <div class="exportar-main">

                        <div class="content-card">
                            <div class="card-header">
                                <h2 class="card-title"><asp:Label ID="lblCardTitulo" runat="server" Text="" /></h2>
                                <p class="card-subtitle"><asp:Label ID="lblCardSubtitulo" runat="server" Text="" /></p>
                            </div>

                            <asp:Label ID="lblSeccionPaciente" runat="server" CssClass="section-sep" Text="" />

                            <div class="grid-2" style="max-width:500px;">
                                <div class="field full">
                                    <asp:Label ID="lblEtiquetaPaciente" runat="server" AssociatedControlID="ddlPaciente" Text="" />
                                    <asp:DropDownList ID="ddlPaciente" runat="server"
                                        ClientIDMode="Static" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPaciente_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <asp:Label ID="lblSeccionTipoDoc" runat="server" CssClass="section-sep" Text="" />

                            <asp:HiddenField ID="hfTipoSeleccionado" runat="server" Value="" ClientIDMode="Static" />

                            <div class="documentos-grid">

                                <div class="doc-card doc-card-seleccionable" id="docResumen">
                                    <div class="doc-icono">🤖</div>
                                    <div class="doc-info">
                                        <asp:Label ID="lblTipoResumen" runat="server" CssClass="doc-tipo" Text="" />
                                        <asp:Label ID="lblFechaResumen" runat="server" CssClass="doc-meta" Text="" />
                                        <asp:Label ID="lblEstadoResumen" runat="server" CssClass="doc-badge doc-badge-ok" Text="" />
                                    </div>
                                </div>

                                <div class="doc-card doc-card-disabled">
                                    <div class="doc-icono">📤</div>
                                    <div class="doc-info">
                                        <asp:Label ID="lblTipoDerivacion" runat="server" CssClass="doc-tipo" Text="" />
                                        <asp:Label ID="lblFechaDerivacion" runat="server" CssClass="doc-meta" Text="" />
                                        <asp:Label ID="lblProximamenteDerivacion" runat="server" CssClass="doc-badge doc-badge-proximamente" Text="" />
                                    </div>
                                </div>

                                <div class="doc-card doc-card-disabled">
                                    <div class="doc-icono">🧠</div>
                                    <div class="doc-info">
                                        <asp:Label ID="lblTipoPerfil" runat="server" CssClass="doc-tipo" Text="" />
                                        <asp:Label ID="lblFechaPerfil" runat="server" CssClass="doc-meta" Text="" />
                                        <asp:Label ID="lblProximamentePerfil" runat="server" CssClass="doc-badge doc-badge-proximamente" Text="" />
                                    </div>
                                </div>

                            </div>

                            <div class="form-actions">
                                <asp:HyperLink ID="lnkVolver" runat="server" NavigateUrl="~/FormDashboard.aspx" CssClass="btn-secondary" Text="" />
                                <asp:Button ID="btnExportar" runat="server"
                                    Text="📄 Exportar en PDF"
                                    CssClass="btn-primary btn-exportar"
                                    OnClick="btnExportar_Click"
                                    CausesValidation="false" />
                            </div>
                        </div>

                    </div>

                    <div class="exportar-aside">

                        <div class="content-card historial-exportaciones-card">
                            <p class="accesos-titulo"><asp:Label ID="lblExportacionesRecientesTitulo" runat="server" Text="" /></p>
                            <asp:Repeater ID="rptExportaciones" runat="server">
                                <ItemTemplate>
                                    <div class="exp-item">
                                        <span class="exp-icono"><%# Eval("Icono") %></span>
                                        <div class="exp-info">
                                            <span class="exp-tipo"><%# Eval("Tipo") %></span>
                                            <span class="exp-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy HH:mm}") %></span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Label ID="lblSinExportaciones" runat="server"
                                CssClass="sin-perfiles-txt" Visible="false" Text="" />
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">📄</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoFormatoTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoFormatoTexto" runat="server" Text="" /></p>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoProteccionTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoProteccionTexto" runat="server" Text="" /></p>
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </form>
</body>
</html>