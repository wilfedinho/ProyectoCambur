<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormExportarReporte.aspx.cs" Inherits="FormExportarReporte" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Exportar Reporte</title>
    <link href="EstilosPaginas/Shared.css"               rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"        rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"    rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormExportarReporte.css"  rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Gestión Clínica</div>
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_exportar_reporte" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Documentación</span>
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderTitulo" runat="server" CssClass="header-page" Text="Exportar reporte" />
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <div class="exportar-layout">

                    <div class="exportar-main">

                        <div class="content-card">
                            <div class="card-header">
                                <h2 class="card-title"><asp:Label ID="lblCardTitulo" runat="server" Text="" /></h2>
                                <p class="card-subtitle"><asp:Label ID="lblCardSubtitulo" runat="server" Text="" /></p>
                            </div>

                            <asp:Label ID="lblSeccionPaciente" runat="server" CssClass="section-sep" Text="" />

                            <div class="grid-2" style="max-width:500px;">
                                <div class="field full">
                                    <asp:Label ID="lblEtiquetaPaciente" runat="server" AssociatedControlID="ddlPaciente" Text="" />
                                    <asp:DropDownList ID="ddlPaciente" runat="server"
                                        ClientIDMode="Static" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPaciente_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </div>

                            <asp:Label ID="lblSeccionTipoDoc" runat="server" CssClass="section-sep" Text="" />

                            <asp:HiddenField ID="hfTipoSeleccionado" runat="server" Value="" ClientIDMode="Static" />
                            <asp:HiddenField ID="hfDocumentoSeleccionado" runat="server" Value="" ClientIDMode="Static" />

                            <div class="documentos-grid">

                                <div class="doc-card <%= ClaseSeleccionado(BLL.GestorExportacion.TIPO_RESUMEN) %>" id="docResumen"
                                     data-disponible="<%= DisponibleResumen ? "true" : "false" %>"
                                     onclick="seleccionarDocumento(this, '<%= BLL.GestorExportacion.TIPO_RESUMEN %>')">
                                    <div class="doc-icono">🤖</div>
                                    <div class="doc-info">
                                        <asp:Label ID="lblTipoResumen" runat="server" CssClass="doc-tipo" Text="" />
                                        <asp:Label ID="lblFechaResumen" runat="server" CssClass="doc-meta" Text="" />
                                        <asp:Label ID="lblEstadoResumen" runat="server" CssClass="doc-badge doc-badge-ok" Text="" />
                                    </div>
                                    <span class="doc-check"><%= CheckIcono(BLL.GestorExportacion.TIPO_RESUMEN) %></span>
                                </div>

                                <div class="doc-card <%= ClaseSeleccionado(BLL.GestorExportacion.TIPO_DERIVACION) %>" id="docDerivacion"
                                     data-disponible="<%= DisponibleDerivacion ? "true" : "false" %>"
                                     onclick="seleccionarDocumento(this, '<%= BLL.GestorExportacion.TIPO_DERIVACION %>')">
                                    <div class="doc-icono">📤</div>
                                    <div class="doc-info">
                                        <asp:Label ID="lblTipoDerivacion" runat="server" CssClass="doc-tipo" Text="" />
                                        <asp:Label ID="lblFechaDerivacion" runat="server" CssClass="doc-meta" Text="" />
                                        <asp:Label ID="lblEstadoDerivacion" runat="server" CssClass="doc-badge doc-badge-ok" Text="" />
                                    </div>
                                    <span class="doc-check"><%= CheckIcono(BLL.GestorExportacion.TIPO_DERIVACION) %></span>
                                </div>

                                <div class="doc-card <%= ClaseSeleccionado(BLL.GestorExportacion.TIPO_PERFIL) %>" id="docPerfil"
                                     data-disponible="<%= DisponiblePerfil ? "true" : "false" %>"
                                     onclick="seleccionarDocumento(this, '<%= BLL.GestorExportacion.TIPO_PERFIL %>')">
                                    <div class="doc-icono">🧠</div>
                                    <div class="doc-info">
                                        <asp:Label ID="lblTipoPerfil" runat="server" CssClass="doc-tipo" Text="" />
                                        <asp:Label ID="lblFechaPerfil" runat="server" CssClass="doc-meta" Text="" />
                                        <asp:Label ID="lblEstadoPerfil" runat="server" CssClass="doc-badge doc-badge-ok" Text="" />
                                    </div>
                                    <span class="doc-check"><%= CheckIcono(BLL.GestorExportacion.TIPO_PERFIL) %></span>
                                </div>

                            </div>

                            <div class="documentos-lista-wrap" id="wrapDocumentosLista">
                                <asp:Label ID="lblElegirDocumento" runat="server" CssClass="section-sep" Text="" />
                                <div class="documentos-lista" id="documentosLista"></div>
                            </div>

                            <div class="form-actions">
                                <asp:HyperLink ID="lnkVolver" runat="server" NavigateUrl="~/FormDashboard.aspx" CssClass="btn-secondary" Text="" />
                                <asp:Button ID="btnExportar" runat="server"
                                    Text="📄 Exportar en PDF"
                                    CssClass="btn-primary btn-exportar"
                                    OnClick="btnExportar_Click"
                                    CausesValidation="false" />
                            </div>
                        </div>

                    </div>

                    <div class="exportar-aside">

                        <div class="content-card historial-exportaciones-card">
                            <p class="accesos-titulo"><asp:Label ID="lblExportacionesRecientesTitulo" runat="server" Text="" /></p>
                            <asp:Repeater ID="rptExportaciones" runat="server">
                                <ItemTemplate>
                                    <div class="exp-item">
                                        <span class="exp-icono"><%# Eval("Icono") %></span>
                                        <div class="exp-info">
                                            <span class="exp-tipo"><%# Eval("Tipo") %></span>
                                            <span class="exp-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy HH:mm}") %></span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <asp:Label ID="lblSinExportaciones" runat="server"
                                CssClass="sin-perfiles-txt" Visible="false" Text="" />
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">📄</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoFormatoTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoFormatoTexto" runat="server" Text="" /></p>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoProteccionTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoProteccionTexto" runat="server" Text="" /></p>
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </form>

    <script type="text/javascript">
        var DOCS_DISPONIBLES = <%= DocumentosDisponiblesJson %>;

        function seleccionarDocumento(card, tipo) {
            if (card.getAttribute('data-disponible') !== 'true') return;

            document.querySelectorAll('.doc-card').forEach(function (c) {
                c.classList.remove('seleccionado');
                var chk = c.querySelector('.doc-check');
                if (chk) chk.textContent = '○';
            });

            card.classList.add('seleccionado');
            var chk = card.querySelector('.doc-check');
            if (chk) chk.textContent = '●';

            var hf = document.getElementById('hfTipoSeleccionado');
            if (hf) hf.value = tipo;

            renderListaDocumentos(tipo, null);
        }

        function seleccionarDocumentoEspecifico(fila, idDocumento) {
            document.querySelectorAll('.doc-version-item').forEach(function (f) {
                f.classList.remove('seleccionado');
                var chk = f.querySelector('.doc-version-check');
                if (chk) chk.textContent = '○';
            });

            fila.classList.add('seleccionado');
            var chk = fila.querySelector('.doc-version-check');
            if (chk) chk.textContent = '●';

            var hf = document.getElementById('hfDocumentoSeleccionado');
            if (hf) hf.value = idDocumento;
        }

        function renderListaDocumentos(tipo, idPreseleccionado) {
            var wrap = document.getElementById('wrapDocumentosLista');
            var contenedor = document.getElementById('documentosLista');
            if (!wrap || !contenedor) return;

            var documentos = (DOCS_DISPONIBLES && DOCS_DISPONIBLES[tipo]) ? DOCS_DISPONIBLES[tipo] : [];
            contenedor.innerHTML = '';

            if (!tipo || documentos.length === 0) {
                wrap.style.display = 'none';
                var hfVacio = document.getElementById('hfDocumentoSeleccionado');
                if (hfVacio) hfVacio.value = '';
                return;
            }

            wrap.style.display = '';

            var idSeleccionado = idPreseleccionado;
            if (!idSeleccionado || !documentos.some(function (d) { return String(d.id) === String(idSeleccionado); })) {
                idSeleccionado = documentos[0].id;
            }

            documentos.forEach(function (doc) {
                var fila = document.createElement('div');
                fila.className = 'doc-version-item' + (String(doc.id) === String(idSeleccionado) ? ' seleccionado' : '');
                fila.setAttribute('onclick', 'seleccionarDocumentoEspecifico(this, ' + doc.id + ')');

                var info = document.createElement('div');
                info.className = 'doc-version-info';

                var fecha = document.createElement('span');
                fecha.className = 'doc-version-fecha';
                fecha.textContent = doc.fecha;
                info.appendChild(fecha);

                if (doc.detalle) {
                    var detalle = document.createElement('span');
                    detalle.className = 'doc-version-detalle';
                    detalle.textContent = doc.detalle;
                    info.appendChild(detalle);
                }

                var check = document.createElement('span');
                check.className = 'doc-version-check';
                check.textContent = String(doc.id) === String(idSeleccionado) ? '●' : '○';

                fila.appendChild(info);
                fila.appendChild(check);
                contenedor.appendChild(fila);
            });

            var hf = document.getElementById('hfDocumentoSeleccionado');
            if (hf) hf.value = idSeleccionado;
        }

        (function inicializarListaDocumentos() {
            var hfTipo = document.getElementById('hfTipoSeleccionado');
            var hfDoc = document.getElementById('hfDocumentoSeleccionado');
            renderListaDocumentos(hfTipo ? hfTipo.value : '', hfDoc ? hfDoc.value : null);
        })();
    </script>
</body>
</html>