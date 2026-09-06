<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormLanding.aspx.cs" Inherits="FormLanding" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — CZ Consulting</title>
    <link href="EstilosPaginas/FormLanding.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <header class="landing-nav">
            <div class="landing-nav-inner">
                <div class="landing-logo">
                    <div class="logotype">CAM<span>BUR</span></div>
                    <div class="landing-tagline">CZ Consulting</div>
                </div>

                <nav class="landing-nav-links">
                    <a href="#hero"><asp:Label ID="lblNavInicio" runat="server" Text="" /></a>
                    <a href="#nosotros"><asp:Label ID="lblNavNosotros" runat="server" Text="" /></a>
                    <a href="#servicios"><asp:Label ID="lblNavServicios" runat="server" Text="" /></a>
                    <a href="#testimonios"><asp:Label ID="lblNavTestimonios" runat="server" Text="" /></a>
                    <a href="#faq"><asp:Label ID="lblNavFaq" runat="server" Text="" /></a>
                    <a href="#rse"><asp:Label ID="lblNavRse" runat="server" Text="" /></a>
                    <a href="#contacto"><asp:Label ID="lblNavContacto" runat="server" Text="" /></a>
                </nav>

                <div class="landing-nav-actions">
                    <asp:Panel ID="pnlAccionesAnonimo" runat="server">
                        <a href="FormLogin.aspx" class="btn-nav-secundario"><asp:Label ID="lblBtnIniciarSesion" runat="server" Text="" /></a>
                        <a href="FormRegistroProfesional.aspx" class="btn-nav-primario"><asp:Label ID="lblBtnCrearCuenta" runat="server" Text="" /></a>
                    </asp:Panel>
                    <asp:Panel ID="pnlAccionesAutenticado" runat="server" Visible="false">
                        <a href="FormLogout.aspx" class="btn-nav-secundario"><asp:Label ID="lblBtnCerrarSesion" runat="server" Text="" /></a>
                        <a href="FormMenu.aspx" class="btn-nav-primario"><asp:Label ID="lblBtnIrPanel" runat="server" Text="" /></a>
                    </asp:Panel>
                </div>
            </div>
        </header>

        <main>
            <section id="hero" class="landing-section hero-section">
                <div class="hero-eyebrow"><asp:Label ID="lblHeroEyebrow" runat="server" Text="" /></div>
                <h1 class="hero-titulo"><asp:Label ID="lblHeroTitulo" runat="server" Text="" /></h1>
                <p class="hero-texto"><asp:Label ID="lblHeroTexto" runat="server" Text="" /></p>
                <div class="hero-cta">
                    <a href="#servicios" class="btn-hero-primario"><asp:Label ID="lblHeroCtaServicios" runat="server" Text="" /></a>
                    <a href="#servicios" class="btn-hero-secundario"><asp:Label ID="lblHeroCtaCambur" runat="server" Text="" /></a>
                </div>
            </section>

            <section id="nosotros" class="landing-section">
                <h2 class="section-titulo"><asp:Label ID="lblNosotrosTitulo" runat="server" Text="" /></h2>
                <div class="nosotros-layout">
                    <p class="nosotros-texto"><asp:Label ID="lblNosotrosTexto" runat="server" Text="" /></p>
                    <div class="nosotros-datos">
                        <div class="nosotros-dato">
                            <div class="nosotros-dato-num"><asp:Label ID="lblNosotrosDato1Num" runat="server" Text="" /></div>
                            <div class="nosotros-dato-txt"><asp:Label ID="lblNosotrosDato1Texto" runat="server" Text="" /></div>
                        </div>
                        <div class="nosotros-dato">
                            <div class="nosotros-dato-num"><asp:Label ID="lblNosotrosDato2Num" runat="server" Text="" /></div>
                            <div class="nosotros-dato-txt"><asp:Label ID="lblNosotrosDato2Texto" runat="server" Text="" /></div>
                        </div>
                        <div class="nosotros-dato">
                            <div class="nosotros-dato-num"><asp:Label ID="lblNosotrosDato3Num" runat="server" Text="" /></div>
                            <div class="nosotros-dato-txt"><asp:Label ID="lblNosotrosDato3Texto" runat="server" Text="" /></div>
                        </div>
                    </div>
                </div>
            </section>
            <section id="servicios" class="landing-section servicios-section">
                <h2 class="section-titulo"><asp:Label ID="lblServiciosTitulo" runat="server" Text="" /></h2>
                <p class="section-subtitulo"><asp:Label ID="lblServiciosSubtitulo" runat="server" Text="" /></p>

                <div class="servicios-grid">
                    <div class="servicio-card">
                        <h3><asp:Label ID="lblServicio1Titulo" runat="server" Text="" /></h3>
                        <p><asp:Label ID="lblServicio1Texto" runat="server" Text="" /></p>
                    </div>
                    <div class="servicio-card">
                        <h3><asp:Label ID="lblServicio2Titulo" runat="server" Text="" /></h3>
                        <p><asp:Label ID="lblServicio2Texto" runat="server" Text="" /></p>
                    </div>
                    <div class="servicio-card servicio-destacado">
                        <span class="servicio-badge"><asp:Label ID="lblServicio3Badge" runat="server" Text="" /></span>
                        <h3><asp:Label ID="lblServicio3Titulo" runat="server" Text="" /></h3>
                        <p><asp:Label ID="lblServicio3Texto" runat="server" Text="" /></p>
                    </div>
                </div>
            </section>
            <section id="testimonios" class="landing-section testimonios-section">
                <h2 class="section-titulo"><asp:Label ID="lblTestimoniosTitulo" runat="server" Text="" /></h2>

                <div class="resumen-valoraciones">
                    <asp:Label ID="lblResumenEstrellas" runat="server" CssClass="resumen-estrellas" Text="" />
                    <asp:Label ID="lblResumenPromedio" runat="server" CssClass="resumen-promedio" Text="" />
                    <asp:Label ID="lblResumenCantidad" runat="server" CssClass="resumen-cantidad" Text="" />
                </div>

                <div class="testimonios-grid">
                    <asp:Repeater ID="rptTestimonios" runat="server">
                        <ItemTemplate>
                            <div class="testimonio-card">
                                <div class="testimonio-estrellas"><%# Eval("EstrellasHtml") %></div>
                                <p class="testimonio-comentario">"<%# Eval("Comentario") %>"</p>
                                <div class="testimonio-autor">
                                    <span class="testimonio-nombre"><%# Eval("NombreProfesional") %> <%# Eval("ApellidoProfesional") %></span>
                                    <span class="testimonio-plan"><%# Eval("PlanTexto") %></span>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <asp:Label ID="lblSinTestimonios" runat="server" CssClass="sin-testimonios-txt" Visible="false" Text="" />
                </div>
                <asp:Panel ID="pnlValorarWrap" runat="server" Visible="false" CssClass="valorar-wrap">

                    <asp:Panel ID="pnlValorarSinSuscripcion" runat="server" Visible="false" CssClass="valorar-aviso">
                        <asp:Label ID="lblValorarSinSuscripcion" runat="server" Text="" />
                    </asp:Panel>

                    <asp:Panel ID="pnlValorarFormulario" runat="server" Visible="false" CssClass="valorar-card">
                        <h3><asp:Label ID="lblValorarTitulo" runat="server" Text="" /></h3>
                        <p class="valorar-texto"><asp:Label ID="lblValorarTexto" runat="server" Text="" /></p>

                        <div class="valorar-plan-actual">
                            <asp:Label ID="lblValorarPlanActualPrefijo" runat="server" Text="" />
                            <asp:Label ID="lblValorarPlanActualNombre" runat="server" CssClass="valorar-plan-nombre" Text="" />
                        </div>

                        <asp:Label ID="lblValorarYaExistenteAviso" runat="server" CssClass="valorar-aviso-menor" Visible="false" Text="" />

                        <div class="valorar-estrellas-input" id="valorarEstrellasInput">
                            <span class="estrella-input" data-valor="1" onclick="elegirEstrella(1)">★</span>
                            <span class="estrella-input" data-valor="2" onclick="elegirEstrella(2)">★</span>
                            <span class="estrella-input" data-valor="3" onclick="elegirEstrella(3)">★</span>
                            <span class="estrella-input" data-valor="4" onclick="elegirEstrella(4)">★</span>
                            <span class="estrella-input" data-valor="5" onclick="elegirEstrella(5)">★</span>
                        </div>
                        <asp:HiddenField ID="hfPuntuacion" runat="server" Value="0" ClientIDMode="Static" />

                        <asp:TextBox ID="txtComentario" runat="server" TextMode="MultiLine" Rows="3" MaxLength="500" CssClass="valorar-comentario" />

                        <asp:Label ID="lblMensajeValoracion" runat="server" CssClass="server-error" Visible="false" />

                        <asp:Button ID="btnEnviarValoracion" runat="server" Text="" CssClass="btn-primary"
                            OnClick="btnEnviarValoracion_Click" OnClientClick="return validarValoracion();" CausesValidation="false" />
                    </asp:Panel>
                </asp:Panel>
            </section>
            <section id="faq" class="landing-section faq-section">
                <h2 class="section-titulo"><asp:Label ID="lblFaqTitulo" runat="server" Text="" /></h2>
                <div class="faq-lista">
                    <details class="faq-item"><summary><asp:Label ID="lblFaqPregunta1" runat="server" Text="" /></summary><p><asp:Label ID="lblFaqRespuesta1" runat="server" Text="" /></p></details>
                    <details class="faq-item"><summary><asp:Label ID="lblFaqPregunta2" runat="server" Text="" /></summary><p><asp:Label ID="lblFaqRespuesta2" runat="server" Text="" /></p></details>
                    <details class="faq-item"><summary><asp:Label ID="lblFaqPregunta3" runat="server" Text="" /></summary><p><asp:Label ID="lblFaqRespuesta3" runat="server" Text="" /></p></details>
                    <details class="faq-item"><summary><asp:Label ID="lblFaqPregunta4" runat="server" Text="" /></summary><p><asp:Label ID="lblFaqRespuesta4" runat="server" Text="" /></p></details>
                    <details class="faq-item"><summary><asp:Label ID="lblFaqPregunta5" runat="server" Text="" /></summary><p><asp:Label ID="lblFaqRespuesta5" runat="server" Text="" /></p></details>
                    <details class="faq-item"><summary><asp:Label ID="lblFaqPregunta6" runat="server" Text="" /></summary><p><asp:Label ID="lblFaqRespuesta6" runat="server" Text="" /></p></details>
                </div>
            </section>
            <section id="rse" class="landing-section rse-section">
                <h2 class="section-titulo"><asp:Label ID="lblRseTitulo" runat="server" Text="" /></h2>
                <p class="section-subtitulo"><asp:Label ID="lblRseTexto" runat="server" Text="" /></p>
                <div class="rse-grid">
                    <div class="rse-pilar">
                        <h3><asp:Label ID="lblRsePilar1Titulo" runat="server" Text="" /></h3>
                        <p><asp:Label ID="lblRsePilar1Texto" runat="server" Text="" /></p>
                    </div>
                    <div class="rse-pilar">
                        <h3><asp:Label ID="lblRsePilar2Titulo" runat="server" Text="" /></h3>
                        <p><asp:Label ID="lblRsePilar2Texto" runat="server" Text="" /></p>
                    </div>
                    <div class="rse-pilar">
                        <h3><asp:Label ID="lblRsePilar3Titulo" runat="server" Text="" /></h3>
                        <p><asp:Label ID="lblRsePilar3Texto" runat="server" Text="" /></p>
                    </div>
                </div>
            </section>
            <section id="contacto" class="landing-section contacto-section">
                <h2 class="section-titulo"><asp:Label ID="lblContactoTitulo" runat="server" Text="" /></h2>
                <p class="section-subtitulo"><asp:Label ID="lblContactoTexto" runat="server" Text="" /></p>

                <asp:Panel ID="pnlContactoExito" runat="server" Visible="false" CssClass="contacto-exito">
                    <asp:Label ID="lblContactoExito" runat="server" Text="" />
                </asp:Panel>

                <asp:Panel ID="pnlContactoForm" runat="server" CssClass="contacto-form">
                    <asp:Label ID="lblMensajeContacto" runat="server" CssClass="server-error" Visible="false" />

                    <div class="contacto-grid">
                        <div class="contacto-campo">
                            <asp:Label ID="lblContactoLblNombre" runat="server" AssociatedControlID="txtContactoNombre" Text="" />
                            <asp:TextBox ID="txtContactoNombre" runat="server" MaxLength="150" />
                        </div>
                        <div class="contacto-campo">
                            <asp:Label ID="lblContactoLblEmail" runat="server" AssociatedControlID="txtContactoEmail" Text="" />
                            <asp:TextBox ID="txtContactoEmail" runat="server" MaxLength="150" TextMode="Email" />
                        </div>
                        <div class="contacto-campo contacto-campo-full">
                            <asp:Label ID="lblContactoLblAsunto" runat="server" AssociatedControlID="txtContactoAsunto" Text="" />
                            <asp:TextBox ID="txtContactoAsunto" runat="server" MaxLength="200" />
                        </div>
                        <div class="contacto-campo contacto-campo-full">
                            <asp:Label ID="lblContactoLblMensaje" runat="server" AssociatedControlID="txtContactoMensaje" Text="" />
                            <asp:TextBox ID="txtContactoMensaje" runat="server" TextMode="MultiLine" Rows="4" MaxLength="1000" />
                        </div>
                    </div>

                    <asp:Button ID="btnEnviarContacto" runat="server" Text="" CssClass="btn-primary" OnClick="btnEnviarContacto_Click" CausesValidation="false" />
                </asp:Panel>
            </section>

        </main>

        <footer class="landing-footer">
            <div class="logotype">CAM<span>BUR</span></div>
            <p><asp:Label ID="lblFooterTexto" runat="server" Text="" /></p>
        </footer>

    </form>

    <script type="text/javascript">
        function elegirEstrella(valor) {
            var hf = document.getElementById('hfPuntuacion');
            if (hf) hf.value = valor;
            var estrellas = document.querySelectorAll('#valorarEstrellasInput .estrella-input');
            estrellas.forEach(function (el) {
                var v = parseInt(el.getAttribute('data-valor'), 10);
                if (v <= valor) el.classList.add('estrella-activa');
                else el.classList.remove('estrella-activa');
            });
        }
        function validarValoracion() {
            var hf = document.getElementById('hfPuntuacion');
            var valor = hf ? parseInt(hf.value, 10) : 0;
            if (!valor || valor < 1 || valor > 5) {
                alert('Elegí una puntuación de 1 a 5 estrellas antes de publicar.');
                return false;
            }
            return true;
        }
    </script>
</body>
</html>