<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormMaestroProfesional.aspx.cs" Inherits="FormMaestroProfesional" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/AyudaRapida.ascx" TagPrefix="uc" TagName="AyudaRapida" %>
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
    <link href="EstilosPaginas/AyudaRapida.css"      rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <input type="checkbox" id="chkMenuMovil" class="chk-menu-movil" />
        <label for="chkMenuMovil" class="btn-menu-movil" aria-label='<%= Traducir("aria_abrir_menu") %>'>☰</label>
        <label for="chkMenuMovil" class="overlay-menu-movil"></label>

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline"><asp:Label ID="lblTaglineSidebar" runat="server" Text="Panel" /></div>
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="profesionales" />
            <div class="sidebar-footer">
                <a href="FormLanding.aspx" class="nav-item nav-landing"><span>🏢</span> <asp:Label ID="lblMenuLandingSidebar" runat="server" Text="" /></a>
                <a href="FormCambiarIdioma.aspx" class="nav-item"><span>🌐</span> <%= Traducir("menu_cambiar_idioma") %></a>
<a href="FormCambiarClave.aspx" class="nav-item"><span>🔑</span> <%= Traducir("menu_cambiar_clave") %></a>
<a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <%= Traducir("menu_cerrar_sesion") %></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <asp:Label ID="lblHeaderSeccion" runat="server" CssClass="header-section" Text="" />
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderPagina" runat="server" CssClass="header-page" Text="" />
                </div>
                <uc:AyudaRapida ID="ucAyudaRapida" runat="server" ClaveAyuda="abm_profesionales" />
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <asp:HiddenField ID="hdnIdPsicologo" runat="server" Value="0" />
                <asp:HiddenField ID="hdnModo" runat="server" Value="GESTION" />

                <div class="modo-toolbar">
                    <asp:LinkButton ID="btnModoGestion" runat="server" CssClass="btn-modo active"
                        Text="👥 Gestión" OnClick="btnModoGestion_Click" CausesValidation="false" />
                    <asp:LinkButton ID="btnModoSerializar" runat="server" CssClass="btn-modo"
                        Text="🗄️ Serializar a XML" OnClick="btnModoSerializar_Click" CausesValidation="false" />
                    <asp:LinkButton ID="btnModoDeserializar" runat="server" CssClass="btn-modo"
                        Text="📥 Deserializar XML" OnClick="btnModoDeserializar_Click" CausesValidation="false" />
                </div>

                <asp:Panel ID="pnlModoGestion" runat="server">

                <div class="content-card">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblFormTitulo" runat="server" Text="Nuevo profesional" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblFormSubtitulo" runat="server" Text="" /></p>
                    </div>

                    <div class="section-sep"><asp:Label ID="lblSeccionDatosPersonales" runat="server" Text="" /></div>

                    <div class="grid-3">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaNombreProf" runat="server" AssociatedControlID="txtNombre" Text="" />
                            <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" placeholder="Ej: Lucía" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                ControlToValidate="txtNombre" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaApellidoProf" runat="server" AssociatedControlID="txtApellido" Text="" />
                            <asp:TextBox ID="txtApellido" runat="server" MaxLength="100" placeholder="Ej: Martínez" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                                ControlToValidate="txtApellido" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaDniProf" runat="server" AssociatedControlID="txtDni" Text="" />
                            <asp:TextBox ID="txtDni" runat="server" MaxLength="10" placeholder="Ej: 12.345.678" ClientIDMode="Static" oninput="aplicarMascaraDni(this)" />
                            <asp:RequiredFieldValidator ID="rfvDni" runat="server"
                                ControlToValidate="txtDni" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                            <asp:RegularExpressionValidator ID="revDni" runat="server"
                                ControlToValidate="txtDni"
                                ValidationExpression="^[0-9]{2}[.][0-9]{3}[.][0-9]{3}$"
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                        </div>
                    </div>

                    <div class="section-sep"><asp:Label ID="lblSeccionAccesoSistema" runat="server" Text="" /></div>

                    <div class="grid-3 grid-acceso-sistema">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaEmailProf" runat="server" AssociatedControlID="txtEmail" Text="" />
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" MaxLength="150" placeholder="lucia@cambur.com" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                                ControlToValidate="txtEmail" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                            <asp:RegularExpressionValidator ID="revEmail" runat="server"
                                ControlToValidate="txtEmail"
                                ValidationExpression="^[\w\.\-]+@[\w\-]+\.[a-zA-Z]{2,}$"
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaIdiomaProf" runat="server" AssociatedControlID="ddlIdioma" Text="" />
                            <asp:DropDownList ID="ddlIdioma" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaRolPlanProf" runat="server" AssociatedControlID="ddlRol" Text="" />
                            <asp:DropDownList ID="ddlRol" runat="server" ClientIDMode="Static">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvRol" runat="server"
                                ControlToValidate="ddlRol" InitialValue=""
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgProfesional" />
                        </div>
                    </div>

                    <asp:Panel ID="pnlAvisoContrasena" runat="server" Visible="false" CssClass="aviso-contrasena">
                        <asp:Label ID="lblAvisoContrasena" runat="server" Text="" />
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
                            <asp:Label ID="lblEtiquetaCategoria" runat="server" AssociatedControlID="ddlFiltroCategoria" Text="" />
                            <asp:DropDownList ID="ddlFiltroCategoria" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroCategoria_SelectedIndexChanged">
                                <asp:ListItem Value="TODOS"     Text="Todos" />
                                <asp:ListItem Value="NEGOCIO"   Text="Profesionales" />
                                <asp:ListItem Value="SISTEMA"   Text="Sistema" />
                            </asp:DropDownList>
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
                                            <%# (bool)Eval("Activo") ? Traducir("estado_activo") : Traducir("estado_inactivo") %>
                                        </span>
                                        <br />
                                        <span class='<%# (bool)Eval("IsHabilitado") ? "badge-estado activo" : "badge-estado inactivo" %>'>
                                            <%# (bool)Eval("IsHabilitado") ? Traducir("estado_habilitado") : Traducir("estado_deshabilitado") %>
                                        </span>
                                        <asp:Label runat="server" CssClass="badge-estado bloqueado"
                                            Visible='<%# (bool)Eval("IsBloqueado") %>' Text='<%# Traducir("estado_bloqueado") %>' />
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

                </asp:Panel>

                <asp:Panel ID="pnlModoSerializar" runat="server" Visible="false">
                    <div class="content-card">
                        <div class="card-header">
                            <h2 class="card-title"><asp:Label ID="lblTituloSerializar" runat="server" Text="Exportar profesionales a XML" /></h2>
                            <p class="card-subtitle"><asp:Label ID="lblSubtituloSerializar" runat="server" Text="Seleccioná los profesionales que querés exportar. El DNI y el email se guardan encriptados dentro del archivo." /></p>
                        </div>

                        <asp:Label ID="lblMensajeSerializar" runat="server" Visible="false" CssClass="server-error" />

                        <div class="table-wrap">
                            <asp:GridView ID="gvSeleccionSerializar" runat="server"
                                CssClass="data-table"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                DataKeyNames="IdPsicologo"
                                EmptyDataText="No hay profesionales para exportar.">

                                <EmptyDataRowStyle CssClass="empty-row" />
                                <HeaderStyle      CssClass="table-header" />
                                <RowStyle         CssClass="table-row" />
                                <AlternatingRowStyle CssClass="table-row table-row-alt" />

                                <Columns>
                                    <asp:TemplateField HeaderText="Seleccionar" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSeleccionar" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NombreCompleto" HeaderText="Profesional" HeaderStyle-CssClass="th-left" />
                                    <asp:BoundField DataField="Dni"            HeaderText="DNI"          HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />
                                    <asp:BoundField DataField="Email"          HeaderText="Email"        HeaderStyle-CssClass="th-left" />
                                    <asp:TemplateField HeaderText="Rol / Plan" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                        <ItemTemplate>
                                            <span class="badge-rol"><%# Eval("RolPermiso") %></span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="FechaRegistro" HeaderText="Registrado" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                </Columns>
                            </asp:GridView>
                        </div>

                        <div class="form-actions">
                            <asp:LinkButton ID="btnVolverGestionSerializar" runat="server"
                                CssClass="btn-secondary" Text="Volver a gestión"
                                OnClick="btnModoGestion_Click" CausesValidation="false" />
                            <asp:Button ID="btnExportarSeleccionados" runat="server"
                                Text="⬇️ Exportar seleccionados"
                                CssClass="btn-primary"
                                OnClick="btnExportarSeleccionados_Click" CausesValidation="false" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel ID="pnlModoDeserializar" runat="server" Visible="false">
                    <div class="content-card">
                        <div class="card-header">
                            <h2 class="card-title"><asp:Label ID="lblTituloDeserializar" runat="server" Text="Importar vista previa desde XML" /></h2>
                            <p class="card-subtitle"><asp:Label ID="lblSubtituloDeserializar" runat="server" Text="Seleccioná un archivo XML exportado por Cambur. Vas a poder ver los profesionales que contiene, con el DNI y el email desencriptados." /></p>
                        </div>

                        <asp:Label ID="lblMensajeDeserializar" runat="server" Visible="false" CssClass="server-error" />

                        <div class="grid-3">
                            <div class="field">
                                <label for="fuArchivoXml"><asp:Label ID="lblEtiquetaArchivoXml" runat="server" Text="Archivo XML" AssociatedControlID="fuArchivoXml" /></label>
                                <div class="file-input-wrap">
                                    <asp:FileUpload ID="fuArchivoXml" runat="server" accept=".xml" ClientIDMode="Static"
                                        CssClass="file-input-native" onchange="cambur_ActualizarNombreArchivoXml(this)" />
                                    <span class="file-input-boton"><asp:Label ID="lblBtnElegirArchivo" runat="server" Text="Elegir archivo" /></span>
                                    <asp:Label ID="lblNombreArchivoXml" runat="server" ClientIDMode="Static" CssClass="file-input-nombre" Text="Ningún archivo seleccionado" />
                                </div>
                                <script type="text/javascript">
                                    function cambur_ActualizarNombreArchivoXml(input) {
                                        var span = document.getElementById('lblNombreArchivoXml');
                                        if (!span) return;
                                        if (input.files && input.files.length > 0) {
                                            span.textContent = input.files[0].name;
                                        }
                                    }
                                </script>
                            </div>
                        </div>

                        <div class="form-actions">
                            <asp:LinkButton ID="btnVolverGestionDeserializar" runat="server"
                                CssClass="btn-secondary" Text="Volver a gestión"
                                OnClick="btnModoGestion_Click" CausesValidation="false" />
                            <asp:Button ID="btnCargarXml" runat="server"
                                Text="📤 Cargar XML"
                                CssClass="btn-primary"
                                OnClick="btnCargarXml_Click" CausesValidation="false" />
                        </div>

                        <h2 class="card-title mt-24"><asp:Label ID="lblTituloVistaPreviaXml" runat="server" Text="Profesionales encontrados en el archivo" /></h2>
                        <div class="table-wrap">
                            <asp:GridView ID="gvProfesionalesXml" runat="server"
                                CssClass="data-table"
                                AutoGenerateColumns="false"
                                GridLines="None"
                                Visible="false"
                                EmptyDataText="No se cargó ningún archivo todavía.">

                                <EmptyDataRowStyle CssClass="empty-row" />
                                <HeaderStyle      CssClass="table-header" />
                                <RowStyle         CssClass="table-row" />
                                <AlternatingRowStyle CssClass="table-row table-row-alt" />

                                <Columns>
                                    <asp:TemplateField HeaderText="Profesional" HeaderStyle-CssClass="th-left">
                                        <ItemTemplate>
                                            <%# Eval("Nombre") %> <%# Eval("Apellido") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Dni"   HeaderText="DNI"   HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />
                                    <asp:BoundField DataField="Email" HeaderText="Email" HeaderStyle-CssClass="th-left" />
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
                                                <%# (bool)Eval("Activo") ? Traducir("estado_activo") : Traducir("estado_inactivo") %>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </asp:Panel>

            </div>
        </div>

    </form>
    <script src="Scripts/Mascaras.js"></script>
</body>
</html>