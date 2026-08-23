<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormAuditoriaBitacora.aspx.cs" Inherits="FormAuditoriaBitacora" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Bitácora</title>
    <link href="EstilosPaginas/Shared.css"                rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"         rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"     rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormAuditoriaBitacora.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="bitacora" />
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

       
                <div class="content-card">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblTituloFiltros" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblSubtituloFiltros" runat="server" Text="" /></p>
                    </div>

                    <div class="grid-3">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaFechaInicio" runat="server" AssociatedControlID="txtFechaInicio" Text="" />
                            <asp:TextBox ID="txtFechaInicio" runat="server" TextMode="Date" ClientIDMode="Static" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaFechaFin" runat="server" AssociatedControlID="txtFechaFin" Text="" />
                            <asp:TextBox ID="txtFechaFin" runat="server" TextMode="Date" ClientIDMode="Static" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaModulo" runat="server" AssociatedControlID="ddlModulo" Text="" />
                            <asp:DropDownList ID="ddlModulo" runat="server" ClientIDMode="Static" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaUsuario" runat="server" AssociatedControlID="ddlUsuario" Text="" />
                            <asp:DropDownList ID="ddlUsuario" runat="server" ClientIDMode="Static" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaCriticidad" runat="server" AssociatedControlID="ddlCriticidad" Text="" />
                            <asp:DropDownList ID="ddlCriticidad" runat="server" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <asp:LinkButton ID="btnLimpiarFiltros" runat="server"
                            CssClass="btn-secondary" Text=""
                            OnClick="btnLimpiarFiltros_Click" CausesValidation="false" />
                        <asp:Button ID="btnFiltrar" runat="server"
                            Text="" CssClass="btn-primary"
                            OnClick="btnFiltrar_Click" />
                    </div>
                </div>

          
                <div class="content-card mt-24">
                    <div class="card-header-row">
                        <h2 class="card-title"><asp:Label ID="lblTituloEventos" runat="server" Text="" /></h2>
                        <asp:Label ID="lblCantidadResultados" runat="server" CssClass="badge-activos" Text="" />
                    </div>

                    <div class="table-wrap">
                        <asp:GridView ID="gvBitacora" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            AllowPaging="True"
                            PageSize="50"
                            OnRowCommand="gvBitacora_RowCommand"
                            OnRowDataBound="gvBitacora_RowDataBound"
                            OnPageIndexChanging="gvBitacora_PageIndexChanging">

                            <PagerStyle CssClass="table-pager" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" />

                            <EmptyDataRowStyle CssClass="empty-row" />
                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />

                            <Columns>
                                <asp:BoundField DataField="Usuario"     HeaderText="Usuario"     HeaderStyle-CssClass="th-left" />
                                <asp:BoundField DataField="Modulo"      HeaderText="Módulo"      HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" HeaderStyle-CssClass="th-left" />

                                <asp:TemplateField HeaderText="Criticidad" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCriticidad" runat="server" Text="" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="FechaEvento" HeaderText="Fecha" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro"
                                    DataFormatString="{0:dd/MM/yyyy HH:mm}" />

                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbVerDetalle" runat="server"
                                            CommandName="VerDetalle"
                                            CommandArgument='<%# Eval("Usuario") %>'
                                            CssClass="tbl-btn tbl-btn-mod" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

      
                <asp:Panel ID="pnlDetalle" runat="server" CssClass="content-card mt-24" Visible="false">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblTituloDetalle" runat="server" Text="" /></h2>
                    </div>

                    <asp:Panel ID="pnlDetalleEncontrado" runat="server" Visible="false">
                        <div class="grid-3">
                            <div class="field">
                                <span class="detalle-etiqueta"><asp:Label ID="lblDetEtiquetaNombre" runat="server" Text="" /></span>
                                <span class="detalle-valor"><asp:Label ID="lblDetNombre" runat="server" Text="" /></span>
                            </div>
                            <div class="field">
                                <span class="detalle-etiqueta"><asp:Label ID="lblDetEtiquetaDni" runat="server" Text="" /></span>
                                <span class="detalle-valor"><asp:Label ID="lblDetDni" runat="server" Text="" /></span>
                            </div>
                            <div class="field">
                                <span class="detalle-etiqueta"><asp:Label ID="lblDetEtiquetaEmail" runat="server" Text="" /></span>
                                <span class="detalle-valor"><asp:Label ID="lblDetEmail" runat="server" Text="" /></span>
                            </div>
                            <div class="field">
                                <span class="detalle-etiqueta"><asp:Label ID="lblDetEtiquetaRol" runat="server" Text="" /></span>
                                <span class="detalle-valor"><asp:Label ID="lblDetRol" runat="server" Text="" /></span>
                            </div>
                            <div class="field">
                                <span class="detalle-etiqueta"><asp:Label ID="lblDetEtiquetaEstado" runat="server" Text="" /></span>
                                <span class="detalle-valor">
                                    <asp:Label ID="lblDetActivo" runat="server" CssClass="badge-estado" Text="" />
                                    <asp:Label ID="lblDetHabilitado" runat="server" CssClass="badge-estado" Text="" />
                                    <asp:Label ID="lblDetBloqueado" runat="server" CssClass="badge-estado bloqueado" Text="" Visible="false" />
                                </span>
                            </div>
                        </div>
                    </asp:Panel>

                    <asp:Panel ID="pnlDetalleNoEncontrado" runat="server" Visible="false">
                        <p class="card-subtitle"><asp:Label ID="lblDetalleNoEncontrado" runat="server" Text="" /></p>
                    </asp:Panel>
                </asp:Panel>

            </div>
        </div>

    </form>
</body>
</html>
