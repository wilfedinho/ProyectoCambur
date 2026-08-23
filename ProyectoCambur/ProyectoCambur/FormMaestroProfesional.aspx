<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormMaestroProfesional.aspx.cs" Inherits="FormMaestroProfesional" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Profesionales</title>
    <link href="EstilosPaginas/Shared.css"                rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"          rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"      rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormMaestroProfesional.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline"><asp:Label ID="lblTaglineSidebar" runat="server" Text="Panel" /></div>
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="profesionales" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Profesionales</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">ABM de profesionales</span>
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <asp:HiddenField ID="hdnIdPsicologo" runat="server" Value="0" />

                <div class="content-card">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblFormTitulo" runat="server" Text="Nuevo profesional" /></h2>
                        <p class="card-subtitle">Completá los datos del profesional. El rol determina a qué menú accede al iniciar sesión.</p>
                    </div>

                    <div class="section-sep">Datos personales</div>

                    <div class="grid-3">
                        <div class="field">
                            <label for="txtNombre">Nombre <sup>*</sup></label>
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" placeholder="Ej: Lucía" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                        </div>

                        <div class="field">
                            <label for="txtApellido">Apellido <sup>*</sup></label>
                            <asp:TextBox ID="txtApellido" runat="server" MaxLength="100" placeholder="Ej: Martínez" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                                ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                        </div>

                        <div class="field">
                            <label for="txtDni">DNI <sup>*</sup></label>
                            <asp:TextBox ID="txtDni" runat="server" MaxLength="10" placeholder="Ej: 12.345.678" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvDni" runat="server"
                                ControlToValidate="txtDni" ErrorMessage="El DNI es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                            <asp:RegularExpressionValidator ID="revDni" runat="server"
                                ControlToValidate="txtDni"
                                ValidationExpression="^[0-9]{2}[.][0-9]{3}[.][0-9]{3}$"
                                ErrorMessage="Formato esperado: 12.345.678"
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                        </div>
                    </div>

                    <div class="section-sep">Acceso al sistema</div>

                    <div class="grid-3">
                        <div class="field">
                            <label for="txtEmail">Correo electrónico <sup>*</sup></label>
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" MaxLength="150" placeholder="lucia@cambur.com" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                                ControlToValidate="txtEmail" ErrorMessage="El email es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                ControlToValidate="txtEmail"
                                ValidationExpression="^[\w\.\-]+@[\w\-]+\.[a-zA-Z]{2,}$"
                                ErrorMessage="Formato de correo inválido."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                        </div>

                        <div class="field">
                            <label for="ddlIdioma">Idioma</label>
                            <asp:DropDownList ID="ddlIdioma" runat="server" ClientIDMode="Static">
                                <asp:ListItem Value="Español" Text="Español" Selected="True" />
                                <asp:ListItem Value="English" Text="English" />
                            </asp:DropDownList>
                        </div>

                        <div class="field">
                            <label for="ddlRol">Rol / Plan <sup>*</sup></label>
                            <asp:DropDownList ID="ddlRol" runat="server" ClientIDMode="Static">
                                <asp:ListItem Value=""              Text="Seleccioná..." />
                                <asp:ListItem Value="Web Master"    Text="Web Master" />
                                <asp:ListItem Value="Administrador" Text="Administrador" />
                                <asp:ListItem Value="Free"          Text="Psicólogo — Plan Free" />
                                <asp:ListItem Value="Profesional"   Text="Psicólogo — Plan Profesional" />
                                <asp:ListItem Value="Premium"       Text="Psicólogo — Plan Premium" />
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvRol" runat="server"
                                ControlToValidate="ddlRol" InitialValue=""
                                ErrorMessage="Seleccioná un rol."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                        </div>
                    </div>

                    <asp:Panel ID="pnlAvisoContrasena" runat="server" Visible="false" CssClass="aviso-contrasena">
                        🔑 La contraseña inicial se genera automáticamente como <strong>DNI+Email</strong>. Comunicásela al profesional para que la cambie en su primer ingreso.
                    </asp:Panel>

                    <div class="form-actions">
                        <asp:LinkButton ID="btnCancelarEdicion" runat="server"
                            CssClass="btn-secondary" Text="Cancelar edición"
                            OnClick="btnCancelarEdicion_Click" CausesValidation="false" Visible="false" />
                        <asp:Button ID="btnGuardar" runat="server"
                            Text="Registrar profesional"
                            CssClass="btn-primary"
                            ValidationGroup="vgProfesional"
                            OnClick="btnGuardar_Click" />
                    </div>
                </div>

                <div class="content-card mt-24">

                    <div class="card-header-row">
                        <div class="card-header-left">
                            <h2 class="card-title"><asp:Label ID="lblTituloListado" runat="server" Text="Profesionales registrados" /></h2>
                            <div class="badges-row">
                                <asp:Label ID="lblBadgeActivos"   runat="server" CssClass="badge-activos"   Text="" />
                                <asp:Label ID="lblBadgeInactivos" runat="server" CssClass="badge-inactivos" Text="" />
                            </div>
                        </div>
                        <div class="filtro-estado">
                            <asp:Label ID="lblEtiquetaMostrar" runat="server" AssociatedControlID="ddlFiltroEstado" Text="Mostrar:" />
                            <asp:DropDownList ID="ddlFiltroEstado" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroEstado_SelectedIndexChanged">
                                <asp:ListItem Value="TODOS"      Text="Todos" />
                                <asp:ListItem Value="ACTIVOS"    Text="Activos" />
                                <asp:ListItem Value="INACTIVOS"  Text="Desactivados" />
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="table-wrap">
                        <asp:GridView ID="gvProfesionales" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            EmptyDataText="No hay profesionales para mostrar con este filtro."
                            OnRowCommand="gvProfesionales_RowCommand">

                            <EmptyDataRowStyle CssClass="empty-row" />
                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />

                            <Columns>
                                <asp:BoundField DataField="NombreCompleto" HeaderText="Profesional" HeaderStyle-CssClass="th-left" />
                                <asp:BoundField DataField="Dni"            HeaderText="DNI"          HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />
                                <asp:BoundField DataField="Email"          HeaderText="Email"        HeaderStyle-CssClass="th-left" />
                                <asp:BoundField DataField="Idioma"         HeaderText="Idioma"       HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />

                                <asp:TemplateField HeaderText="Rol / Plan" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <span class="badge-rol"><%# Eval("RolPermiso") %></span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="FechaRegistro" HeaderText="Registrado" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro"
                                    DataFormatString="{0:dd/MM/yyyy}" />

                                <asp:TemplateField HeaderText="Estado" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <span class='<%# (bool)Eval("Activo") ? "badge-estado activo" : "badge-estado inactivo" %>'>
                                            <%# (bool)Eval("Activo") ? "Activo" : "Inactivo" %>
                                        </span>
                                        <br />
                                        <span class='<%# (bool)Eval("IsHabilitado") ? "badge-estado activo" : "badge-estado inactivo" %>'>
                                            <%# (bool)Eval("IsHabilitado") ? "Habilitado" : "Deshabilitado" %>
                                        </span>
                                        <asp:Label runat="server" CssClass="badge-estado bloqueado"
                                            Visible='<%# (bool)Eval("IsBloqueado") %>' Text="Bloqueado" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>

                                        <asp:LinkButton ID="lbModificar" runat="server"
                                            CommandName="Modificar"
                                            CommandArgument='<%# Eval("IdPsicologo") %>'
                                            CssClass="tbl-btn tbl-btn-mod"
                                            Text="✏️ Modificar" />

                                        <asp:LinkButton ID="lbBaja" runat="server"
                                            CommandName="DarBaja"
                                            CommandArgument='<%# Eval("IdPsicologo") %>'
                                            CssClass='<%# (bool)Eval("Activo") ? "tbl-btn tbl-btn-baja" : "tbl-btn-hidden" %>'
                                            Text="🚫 Dar de baja"
                                            OnClientClick="return confirm('¿Confirmás dar de baja a este profesional?');" />

                                        <asp:LinkButton ID="lbReactivar" runat="server"
                                            CommandName="Reactivar"
                                            CommandArgument='<%# Eval("IdPsicologo") %>'
                                            CssClass='<%# (bool)Eval("Activo") ? "tbl-btn-hidden" : "tbl-btn tbl-btn-reactivar" %>'
                                            Text="✅ Reactivar" />

                                        <asp:LinkButton ID="lbDeshabilitar" runat="server"
                                            CommandName="Deshabilitar"
                                            CommandArgument='<%# Eval("IdPsicologo") %>'
                                            CssClass='<%# (bool)Eval("IsHabilitado") ? "tbl-btn tbl-btn-baja" : "tbl-btn-hidden" %>'
                                            Text="⛔ Deshabilitar"
                                            OnClientClick="return confirm('¿Confirmás deshabilitar a este profesional? No va a poder iniciar sesión.');" />

                                        <asp:LinkButton ID="lbHabilitar" runat="server"
                                            CommandName="Habilitar"
                                            CommandArgument='<%# Eval("IdPsicologo") %>'
                                            CssClass='<%# (bool)Eval("IsHabilitado") ? "tbl-btn-hidden" : "tbl-btn tbl-btn-reactivar" %>'
                                            Text="✅ Habilitar" />

                                        <asp:LinkButton ID="lbDesbloquear" runat="server"
                                            CommandName="Desbloquear"
                                            CommandArgument='<%# Eval("IdPsicologo") %>'
                                            CssClass='<%# (bool)Eval("IsBloqueado") ? "tbl-btn tbl-btn-reactivar" : "tbl-btn-hidden" %>'
                                            Text="🔓 Desbloquear"
                                            OnClientClick="return confirm('Se va a resetear la contraseña a DNI+Email. ¿Confirmás?');" />

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
