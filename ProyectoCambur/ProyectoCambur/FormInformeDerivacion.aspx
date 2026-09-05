<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormInformeDerivacion.aspx.cs" Inherits="FormInformeDerivacion" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Informe de Derivación</title>
    <link href="EstilosPaginas/Shared.css"                  rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"           rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"       rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormInformeDerivacion.css"   rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="acceder_informe_derivacion" />
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

                <asp:Label ID="lblMensaje" runat="server"
                    Visible="false" CssClass="server-error" />

                <asp:Panel ID="pnlFormulario" runat="server">
                    <div class="derivacion-layout">

                        <div class="derivacion-form-col">
                            <div class="content-card">

                                <div class="field full-col" style="margin-bottom:16px;">
                                    <asp:Label ID="lblEtiquetaPacienteDerivacion" runat="server" AssociatedControlID="ddlPacienteDerivacion" Text="" />
                                    <asp:DropDownList ID="ddlPacienteDerivacion" runat="server"
                                        ClientIDMode="Static" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPacienteDerivacion_SelectedIndexChanged" />
                                    <asp:RequiredFieldValidator ID="rfvPacienteDerivacion" runat="server"
                                        ControlToValidate="ddlPacienteDerivacion"
                                        InitialValue=""
                                        ErrorMessage="."
                                        CssClass="field-error" Display="Dynamic"
                                        ValidationGroup="vgDerivacion" />
                                </div>


                                <div class="paciente-header">
                                    <div class="paciente-header-avatar">
                                        <asp:Label ID="lblPacienteIniciales"
                                            runat="server" Text="MG" />
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

                                <asp:Label ID="lblSeccionDatosDerivacion" runat="server" CssClass="section-sep" Text="" />

                                <div class="grid-2">
                                    <div class="field">
                                        <asp:Label ID="lblEtiquetaEspecialidad" runat="server" AssociatedControlID="ddlEspecialidad" Text="" />
                                        <asp:DropDownList ID="ddlEspecialidad" runat="server"
                                            ClientIDMode="Static">
                                            <asp:ListItem Value="" Text="" />
                                            <asp:ListItem Value="PSI" Text="" />
                                            <asp:ListItem Value="NEU" Text="" />
                                            <asp:ListItem Value="CAR" Text="" />
                                            <asp:ListItem Value="NUT" Text="" />
                                            <asp:ListItem Value="TRA" Text="" />
                                            <asp:ListItem Value="MED" Text="" />
                                            <asp:ListItem Value="FIS" Text="" />
                                            <asp:ListItem Value="OTR" Text="" />
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="rfvEsp" runat="server"
                                            ControlToValidate="ddlEspecialidad"
                                            InitialValue=""
                                            ErrorMessage="."
                                            CssClass="field-error" Display="Dynamic"
                                            ValidationGroup="vgDerivacion" />
                                    </div>

                                    <div class="field">
                                        <asp:Label ID="lblEtiquetaProfDestino" runat="server" AssociatedControlID="txtProfDestino" Text="" />
                                        <asp:TextBox ID="txtProfDestino" runat="server"
                                            MaxLength="150"
                                            ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="rfvProf" runat="server"
                                            ControlToValidate="txtProfDestino"
                                            ErrorMessage="."
                                            CssClass="field-error" Display="Dynamic"
                                            ValidationGroup="vgDerivacion" />
                                    </div>

                                    <div class="field full-col">
                                        <asp:Label ID="lblEtiquetaInstitucion" runat="server" AssociatedControlID="txtInstitucion" Text="" />
                                        <asp:TextBox ID="txtInstitucion" runat="server"
                                            MaxLength="200"
                                            ClientIDMode="Static" />
                                    </div>

                                    <div class="field full-col">
                                        <asp:Label ID="lblEtiquetaMotivo" runat="server" AssociatedControlID="txtMotivo" Text="" />
                                        <asp:TextBox ID="txtMotivo" runat="server"
                                            TextMode="MultiLine" Rows="4"
                                            ClientIDMode="Static" />
                                        <asp:RequiredFieldValidator ID="rfvMotivo" runat="server"
                                            ControlToValidate="txtMotivo"
                                            ErrorMessage="."
                                            CssClass="field-error" Display="Dynamic"
                                            ValidationGroup="vgDerivacion" />
                                    </div>
                                </div>

                                <div class="ia-aviso-derivacion">
                                    <div class="ia-aviso-icono">🤖</div>
                                    <div class="ia-aviso-texto">
                                        <strong><asp:Label ID="lblAvisoIATitulo" runat="server" Text="" /></strong>
                                        <asp:Label ID="lblAvisoIA" runat="server"
                                            CssClass="ia-aviso-detalle" Text="" />
                                    </div>
                                </div>

                                <div class="form-actions">
                                    <asp:HyperLink ID="lnkCancelar" runat="server" NavigateUrl="FormMenu.aspx" CssClass="btn-secondary" Text="" />
                                    <asp:Button ID="btnGenerar" runat="server"
                                        Text=""
                                        CssClass="btn-primary btn-ia"
                                        ValidationGroup="vgDerivacion"
                                        OnClick="btnGenerar_Click"
                                        OnClientClick="mostrarCarga(); return true;" />
                                </div>

                            </div>
                        </div>

                        <div class="derivacion-aside">
                            <div class="content-card info-card">
                                <p class="accesos-titulo"><asp:Label ID="lblTituloInfoIncluida" runat="server" Text="" /></p>
                                <div class="info-item">
                                    <span>📋</span>
                                    <asp:Label ID="lblInfoConsultas" runat="server"
                                        CssClass="info-item-texto" Text="" />
                                </div>
                                <div class="info-item">
                                    <span>🧬</span>
                                    <asp:Label runat="server" CssClass="info-item-texto" ID="lblInfoAntecedentes" Text="" />
                                </div>
                                <div class="info-item">
                                    <span>📈</span>
                                    <asp:Label runat="server" CssClass="info-item-texto" ID="lblInfoEvolucionTexto" Text="" />
                                </div>
                                <div class="info-item">
                                    <span>🛠️</span>
                                    <asp:Label runat="server" CssClass="info-item-texto" ID="lblInfoIntervenciones" Text="" />
                                </div>
                            </div>

                            <div class="content-card aviso-card">
                                <div class="aviso-icon">🔒</div>
                                <p class="aviso-titulo"><asp:Label ID="lblAvisoRevisionObligatoriaTitulo" runat="server" Text="" /></p>
                                <p class="aviso-texto"><asp:Label ID="lblAvisoRevisionObligatoriaTexto" runat="server" Text="" /></p>
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

                <asp:HiddenField ID="hdnIdInforme" runat="server" ClientIDMode="Static" />

                <asp:Panel ID="pnlAuditoria" runat="server"
                    CssClass="auditoria-layout" Visible="false">

                    <div class="auditoria-form-col">
                        <div class="content-card">

                            <div class="auditoria-header">
                                <div>
                                    <h2 class="card-title"><asp:Label ID="lblTituloInformeGenerado" runat="server" Text="" /></h2>
                                    <asp:Label ID="lblAuditoriaMeta" runat="server"
                                        CssClass="card-subtitle" Text="" />
                                </div>
                                <asp:Label ID="lblBadgePendienteRevision" runat="server"
                                    CssClass="badge-pendiente-revision"
                                    Text="" />
                            </div>

                            <div class="ia-badge-resultado">
                                🤖 <asp:Label ID="lblAvisoIABadgeInforme" runat="server" Text="" />
                            </div>

                            <asp:Label ID="lblSeccionContenidoInforme" runat="server" CssClass="section-sep" Text="" />

                            <div class="auditoria-secciones">

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaSintesis" runat="server" AssociatedControlID="txtSintesisDiagnostica" Text="" />
                                    <asp:TextBox ID="txtSintesisDiagnostica" runat="server"
                                        TextMode="MultiLine" Rows="5"
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaAndamiajes" runat="server" AssociatedControlID="txtAndamiajes" Text="" />
                                    <asp:TextBox ID="txtAndamiajes" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static" />
                                </div>

                                <div class="field">
                                    <asp:Label ID="lblEtiquetaObjetivos" runat="server" AssociatedControlID="txtObjetivos" Text="" />
                                    <asp:TextBox ID="txtObjetivos" runat="server"
                                        TextMode="MultiLine" Rows="4"
                                        ClientIDMode="Static" />
                                </div>

                                <div class="grid-2">
                                    <div class="field">
                                        <asp:Label ID="lblEtiquetaModalidad" runat="server" AssociatedControlID="txtModalidadTrabajo" Text="" />
                                        <asp:TextBox ID="txtModalidadTrabajo" runat="server"
                                            TextMode="MultiLine" Rows="3"
                                            ClientIDMode="Static" />
                                    </div>
                                    <div class="field">
                                        <asp:Label ID="lblEtiquetaMotivoAuditoria" runat="server" AssociatedControlID="txtMotivoDerivacion" Text="" />
                                        <asp:TextBox ID="txtMotivoDerivacion" runat="server"
                                            TextMode="MultiLine" Rows="3"
                                            ClientIDMode="Static" />
                                    </div>
                                </div>

                            </div>

                            <asp:Label ID="lblSeccionFirma" runat="server" CssClass="section-sep" Text="" />

                            <div class="field" style="max-width:400px;">
                                <asp:Label ID="lblEtiquetaFirma" runat="server" AssociatedControlID="txtFirma" Text="" />
                                <asp:TextBox ID="txtFirma" runat="server"
                                    MaxLength="200"
                                    ClientIDMode="Static" />
                                <asp:RequiredFieldValidator ID="rfvFirma" runat="server"
                                    ControlToValidate="txtFirma"
                                    ErrorMessage="."
                                    CssClass="field-error" Display="Dynamic"
                                    ValidationGroup="vgAuditoria" />
                            </div>

                            <div class="form-actions">
                                <asp:Button ID="btnDescartar" runat="server"
                                    Text=""
                                    CssClass="btn-danger"
                                    OnClick="btnDescartar_Click"
                                    CausesValidation="false"
                                    OnClientClick="return confirmarDescarte();" />
                                <asp:Button ID="btnGuardarBorrador" runat="server"
                                    Text=""
                                    CssClass="btn-secondary"
                                    OnClick="btnGuardarBorrador_Click"
                                    CausesValidation="false" />
                                <asp:Button ID="btnValidar" runat="server"
                                    Text=""
                                    CssClass="btn-success"
                                    ValidationGroup="vgAuditoria"
                                    OnClick="btnValidar_Click" />
                            </div>

                        </div>
                    </div>

                    <div class="auditoria-aside">

                        <div class="content-card meta-card">
                            <p class="accesos-titulo"><asp:Label ID="lblTituloDatosInforme" runat="server" Text="" /></p>
                            <div class="meta-fila">
                                <asp:Label runat="server" CssClass="meta-label" ID="lblMetaLabelPaciente" Text="" />
                                <asp:Label ID="lblMetaPaciente" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <asp:Label runat="server" CssClass="meta-label" ID="lblMetaLabelEspecialidad" Text="" />
                                <asp:Label ID="lblMetaEspecialidad" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <asp:Label runat="server" CssClass="meta-label" ID="lblMetaLabelDestino" Text="" />
                                <asp:Label ID="lblMetaDestino" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila" style="border-bottom:none;">
                                <asp:Label runat="server" CssClass="meta-label" ID="lblMetaLabelFecha" Text="" />
                                <asp:Label ID="lblMetaFecha" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">⚠️</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoRevisionProfesionalTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoRevisionProfesionalTexto" runat="server" Text="" /></p>
                        </div>

                    </div>

                </asp:Panel>

            </div>
        </div>

    </form>

    <script type="text/javascript">
        var MSG_CONFIRMAR_DESCARTE = <%= JsonConfirmarDescarte %>;

        function mostrarCarga() {
            var o = document.getElementById('cargaOverlay');
            if (o) o.style.display = 'flex';
            return true;
        }
        function confirmarDescarte() {
            return confirm(MSG_CONFIRMAR_DESCARTE);
        }
        window.addEventListener('DOMContentLoaded', function () {
            var o = document.getElementById('cargaOverlay');
            if (o) o.style.display = 'none';
        });
    </script>
</body>
</html>