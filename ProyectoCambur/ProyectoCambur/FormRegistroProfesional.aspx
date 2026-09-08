<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormRegistroProfesional.aspx.cs" Inherits="FormRegistroProfesional" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Registro de Profesional</title>
    <link href="EstilosPaginas/FormRegistroProfesional.css" rel="stylesheet" type="text/css"/>
    <script src="https://js.stripe.com/v3/"></script>
</head>
<body>
    <form id="form1" runat="server">


        <div class="col-brand">
            <div class="brand-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Gestión Clínica Asistida por IA</div>
            </div>
            <div class="brand-hero">
                <h2>Tu práctica,<br />
                    <em>organizada.</em><br />
                    Tu tiempo,<br />
                    recuperado.</h2>
                <p>Plataforma SaaS para profesionales de la salud mental.
                   Síntesis clínica inteligente, sin intervención en tu criterio profesional.</p>
            </div>
        </div>


        <div class="col-form">

            <div class="form-header">
                <div class="form-eyebrow">Acceso profesional</div>
                <h1 class="form-title">Crear cuenta</h1>
            </div>


            <asp:Label ID="lblMensaje" runat="server" CssClass="server-error" Visible="false" />


            <div class="section-sep">Datos personales</div>

            <div class="grid-2">

                <div class="field">
                    <label for="txtNombre">Nombre <sup>*</sup></label>
                    <asp:TextBox ID="txtNombre" runat="server" MaxLength="100" placeholder="Ej: Lucía" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                        ControlToValidate="txtNombre"
                        ErrorMessage="El nombre es obligatorio."
                        CssClass="field-error"
                        Display="Dynamic"
                        ValidationGroup="vgRegistro" />
                </div>

                <div class="field">
                    <label for="txtApellido">Apellido <sup>*</sup></label>
                    <asp:TextBox ID="txtApellido" runat="server" MaxLength="100" placeholder="Ej: Martínez" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                        ControlToValidate="txtApellido"
                        ErrorMessage="El apellido es obligatorio."
                        CssClass="field-error"
                        Display="Dynamic"
                        ValidationGroup="vgRegistro" />
                </div>

                <div class="field">
                    <label for="txtDNI">DNI <sup>*</sup></label>
                    <asp:TextBox ID="txtDNI" runat="server" MaxLength="10" placeholder="Ej: 12.345.678" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="rfvDNI" runat="server"
                        ControlToValidate="txtDNI"
                        ErrorMessage="El DNI es obligatorio."
                        CssClass="field-error"
                        Display="Dynamic"
                        ValidationGroup="vgRegistro" />
                    <asp:RegularExpressionValidator ID="revDNI" runat="server"
                        ControlToValidate="txtDNI"
                        ValidationExpression="^[0-9]{2}[.][0-9]{3}[.][0-9]{3}$"
                        ErrorMessage="Formato esperado: 12.345.678"
                        CssClass="field-error"
                        Display="Dynamic"
                        ValidationGroup="vgRegistro" />
                </div>

                <div class="field">
                    <label for="txtEmail">Correo electrónico <sup>*</sup></label>
                    <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" MaxLength="150" placeholder="lucia@consultorio.com" ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="El correo es obligatorio."
                        CssClass="field-error"
                        Display="Dynamic"
                        ValidationGroup="vgRegistro" />
                    <asp:RegularExpressionValidator ID="revEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ValidationExpression="^[\w\.\-]+@[\w\-]+\.[a-zA-Z]{2,}$"
                        ErrorMessage="Formato de correo inválido."
                        CssClass="field-error"
                        Display="Dynamic"
                        ValidationGroup="vgRegistro" />
                </div>

                <div class="field">
                    <label for="txtPassword">Contraseña <sup>*</sup></label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" MaxLength="100"
                        placeholder="Mín. 8 car., una mayúscula y un número"
                        ClientIDMode="Static"
                        oninput="checkStrength(this.value)" />
                    <div class="strength-bars">
                        <div class="strength-bar" id="bar1"></div>
                        <div class="strength-bar" id="bar2"></div>
                        <div class="strength-bar" id="bar3"></div>
                    </div>
                    <span class="strength-label" id="lblStrength"></span>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ErrorMessage="La contraseña es obligatoria."
                        CssClass="field-error"
                        Display="Dynamic"
                        ValidationGroup="vgRegistro" />
                    <asp:RegularExpressionValidator ID="revPassword" runat="server"
                        ControlToValidate="txtPassword"
                        ValidationExpression="^(?=.*[A-Z])(?=.*[0-9]).{8,}$"
                        ErrorMessage="Mínimo 8 caracteres, con al menos una mayúscula y un número."
                        CssClass="field-error"
                        Display="Dynamic"
                        ValidationGroup="vgRegistro" />
                </div>

                <div class="field">
                    <label for="txtConfirmPassword">Confirmar contraseña <sup>*</sup></label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" MaxLength="100"
                        placeholder="Repetir contraseña"
                        ClientIDMode="Static" />
                    <asp:RequiredFieldValidator ID="rfvConfirm" runat="server"
                        ControlToValidate="txtConfirmPassword"
                        ErrorMessage="Confirmá tu contraseña."
                        CssClass="field-error"
                        Display="Dynamic"
                        ValidationGroup="vgRegistro" />
                    <asp:CompareValidator ID="cvPassword" runat="server"
                        ControlToValidate="txtConfirmPassword"
                        ControlToCompare="txtPassword"
                        ErrorMessage="Las contraseñas no coinciden."
                        CssClass="field-error"
                        Display="Dynamic"
                        ValidationGroup="vgRegistro" />
                </div>

            </div>


            <div class="section-sep">Plan de suscripción</div>

            <asp:HiddenField ID="hfPlanSeleccionado" runat="server" Value="2" ClientIDMode="Static" />

            <div class="plan-grid">
                <div class="plan-card" id="planBasico" onclick="selectPlan(this,'1')">
                    <div class="plan-name">Básico</div>
                    <div class="plan-price">USD $4.99 <span>/mes</span></div>
                    <div class="plan-desc">Hasta 20 pacientes. Historial + Consultas. Sin funciones IA.</div>
                </div>
                <div class="plan-card highlighted selected" id="planProfesional" onclick="selectPlan(this,'2')">
                    <div class="plan-badge">Más elegido</div>
                    <div class="plan-name">Profesional</div>
                    <div class="plan-price">USD $14.99 <span>/mes</span></div>
                    <div class="plan-desc">Pacientes ilimitados. IA asistiva, derivaciones y perfilación.</div>
                </div>
                <div class="plan-card" id="planPremium" onclick="selectPlan(this,'3')">
                    <div class="plan-name">Premium</div>
                    <div class="plan-price">USD $21.99 <span>/mes</span></div>
                    <div class="plan-desc">Todo Profesional + exportaciones avanzadas y soporte prioritario.</div>
                </div>
            </div>


            <div class="section-sep">Datos de pago</div>
            <p class="pago-aviso">Pago procesado de forma segura por Stripe, en dólares (USD). Cambur no almacena el número de tu tarjeta.</p>

            <div class="field mb-14">
                <label for="txtTitular">Titular <sup>*</sup></label>
                <asp:TextBox ID="txtTitular" runat="server" MaxLength="100" placeholder="Nombre en la tarjeta" ClientIDMode="Static" autocomplete="off" />
                <asp:RequiredFieldValidator ID="rfvTitular" runat="server"
                    ControlToValidate="txtTitular"
                    ErrorMessage="Obligatorio."
                    CssClass="field-error"
                    Display="Dynamic"
                    ValidationGroup="vgRegistro" />
            </div>

            <div class="field mb-14">
                <label for="stripeCardElement">Datos de la tarjeta <sup>*</sup></label>
                <div id="stripeCardElement" class="stripe-card-element"></div>
                <div id="stripeCardErrors" class="field-error" role="alert"></div>
            </div>
            <asp:HiddenField ID="hfTokenTarjeta" runat="server" ClientIDMode="Static" />


            <div class="form-footer">
                <div class="login-link">
                    ¿Ya tenés cuenta? <a href="FormLogin.aspx">Iniciar sesión</a>
                </div>
                <asp:Button ID="btnRegistrar" runat="server"
                    Text="Registrarse →"
                    CssClass="btn-register"
                    ValidationGroup="vgRegistro"
                    ClientIDMode="Static"
                    OnClientClick="return iniciarRegistroConPago();"
                    OnClick="btnRegistrar_Click" />
            </div>

            <p class="legal-text">
                Al registrarte aceptás los <a href="#">Términos de Servicio</a> y la
                <a href="#">Política de Privacidad</a>. La información clínica es tratada
                según la Ley 25.326 de Protección de Datos Personales.
            </p>

        </div>


        <div class="toast" id="toastExito">Cuenta creada correctamente. Redirigiendo...</div>

    </form>

    <script type="text/javascript">
        var stripe = Stripe('<%= ObtenerPublicKeyStripe() %>');
        var stripeElements = stripe.elements();
        var stripeCardElement = stripeElements.create('card', {
            style: {
                base: { fontSize: '15px', color: '#1a1a1a', fontFamily: 'inherit', '::placeholder': { color: '#9a9a9a' } },
                invalid: { color: '#E8455A' }
            },
            hidePostalCode: true
        });
        stripeCardElement.mount('#stripeCardElement');
        stripeCardElement.on('change', function (evento) {
            var lblErrorTarjeta = document.getElementById('stripeCardErrors');
            lblErrorTarjeta.textContent = evento.error ? evento.error.message : '';
        });

        function selectPlan(card, planId) {
            document.querySelectorAll('.plan-card').forEach(function (c) {
                c.classList.remove('selected');
            });
            card.classList.add('selected');
            var hf = document.getElementById('hfPlanSeleccionado');
            if (hf) hf.value = planId;
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


        function iniciarRegistroConPago() {
            if (typeof Page_ClientValidate === 'function') {
                if (!Page_ClientValidate('vgRegistro')) return false;
            }

            var btn = document.getElementById('btnRegistrar');
            var textoOriginal = btn.value;
            btn.disabled = true;
            btn.value = 'Procesando pago...';

            function mostrarErrorTarjeta(mensaje) {
                btn.disabled = false;
                btn.value = textoOriginal;
                var lbl = document.getElementById('lblMensaje');
                if (lbl) {
                    lbl.textContent = mensaje;
                    lbl.style.display = 'block';
                }
            }

            stripe.createPaymentMethod({
                type: 'card',
                card: stripeCardElement,
                billing_details: {
                    name: document.getElementById('txtTitular').value
                }
            }).then(function (resultado) {
                if (resultado.error) {
                    console.log('Error al tokenizar la tarjeta con Stripe:', resultado.error);
                    mostrarErrorTarjeta(resultado.error.message || 'No pudimos validar los datos de la tarjeta. Revisá el número, el vencimiento y el código de seguridad.');
                    return;
                }
                document.getElementById('hfTokenTarjeta').value = resultado.paymentMethod.id;
                __doPostBack('btnRegistrar', '');
            }).catch(function (error) {
                console.log('Error al procesar la tarjeta con Stripe:', error);
                mostrarErrorTarjeta('No pudimos procesar los datos de la tarjeta. Intentá nuevamente.');
            });

            return false;
        }

        window.addEventListener('DOMContentLoaded', function () {
            var mostrar = document.getElementById('hfMostrarToast');
            if (mostrar && mostrar.value === '1') {
                var toast = document.getElementById('toastExito');
                toast.classList.add('visible');
                setTimeout(function () { toast.classList.remove('visible'); }, 3000);
            }
        });
    </script>
</body>
</html>