<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormPerfilPaciente.aspx.cs" Inherits="FormPerfilPaciente" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Perfilación del Paciente</title>
    <link href="EstilosPaginas/Shared.css"              rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormPerfilPaciente.css"  rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

      
        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <div class="tagline">Gestión Clínica</div>
            </div>
            <nav class="sidebar-nav">
                <a href="FormDashboard.aspx"         class="nav-item">🏠 Dashboard</a>
                <a href="FormRegistroPaciente.aspx"  class="nav-item">👤 Pacientes</a>
                <a href="FormRealizarConsulta.aspx"  class="nav-item">🗒️ Consultas</a>
                <a href="FormHistorialClinico.aspx"  class="nav-item">📋 Historial Clínico</a>
                <a href="FormResumenIA.aspx"         class="nav-item">🤖 Resumen IA</a>
                <a href="FormLineaTemporal.aspx"     class="nav-item">📅 Línea Temporal</a>
                <a href="FormInformeDerivacion.aspx" class="nav-item">📤 Derivaciones</a>
                <a href="FormPerfilPaciente.aspx"    class="nav-item active">🧠 Perfilación</a>
                <a href="FormExportarReporte.aspx"   class="nav-item">💾 Exportar</a>
            </nav>
            <div class="sidebar-footer">
                <a href="FormSuscripcion.aspx" class="nav-item">💳 Mi Suscripción</a>
                <a href="FormLogin.aspx"       class="nav-item nav-logout">🚪 Cerrar sesión</a>
            </div>
        </aside>

        
        <div class="main-wrap">

         
            <header class="top-header">
                <div class="header-title">
                    <span class="header-section">Perfilación</span>
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderTitulo" runat="server"
                        CssClass="header-page" Text="Generar perfil del paciente" />
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="LM" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server"
                            CssClass="user-name" Text="" />
                        <span class="user-role">Psicólogo/a</span>
                    </div>
                </div>
            </header>

            
            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server"
                    Visible="false" CssClass="server-error" />

             
                <asp:Panel ID="pnlSeleccion" runat="server">
                    <div class="perfil-layout">

                        <div class="perfil-form-col">

                       
                            <div class="content-card">
                                <div class="field full-col" style="margin-bottom:16px;">
                                    <label for="ddlPacientePerfil">Paciente <sup>*</sup></label>
                                    <asp:DropDownList ID="ddlPacientePerfil" runat="server"
                                        ClientIDMode="Static" AutoPostBack="true"
                                        OnSelectedIndexChanged="ddlPacientePerfil_SelectedIndexChanged" />
                                </div>

                                <div class="paciente-header">
                                    <div class="paciente-header-avatar">
                                        <asp:Label ID="lblPacienteIniciales" runat="server" Text="MG" />
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
                                    🧠 Los perfiles son representaciones descriptivas y contextuales. No constituyen diagnósticos clínicos ni reemplazan el criterio profesional.
                                </div>

                                <div class="section-sep">Seleccioná el modelo de evaluación</div>
                                <p class="hint-text">Elegí el marco de evaluación pertinente para este proceso de perfilación. Podés generar múltiples perfiles con distintos modelos.</p>

                            
                                <asp:HiddenField ID="hfModeloSeleccionado" runat="server"
                                    Value="" ClientIDMode="Static" />

                          
                                <div class="modelos-grid">

                                    <div class="modelo-card" id="cardBigFive"
                                         onclick="seleccionarModelo(this, 'BIGFIVE')">
                                        <div class="modelo-icono">🌐</div>
                                        <div class="modelo-info">
                                            <div class="modelo-nombre">Big Five (BFI)</div>
                                            <div class="modelo-desc">Análisis de rasgos generales de personalidad en cinco dimensiones: apertura, responsabilidad, extroversión, amabilidad y neuroticismo.</div>
                                        </div>
                                        <div class="modelo-check">○</div>
                                    </div>

                                    <div class="modelo-card" id="cardCOPE"
                                         onclick="seleccionarModelo(this, 'COPE')">
                                        <div class="modelo-icono">🛡️</div>
                                        <div class="modelo-info">
                                            <div class="modelo-nombre">COPE Simplificado</div>
                                            <div class="modelo-desc">Estilos de afrontamiento ante situaciones de estrés. Identifica estrategias activas, evitativas y de soporte social.</div>
                                        </div>
                                        <div class="modelo-check">○</div>
                                    </div>

                                    <div class="modelo-card" id="cardAutoeficacia"
                                         onclick="seleccionarModelo(this, 'AUTOEFICACIA')">
                                        <div class="modelo-icono">⚡</div>
                                        <div class="modelo-info">
                                            <div class="modelo-nombre">Autoeficacia de Schwarzer</div>
                                            <div class="modelo-desc">Autoconcepto y percepción de capacidades del paciente para enfrentar desafíos y situaciones demandantes.</div>
                                        </div>
                                        <div class="modelo-check">○</div>
                                    </div>

                                    <div class="modelo-card" id="cardApego"
                                         onclick="seleccionarModelo(this, 'APEGO')">
                                        <div class="modelo-icono">🔗</div>
                                        <div class="modelo-info">
                                            <div class="modelo-nombre">Estilos de Apego (ECR)</div>
                                            <div class="modelo-desc">Modelo de estilos de apego adulto basado en versiones acotadas del ECR. Evalúa ansiedad y evitación en vínculos.</div>
                                        </div>
                                        <div class="modelo-check">○</div>
                                    </div>

                                    <div class="modelo-card" id="cardValores"
                                         onclick="seleccionarModelo(this, 'VALORES')">
                                        <div class="modelo-icono">🌱</div>
                                        <div class="modelo-info">
                                            <div class="modelo-nombre">Valores y Sentido de Vida (PVQ)</div>
                                            <div class="modelo-desc">Modelos de valores personales y sentido de vida basados en PVQ y enfoques logoterapéuticos de Frankl.</div>
                                        </div>
                                        <div class="modelo-check">○</div>
                                    </div>

                                </div>

                                <div class="form-actions">
                                    <a href="FormRegistroPaciente.aspx" class="btn-secondary">Cancelar</a>
                                    <asp:Button ID="btnGenerar" runat="server"
                                        Text="Generar perfil"
                                        CssClass="btn-primary btn-ia"
                                        OnClick="btnGenerar_Click"
                                        CausesValidation="false"
                                        OnClientClick="return validarYCargar();" />
                                </div>

                            </div>
                        </div>

                  
                        <div class="perfil-aside">

                            <div class="content-card modelo-seleccionado-card">
                                <p class="accesos-titulo">Modelo seleccionado</p>
                                <div id="modeloSeleccionadoInfo" class="modelo-sel-vacio">
                                    Ningún modelo seleccionado todavía.
                                </div>
                            </div>

                            <div class="content-card perfiles-anteriores-card">
                                <p class="accesos-titulo">Perfiles anteriores</p>
                                <asp:Repeater ID="rptPerfilesAnteriores" runat="server">
                                    <ItemTemplate>
                                        <div class="perfil-anterior-item">
                                            <span class="pa-modelo"><%# Eval("Modelo") %></span>
                                            <span class="pa-fecha"><%# Eval("Fecha", "{0:dd/MM/yyyy}") %></span>
                                        </div>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:Label ID="lblSinPerfiles" runat="server"
                                    CssClass="sin-perfiles-txt"
                                    Text="Aún no se generaron perfiles para este paciente."
                                    Visible="false" />
                            </div>

                            <div class="content-card aviso-card">
                                <div class="aviso-icon">🔒</div>
                                <p class="aviso-titulo">Datos encriptados</p>
                                <p class="aviso-texto">El perfil generado se encripta con AES al guardarse, cumpliendo la Ley 25.326.</p>
                            </div>

                        </div>
                    </div>
                </asp:Panel>

             
                <div class="carga-overlay" id="cargaOverlay" style="display:none;">
                    <div class="carga-card">
                        <div class="carga-spinner"></div>
                        <p class="carga-titulo">Generando perfil con IA...</p>
                        <p class="carga-subtitulo">Analizando información clínica y contrastando con el modelo seleccionado</p>
                    </div>
                </div>

           
                <asp:Panel ID="pnlResultado" runat="server"
                    CssClass="resultado-layout" Visible="false">

                    <div class="resultado-main">
                        <div class="content-card">

                            <div class="resultado-header">
                                <div>
                                    <h2 class="card-title">Perfil generado</h2>
                                    <asp:Label ID="lblResultadoMeta" runat="server"
                                        CssClass="card-subtitle" Text="" />
                                </div>
                                <div class="resultado-header-actions">
                                    <asp:Button ID="btnNuevoPerfil" runat="server"
                                        Text="← Nuevo perfil"
                                        CssClass="btn-secondary"
                                        OnClick="btnNuevoPerfil_Click"
                                        CausesValidation="false" />
                                    <asp:Button ID="btnGuardar" runat="server"
                                        Text="💾 Guardar perfil"
                                        CssClass="btn-primary"
                                        OnClick="btnGuardar_Click"
                                        CausesValidation="false" />
                                </div>
                            </div>

                  
                            <div class="modelo-usado-badge">
                                <asp:Label ID="lblModeloUsado" runat="server"
                                    CssClass="modelo-badge-texto" Text="" />
                            </div>

              
                            <div class="ia-badge-resultado">
                                🧠 Perfil orientativo generado por IA · No diagnóstico · Solo representación descriptiva contextual · Revisión profesional recomendada
                            </div>

             
                            <div class="perfil-secciones">

                                <div class="perfil-seccion">
                                    <div class="ps-titulo">
                                        <span class="ps-icono">📌</span> Descripción general del perfil
                                    </div>
                                    <asp:Label ID="lblDescripcionGeneral" runat="server"
                                        CssClass="ps-texto" Text="" />
                                </div>

                                <div class="perfil-seccion">
                                    <div class="ps-titulo">
                                        <span class="ps-icono">📊</span> Dimensiones evaluadas
                                    </div>
                                    <asp:Label ID="lblDimensiones" runat="server"
                                        CssClass="ps-texto" Text="" />
                                </div>

                                <div class="perfil-seccion">
                                    <div class="ps-titulo">
                                        <span class="ps-icono">🔍</span> Patrones identificados en el contexto clínico
                                    </div>
                                    <asp:Label ID="lblPatrones" runat="server"
                                        CssClass="ps-texto" Text="" />
                                </div>

                                <div class="perfil-seccion" style="border-bottom:none; margin-bottom:0; padding-bottom:0;">
                                    <div class="ps-titulo">
                                        <span class="ps-icono">💡</span> Consideraciones para el tratamiento
                                    </div>
                                    <asp:Label ID="lblConsideraciones" runat="server"
                                        CssClass="ps-texto" Text="" />
                                </div>

                            </div>

                   
                            <div class="perfil-nota-pie">
                                ⚠️ Esta perfilación es orientativa y no constituye diagnóstico clínico. El profesional es el único responsable de la interpretación y uso de esta información.
                            </div>

                        </div>
                    </div>

         
                    <div class="resultado-aside">

                        <div class="content-card meta-resultado-card">
                            <p class="accesos-titulo">Detalles del perfil</p>
                            <div class="meta-fila">
                                <span class="meta-label">Paciente</span>
                                <asp:Label ID="lblMetaPaciente" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <span class="meta-label">Modelo utilizado</span>
                                <asp:Label ID="lblMetaModelo" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila">
                                <span class="meta-label">Consultas analizadas</span>
                                <asp:Label ID="lblMetaConsultas" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                            <div class="meta-fila" style="border-bottom:none;">
                                <span class="meta-label">Generado</span>
                                <asp:Label ID="lblMetaFecha" runat="server"
                                    CssClass="meta-valor" Text="" />
                            </div>
                        </div>

                        <div class="content-card accesos-card">
                            <p class="accesos-titulo">Acciones relacionadas</p>
                            <a href="FormExportarReporte.aspx"   class="acceso-item">💾 <span>Exportar en PDF</span></a>
                            <a href="FormResumenIA.aspx"         class="acceso-item">🤖 <span>Resumen IA</span></a>
                            <a href="FormInformeDerivacion.aspx" class="acceso-item">📤 <span>Generar derivación</span></a>
                        </div>

                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo">Perfil encriptado</p>
                            <p class="aviso-texto">Al guardar, el contenido se encripta con AES cumpliendo la Ley 25.326.</p>
                        </div>

                    </div>

                </asp:Panel>

            </div>
        </div>

    </form>

    <script type="text/javascript">
        var modeloActual = '';

        var MODELOS_INFO = {
            'BIGFIVE': { nombre: 'Big Five (BFI)', icono: '🌐', desc: 'Rasgos generales de personalidad en 5 dimensiones.' },
            'COPE': { nombre: 'COPE Simplificado', icono: '🛡️', desc: 'Estilos de afrontamiento ante situaciones de estrés.' },
            'AUTOEFICACIA': { nombre: 'Autoeficacia de Schwarzer', icono: '⚡', desc: 'Autoconcepto y percepción de capacidades propias.' },
            'APEGO': { nombre: 'Estilos de Apego (ECR)', icono: '🔗', desc: 'Ansiedad y evitación en vínculos afectivos adultos.' },
            'VALORES': { nombre: 'Valores y Sentido de Vida', icono: '🌱', desc: 'Valores personales y sentido de vida (PVQ/Logoterapia).' }
        };

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
                alert('Seleccioná un modelo de evaluación antes de generar el perfil.');
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