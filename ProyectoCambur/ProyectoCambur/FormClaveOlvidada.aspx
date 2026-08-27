<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormClaveOlvidada.aspx.cs" Inherits="FormClaveOlvidada" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Recuperar contraseña</title>
    <link href="EstilosPaginas/FormClaveOlvidada.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <div class="login-bg">

            <div class="login-card">

                <div class="card-brand">
                    <div class="brand-top">
                        <div class="logotype">CAM<span>BUR</span></div>
                        <div class="tagline">Gestión Clínica Asistida por IA</div>
                    </div>
                    <div class="brand-body">
                        <h2 class="brand-titulo">
                            Recuperá el<br/><em>acceso.</em>
                        </h2>
                        <p class="brand-desc">
                            Te enviamos un enlace seguro a tu correo registrado para que puedas elegir una contraseña nueva.
                        </p>
                    </div>

                    <div class="deco-circle deco-1"></div>
                    <div class="deco-circle deco-2"></div>
                    <div class="deco-circle deco-3"></div>
                </div>

                <div class="card-form">

                    <asp:Panel ID="pnlFormulario" runat="server">

                        <div class="form-header">
                            <div class="form-eyebrow">Acceso profesional</div>
                            <h1 class="form-title">¿Olvidaste tu contraseña?</h1>
                            <p class="form-subtitle">Ingresá tu correo y te mandamos las instrucciones para restablecerla.</p>
                        </div>

                        <asp:Label ID="lblMensaje" runat="server"
                            Visible="false" CssClass="server-error" />

                        <div class="field">
                            <label for="txtEmail">Correo electrónico <sup>*</sup></label>
                            <div class="input-wrap">
                                <span class="input-icono">✉</span>
                                <asp:TextBox ID="txtEmail" runat="server"
                                    TextMode="Email" MaxLength="150"
                                    placeholder="lucia@consultorio.com"
                                    ClientIDMode="Static" />
                            </div>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                                ControlToValidate="txtEmail"
                                ErrorMessage="El correo es obligatorio."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgClaveOlvidada" />
                        </div>

                        <asp:Button ID="btnEnviar" runat="server"
                            Text="Enviar instrucciones →"
                            CssClass="btn-login"
                            ValidationGroup="vgClaveOlvidada"
                            OnClientClick="return bloquearDobleEnvio();"
                            OnClick="btnEnviar_Click" />

                        <div class="form-divider"></div>

                        <p class="link-registro">
                            <a href="FormLogin.aspx">← Volver a iniciar sesión</a>
                        </p>

                    </asp:Panel>

                    <asp:Panel ID="pnlEnviado" runat="server" CssClass="resultado-panel" Visible="false">
                        <div class="resultado-icono">📩</div>
                        <h1 class="resultado-titulo">Revisá tu correo</h1>
                        <p class="resultado-texto">
                            Si el email ingresado está registrado en Cambur, te enviamos un enlace para restablecer tu contraseña.
                            El enlace es válido por <strong>30 minutos</strong>.
                        </p>
                        <p class="link-registro" style="margin-top:8px;">
                            <a href="FormLogin.aspx">← Volver a iniciar sesión</a>
                        </p>
                    </asp:Panel>

                </div>

            </div>

        </div>

    </form>

    <script type="text/javascript">
        var envioEnCurso = false;
        function bloquearDobleEnvio() {
            if (typeof Page_ClientValidate === 'function' && !Page_ClientValidate('vgClaveOlvidada')) {
                return false;
            }
            if (envioEnCurso) return false;
            envioEnCurso = true;
            return true;
        }
    </script>
</body>
</html>