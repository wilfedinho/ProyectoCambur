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

      
        <div class="login-bg">

        
            <div class="login-card">

           
                <div class="card-brand">
                    <div class="brand-top">
                        <div class="logotype">CAM<span>BUR</span></div>
                        <div class="tagline">Gestión Clínica Asistida por IA</div>
                    </div>
                    <div class="brand-body">
                        <h2 class="brand-titulo">
                            Tu práctica,<br/><em>en orden.</em>
                        </h2>
                        <p class="brand-desc">
                            Accedé a tu entorno clínico seguro. Toda tu información encriptada y disponible cuando la necesitás.
                        </p>
                    </div>
                
                    <div class="deco-circle deco-1"></div>
                    <div class="deco-circle deco-2"></div>
                    <div class="deco-circle deco-3"></div>
                </div>

          
                <div class="card-form">

                    <div class="form-header">
                        <div class="form-eyebrow">Acceso profesional</div>
                        <h1 class="form-title">Iniciar sesión</h1>
                        <p class="form-subtitle">Ingresá tus credenciales para continuar</p>
                    </div>

                  
                    <asp:Label ID="lblMensaje" runat="server"
                        Visible="false" CssClass="server-error" />

              
                    <asp:Panel ID="pnlBloqueado" runat="server"
                        CssClass="bloqueo-panel" Visible="false">
                        <span class="bloqueo-icono">🔒</span>
                        <div>
                            <p class="bloqueo-titulo">Cuenta bloqueada</p>
                            <asp:Label ID="lblMensajeBloqueado" runat="server"
                                CssClass="bloqueo-texto" Text="" />
                        </div>
                    </asp:Panel>

               
                    <asp:Panel ID="pnlIntentos" runat="server"
                        CssClass="intentos-panel" Visible="false">
                        <asp:Label ID="lblIntentos" runat="server"
                            CssClass="intentos-texto" Text="" />
                    </asp:Panel>

                 
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
                            ValidationGroup="vgLogin" />
                    </div>

               
                    <div class="field">
                        <label for="txtPassword">Contraseña <sup>*</sup></label>
                        <div class="input-wrap">
                            <span class="input-icono">🔑</span>
                            <asp:TextBox ID="txtPassword" runat="server"
                                TextMode="Password" MaxLength="100"
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
                        <p class="link-clave-olvidada">
                            <a href="FormClaveOlvidada.aspx">¿Olvidaste tu contraseña?</a>
                        </p>
                    </div>


                    <asp:Button ID="btnLogin" runat="server"
                        Text="Iniciar sesión →"
                        CssClass="btn-login"
                        ValidationGroup="vgLogin"
                        OnClick="btnLogin_Click" />

                    <div class="form-divider"></div>

              
                    <p class="link-registro">
                        ¿Aún no tenés cuenta?
                        <a href="FormRegistroProfesional.aspx">Registrarse</a>
                    </p>

                    <p class="link-registro">
                        <a href="FormLanding.aspx">Conocé CZ Consulting y Cambur</a>
                    </p>

                </div>

            </div>

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