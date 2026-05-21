<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormLogin.aspx.cs" Inherits="FormLogin" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Iniciar sesión</title>
    <link href="EstilosPaginas/FormLogin.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <!-- COLUMNA IZQUIERDA — BRANDING -->
        <div class="col-brand">
            <div class="brand-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Gestión Clínica Asistida por IA</div>
            </div>
            <div class="brand-hero">
                <h2>Tu práctica,<br /><em>en orden.</em><br />Cada sesión,<br />preparada.</h2>
                <p>Accedé a tu entorno clínico seguro. Toda tu información encriptada y disponible cuando la necesitás.</p>
                <div class="brand-pills">
                    <span class="pill">🔒 Cifrado AES</span>
                    <span class="pill">📋 Historial Clínico</span>
                    <span class="pill">🤖 IA Asistiva</span>
                    <span class="pill">📄 Ley 25.326</span>
                </div>
            </div>
        </div>

        <!-- COLUMNA DERECHA — FORMULARIO -->
        <div class="col-form">

            <div class="form-header">
                <div class="form-eyebrow">Acceso profesional</div>
                <h1 class="form-title">Iniciar sesión</h1>
            </div>

            <%-- Mensaje de error/éxito del servidor --%>
            <asp:Label ID="lblMensaje" runat="server"
                Visible="false" CssClass="server-error" />

            <%-- Panel de cuenta bloqueada --%>
            <asp:Panel ID="pnlBloqueado" runat="server"
                CssClass="bloqueo-panel" Visible="false">
                <div class="bloqueo-icono">🔒</div>
                <div class="bloqueo-info">
                    <p class="bloqueo-titulo">Cuenta bloqueada</p>
                    <asp:Label ID="lblMensajeBloqueado" runat="server"
                        CssClass="bloqueo-texto" Text="" />
                </div>
            </asp:Panel>

            <%-- Indicador de intentos fallidos --%>
            <asp:Panel ID="pnlIntentos" runat="server"
                CssClass="intentos-panel" Visible="false">
                <asp:Label ID="lblIntentos" runat="server"
                    CssClass="intentos-texto" Text="" />
            </asp:Panel>

            <div class="section-sep">Credenciales</div>

            <div class="field">
                <label for="txtEmail">Correo electrónico <sup>*</sup></label>
                <asp:TextBox ID="txtEmail" runat="server"
                    TextMode="Email"
                    MaxLength="150"
                    placeholder="lucia@consultorio.com"
                    ClientIDMode="Static" />
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                    ControlToValidate="txtEmail"
                    ErrorMessage="El correo es obligatorio."
                    CssClass="field-error" Display="Dynamic"
                    ValidationGroup="vgLogin" />
            </div>

            <div class="field" style="margin-top:14px;">
                <label for="txtPassword">Contraseña <sup>*</sup></label>
                <div class="password-wrap">
                    <asp:TextBox ID="txtPassword" runat="server"
                        TextMode="Password"
                        MaxLength="100"
                        placeholder="Tu contraseña"
                        ClientIDMode="Static" />
                    <button type="button" class="password-toggle"
                            onclick="togglePass()"
                            title="Mostrar/ocultar contraseña">👁</button>
                </div>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                    ControlToValidate="txtPassword"
                    ErrorMessage="La contraseña es obligatoria."
                    CssClass="field-error" Display="Dynamic"
                    ValidationGroup="vgLogin" />
            </div>

            <%-- Botón principal --%>
            <asp:Button ID="btnLogin" runat="server"
                Text="Iniciar sesión"
                CssClass="btn-login"
                ValidationGroup="vgLogin"
                OnClick="btnLogin_Click" />

            <div class="login-footer">
                <a href="FormRegistroProfesional.aspx" class="link-registro">
                    ¿Aún no tenés cuenta? <strong>Registrarse</strong>
                </a>
            </div>

            <p class="legal-text">
                El acceso a esta plataforma es exclusivo para profesionales registrados.
                La información es tratada según la Ley 25.326 de Protección de Datos Personales.
            </p>

        </div>

    </form>

    <script type="text/javascript">
        function togglePass() {
            var input = document.getElementById('txtPassword');
            if (!input) return;
            input.type = input.type === 'password' ? 'text' : 'password';
        }
    </script>
</body>
</html>
