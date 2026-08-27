<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormRestablecerClave.aspx.cs" Inherits="FormRestablecerClave" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Restablecer contraseña</title>
    <link href="EstilosPaginas/FormRestablecerClave.css" rel="stylesheet" type="text/css"/>
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
                            Elegí tu<br/><em>nueva clave.</em>
                        </h2>
                        <p class="brand-desc">
                            Por seguridad, este enlace es de un solo uso y vence a los 30 minutos de haberlo solicitado.
                        </p>
                    </div>

                    <div class="deco-circle deco-1"></div>
                    <div class="deco-circle deco-2"></div>
                    <div class="deco-circle deco-3"></div>
                </div>

                <div class="card-form">

                    <asp:Panel ID="pnlFormulario" runat="server" Visible="false">

                        <div class="form-header">
                            <div class="form-eyebrow">Acceso profesional</div>
                            <h1 class="form-title">Restablecer contraseña</h1>
                            <p class="form-subtitle">Ingresá tu nueva contraseña para volver a acceder.</p>
                        </div>

                        <asp:Label ID="lblMensaje" runat="server"
                            Visible="false" CssClass="server-error" />

                        <div class="field">
                            <label for="txtClaveNueva">Contraseña nueva <sup>*</sup></label>
                            <div class="input-wrap">
                                <span class="input-icono">🔑</span>
                                <asp:TextBox ID="txtClaveNueva" runat="server"
                                    TextMode="Password" MaxLength="100"
                                    placeholder="Tu nueva contraseña"
                                    ClientIDMode="Static"
                                    oninput="checkStrength(this.value)" />
                                <button type="button" class="password-toggle"
                                        onclick="toggleField('txtClaveNueva')"
                                        title="Mostrar/ocultar contraseña">👁</button>
                            </div>
                            <div class="strength-bars">
                                <div class="strength-bar" id="bar1"></div>
                                <div class="strength-bar" id="bar2"></div>
                                <div class="strength-bar" id="bar3"></div>
                            </div>
                            <span class="strength-label" id="lblStrength"></span>
                            <asp:RequiredFieldValidator ID="rfvClaveNueva" runat="server"
                                ControlToValidate="txtClaveNueva"
                                ErrorMessage="La contraseña nueva es obligatoria."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgRestablecer" />
                            <asp:RegularExpressionValidator ID="revClaveNueva" runat="server"
                                ControlToValidate="txtClaveNueva"
                                ValidationExpression="^(?=.*[A-Z])(?=.*[0-9]).{8,}$"
                                ErrorMessage="Mínimo 8 caracteres, con al menos una mayúscula y un número."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgRestablecer" />
                        </div>

                        <div class="field">
                            <label for="txtClaveConfirmacion">Confirmar contraseña <sup>*</sup></label>
                            <div class="input-wrap">
                                <span class="input-icono">🔑</span>
                                <asp:TextBox ID="txtClaveConfirmacion" runat="server"
                                    TextMode="Password" MaxLength="100"
                                    placeholder="Repetí la contraseña"
                                    ClientIDMode="Static" />
                                <button type="button" class="password-toggle"
                                        onclick="toggleField('txtClaveConfirmacion')"
                                        title="Mostrar/ocultar contraseña">👁</button>
                            </div>
                            <asp:RequiredFieldValidator ID="rfvClaveConfirmacion" runat="server"
                                ControlToValidate="txtClaveConfirmacion"
                                ErrorMessage="Confirmá la contraseña."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgRestablecer" />
                            <asp:CompareValidator ID="cvClaves" runat="server"
                                ControlToValidate="txtClaveConfirmacion"
                                ControlToCompare="txtClaveNueva"
                                ErrorMessage="Las contraseñas no coinciden."
                                CssClass="field-error" Display="Dynamic"
                                ValidationGroup="vgRestablecer" />
                        </div>

                        <asp:Button ID="btnConfirmar" runat="server"
                            Text="Restablecer contraseña →"
                            CssClass="btn-login"
                            ValidationGroup="vgRestablecer"
                            OnClick="btnConfirmar_Click" />

                    </asp:Panel>

                    <asp:Panel ID="pnlTokenInvalido" runat="server" CssClass="resultado-panel" Visible="false">
                        <div class="resultado-icono">⏱️</div>
                        <h1 class="resultado-titulo">Este enlace ya no es válido</h1>
                        <p class="resultado-texto">
                            El enlace de recuperación no existe, ya fue utilizado, o venció (son válidos por 30 minutos).
                            Pedí uno nuevo para continuar.
                        </p>
                        <a href="FormClaveOlvidada.aspx" class="btn-login" style="display:block;text-align:center;text-decoration:none;margin-top:8px;">
                            Solicitar un nuevo enlace
                        </a>
                    </asp:Panel>

                </div>

            </div>

        </div>

    </form>

    <script type="text/javascript">
        function toggleField(id) {
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

            if (!val) return;

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
        }
    </script>
</body>
</html>