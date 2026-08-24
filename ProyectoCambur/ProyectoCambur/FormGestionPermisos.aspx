<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormGestionPermisos.aspx.cs" Inherits="FormGestionPermisos" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Gestión de permisos</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"       rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"   rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormGestionPermisos.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="permisos" />
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
                        <h2 class="card-title"><asp:Label ID="lblTituloAltas" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblSubtituloAltas" runat="server" Text="" /></p>
                    </div>

                    <div class="grid-2">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaNuevaFamilia" runat="server" AssociatedControlID="txtNuevaFamilia" Text="" />
                            <asp:TextBox ID="txtNuevaFamilia" runat="server" MaxLength="100" ClientIDMode="Static" placeholder="ej: ModuloReportes" />
                            <asp:Button ID="btnAltaFamilia" runat="server" Text="" CssClass="btn-secondary mt-8" OnClick="btnAltaFamilia_Click" CausesValidation="false" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaNuevoPerfil" runat="server" AssociatedControlID="txtNuevoPerfil" Text="" />
                            <asp:TextBox ID="txtNuevoPerfil" runat="server" MaxLength="100" ClientIDMode="Static" placeholder="ej: Supervisor" />
                            <asp:Button ID="btnAltaPerfil" runat="server" Text="" CssClass="btn-secondary mt-8" OnClick="btnAltaPerfil_Click" CausesValidation="false" />
                        </div>
                    </div>
                </div>

            
                <div class="content-card mt-24">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblTituloEstructura" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblSubtituloEstructura" runat="server" Text="" /></p>
                    </div>

                    <div class="grid-3">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaTipo" runat="server" AssociatedControlID="ddlTipoElemento" Text="" />
                            <asp:DropDownList ID="ddlTipoElemento" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlTipoElemento_SelectedIndexChanged">
                                <asp:ListItem Value="Perfil" Text="Perfil (Rol)" />
                                <asp:ListItem Value="Familia" Text="Familia" />
                            </asp:DropDownList>
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaElemento" runat="server" AssociatedControlID="ddlElemento" Text="" />
                            <asp:DropDownList ID="ddlElemento" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlElemento_SelectedIndexChanged" />
                        </div>

                        <div class="field">
                            <asp:LinkButton ID="btnBorrarElementoSeleccionado" runat="server"
                                CssClass="tbl-btn tbl-btn-baja mt-24" Text=""
                                OnClick="btnBorrarElementoSeleccionado_Click"
                                OnClientClick="return confirm('¿Confirmás borrar esto? Se borra también toda su estructura interna.');"
                                CausesValidation="false" />
                        </div>
                    </div>
                </div>

                <asp:Panel ID="pnlEstructura" runat="server" Visible="false">

               
                    <div class="content-card mt-24">
                        <div class="card-header">
                            <h2 class="card-title"><asp:Label ID="lblTituloHijosDirectos" runat="server" Text="" /> — <asp:Label ID="lblNombreSeleccionado" runat="server" Text="" /></h2>
                            <p class="card-subtitle"><asp:Label ID="lblSubtituloHijosDirectos" runat="server" Text="" /></p>
                        </div>

                        <div class="table-wrap">
                            <asp:GridView ID="gvHijosDirectos" runat="server"
                                CssClass="data-table"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                OnRowCommand="gvHijosDirectos_RowCommand"
                                OnRowDataBound="gvHijosDirectos_RowDataBound">

                                <EmptyDataRowStyle CssClass="empty-row" />
                                <HeaderStyle      CssClass="table-header" />
                                <RowStyle         CssClass="table-row" />
                                <AlternatingRowStyle CssClass="table-row table-row-alt" />

                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" HeaderStyle-CssClass="th-left" />

                                    <asp:TemplateField HeaderText="Tipo" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTipoHijo" runat="server" CssClass="badge-rol" Text="" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbQuitar" runat="server"
                                                CommandName="Quitar"
                                                CommandArgument='<%# Eval("Nombre") + "|" + Eval("EsFamilia") %>'
                                                CssClass="tbl-btn tbl-btn-baja" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="grid-3 mt-24">
                            <div class="field">
                                <asp:Label ID="lblEtiquetaAgregar" runat="server" AssociatedControlID="ddlElementoParaAgregar" Text="" />
                                <asp:DropDownList ID="ddlElementoParaAgregar" runat="server" ClientIDMode="Static" />
                            </div>
                            <div class="field">
                                <asp:Button ID="btnAgregarElemento" runat="server" Text="" CssClass="btn-primary mt-24" OnClick="btnAgregarElemento_Click" CausesValidation="false" />
                            </div>
                        </div>
                    </div>

                
                    <div class="content-card mt-24">
                        <div class="card-header">
                            <h2 class="card-title"><asp:Label ID="lblTituloArbolCompleto" runat="server" Text="" /></h2>
                            <p class="card-subtitle"><asp:Label ID="lblSubtituloArbolCompleto" runat="server" Text="" /></p>
                        </div>

                        <asp:TreeView ID="tvEstructuraCompleta" runat="server" CssClass="arbol-permisos" />
                    </div>

                </asp:Panel>

            </div>
        </div>

    </form>
</body>
</html>
