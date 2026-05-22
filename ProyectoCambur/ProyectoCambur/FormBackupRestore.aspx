<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormBackupRestore.aspx.cs" Inherits="FormBackupRestore" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Backup / Restore</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormBackupRestore.css"   rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <!-- SIDEBAR ADMIN -->
        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline-admin">Panel Web Master</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormDashboard.aspx"              class="nav-item">🏠 Dashboard</a>
                <a href="FormAuditoriaBitacora.aspx"      class="nav-item">📜 Bitácora</a>
                <a href="FormBackupRestore.aspx"          class="nav-item active">💾 Backup / Restore</a>
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
                    <span class="header-page">Backup / Restore</span>
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

                <!-- AVISO CRÍTICO -->
                <div class="aviso-critico">
                    <span class="aviso-crit-icono">⚠️</span>
                    <div>
                        <p class="aviso-crit-titulo">Operación crítica del sistema</p>
                        <p class="aviso-crit-texto">Las operaciones de backup y restore afectan directamente a la base de datos de producción. Asegurate de tener permisos y de informar al equipo antes de ejecutar una restauración.</p>
                    </div>
                </div>

                <div class="br-layout">

                    <!-- ============================================
                         COLUMNA IZQUIERDA: GENERAR BACKUP
                         ============================================ -->
                    <div class="br-col">
                        <div class="content-card br-card">
                            <div class="br-card-header">
                                <div class="br-icono-wrap br-icono-backup">💾</div>
                                <div>
                                    <h2 class="card-title">Generar Backup</h2>
                                    <p class="card-subtitle">Crea una copia completa de la base de datos en formato .bak.</p>
                                </div>
                            </div>

                            <div class="section-sep">Configuración del backup</div>

                            <div class="br-info-grid">
                                <div class="br-info-item">
                                    <span class="br-info-label">Carpeta destino</span>
                                    <span class="br-info-valor">BackupsSQL\</span>
                                </div>
                                <div class="br-info-item">
                                    <span class="br-info-label">Formato de archivo</span>
                                    <span class="br-info-valor">Backup_AAAAMMDD_HHMMSS.bak</span>
                                </div>
                                <div class="br-info-item">
                                    <span class="br-info-label">Tipo de backup</span>
                                    <span class="br-info-valor">Completo (Full Backup)</span>
                                </div>
                                <div class="br-info-item">
                                    <span class="br-info-label">Último backup</span>
                                    <asp:Label ID="lblUltimoBackup" runat="server"
                                        CssClass="br-info-valor" Text="" />
                                </div>
                            </div>

                            <!-- Nombre del archivo que se generará -->
                            <div class="br-nombre-preview">
                                <span class="br-nombre-label">Archivo que se generará:</span>
                                <asp:Label ID="lblNombreArchivo" runat="server"
                                    CssClass="br-nombre-valor" Text="" />
                            </div>

                            <asp:Button ID="btnGenerarBackup" runat="server"
                                Text="💾 Generar Backup ahora"
                                CssClass="btn-primary btn-backup"
                                OnClick="btnGenerarBackup_Click"
                                CausesValidation="false"
                                OnClientClick="return confirm('¿Confirmar generación de backup? Se creará un nuevo archivo .bak en la carpeta BackupsSQL.');" />

                            <!-- Resultado del backup -->
                            <asp:Panel ID="pnlResultadoBackup" runat="server"
                                CssClass="resultado-backup" Visible="false">
                                <div class="resultado-icono">✓</div>
                                <div>
                                    <p class="resultado-titulo">Backup generado correctamente</p>
                                    <asp:Label ID="lblResultadoBackup" runat="server"
                                        CssClass="resultado-texto" Text="" />
                                </div>
                            </asp:Panel>

                        </div>
                    </div>

                    <!-- ============================================
                         COLUMNA DERECHA: RESTAURAR BACKUP
                         ============================================ -->
                    <div class="br-col">
                        <div class="content-card br-card">
                            <div class="br-card-header">
                                <div class="br-icono-wrap br-icono-restore">🔄</div>
                                <div>
                                    <h2 class="card-title">Restaurar Backup</h2>
                                    <p class="card-subtitle">Restaura la base de datos desde un archivo .bak existente.</p>
                                </div>
                            </div>

                            <div class="section-sep">Archivos disponibles</div>

                            <!-- Listado de archivos .bak -->
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
                            </div>

                            <asp:HiddenField ID="hfArchivoSeleccionado" runat="server"
                                Value="" ClientIDMode="Static" />

                            <!-- Confirmación de restore -->
                            <asp:Panel ID="pnlConfirmRestore" runat="server"
                                CssClass="confirm-restore-panel" Visible="false">
                                <div class="confirm-restore-aviso">
                                    🚨 <strong>Atención:</strong> La restauración reemplazará
                                    <strong>todos los datos actuales</strong> de la base de datos
                                    con el contenido del archivo seleccionado. Esta acción no se puede deshacer.
                                </div>
                                <div class="confirm-restore-archivo">
                                    Archivo seleccionado:
                                    <asp:Label ID="lblArchivoARestaurar" runat="server"
                                        CssClass="confirm-archivo-nombre" Text="" />
                                </div>
                                <div class="confirm-restore-actions">
                                    <asp:Button ID="btnCancelarRestore" runat="server"
                                        Text="Cancelar"
                                        CssClass="btn-secondary"
                                        OnClick="btnCancelarRestore_Click"
                                        CausesValidation="false" />
                                    <asp:Button ID="btnConfirmarRestore" runat="server"
                                        Text="🔄 Confirmar restauración"
                                        CssClass="btn-danger-solid"
                                        OnClick="btnConfirmarRestore_Click"
                                        CausesValidation="false" />
                                </div>
                            </asp:Panel>

                            <!-- Botón iniciar restore -->
                            <asp:Button ID="btnIniciarRestore" runat="server"
                                Text="Seleccionar archivo y restaurar →"
                                CssClass="btn-restore"
                                OnClick="btnIniciarRestore_Click"
                                CausesValidation="false" />

                            <!-- Resultado del restore -->
                            <asp:Panel ID="pnlResultadoRestore" runat="server"
                                CssClass="resultado-backup" Visible="false">
                                <div class="resultado-icono">✓</div>
                                <div>
                                    <p class="resultado-titulo">Base de datos restaurada correctamente</p>
                                    <asp:Label ID="lblResultadoRestore" runat="server"
                                        CssClass="resultado-texto" Text="" />
                                </div>
                            </asp:Panel>

                        </div>
                    </div>

                </div>

                <!-- HISTORIAL DE OPERACIONES -->
                <div class="content-card mt-24">
                    <div class="card-header">
                        <h3 class="card-title" style="font-size:18px;">Historial de operaciones</h3>
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
