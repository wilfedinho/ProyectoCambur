<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormAuditoriaBitacora.aspx.cs" Inherits="FormAuditoriaBitacora" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Auditoría Bitácora</title>
    <link href="EstilosPaginas/Shared.css"                   rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormAuditoriaBitacora.css"    rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <!-- SIDEBAR — ROL ADMINISTRADOR -->
        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline-admin">Panel Web Master</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormDashboard.aspx"              class="nav-item">🏠 Dashboard</a>
                <a href="FormAuditoriaBitacora.aspx"      class="nav-item active">📜 Bitácora</a>
                <a href="FormBackupRestore.aspx"          class="nav-item">💾 Backup / Restore</a>
                <a href="FormDigitoVerificador.aspx"      class="nav-item">🔢 Dígito Verificador</a>
                <a href="FormGestionIdiomas.aspx"         class="nav-item">🌐 Gestionar Idiomas</a>
                <a href="FormABMProfesionales.aspx"       class="nav-item">👤 ABM Profesionales</a>
                <a href="FormABMPacientes.aspx"           class="nav-item">🧑‍⚕️ ABM Pacientes</a>
                <a href="FormABMConsultas.aspx"           class="nav-item">🗒️ ABM Consultas</a>
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
                    <span class="header-page">Auditoría Bitácora</span>
                </div>
                <div class="header-user">
                    <div class="user-avatar user-avatar-admin">
                        <asp:Label ID="lblIniciales" runat="server" Text="AD" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreAdmin" runat="server" CssClass="user-name" Text="" />
                        <span class="user-role admin-role">Web Master</span>
                    </div>
                </div>
            </header>

            <!-- CONTENIDO -->
            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <!-- PANEL DE FILTROS -->
                <div class="content-card filtros-card">
                    <div class="filtros-header">
                        <h2 class="card-title">Registro de bitácora</h2>
                        <div class="badges-row">
                            <asp:Label ID="lblTotalRegistros" runat="server"
                                CssClass="badge-activos" Text="" Visible="false" />
                            <asp:Label ID="lblFiltroActivo" runat="server"
                                CssClass="badge-inactivos" Text="" Visible="false" />
                        </div>
                    </div>
                    <p class="card-subtitle" style="margin-bottom:16px;">Consultá el historial de acciones realizadas en la plataforma. Aplicá filtros para acotar los resultados.</p>

                    <div class="grid-4">
                        <div class="field">
                            <label for="ddlFiltroUsuario">Usuario</label>
                            <asp:DropDownList ID="ddlFiltroUsuario" runat="server" ClientIDMode="Static">
                                <asp:ListItem Value="" Text="Todos los usuarios" />
                            </asp:DropDownList>
                        </div>
                        <div class="field">
                            <label for="ddlFiltroModulo">Módulo</label>
                            <asp:DropDownList ID="ddlFiltroModulo" runat="server" ClientIDMode="Static">
                                <asp:ListItem Value=""         Text="Todos los módulos" />
                                <asp:ListItem Value="Login"    Text="Login" />
                                <asp:ListItem Value="Logout"   Text="Logout" />
                                <asp:ListItem Value="Pacientes" Text="Pacientes" />
                                <asp:ListItem Value="Consultas" Text="Consultas" />
                                <asp:ListItem Value="IA"       Text="IA Asistiva" />
                                <asp:ListItem Value="Seguridad" Text="Seguridad" />
                                <asp:ListItem Value="Configuración" Text="Configuración" />
                                <asp:ListItem Value="Exportación"   Text="Exportación" />
                                <asp:ListItem Value="Administración" Text="Administración" />
                            </asp:DropDownList>
                        </div>
                        <div class="field">
                            <label for="ddlFiltroCriticidad">Criticidad</label>
                            <asp:DropDownList ID="ddlFiltroCriticidad" runat="server" ClientIDMode="Static">
                                <asp:ListItem Value=""  Text="Todas" />
                                <asp:ListItem Value="1" Text="1 — Alta (impacto mayor)" />
                                <asp:ListItem Value="2" Text="2 — Media" />
                                <asp:ListItem Value="3" Text="3 — Baja" />
                            </asp:DropDownList>
                        </div>
                        <div class="field">
                            <label>Rango de fechas</label>
                            <div class="fecha-range">
                                <asp:TextBox ID="txtFechaDesde" runat="server"
                                    TextMode="Date" CssClass="filtro-input" ClientIDMode="Static" />
                                <span class="filtro-sep">→</span>
                                <asp:TextBox ID="txtFechaHasta" runat="server"
                                    TextMode="Date" CssClass="filtro-input" ClientIDMode="Static" />
                            </div>
                        </div>
                    </div>

                    <div class="filtros-actions">
                        <asp:Button ID="btnLimpiar" runat="server"
                            Text="Limpiar filtros"
                            CssClass="btn-secondary"
                            OnClick="btnLimpiar_Click"
                            CausesValidation="false" />
                        <asp:Button ID="btnFiltrar" runat="server"
                            Text="Aplicar filtros"
                            CssClass="btn-primary"
                            OnClick="btnFiltrar_Click"
                            CausesValidation="false" />
                    </div>
                </div>

                <!-- TABLA DE REGISTROS -->
                <div class="content-card mt-24">

                    <asp:Label ID="lblVacio" runat="server"
                        CssClass="timeline-vacio" Visible="false"
                        Text="No se encontraron registros que coincidan con los filtros aplicados." />

                    <div class="table-wrap">
                        <asp:GridView ID="gvBitacora" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            AllowPaging="true"
                            PageSize="15"
                            OnPageIndexChanging="gvBitacora_PageIndexChanging"
                            OnSelectedIndexChanged="gvBitacora_SelectedIndexChanged">

                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />
                            <PagerStyle       CssClass="grid-pager" />

                            <Columns>
                                <asp:BoundField DataField="FechaEvento"
                                    HeaderText="Fecha / Hora"
                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                                    HeaderStyle-CssClass="th-left"
                                    ItemStyle-CssClass="td-fecha-bit" />

                                <asp:BoundField DataField="Usuario"
                                    HeaderText="Usuario"
                                    HeaderStyle-CssClass="th-left" />

                                <asp:BoundField DataField="Modulo"
                                    HeaderText="Módulo"
                                    HeaderStyle-CssClass="th-centro"
                                    ItemStyle-CssClass="td-centro" />

                                <asp:TemplateField HeaderText="Criticidad"
                                    HeaderStyle-CssClass="th-centro"
                                    ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <span class='<%# "badge-criticidad crit-" + Eval("Criticidad") %>'>
                                            <%# Eval("CriticidadLabel") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="Descripcion"
                                    HeaderText="Descripción"
                                    HeaderStyle-CssClass="th-left"
                                    ItemStyle-CssClass="td-desc" />

                                <asp:TemplateField HeaderText=""
                                    HeaderStyle-CssClass="th-centro"
                                    ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDetalle" runat="server"
                                            CommandName="Select"
                                            CommandArgument='<%# Eval("IdBitacora") %>'
                                            CssClass="tbl-btn tbl-btn-ver"
                                            Text="Ver detalle" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

                <!-- PANEL DE DETALLE DEL REGISTRO -->
                <asp:Panel ID="pnlDetalle" runat="server"
                    CssClass="content-card detalle-card mt-24" Visible="false">

                    <div class="detalle-header">
                        <h3 class="card-title">Detalle del registro</h3>
                        <asp:Button ID="btnCerrarDetalle" runat="server"
                            Text="✕ Cerrar"
                            CssClass="btn-secondary btn-sm-det"
                            OnClick="btnCerrarDetalle_Click"
                            CausesValidation="false" />
                    </div>

                    <div class="detalle-grid">
                        <div class="detalle-item">
                            <span class="det-label">ID Registro</span>
                            <asp:Label ID="lblDetId"       runat="server" CssClass="det-valor" Text="" />
                        </div>
                        <div class="detalle-item">
                            <span class="det-label">Fecha y hora</span>
                            <asp:Label ID="lblDetFecha"    runat="server" CssClass="det-valor" Text="" />
                        </div>
                        <div class="detalle-item">
                            <span class="det-label">Usuario</span>
                            <asp:Label ID="lblDetUsuario"  runat="server" CssClass="det-valor" Text="" />
                        </div>
                        <div class="detalle-item">
                            <span class="det-label">Módulo</span>
                            <asp:Label ID="lblDetModulo"   runat="server" CssClass="det-valor" Text="" />
                        </div>
                        <div class="detalle-item">
                            <span class="det-label">Criticidad</span>
                            <asp:Label ID="lblDetCriticidad" runat="server" CssClass="det-valor" Text="" />
                        </div>
                        <div class="detalle-item detalle-item-full">
                            <span class="det-label">Descripción completa</span>
                            <asp:Label ID="lblDetDescripcion" runat="server" CssClass="det-valor" Text="" />
                        </div>
                    </div>

                </asp:Panel>

            </div>
        </div>

    </form>
</body>
</html>
