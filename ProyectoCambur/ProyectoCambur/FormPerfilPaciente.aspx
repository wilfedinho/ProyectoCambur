<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormPerfilPaciente.aspx.cs" Inherits="FormPerfilPaciente" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Perfilación del Paciente</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"       rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"   rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormPerfilPaciente.css"  rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_perfilacion_paciente" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
            </div>
        </aside>


        <div class="main-wrap">


            <header class="top-header">
                <div class="header-title">
                    <asp:Label ID="lblHeaderSeccion" runat="server" CssClass="header-section" Text="" />
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderTitulo" runat="server"
                        CssClass="header-page" Text="" />
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>


            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server"
                    Visible="false" CssClass="server-error" />


                <asp:Panel ID="pnlSeleccion" runat="server">
                    <div class="perfil-layout">

                        <div class="perfil-form-col">


                            <div class="content-card">
                                <div class="field full-col" style="margin-bottom:16px;">
                                    <label for="ddlPacientePerfil"><asp:Label ID="lblEtiquetaPacientePerfil" runat="server" Text="" /> <sup>*</sup></label>
                                    <asp:DropDownList ID="ddlPacientePerfil" runat="server"
                                        ClientIDMode="Static" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPacientePerfil_SelectedIndexChanged" />
                                </div>

                                <div class="paciente-header">
                                    <div class="paciente-header-avatar">
                                        <asp:Label ID="lblPacienteIniciales" runat="server" Text="" />
                                    </div>
                                    <div class="paciente-header-info">
                                        <asp:Label ID="lblPacienteNombre" runat="server"
                                            CssClass="paciente-header-nombre" Text="" />
                                        <div class="paciente-header-meta">
                                            <asp:Label ID="lblPacienteEdad"
                                                runat="server" CssClass="meta-item" Text="" />
                                            <span class="meta-sep">·</span>
                                            <asp:Label ID="lblPacienteConsultas"
                                                runat="server" CssClass="meta-item" Text="" />
                                        </div>
                                    </div>
                                </div>

                                <div class="ia-badge-aviso">
                                    <asp:Label ID="lblAvisoPerfil" runat="server" Text="" />
                                </div>

                                <div class="section-sep"><asp:Label ID="lblSeccionSeleccionarModelo" runat="server" Text="" /></div>
                                <p class="hint-text"><asp:Label ID="lblHintSeleccionarModelo" runat="server" Text="" /></p>


                                <asp:HiddenField ID="hfModeloSeleccionado" runat="server"
                                    Value="" ClientIDMode="Static" />


                                <div class="modelos-grid">

                                    <div class="modelo-card" id="cardBigFive"
                                         onclick="seleccionarModelo(this, 'BIGFIVE')">
                                        <div class="modelo-icono">🌐</div>
                                        <div class="modelo-info">
                                            <div class="modelo-nombre"><asp:Label ID="lblModeloBigFiveNombre" runat="server" Text="" /></div>
                                            <div class="modelo-desc"><asp:Label ID="lblModeloBigFiveDesc" runat="server" Text="" /></div>
                                        </div>
                                        <div class="modelo-check">○</div>
                                    </div>

                                    <div class="modelo-card" id="cardCOPE"
                                         onclick="seleccionarModelo(this, 'COPE')">
                                        <div class="modelo-icono">🛡️</div>
                                        <div class="modelo-info">
                                            <div class="modelo-nombre"><asp:Label ID="lblModeloCopeNombre" runat="server" Text="" /></div>
                                            <div class="modelo-desc"><asp:Label ID="lblModeloCopeDesc" runat="server" Text="" /></div>
                                        </div>
                                        <div class="modelo-check">○</div>
                                    </div>

                                    <div class="modelo-card" id="cardAutoeficacia"
                                         onclick="seleccionarModelo(this, 'AUTOEFICACIA')">
                                        <div class="modelo-icono">⚡</div>
                                        <div class="modelo-info">
                                            <div class="modelo-nombre"><asp:Label ID="lblModeloAutoeficaciaNombre" runat="server" Text="" /></div>
                                            <div class="modelo-desc"><asp:Label ID="lblModeloAutoeficaciaDesc" runat="server" Text="" /></div>
                                        </div>
                                        <div class="modelo-check">○</div>
                                    </div>

                                    <div class="modelo-card" id="cardApego"
                                         onclick="seleccionarModelo(this, 'APEGO')">
                                        <div class="modelo-icono">🔗</div>
                                        <div class="modelo-info">
                                            <div class="modelo-nombre"><asp:Label ID="lblModeloApegoNombre" runat="server" Text="" /></div>
                                            <div class="modelo-desc"><asp:Label ID="lblModeloApegoDesc" runat="server" Text="" /></div>
                                        </div>
                                        <div class="modelo-check">○</div>
                                    </div>

                                    <div class="modelo-card" id="cardValores"
                                         onclick="seleccionarModelo(this, 'VALORES')">
                                        <div class="modelo-icono">🌱</div>
                                        <div class="modelo-info">
                                            <div class="modelo-nombre"><asp:Label ID="lblModeloValoresNombre" runat="server" Text="" /></div>
                                            <div class="modelo-desc"><asp:Label ID="lblModeloValoresDesc" runat="server" Text="" /></div>
                                        </div>
                                        <div class="modelo-check">○</div>
                                    </div>

                                </div>

                                <div class="form-actions">
                                    <asp:HyperLink ID="lnkCancelar" runat="server" NavigateUrl="~/FormMenu.aspx" CssClass="btn-secondary" Text="" />
                                    <asp:Button ID="btnGenerar" runat="server"
                                        Text=""
                                        CssClass="btn-primary btn-ia"
                                        OnClick="btnGenerar_Click"
                                        CausesValidation="false"
                                        OnClientClick="return validarYCargar();" />
                                </div>

                            </div>
                        </div>


                        <div class="perfil-aside">

                            <div class="content-card modelo-seleccionado-card">
                                <p class="accesos-titulo"><asp:Label ID="lblTituloModeloSeleccionado" runat="server" Text="" /></p>
                                <div id="modeloSeleccionadoInfo" class="modelo-sel-vacio">
                                    <asp:Label ID="lblNingunModeloSeleccionado" runat="server" Text="" />
                                </div>
                            </div>

                            <div class="content-card perfiles-anteriores-card">
                                <p class="accesos-titulo"><asp:Label ID="lblTituloPerfilesAnteriores" runat="server" Text="" /></p>
                                <asp:Repeater ID="rptPerfilesAnteriores" runat="server" OnItemCommand="rptPerfilesAnteriores_ItemCommand">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CssClass="perfil-anterior-item"
                                            CommandName="VerPerfil" CommandArgument='<%# Eval("IdPerfil") %>'
                                            style="display:flex; width:100%; text-align:left; background:none; border:none; cursor:pointer;">
                                            <span class="pa-modelo"><%# Eval("Modelo") %></span>
                                            <span class="pa-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Label ID="lblSinPerfiles" runat="server"
                                    CssClass="sin-perfiles-txt"
                                    Text=""
                                    Visible="false" />
                            </div>

                            <div class="content-card aviso-card">
                                <div class="aviso-icon">🔒</div>
                                <p class="aviso-titulo"><asp:Label ID="lblAvisoTituloDatosEncriptados" runat="server" Text="" /></p>
                                <p class="aviso-texto"><asp:Label ID="lblAvisoTextoDatosEncriptados" runat="server" Text="" /></p>
                            </div>

                        </div>
                    </div>
                </asp:Panel>


                <div class="carga-overlay" id="cargaOverlay" style="display:none;">
                    <div class="carga-card">
                        <div class="carga-spinner"></div>
                        <p class="carga-titulo"><asp:Label ID="lblCargaTitulo" runat="server" Text="" /></p>
                        <p class="carga-subtitulo"><asp:Label ID="lblCargaSubtitulo" runat="server" Text="" /></p>
                    </div>
                </div>


                <asp:Panel ID="pnlResultado" runat="server"
                    CssClass="resultado-layout" Visible="false">

                    <div class="resultado-main">
                        <div class="content-card">

                            <div class="resultado-header">
                                <div>
                                    <h2 class="card-title"><asp:Label ID="lblTituloPerfilGenerado" runat="server" Text="" /></h2>
                                    <asp:Label ID="lblResultadoMeta" runat="server"
                                        CssClass="card-subtitle" Text="" />
                                </div>
                                <div class="resultado-header-actions">
                                    <asp:Button ID="btnNuevoPerfil" runat="server"
                                        Text=""
                                        CssClass="btn-secondary"
                                        OnClick="btnNuevoPerfil_Click"
                                        CausesValidation="false" />
                                </div>
                            </div>


                            <div class="modelo-usado-badge">
                                <asp:Label ID="lblModeloUsado" runat="server"
                                    CssClass="modelo-badge-texto" Text="" />
                            </div>


                            <div class="ia-badge-resultado">
                                <asp:Label ID="lblAvisoIABadgePerfil" runat="server" Text="" />
                            </div>


                            <div class="perfil-secciones">

                                <div class="perfil-seccion">
                                    <div class="ps-titulo">
                                        <span class="ps-icono">📌</span> <asp:Label ID="lblSeccionDescripcionGeneral" runat="server" Text="" />
                                    </div>
                                    <asp:Label ID="lblDescripcionGeneral" runat="server"
                                        CssClass="ps-texto" Text="" />
                                </div>

                                <div class="perfil-seccion">
                                    <div class="ps-titulo">
                                        <span class="ps-icono">📊</span> <asp:Label ID="lblSeccionDimensionesEvaluadas" runat="server" Text="" />
                                    </div>
                                    <asp:Label ID="lblDimensiones" runat="server"
                                        CssClass="ps-texto" Text="" />
                                </div>

                                <div class="perfil-seccion">
                                    <div class="ps-titulo">
                                        <span class="ps-icono">🔍</span> <asp:Label ID="lblSeccionPatronesIdentificados" runat="server" Text="" />
                                    </div>
                                    <asp:Label ID="lblPatrones" runat="server"
                                        CssClass="ps-texto" Text="" />
                                </div>

                                <div class="perfil-seccion" style="border-bottom:none; margin-bottom:0; padding-bottom:0;">
                                    <div class="ps-titulo">
                                        <span class="ps-icono">💡</span> <asp:Label ID="lblSeccionConsideracionesTratamiento" runat="server" Text="" />
                                    </div>
                                    <asp:Label ID="lblConsideraciones" runat="server"
                                        CssClass="ps-texto" Text="" />
                                </div>

                            </div>


                            <div class="perfil-nota-pie">
                                <asp:Label ID="lblNotaPiePerfil" runat="server" Text="" />
                            </div>

                        </div>
                    </div>


                    <div class="resultado-aside">

                        <div class="content-card meta-resultado-card">
                            <p class="accesos-titulo"><asp:Label ID="lblTituloDetallesPerfil" runat="server" Text="" /></p>
                            <div class="meta-fila">
                                <span class="meta-label"><asp:Label ID="lblMetaLabelPaciente" runat="server" Text="" /></span>
                                <asp:Label ID="lblMetaPaciente" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <span class="meta-label"><asp:Label ID="lblMetaLabelModelo" runat="server" Text="" /></span>
                                <asp:Label ID="lblMetaModelo" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <span class="meta-label"><asp:Label ID="lblMetaLabelConsultas" runat="server" Text="" /></span>
                                <asp:Label ID="lblMetaConsultas" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila" style="border-bottom:none;">
                                <span class="meta-label"><asp:Label ID="lblMetaLabelFecha" runat="server" Text="" /></span>
                                <asp:Label ID="lblMetaFecha" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                        </div>

                        <div class="content-card accesos-card">
                            <p class="accesos-titulo"><asp:Label ID="lblTituloAccionesRelacionadas" runat="server" Text="" /></p>
                            <a href="FormExportarReporte.aspx"   class="acceso-item">💾 <span><asp:Label ID="lblAccesoExportarPdf" runat="server" Text="" /></span></a>
                            <a href="FormResumenIA.aspx"         class="acceso-item">🤖 <span><asp:Label ID="lblAccesoResumenIA" runat="server" Text="" /></span></a>
                            <a href="FormInformeDerivacion.aspx" class="acceso-item">📤 <span><asp:Label ID="lblAccesoGenerarDerivacion" runat="server" Text="" /></span></a>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoTituloPerfilEncriptado" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoTextoPerfilEncriptado" runat="server" Text="" /></p>
                        </div>

                    </div>

                </asp:Panel>

            </div>
        </div>

    </form>

    <script type="text/javascript">
        var modeloActual = '';

        var MODELOS_INFO = <%= ModelosInfoJson %>;
        var MSG_SELECCIONAR_MODELO = <%= JsonSeleccionarModeloAlerta %>;

        function seleccionarModelo(card, codigo) {

            document.querySelectorAll('.modelo-card').forEach(function (c) {
                c.classList.remove('seleccionado');
                var chk = c.querySelector('.modelo-check');
                if (chk) chk.textContent = '○';
            });


            card.classList.add('seleccionado');
            var chk = card.querySelector('.modelo-check');
            if (chk) chk.textContent = '●';


            modeloActual = codigo;
            var hf = document.getElementById('hfModeloSeleccionado');
            if (hf) hf.value = codigo;


            var info = MODELOS_INFO[codigo];
            var panel = document.getElementById('modeloSeleccionadoInfo');
            if (panel && info) {
                panel.className = 'modelo-sel-activo';
                panel.innerHTML =
                    '<span class="ms-icono">' + info.icono + '</span>' +
                    '<div><div class="ms-nombre">' + info.nombre + '</div>' +
                    '<div class="ms-desc">' + info.desc + '</div></div>';
            }
        }

        function validarYCargar() {
            var hf = document.getElementById('hfModeloSeleccionado');
            if (!hf || !hf.value) {
                alert(MSG_SELECCIONAR_MODELO);
                return false;
            }
            var o = document.getElementById('cargaOverlay');
            if (o) o.style.display = 'flex';
            return true;
        }

        window.addEventListener('DOMContentLoaded', function () {
            var o = document.getElementById('cargaOverlay');
            if (o) o.style.display = 'none';
        });
    </script>
</body>
</html>