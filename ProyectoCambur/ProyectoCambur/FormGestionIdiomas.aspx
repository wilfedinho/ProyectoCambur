<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormGestionIdiomas.aspx.cs" Inherits="FormGestionIdiomas" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Gestión de idiomas</title>
    <link href="EstilosPaginas/Shared.css"             rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"      rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormGestionIdiomas.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <nav class="sidebar-nav">
                <a href="FormMenuAdministrador.aspx"  class="nav-item"><span>🏠</span> <asp:Label ID="lblMenuInicio" runat="server" Text="" /></a>
                <a href="FormMaestroProfesional.aspx" class="nav-item"><span>👥</span> <asp:Label ID="lblMenuProfesionales" runat="server" Text="" /></a>
                <a href="FormGestionIdiomas.aspx"      class="nav-item active"><span>🌐</span> <asp:Label ID="lblMenuIdiomas" runat="server" Text="" /></a>
                <a href="FormAuditoriaBitacora.aspx"   class="nav-item"><span>📜</span> <asp:Label ID="lblMenuBitacora" runat="server" Text="" /></a>
            </nav>
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
                        <h2 class="card-title"><asp:Label ID="lblTituloNuevoIdioma" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblSubtituloNuevoIdioma" runat="server" Text="" /></p>
                    </div>

                    <div class="grid-3">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaNombreIdioma" runat="server" AssociatedControlID="txtNombreIdioma" Text="" />
                            <asp:TextBox ID="txtNombreIdioma" runat="server" MaxLength="50" placeholder="Ej: English" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvNombreIdioma" runat="server"
                                ControlToValidate="txtNombreIdioma" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgIdioma" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaCodigoIso" runat="server" AssociatedControlID="txtCodigoIso" Text="" />
                            <asp:TextBox ID="txtCodigoIso" runat="server" MaxLength="5" placeholder="Ej: en" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvCodigoIso" runat="server"
                                ControlToValidate="txtCodigoIso" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgIdioma" />
                            <asp:Label ID="lblHintCodigoIso" runat="server" CssClass="hint-text" Text="" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <asp:Button ID="btnAltaIdioma" runat="server"
                            Text=""
                            CssClass="btn-primary"
                            ValidationGroup="vgIdioma"
                            OnClick="btnAltaIdioma_Click" />
                    </div>
                </div>

           
                <div class="content-card mt-24">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblTituloIdiomasSistema" runat="server" Text="" /></h2>
                    </div>

                    <div class="table-wrap">
                        <asp:GridView ID="gvIdiomas" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            OnRowCommand="gvIdiomas_RowCommand"
                            OnRowDataBound="gvIdiomas_RowDataBound">

                            <EmptyDataRowStyle CssClass="empty-row" />
                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />

                            <Columns>
                                <asp:BoundField DataField="NombreIdioma" HeaderText="Idioma"     HeaderStyle-CssClass="th-left" />
                                <asp:BoundField DataField="CodigoIso"    HeaderText="Código ISO" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />

                                <asp:TemplateField HeaderText="Estado" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstadoIdioma" runat="server" CssClass="badge-estado" Text="" />
                                        <asp:Label ID="lblEnUso" runat="server" CssClass="badge-estado bloqueado" Text="" Visible="false" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDesactivar" runat="server"
                                            CommandName="Desactivar"
                                            CommandArgument='<%# Eval("NombreIdioma") %>'
                                            CssClass='<%# (bool)Eval("IsDisponible") ? "tbl-btn tbl-btn-baja" : "tbl-btn-hidden" %>'
                                            OnClientClick="return confirm('¿Confirmás desactivar este idioma? Solo se puede si ningún profesional activo lo está usando.');" />

                                        <asp:LinkButton ID="lbActivar" runat="server"
                                            CommandName="Activar"
                                            CommandArgument='<%# Eval("NombreIdioma") %>'
                                            CssClass='<%# (bool)Eval("IsDisponible") ? "tbl-btn-hidden" : "tbl-btn tbl-btn-reactivar" %>' />

                                        <asp:LinkButton ID="lbVerPendientes" runat="server"
                                            CommandName="VerPendientes"
                                            CommandArgument='<%# Eval("NombreIdioma") %>'
                                            CssClass="tbl-btn tbl-btn-mod" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

           
                <asp:Panel ID="pnlTraducciones" runat="server" CssClass="content-card mt-24" Visible="false">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblTituloTraducciones" runat="server" Text="" /> — <asp:Label ID="lblIdiomaSeleccionado" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblSubtituloTraducciones" runat="server" Text="" /></p>
                    </div>

                    <div class="table-wrap">
                        <asp:GridView ID="gvTraducciones" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            OnRowCommand="gvTraducciones_RowCommand"
                            OnRowDataBound="gvTraducciones_RowDataBound">

                            <EmptyDataRowStyle CssClass="empty-row" />
                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />

                            <Columns>
                                <asp:BoundField DataField="Clave" HeaderText="Clave" HeaderStyle-CssClass="th-left" />

                                <asp:TemplateField HeaderText="Texto" HeaderStyle-CssClass="th-left">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtTexto" runat="server" Text='<%# Eval("Texto") %>' CssClass="txt-inline" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Estado" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstadoTraduccion" runat="server" CssClass="badge-estado" Text="" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbGuardarTraduccion" runat="server"
                                            CommandName="GuardarTraduccion"
                                            CommandArgument='<%# Eval("IdTraduccion") %>'
                                            CssClass="tbl-btn tbl-btn-mod" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </asp:Panel>

            </div>
        </div>

    </form>
</body>
</html>
