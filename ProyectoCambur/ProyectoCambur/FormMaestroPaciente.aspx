<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormMaestroPaciente.aspx.cs" Inherits="FormMaestroPaciente" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Pacientes</title>
    <link href="EstilosPaginas/Shared.css"             rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"      rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"  rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormMaestroPaciente.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="pacientes" />
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
                        <h2 class="card-title"><asp:Label ID="lblFormTitulo" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblSubtituloForm" runat="server" Text="" /></p>
                    </div>

                    <asp:Label ID="lblSeccionVinculo" runat="server" CssClass="section-sep" Text="" />

                    <div class="grid-3">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaPsicologo" runat="server" AssociatedControlID="ddlPsicologo" Text="" />
                            <asp:DropDownList ID="ddlPsicologo" runat="server" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvPsicologo" runat="server"
                                ControlToValidate="ddlPsicologo" InitialValue=""
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                            <asp:Label ID="lblHintPsicologo" runat="server" CssClass="hint-text" Text="" />
                        </div>
                    </div>

                    <asp:Label ID="lblSeccionDatos" runat="server" CssClass="section-sep" Text="" />

                    <div class="grid-3">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaNombre" runat="server" AssociatedControlID="txtNombre" Text="" />
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                ControlToValidate="txtNombre" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaApellido" runat="server" AssociatedControlID="txtApellido" Text="" />
                            <asp:TextBox ID="txtApellido" runat="server" MaxLength="100" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                                ControlToValidate="txtApellido" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaDni" runat="server" AssociatedControlID="txtDni" Text="" />
                            <asp:TextBox ID="txtDni" runat="server" MaxLength="10" ClientIDMode="Static" placeholder="12.345.678" />
                            <asp:RegularExpressionValidator ID="revDni" runat="server"
                                ControlToValidate="txtDni"
                                ValidationExpression="^[0-9]{2}[.][0-9]{3}[.][0-9]{3}$"
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaFechaNacimiento" runat="server" AssociatedControlID="txtFechaNacimiento" Text="" />
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server"
                                ControlToValidate="txtFechaNacimiento" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaSexo" runat="server" AssociatedControlID="ddlSexo" Text="" />
                            <asp:DropDownList ID="ddlSexo" runat="server" ClientIDMode="Static">
                                <asp:ListItem Value="Femenino" Text="Femenino" />
                                <asp:ListItem Value="Masculino" Text="Masculino" />
                                <asp:ListItem Value="Otro" Text="Otro" />
                                <asp:ListItem Value="Prefiere no decir" Text="Prefiere no decir" />
                            </asp:DropDownList>
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaEstadoCivil" runat="server" AssociatedControlID="txtEstadoCivil" Text="" />
                            <asp:TextBox ID="txtEstadoCivil" runat="server" MaxLength="50" ClientIDMode="Static" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaOcupacion" runat="server" AssociatedControlID="txtOcupacion" Text="" />
                            <asp:TextBox ID="txtOcupacion" runat="server" MaxLength="100" ClientIDMode="Static" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaEmail" runat="server" AssociatedControlID="txtEmail" Text="" />
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" MaxLength="150" ClientIDMode="Static" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                ControlToValidate="txtEmail"
                                ValidationExpression="^[\w\.\-]+@[\w\-]+\.[a-zA-Z]{2,}$"
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaTelefono" runat="server" AssociatedControlID="txtTelefono" Text="" />
                            <asp:TextBox ID="txtTelefono" runat="server" MaxLength="30" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <asp:LinkButton ID="btnCancelarEdicion" runat="server"
                            CssClass="btn-secondary" Text=""
                            OnClick="btnCancelarEdicion_Click" CausesValidation="false" Visible="false" />
                        <asp:Button ID="btnGuardar" runat="server"
                            Text=""
                            CssClass="btn-primary"
                            ValidationGroup="vgPaciente"
                            OnClick="btnGuardar_Click" />
                    </div>
                </div>

                <asp:HiddenField ID="hdnIdPaciente" runat="server" Value="0" />

                <div class="content-card mt-24">
                    <div class="card-header-row">
                        <div class="card-header-left">
                            <h2 class="card-title"><asp:Label ID="lblTituloListado" runat="server" Text="" /></h2>
                            <div class="badges-row">
                                <asp:Label ID="lblBadgeActivos" runat="server" CssClass="badge-activos" Text="" />
                                <asp:Label ID="lblBadgeInactivos" runat="server" CssClass="badge-inactivos" Text="" />
                            </div>
                        </div>
                        <div class="filtro-estado">
                            <asp:Label ID="lblEtiquetaMostrar" runat="server" AssociatedControlID="ddlFiltroEstado" Text="" />
                            <asp:DropDownList ID="ddlFiltroEstado" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroEstado_SelectedIndexChanged">
                                <asp:ListItem Value="TODOS" Text="Todos" />
                                <asp:ListItem Value="ACTIVOS" Text="Activos" />
                                <asp:ListItem Value="INACTIVOS" Text="Desactivados" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="table-wrap">
                        <asp:GridView ID="gvPacientes" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            AllowPaging="True"
                            PageSize="50"
                            OnRowCommand="gvPacientes_RowCommand"
                            OnRowDataBound="gvPacientes_RowDataBound"
                            OnPageIndexChanging="gvPacientes_PageIndexChanging">

                            <PagerStyle CssClass="table-pager" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" />

                            <EmptyDataRowStyle CssClass="empty-row" />
                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />

                            <Columns>
                                <asp:BoundField DataField="NombreCompleto" HeaderText="Paciente" HeaderStyle-CssClass="th-left" />
                                <asp:BoundField DataField="Dni"            HeaderText="DNI"       HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />
                                <asp:BoundField DataField="NombrePsicologo" HeaderText="Psicólogo" HeaderStyle-CssClass="th-left" />
                                <asp:BoundField DataField="Email"          HeaderText="Email"     HeaderStyle-CssClass="th-left" />
                                <asp:BoundField DataField="FechaRegistro"  HeaderText="Registrado" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro"
                                    DataFormatString="{0:dd/MM/yyyy}" />

                                <asp:TemplateField HeaderText="Estado" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstadoPaciente" runat="server" CssClass="badge-estado" Text="" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbModificar" runat="server"
                                            CommandName="Modificar"
                                            CommandArgument='<%# Eval("IdPaciente") %>'
                                            CssClass="tbl-btn tbl-btn-mod" />

                                        <asp:LinkButton ID="lbBaja" runat="server"
                                            CommandName="DarBaja"
                                            CommandArgument='<%# Eval("IdPaciente") %>'
                                            CssClass='<%# (bool)Eval("Activo") ? "tbl-btn tbl-btn-baja" : "tbl-btn-hidden" %>'
                                            OnClientClick="return confirm('¿Confirmás dar de baja a este paciente?');" />

                                        <asp:LinkButton ID="lbReactivar" runat="server"
                                            CommandName="Reactivar"
                                            CommandArgument='<%# Eval("IdPaciente") %>'
                                            CssClass='<%# (bool)Eval("Activo") ? "tbl-btn-hidden" : "tbl-btn tbl-btn-reactivar" %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </div>
        </div>

    </form>
</body>
</html>
