<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormCambiarClave.aspx.cs" Inherits="FormCambiarClave" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Cambiar Clave</title>
    <link href="EstilosPaginas/Shared.css"            rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"     rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css" rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormCambiarClave.css"  rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout">🚪 <asp:Label ID="lblMenuCerrarSesion" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <asp:Label ID="lblHeaderSeccion" runat="server" CssClass="header-section" Text="" />
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderPagina" runat="server" CssClass="header-page" Text="" />
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

                <div class="clave-layout">

                    <div class="clave-main">
                        <div class="content-card">

                            <div class="card-header">
                                <h2 class="card-title"><asp:Label ID="lblTituloCard" runat="server" Text="" /></h2>
                                <p class="card-subtitle"><asp:Label ID="lblSubtituloCard" runat="server" Text="" /></p>
                            </div>

                            <asp:Label ID="lblSeccionVerificacion" runat="server" CssClass="section-sep" Text="" />

                            <div class="field">
                                <asp:Label ID="lblEtiquetaClaveActual" runat="server" AssociatedControlID="txtClaveActual" Text="" />
                                <div class="pass-wrap">
                                    <asp:TextBox ID="txtClaveActual" runat="server"
                                        TextMode="Password" MaxLength="100"
                                        ClientIDMode="Static" />
                                    <button type="button" class="pass-toggle"
                                            onclick="toggleField('txtClaveActual', this)">👁</button>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvClaveActual" runat="server"
                                    ControlToValidate="txtClaveActual"
                                    ErrorMessage="."
                                    CssClass="field-error" Display="Dynamic"
                                    ValidationGroup="vgClave" />
                            </div>

                            <asp:Label ID="lblSeccionNueva" runat="server" CssClass="section-sep" Text="" />

                            <div class="field">
                                <asp:Label ID="lblEtiquetaClaveNueva" runat="server" AssociatedControlID="txtClaveNueva" Text="" />
                                <div class="pass-wrap">
                                    <asp:TextBox ID="txtClaveNueva" runat="server"
                                        TextMode="Password" MaxLength="100"
                                        ClientIDMode="Static"
                                        oninput="checkStrength(this.value)" />
                                    <button type="button" class="pass-toggle"
                                            onclick="toggleField('txtClaveNueva', this)">👁</button>
                                </div>

                                <div class="strength-bars">
                                    <div class="strength-bar" id="bar1"></div>
                                    <div class="strength-bar" id="bar2"></div>
                                    <div class="strength-bar" id="bar3"></div>
                                </div>
                                <span class="strength-label" id="lblStrength"></span>
                                <asp:RequiredFieldValidator ID="rfvClaveNueva" runat="server"
                                    ControlToValidate="txtClaveNueva"
                                    ErrorMessage="."
                                    CssClass="field-error" Display="Dynamic"
                                    ValidationGroup="vgClave" />
                            </div>

                            <div class="field" style="margin-top:4px;">
                                <asp:Label ID="lblEtiquetaConfirmacion" runat="server" AssociatedControlID="txtClaveConfirmacion" Text="" />
                                <div class="pass-wrap">
                                    <asp:TextBox ID="txtClaveConfirmacion" runat="server"
                                        TextMode="Password" MaxLength="100"
                                        ClientIDMode="Static" />
                                    <button type="button" class="pass-toggle"
                                            onclick="toggleField('txtClaveConfirmacion', this)">👁</button>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvConfirmacion" runat="server"
                                    ControlToValidate="txtClaveConfirmacion"
                                    ErrorMessage="."
                                    CssClass="field-error" Display="Dynamic"
                                    ValidationGroup="vgClave" />
                                <asp:CompareValidator ID="cvClaves" runat="server"
                                    ControlToValidate="txtClaveConfirmacion"
                                    ControlToCompare="txtClaveNueva"
                                    ErrorMessage="."
                                    CssClass="field-error" Display="Dynamic"
                                    ValidationGroup="vgClave" />
                            </div>

                            <div class="form-actions">
                                <asp:HyperLink ID="lnkCancelar" runat="server" CssClass="btn-secondary" Text="" NavigateUrl="~/FormLogin.aspx" />
                                <asp:Button ID="btnConfirmar" runat="server"
                                    Text=""
                                    CssClass="btn-primary"
                                    ValidationGroup="vgClave"
                                    OnClick="btnConfirmar_Click" />
                            </div>

                        </div>
                    </div>

                    <div class="clave-aside">

                        <div class="content-card politica-card">
                            <p class="accesos-titulo"><asp:Label ID="lblTituloPolitica" runat="server" Text="" /></p>
                            <div class="politica-item" id="polLong">
                                <span class="pol-icono pol-pendiente">○</span>
                                <span class="pol-texto"><asp:Label ID="lblPolLongitud" runat="server" Text="" /></span>
                            </div>
                            <div class="politica-item" id="polMay">
                                <span class="pol-icono pol-pendiente">○</span>
                                <span class="pol-texto"><asp:Label ID="lblPolMayuscula" runat="server" Text="" /></span>
                            </div>
                            <div class="politica-item" id="polNum">
                                <span class="pol-icono pol-pendiente">○</span>
                                <span class="pol-texto"><asp:Label ID="lblPolNumero" runat="server" Text="" /></span>
                            </div>
                            <div class="politica-item" id="polDif">
                                <span class="pol-icono pol-pendiente">○</span>
                                <span class="pol-texto"><asp:Label ID="lblPolDistinta" runat="server" Text="" /></span>
                            </div>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoSeguroTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoSeguroTexto" runat="server" Text="" /></p>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">⚠️</div>
                            <p class="aviso-titulo"><asp:Label ID="lblAvisoSesionTitulo" runat="server" Text="" /></p>
                            <p class="aviso-texto"><asp:Label ID="lblAvisoSesionTexto" runat="server" Text="" /></p>
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </form>

    <script type="text/javascript">
        function toggleField(id, btn) {
            var input = document.getElementById(id);
            if (!input) return;
            input.type = input.type === 'password' ? 'text' : 'password';
        }
        function checkStrength(val) {
            var b1 = document.getElementById('bar1');
            var b2 = document.getElementById('bar2');
            var b3 = document.getElementById('bar3');
            var lbl = document.getElementById('lblStrength');
            if (!b1) return;

            [b1, b2, b3].forEach(function (b) { b.style.background = '#C8C2B8'; });
            lbl.style.display = 'none';

            if (!val) { actualizarPolitica(val); return; }

            var score = 0;
            if (val.length >= 8) score++;
            if (/[A-Z]/.test(val)) score++;
            if (/[0-9]/.test(val)) score++;

            lbl.style.display = 'block';
            if (score === 1) {
                b1.style.background = '#E8455A';
                lbl.textContent = 'Contraseña débil';
                lbl.style.color = '#E8455A';
            } else if (score === 2) {
                b1.style.background = b2.style.background = '#F4A261';
                lbl.textContent = 'Contraseña regular';
                lbl.style.color = '#F4A261';
            } else {
                b1.style.background = b2.style.background = b3.style.background = '#2A9D8F';
                lbl.textContent = 'Contraseña segura';
                lbl.style.color = '#2A9D8F';
            }

            actualizarPolitica(val);
        }

        function actualizarPolitica(val) {
            marcarPol('polLong', val && val.length >= 8);
            marcarPol('polMay', val && /[A-Z]/.test(val));
            marcarPol('polNum', val && /[0-9]/.test(val));
        }

        function marcarPol(id, ok) {
            var el = document.getElementById(id);
            if (!el) return;
            var icono = el.querySelector('.pol-icono');
            if (!icono) return;
            icono.textContent = ok ? '●' : '○';
            icono.className = ok ? 'pol-icono pol-ok' : 'pol-icono pol-pendiente';
        }
    </script>
</body>
</html>
