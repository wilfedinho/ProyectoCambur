<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormBackupRestore.aspx.cs" Inherits="FormBackupRestore" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Backup / Restore</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"       rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"   rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormBackupRestore.css"   rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline-admin" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_backup_restore" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <asp:Label ID="lblHeaderSeccion" runat="server" CssClass="header-section" Text="" />
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderTitulo" runat="server" CssClass="header-page" Text="" />
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <div class="aviso-critico">
                    <span class="aviso-crit-icono">⚠️</span>
                    <div>
                        <p class="aviso-crit-titulo"><asp:Label ID="lblAvisoCriticoTitulo" runat="server" Text="" /></p>
                        <p class="aviso-crit-texto"><asp:Label ID="lblAvisoCriticoTexto" runat="server" Text="" /></p>
                    </div>
                </div>

                <div class="br-layout">

                    <div class="br-col">
                        <div class="content-card br-card">
                            <div class="br-card-header">
                                <div class="br-icono-wrap br-icono-backup">💾</div>
                                <div>
                                    <h2 class="card-title"><asp:Label ID="lblTituloGenerarBackup" runat="server" Text="" /></h2>
                                    <p class="card-subtitle"><asp:Label ID="lblSubtituloGenerarBackup" runat="server" Text="" /></p>
                                </div>
                            </div>

                            <p class="section-sep"><asp:Label ID="lblConfigBackup" runat="server" Text="" /></p>

                            <div class="br-info-grid">
                                <div class="br-info-item">
                                    <asp:Label ID="lblEtiquetaCarpetaDestino" runat="server" CssClass="br-info-label" Text="" />
                                    <asp:Label ID="lblCarpetaDestino" runat="server" CssClass="br-info-valor" Text="" />
                                </div>
                                <div class="br-info-item">
                                    <asp:Label ID="lblEtiquetaFormatoArchivo" runat="server" CssClass="br-info-label" Text="" />
                                    <span class="br-info-valor">Backup_AAAAMMDD_HHMMSS.bak</span>
                                </div>
                                <div class="br-info-item">
                                    <asp:Label ID="lblEtiquetaTipoBackup" runat="server" CssClass="br-info-label" Text="" />
                                    <asp:Label ID="lblValorTipoBackup" runat="server" CssClass="br-info-valor" Text="" />
                                </div>
                                <div class="br-info-item">
                                    <asp:Label ID="lblEtiquetaUltimoBackup" runat="server" CssClass="br-info-label" Text="" />
                                    <asp:Label ID="lblUltimoBackup" runat="server" CssClass="br-info-valor" Text="" />
                                </div>
                            </div>

                            <div class="br-nombre-preview">
                                <asp:Label ID="lblEtiquetaArchivoAGenerar" runat="server" CssClass="br-nombre-label" Text="" />
                                <asp:Label ID="lblNombreArchivo" runat="server" CssClass="br-nombre-valor" Text="" />
                            </div>

                            <asp:Button ID="btnGenerarBackup" runat="server"
                                Text=""
                                CssClass="btn-primary btn-backup"
                                OnClick="btnGenerarBackup_Click"
                                CausesValidation="false"
                                OnClientClick="return confirm(<%= JsonConfirmarGenerarBackup %>);" />

                            <asp:Panel ID="pnlResultadoBackup" runat="server"
                                CssClass="resultado-backup" Visible="false">
                                <div class="resultado-icono">✓</div>
                                <div>
                                    <p class="resultado-titulo"><asp:Label ID="lblResultadoTituloBackup" runat="server" Text="" /></p>
                                    <asp:Label ID="lblResultadoBackup" runat="server"
                                        CssClass="resultado-texto" Text="" />
                                </div>
                            </asp:Panel>

                        </div>
                    </div>

                    <div class="br-col">
                        <div class="content-card br-card">
                            <div class="br-card-header">
                                <div class="br-icono-wrap br-icono-restore">🔄</div>
                                <div>
                                    <h2 class="card-title"><asp:Label ID="lblTituloRestaurarBackup" runat="server" Text="" /></h2>
                                    <p class="card-subtitle"><asp:Label ID="lblSubtituloRestaurarBackup" runat="server" Text="" /></p>
                                </div>
                            </div>

                            <p class="section-sep"><asp:Label ID="lblArchivosDisponibles" runat="server" Text="" /></p>

                            <div class="archivos-lista">
                                <asp:Repeater ID="rptArchivos" runat="server">
                                    <ItemTemplate>
                                        <div class='<%# (bool)Eval("Seleccionado") ? "archivo-item archivo-sel" : "archivo-item" %>'
                                             onclick="seleccionarArchivo(this, '<%# Eval("NombreArchivo") %>')">
                                            <span class="archivo-icono">🗄</span>
                                            <div class="archivo-info">
                                                <div class="archivo-nombre"><%# Eval("NombreArchivo") %></div>
                                                <div class="archivo-meta">
                                                    <%# Eval("Tamanio") %> · <%# Eval("Fecha", "{0:dd/MM/yyyy HH:mm}") %>
                                                </div>
                                            </div>
                                            <div class="archivo-check">
                                                <%# (bool)Eval("Seleccionado") ? "●" : "○" %>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Label ID="lblSinBackupsDisponibles" runat="server" CssClass="sin-perfiles-txt" Visible="false" Text="" />
                            </div>

                            <asp:HiddenField ID="hfArchivoSeleccionado" runat="server"
                                Value="" ClientIDMode="Static" />

                            <asp:Panel ID="pnlConfirmRestore" runat="server"
                                CssClass="confirm-restore-panel" Visible="false">
                                <div class="confirm-restore-aviso">
                                    🚨 <asp:Label ID="lblAvisoRestoreTexto" runat="server" Text="" />
                                </div>
                                <div class="confirm-restore-archivo">
                                    <asp:Label ID="lblEtiquetaArchivoSeleccionado" runat="server" AssociatedControlID="lblArchivoARestaurar" Text="" />
                                    <asp:Label ID="lblArchivoARestaurar" runat="server"
                                        CssClass="confirm-archivo-nombre" Text="" />
                                </div>
                                <div class="confirm-restore-actions">
                                    <asp:Button ID="btnCancelarRestore" runat="server"
                                        Text=""
                                        CssClass="btn-secondary"
                                        OnClick="btnCancelarRestore_Click"
                                        CausesValidation="false" />
                                    <asp:Button ID="btnConfirmarRestore" runat="server"
                                        Text=""
                                        CssClass="btn-danger-solid"
                                        OnClick="btnConfirmarRestore_Click"
                                        CausesValidation="false" />
                                </div>
                            </asp:Panel>

                            <asp:Button ID="btnIniciarRestore" runat="server"
                                Text=""
                                CssClass="btn-restore"
                                OnClick="btnIniciarRestore_Click"
                                CausesValidation="false" />

                            <asp:Panel ID="pnlResultadoRestore" runat="server"
                                CssClass="resultado-backup" Visible="false">
                                <div class="resultado-icono">✓</div>
                                <div>
                                    <p class="resultado-titulo"><asp:Label ID="lblResultadoTituloRestore" runat="server" Text="" /></p>
                                    <asp:Label ID="lblResultadoRestore" runat="server"
                                        CssClass="resultado-texto" Text="" />
                                </div>
                            </asp:Panel>

                        </div>
                    </div>

                </div>

                <div class="content-card mt-24">
                    <div class="card-header">
                        <h3 class="card-title" style="font-size:18px;"><asp:Label ID="lblTituloHistorial" runat="server" Text="" /></h3>
                    </div>
                    <div class="table-wrap" style="margin-top:14px;">
                        <asp:GridView ID="gvHistorial" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            EmptyDataText="No se registraron operaciones aún.">
                            <EmptyDataRowStyle CssClass="empty-row" />
                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />
                            <Columns>
                                <asp:BoundField DataField="Fecha"
                                    HeaderText="Fecha / Hora"
                                    DataFormatString="{0:dd/MM/yyyy HH:mm:ss}"
                                    HeaderStyle-CssClass="th-left"
                                    ItemStyle-CssClass="td-fecha-bit" />
                                <asp:TemplateField HeaderText="Tipo" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro">
                                    <ItemTemplate>
                                        <span class='<%# "badge-op op-" + Eval("Tipo").ToString().ToLower() %>'>
                                            <%# Eval("Tipo") %>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Archivo"
                                    HeaderText="Archivo"
                                    HeaderStyle-CssClass="th-left"
                                    ItemStyle-CssClass="td-mono" />
                                <asp:BoundField DataField="Resultado"
                                    HeaderText="Resultado"
                                    HeaderStyle-CssClass="th-left" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </div>
        </div>

    </form>

    <script type="text/javascript">
        function seleccionarArchivo(item, nombre) {
            document.querySelectorAll('.archivo-item').forEach(function (el) {
                el.classList.remove('archivo-sel');
                var chk = el.querySelector('.archivo-check');
                if (chk) chk.textContent = '○';
            });
            item.classList.add('archivo-sel');
            var chk = item.querySelector('.archivo-check');
            if (chk) chk.textContent = '●';
            var hf = document.getElementById('hfArchivoSeleccionado');
            if (hf) hf.value = nombre;
        }
    </script>
</body>
</html>