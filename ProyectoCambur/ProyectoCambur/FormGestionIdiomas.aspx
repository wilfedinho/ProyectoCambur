<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormGestionIdiomas.aspx.cs" Inherits="FormGestionIdiomas" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Gestión de idiomas</title>
    <link href="EstilosPaginas/Shared.css"             rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormGestionIdiomas.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Panel Técnico</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormMenuWebMaster.aspx"      class="nav-item">🏠 Menú</a>
                <a href="FormMaestroProfesional.aspx" class="nav-item">👥 Profesionales</a>
                <a href="FormGestionIdiomas.aspx"      class="nav-item active">🌐 Idiomas</a>
                <a href="FormDigitoVerificador.aspx"   class="nav-item">🔐 Integridad</a>
                <a href="FormBackupRestore.aspx"       class="nav-item">💾 Backup / Restore</a>
                <a href="FormAuditoriaBitacora.aspx"   class="nav-item">📜 Bitácora</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Web Master</span>
                    <span class="header-sep">/</span>
                    <span class="header-page">Gestión de idiomas</span>
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server" CssClass="user-name" Text="" />
                        <span class="user-role">Web Master</span>
                    </div>
                </div>
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" />

                <!-- ============ ALTA DE IDIOMA ============ -->
                <div class="content-card">
                    <div class="card-header">
                        <h2 class="card-title">Nuevo idioma</h2>
                        <p class="card-subtitle">Se generan automáticamente todas las traducciones copiando las claves de "Español" y traduciéndolas con el traductor automático. Puede tardar según la cantidad de claves cargadas — no cierres la página mientras procesa.</p>
                    </div>

                    <div class="grid-3">
                        <div class="field">
                            <label for="txtNombreIdioma">Nombre del idioma <sup>*</sup></label>
                            <asp:TextBox ID="txtNombreIdioma" runat="server" MaxLength="50" placeholder="Ej: English" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvNombreIdioma" runat="server"
                                ControlToValidate="txtNombreIdioma" ErrorMessage="El nombre del idioma es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgIdioma" />
                        </div>

                        <div class="field">
                            <label for="txtCodigoIso">Código ISO 639-1 <sup>*</sup></label>
                            <asp:TextBox ID="txtCodigoIso" runat="server" MaxLength="5" placeholder="Ej: en" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvCodigoIso" runat="server"
                                ControlToValidate="txtCodigoIso" ErrorMessage="El código ISO es obligatorio."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgIdioma" />
                            <span class="hint-text">Lo usa el traductor automático (ej: es, en, pt, fr).</span>
                        </div>
                    </div>

                    <div class="form-actions">
                        <asp:Button ID="btnAltaIdioma" runat="server"
                            Text="Generar idioma con traducciones"
                            CssClass="btn-primary"
                            ValidationGroup="vgIdioma"
                            OnClick="btnAltaIdioma_Click"
                            OnClientClick="return confirm('Esto va a llamar al traductor automatico para generar todas las traducciones. Puede tardar unos minutos segun la cantidad de claves. ¿Confirmas?');" />
                    </div>
                </div>

                <!-- ============ IDIOMAS EXISTENTES ============ -->
                <div class="content-card mt-24">
                    <div class="card-header">
                        <h2 class="card-title">Idiomas del sistema</h2>
                    </div>

                    <div class="table-wrap">
                        <asp:GridView ID="gvIdiomas" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            EmptyDataText="No hay idiomas cargados."
                            OnRowCommand="gvIdiomas_RowCommand">

                            <EmptyDataRowStyle CssClass="empty-row" />
                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />

                            <Columns>
                                <asp:BoundField DataField="NombreIdioma" HeaderText="Idioma"     HeaderStyle-CssClass="th-left" />
                                <asp:BoundField DataField="CodigoIso"    HeaderText="Código ISO" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />

                                <asp:TemplateField HeaderText="Estado" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <span class='<%# (bool)Eval("IsDisponible") ? "badge-estado activo" : "badge-estado inactivo" %>'>
                                            <%# (bool)Eval("IsDisponible") ? "Disponible" : "Desactivado" %>
                                        </span>
                                        <asp:Label runat="server" CssClass="badge-estado bloqueado"
                                            Visible='<%# (bool)Eval("IsOcupado") %>' Text="En uso" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDesactivar" runat="server"
                                            CommandName="Desactivar"
                                            CommandArgument='<%# Eval("NombreIdioma") %>'
                                            CssClass='<%# (bool)Eval("IsDisponible") ? "tbl-btn tbl-btn-baja" : "tbl-btn-hidden" %>'
                                            Text="🚫 Desactivar"
                                            OnClientClick="return confirm('¿Confirmás desactivar este idioma? Solo se puede si ningún profesional activo lo está usando.');" />

                                        <asp:LinkButton ID="lbActivar" runat="server"
                                            CommandName="Activar"
                                            CommandArgument='<%# Eval("NombreIdioma") %>'
                                            CssClass='<%# (bool)Eval("IsDisponible") ? "tbl-btn-hidden" : "tbl-btn tbl-btn-reactivar" %>'
                                            Text="✅ Activar" />

                                        <asp:LinkButton ID="lbVerPendientes" runat="server"
                                            CommandName="VerPendientes"
                                            CommandArgument='<%# Eval("NombreIdioma") %>'
                                            CssClass="tbl-btn tbl-btn-mod"
                                            Text="✏️ Traducciones" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <!-- ============ TRADUCCIONES DE UN IDIOMA ============ -->
                <asp:Panel ID="pnlTraducciones" runat="server" CssClass="content-card mt-24" Visible="false">
                    <div class="card-header">
                        <h2 class="card-title">Traducciones — <asp:Label ID="lblIdiomaSeleccionado" runat="server" Text="" /></h2>
                        <p class="card-subtitle">Las marcadas "pendiente" fueron generadas por el traductor automático y todavía no fueron revisadas por un humano.</p>
                    </div>

                    <div class="table-wrap">
                        <asp:GridView ID="gvTraducciones" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            EmptyDataText="Este idioma no tiene traducciones cargadas."
                            OnRowCommand="gvTraducciones_RowCommand">

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
                                        <span class='<%# (bool)Eval("Pendiente") ? "badge-estado bloqueado" : "badge-estado activo" %>'>
                                            <%# (bool)Eval("Pendiente") ? "Pendiente" : "Revisado" %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbGuardarTraduccion" runat="server"
                                            CommandName="GuardarTraduccion"
                                            CommandArgument='<%# Eval("IdTraduccion") %>'
                                            CssClass="tbl-btn tbl-btn-mod"
                                            Text="💾 Guardar" />
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
