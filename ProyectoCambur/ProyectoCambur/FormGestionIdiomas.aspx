<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormGestionIdiomas.aspx.cs" Inherits="FormGestionIdiomas" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Gestionar Idiomas</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormGestionIdiomas.css"  rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <!-- SIDEBAR ADMIN -->
        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline-admin">Panel Administrador</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormDashboard.aspx"          class="nav-item">🏠 Dashboard</a>
                <a href="FormAuditoriaBitacora.aspx"  class="nav-item">📜 Bitácora</a>
                <a href="FormBackupRestore.aspx"      class="nav-item">💾 Backup / Restore</a>
                <a href="FormDigitoVerificador.aspx"  class="nav-item">🔢 Dígito Verificador</a>
                <a href="FormGestionIdiomas.aspx"     class="nav-item active">🌐 Gestionar Idiomas</a>
                <a href="FormABMProfesionales.aspx"   class="nav-item">👤 ABM Profesionales</a>
                <a href="FormABMPacientes.aspx"       class="nav-item">🧑‍⚕️ ABM Pacientes</a>
                <a href="FormABMConsultas.aspx"       class="nav-item">🗒️ ABM Consultas</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

        <!-- ÁREA PRINCIPAL -->
        <div class="main-wrap">

            <!-- HEADER -->
            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Administración</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Gestionar Idiomas</span>
                </div>
                <div class="header-user">
                    <div class="user-avatar user-avatar-admin">
                        <asp:Label ID="lblIniciales" runat="server" Text="AD" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreAdmin" runat="server" CssClass="user-name" Text="" />
                        <span class="user-role admin-role">Administrador</span>
                    </div>
                </div>
            </header>

            <!-- CONTENIDO -->
            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <div class="gi-layout">

                    <!-- ============================================
                         COLUMNA IZQUIERDA: Grilla de idiomas
                         ============================================ -->
                    <div class="gi-main">

                        <!-- Card: lista de idiomas -->
                        <div class="content-card">
                            <div class="card-header-row">
                                <div class="card-header-left">
                                    <h2 class="card-title">Idiomas disponibles</h2>
                                    <asp:Label ID="lblTotalIdiomas" runat="server"
                                        CssClass="badge-activos" Text="" Visible="false" />
                                </div>
                                <asp:Button ID="btnMostrarAlta" runat="server"
                                    Text="+ Agregar idioma"
                                    CssClass="btn-primary btn-sm-gi"
                                    OnClick="btnMostrarAlta_Click"
                                    CausesValidation="false" />
                            </div>

                            <div class="table-wrap" style="margin-top:16px;">
                                <asp:GridView ID="gvIdiomas" runat="server"
                                    CssClass="data-table"
                                    AutoGenerateColumns="false"
                                    GridLines="None"
                                    OnRowCommand="gvIdiomas_RowCommand"
                                    EmptyDataText="No hay idiomas registrados.">
                                    <EmptyDataRowStyle CssClass="empty-row" />
                                    <HeaderStyle      CssClass="table-header" />
                                    <RowStyle         CssClass="table-row" />
                                    <AlternatingRowStyle CssClass="table-row table-row-alt" />
                                    <Columns>
                                        <asp:BoundField DataField="Flag"
                                            HeaderText=""
                                            HeaderStyle-CssClass="th-centro"
                                            ItemStyle-CssClass="td-flag" />
                                        <asp:BoundField DataField="Nombre"
                                            HeaderText="Idioma"
                                            HeaderStyle-CssClass="th-left" />
                                        <asp:BoundField DataField="Codigo"
                                            HeaderText="Código"
                                            HeaderStyle-CssClass="th-centro"
                                            ItemStyle-CssClass="td-centro td-mono" />
                                        <asp:BoundField DataField="Traducciones"
                                            HeaderText="Traducciones"
                                            HeaderStyle-CssClass="th-centro"
                                            ItemStyle-CssClass="td-centro" />
                                        <asp:TemplateField HeaderText="Estado"
                                            HeaderStyle-CssClass="th-centro"
                                            ItemStyle-CssClass="td-centro">
                                            <ItemTemplate>
                                                <span class='<%# (bool)Eval("Activo") ? "badge-estado activo" : "badge-estado inactivo" %>'>
                                                    <%# (bool)Eval("Activo") ? "Activo" : "Inactivo" %>
                                                </span>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Acciones"
                                            HeaderStyle-CssClass="th-centro"
                                            ItemStyle-CssClass="td-acciones">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbVerTrad" runat="server"
                                                    CommandName="VerTraducciones"
                                                    CommandArgument='<%# Eval("Codigo") %>'
                                                    CssClass="tbl-btn tbl-btn-ver"
                                                    Text="Traducciones" />
                                                <asp:LinkButton ID="lbToggle" runat="server"
                                                    CommandName="ToggleActivo"
                                                    CommandArgument='<%# Eval("Codigo") + "|" + Eval("Activo") %>'
                                                    CssClass='<%# (bool)Eval("Activo") ? "tbl-btn tbl-btn-baja" : "tbl-btn tbl-btn-reactivar" %>'
                                                    Text='<%# (bool)Eval("Activo") ? "Desactivar" : "Activar" %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>

                        <!-- Panel: Alta de nuevo idioma -->
                        <asp:Panel ID="pnlAltaIdioma" runat="server"
                            CssClass="content-card alta-idioma-card mt-24" Visible="false">

                            <div class="alta-header">
                                <h3 class="card-title">Agregar nuevo idioma</h3>
                                <asp:Button ID="btnCancelarAlta" runat="server"
                                    Text="✕ Cancelar"
                                    CssClass="btn-secondary btn-sm-gi"
                                    OnClick="btnCancelarAlta_Click"
                                    CausesValidation="false" />
                            </div>

                            <div class="grid-2" style="max-width:480px; margin-top:16px;">
                                <div class="field">
                                    <label for="txtNombreIdioma">Nombre del idioma <sup>*</sup></label>
                                    <asp:TextBox ID="txtNombreIdioma" runat="server"
                                        MaxLength="80"
                                        placeholder="Ej: Deutsch"
                                        ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                                        ControlToValidate="txtNombreIdioma"
                                        ErrorMessage="El nombre es obligatorio."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgAlta" />
                                </div>
                                <div class="field">
                                    <label for="txtCodigoIdioma">Código ISO <sup>*</sup></label>
                                    <asp:TextBox ID="txtCodigoIdioma" runat="server"
                                        MaxLength="5"
                                        placeholder="Ej: DE"
                                        ClientIDMode="Static"
                                        oninput="this.value = this.value.toUpperCase()" />
                                    <asp:RequiredFieldValidator ID="rfvCodigo" runat="server"
                                        ControlToValidate="txtCodigoIdioma"
                                        ErrorMessage="El código es obligatorio."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgAlta" />
                                </div>
                                <div class="field">
                                    <label for="txtFlagIdioma">Emoji de bandera</label>
                                    <asp:TextBox ID="txtFlagIdioma" runat="server"
                                        MaxLength="4"
                                        placeholder="Ej: 🇩🇪"
                                        ClientIDMode="Static" />
                                </div>
                            </div>

                            <div class="alta-aviso">
                                📝 Al agregar el idioma, el sistema generará automáticamente todas las
                                traducciones con la clave como valor por defecto (ej: BT_Login → BT_Login).
                                Luego podrás editarlas desde la vista de traducciones.
                            </div>

                            <div class="form-actions">
                                <asp:Button ID="btnGuardarAlta" runat="server"
                                    Text="Agregar idioma"
                                    CssClass="btn-primary"
                                    ValidationGroup="vgAlta"
                                    OnClick="btnGuardarAlta_Click" />
                            </div>

                        </asp:Panel>

                        <!-- Panel: Grilla de traducciones del idioma seleccionado -->
                        <asp:Panel ID="pnlTraducciones" runat="server"
                            CssClass="content-card mt-24" Visible="false">

                            <div class="card-header-row">
                                <div class="card-header-left">
                                    <h3 class="card-title">
                                        Traducciones —
                                        <asp:Label ID="lblIdiomaEditar" runat="server"
                                            CssClass="idioma-editando" Text="" />
                                    </h3>
                                </div>
                                <asp:Button ID="btnCerrarTrad" runat="server"
                                    Text="✕ Cerrar"
                                    CssClass="btn-secondary btn-sm-gi"
                                    OnClick="btnCerrarTrad_Click"
                                    CausesValidation="false" />
                            </div>

                            <p class="card-subtitle" style="margin:6px 0 14px;">
                                Hacé click en cualquier celda de "Traducción" para editarla en línea y luego presioná "Guardar" en esa fila.
                            </p>

                            <div class="table-wrap">
                                <asp:GridView ID="gvTraducciones" runat="server"
                                    CssClass="data-table"
                                    AutoGenerateColumns="false"
                                    GridLines="None"
                                    OnRowCommand="gvTraducciones_RowCommand"
                                    AllowPaging="true"
                                    PageSize="12"
                                    OnPageIndexChanging="gvTraducciones_PageIndexChanging">
                                    <HeaderStyle      CssClass="table-header" />
                                    <RowStyle         CssClass="table-row" />
                                    <AlternatingRowStyle CssClass="table-row table-row-alt" />
                                    <PagerStyle       CssClass="grid-pager" />
                                    <Columns>
                                        <asp:BoundField DataField="Clave"
                                            HeaderText="Clave del sistema"
                                            HeaderStyle-CssClass="th-left"
                                            ItemStyle-CssClass="td-clave" />
                                        <asp:TemplateField HeaderText="Traducción"
                                            HeaderStyle-CssClass="th-left">
                                            <ItemTemplate>
                                                <div class="trad-edit-wrap">
                                                    <asp:TextBox ID="txtTraduccion" runat="server"
                                                        Text='<%# Eval("Valor") %>'
                                                        CssClass="trad-input"
                                                        MaxLength="300" />
                                                    <asp:HiddenField ID="hfClave" runat="server"
                                                        Value='<%# Eval("Clave") %>' />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText=""
                                            HeaderStyle-CssClass="th-centro"
                                            ItemStyle-CssClass="td-centro">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbGuardarTrad" runat="server"
                                                    CommandName="GuardarTrad"
                                                    CommandArgument='<%# Container.DataItemIndex %>'
                                                    CssClass="tbl-btn tbl-btn-mod"
                                                    Text="Guardar" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>

                        </asp:Panel>

                    </div>

                    <!-- COLUMNA LATERAL -->
                    <div class="gi-aside">

                        <div class="content-card gi-info-card">
                            <p class="accesos-titulo">Información</p>
                            <div class="gi-info-item">
                                <span>🌐</span>
                                <div class="gi-info-texto">Los idiomas activos están disponibles para que los profesionales los seleccionen en Configuración.</div>
                            </div>
                            <div class="gi-info-item">
                                <span>🔑</span>
                                <div class="gi-info-texto">Las claves son identificadores del sistema (ej: BT_Login). Los valores son los textos que verá el usuario.</div>
                            </div>
                            <div class="gi-info-item">
                                <span>📝</span>
                                <div class="gi-info-texto">Si falta una traducción, el sistema muestra la clave como fallback automático.</div>
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">⚠️</div>
                            <p class="aviso-titulo">Desactivar idioma</p>
                            <p class="aviso-texto">Desactivar un idioma lo oculta de la selección del usuario pero conserva todas sus traducciones en la base de datos.</p>
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </form>
</body>
</html>
