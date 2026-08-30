<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormRegistrarPaciente.aspx.cs" Inherits="FormRegistrarPaciente" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Registrar Paciente</title>
    <link href="EstilosPaginas/Shared.css"               rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"        rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"    rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormRegistroPaciente.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline"><asp:Label ID="lblTaglineSidebar" runat="server" Text="Panel" /></div>
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_registrar_paciente" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout">🚪 <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <span class="header-section"><asp:Label ID="lblHeaderSeccion" runat="server" Text="Pacientes" /></span>
                    <span class="header-sep">/</span>
                    <span class="header-page"><asp:Label ID="lblHeaderPagina" runat="server" Text="Registrar paciente" /></span>
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <div class="content-card">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblFormTitulo" runat="server" Text="Nuevo paciente" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblFormSubtitulo" runat="server" Text="" /></p>
                    </div>

                    <div class="section-sep">Datos personales</div>

                    <div class="grid-3">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaNombre" runat="server" AssociatedControlID="txtNombre" Text="Nombre" />
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" placeholder="Ej: Martín" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaApellido" runat="server" AssociatedControlID="txtApellido" Text="Apellido" />
                            <asp:TextBox ID="txtApellido" runat="server" MaxLength="100" placeholder="Ej: González" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                                ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaDni" runat="server" AssociatedControlID="txtDni" Text="DNI" />
                            <asp:TextBox ID="txtDni" runat="server" MaxLength="10" placeholder="Ej: 12.345.678" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvDni" runat="server"
                                ControlToValidate="txtDni" ErrorMessage="El DNI es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                            <asp:RegularExpressionValidator ID="revDni" runat="server"
                                ControlToValidate="txtDni"
                                ValidationExpression="^[0-9]{2}[.][0-9]{3}[.][0-9]{3}$"
                                ErrorMessage="Formato de DNI inválido. Usá el formato 12.345.678."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaFechaNacimiento" runat="server" AssociatedControlID="txtFechaNacimiento" Text="Fecha de nacimiento" />
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" TextMode="Date" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvFecha" runat="server"
                                ControlToValidate="txtFechaNacimiento" ErrorMessage="La fecha es obligatoria."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaOcupacion" runat="server" AssociatedControlID="txtOcupacion" Text="Ocupación" />
                            <asp:TextBox ID="txtOcupacion" runat="server" MaxLength="150" placeholder="Ej: Docente" ClientIDMode="Static" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaEstadoCivil" runat="server" AssociatedControlID="ddlEstadoCivil" Text="Estado civil" />
                            <asp:DropDownList ID="ddlEstadoCivil" runat="server" ClientIDMode="Static">
                                <asp:ListItem Value=""              Text="Seleccioná..." />
                                <asp:ListItem Value="Soltero/a"     Text="Soltero/a" />
                                <asp:ListItem Value="Casado/a"      Text="Casado/a" />
                                <asp:ListItem Value="Divorciado/a"  Text="Divorciado/a" />
                                <asp:ListItem Value="Viudo/a"       Text="Viudo/a" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvEstado" runat="server"
                                ControlToValidate="ddlEstadoCivil" InitialValue=""
                                ErrorMessage="Seleccioná el estado civil."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaSexo" runat="server" Text="Sexo" />
                            <div class="radio-group">
                                <label class="radio-label">
                                    <asp:RadioButton ID="rbMasculino"   runat="server" GroupName="Sexo" Text="Masculino" />
                                </label>
                                <label class="radio-label">
                                    <asp:RadioButton ID="rbFemenino"    runat="server" GroupName="Sexo" Text="Femenino" />
                                </label>
                                <label class="radio-label">
                                    <asp:RadioButton ID="rbNoEspecifica" runat="server" GroupName="Sexo" Text="No especifica" />
                                </label>
                            </div>
                        </div>
                    </div>

                    <div class="section-sep">Contacto</div>

                    <div class="grid-2">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaEmail" runat="server" AssociatedControlID="txtEmail" Text="Correo electrónico" />
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" MaxLength="150"
                                placeholder="martin@email.com" ClientIDMode="Static" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                ControlToValidate="txtEmail"
                                ValidationExpression="^[\w\.\-]+@[\w\-]+\.[a-zA-Z]{2,}$"
                                ErrorMessage="Formato de correo inválido."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgPaciente" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaTelefono" runat="server" AssociatedControlID="txtTelefono" Text="Teléfono" />
                            <asp:TextBox ID="txtTelefono" runat="server" MaxLength="20"
                                placeholder="Ej: 11-2345-6789" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <a href="FormMenu.aspx" class="btn-secondary">Cancelar</a>
                        <asp:Button ID="btnRegistrar" runat="server"
                            Text="Registrar paciente"
                            CssClass="btn-primary"
                            ValidationGroup="vgPaciente"
                            OnClick="btnRegistrar_Click" />
                    </div>
                </div>

                <div class="content-card mt-24">

                    <div class="card-header-row">
                        <div class="card-header-left">
                            <h2 class="card-title"><asp:Label ID="lblTituloListado" runat="server" Text="Pacientes registrados" /></h2>
                            <div class="badges-row">
                                <asp:Label ID="lblBadgeActivos"   runat="server" CssClass="badge-activos"   Text="" />
                                <asp:Label ID="lblBadgeInactivos" runat="server" CssClass="badge-inactivos" Text="" />
                            </div>
                        </div>
                    </div>

                    <div class="table-wrap">
                        <asp:GridView ID="gvPacientes" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            EmptyDataText="Todavía no registraste ningún paciente."
                            OnRowCommand="gvPacientes_RowCommand"
                            OnRowDataBound="gvPacientes_RowDataBound">

                            <EmptyDataRowStyle CssClass="empty-row" />
                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />

                            <Columns>
                                <asp:BoundField  DataField="NombreCompleto" HeaderText="Paciente"      HeaderStyle-CssClass="th-left" />
                                <asp:BoundField  DataField="Dni"            HeaderText="DNI"           HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />
                                <asp:BoundField  DataField="Edad"           HeaderText="Edad"          HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />
                                <asp:BoundField  DataField="EstadoCivil"    HeaderText="Estado civil"  HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />
                                <asp:BoundField  DataField="FechaRegistro"  HeaderText="Registrado"    HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro"
                                    DataFormatString="{0:dd/MM/yyyy}" />

                                <asp:TemplateField HeaderText="Estado" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <asp:Label ID="lblEstadoPaciente" runat="server" CssClass="badge-estado" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>

                                        <a href='FormHistorialClinico.aspx?id=<%# Eval("IdPaciente") %>' class="tbl-btn tbl-btn-ver" title="Ver detalle">
                                            👁️ Ver
                                        </a>

                                        <asp:LinkButton ID="lbBaja" runat="server"
                                            CommandName="DarBaja"
                                            CommandArgument='<%# Eval("IdPaciente") %>'
                                            CssClass='<%# (bool)Eval("Activo") ? "tbl-btn tbl-btn-baja" : "tbl-btn-hidden" %>'
                                            Text="🚫 Dar de baja"
                                            OnClientClick="return confirm('¿Confirmás dar de baja a este paciente?');" />

                                        <asp:LinkButton ID="lbReactivar" runat="server"
                                            CommandName="Reactivar"
                                            CommandArgument='<%# Eval("IdPaciente") %>'
                                            CssClass='<%# (bool)Eval("Activo") ? "tbl-btn-hidden" : "tbl-btn tbl-btn-reactivar" %>'
                                            Text="✅ Reactivar" />

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