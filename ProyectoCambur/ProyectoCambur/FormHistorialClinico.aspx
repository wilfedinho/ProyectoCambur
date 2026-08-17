<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormHistorialClinico.aspx.cs" Inherits="FormHistorialClinico" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Historial Clínico</title>
    <link href="EstilosPaginas/Shared.css"               rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormHistorialClinico.css"  rel="stylesheet" type="text/css"/>
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
                <a href="FormHistorialClinico.aspx"  class="nav-item active">📋 Historial Clínico</a>
                <a href="FormResumenIA.aspx"         class="nav-item">🤖 Resumen IA</a>
                <a href="FormLineaTemporal.aspx"     class="nav-item">📅 Línea Temporal</a>
                <a href="FormInformeDerivacion.aspx" class="nav-item">📤 Derivaciones</a>
                <a href="FormPerfilPaciente.aspx"    class="nav-item">🧠 Perfilación</a>
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
                    <span class="header-section">Pacientes</span>
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderPaciente" runat="server" CssClass="header-page" Text="Historial Clínico" />
                </div>
                <div class="header-user">
                    <div class="user-avatar">
                        <asp:Label ID="lblIniciales" runat="server" Text="LM" />
                    </div>
                    <div class="user-info">
                        <asp:Label ID="lblNombreProfesional" runat="server" CssClass="user-name" Text="" />
                        <span class="user-role">Psicólogo/a</span>
                    </div>
                </div>
            </header>

            
            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" CssClass="server-error" />

             
                <div class="historial-layout">

              
                    <div class="historial-form-col">

                        <div class="content-card">

                    
                            <div class="paciente-header">
                                <div class="paciente-header-avatar">
                                    <asp:Label ID="lblPacienteIniciales" runat="server" Text="MG" />
                                </div>
                                <div class="paciente-header-info">
                                    <asp:Label ID="lblPacienteNombre" runat="server"
                                        CssClass="paciente-header-nombre" Text="" />
                                    <div class="paciente-header-meta">
                                        <asp:Label ID="lblPacienteEdad"    runat="server" CssClass="meta-item" Text="" />
                                        <span class="meta-sep">·</span>
                                        <asp:Label ID="lblPacienteEstado"  runat="server" CssClass="meta-item" Text="" />
                                        <span class="meta-sep">·</span>
                                        <asp:Label ID="lblPacienteOcup"    runat="server" CssClass="meta-item" Text="" />
                                    </div>
                                </div>
                                <div class="paciente-header-actions">
                                    <asp:Label ID="lblEstadoHistorial" runat="server"
                                        CssClass="badge-historial-completo" Text="" />
                                </div>
                            </div>

                            <div class="section-sep" style="margin-top:20px;">Información clínica persistente</div>
                            <p class="hint-text">Esta información no proviene de una consulta específica sino del contexto general del paciente. Expandí cada sección para completarla.</p>

                           
                            <div class="seccion-colapsable">
                                <div class="seccion-header" onclick="toggleSeccion(this)">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">🚬</span>
                                        <span class="seccion-titulo">Hábitos nocivos</span>
                                    </div>
                                    <div class="seccion-header-right">
                                        <asp:Label ID="lblBadgeHabitos" runat="server" CssClass="badge-seccion" Text="Pendiente" />
                                        <span class="seccion-flecha">▾</span>
                                    </div>
                                </div>
                                <div class="seccion-body">
                                    <div class="field">
                                        <asp:TextBox ID="txtHabitosNocivos" runat="server"
                                            TextMode="MultiLine" Rows="4"
                                            placeholder="Tabaco, alcohol, sustancias, sedentarismo, trastornos del sueño, alimentación..."
                                            ClientIDMode="Static"
                                            oninput="actualizarBadge(this, 'lblBadgeHabitos')" />
                                    </div>
                                </div>
                            </div>

                          
                            <div class="seccion-colapsable">
                                <div class="seccion-header" onclick="toggleSeccion(this)">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">👨‍👩‍👧</span>
                                        <span class="seccion-titulo">Contexto familiar</span>
                                    </div>
                                    <div class="seccion-header-right">
                                        <asp:Label ID="lblBadgeContexto" runat="server" CssClass="badge-seccion" Text="Pendiente" />
                                        <span class="seccion-flecha">▾</span>
                                    </div>
                                </div>
                                <div class="seccion-body">
                                    <div class="field">
                                        <asp:TextBox ID="txtContextoFamiliar" runat="server"
                                            TextMode="MultiLine" Rows="4"
                                            placeholder="Composición familiar, dinámica, roles, conflictos, vínculos significativos..."
                                            ClientIDMode="Static"
                                            oninput="actualizarBadge(this, 'lblBadgeContexto')" />
                                    </div>
                                </div>
                            </div>

                            
                            <div class="seccion-colapsable">
                                <div class="seccion-header" onclick="toggleSeccion(this)">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">🧬</span>
                                        <span class="seccion-titulo">Antecedentes familiares</span>
                                    </div>
                                    <div class="seccion-header-right">
                                        <asp:Label ID="lblBadgeAntFam" runat="server" CssClass="badge-seccion" Text="Pendiente" />
                                        <span class="seccion-flecha">▾</span>
                                    </div>
                                </div>
                                <div class="seccion-body">
                                    <div class="field">
                                        <asp:TextBox ID="txtAntecedentesFamiliares" runat="server"
                                            TextMode="MultiLine" Rows="4"
                                            placeholder="Antecedentes psiquiátricos, enfermedades crónicas, adicciones en la familia..."
                                            ClientIDMode="Static"
                                            oninput="actualizarBadge(this, 'lblBadgeAntFam')" />
                                    </div>
                                </div>
                            </div>

                      
                            <div class="seccion-colapsable">
                                <div class="seccion-header" onclick="toggleSeccion(this)">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">🏥</span>
                                        <span class="seccion-titulo">Antecedentes médicos</span>
                                    </div>
                                    <div class="seccion-header-right">
                                        <asp:Label ID="lblBadgeAntMed" runat="server" CssClass="badge-seccion" Text="Pendiente" />
                                        <span class="seccion-flecha">▾</span>
                                    </div>
                                </div>
                                <div class="seccion-body">
                                    <div class="field">
                                        <asp:TextBox ID="txtAntecedentesMedicos" runat="server"
                                            TextMode="MultiLine" Rows="4"
                                            placeholder="Enfermedades crónicas, medicación actual, internaciones, cirugías, alergias..."
                                            ClientIDMode="Static"
                                            oninput="actualizarBadge(this, 'lblBadgeAntMed')" />
                                    </div>
                                </div>
                            </div>

                          
                            <div class="seccion-colapsable">
                                <div class="seccion-header" onclick="toggleSeccion(this)">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">💼</span>
                                        <span class="seccion-titulo">Situación laboral</span>
                                    </div>
                                    <div class="seccion-header-right">
                                        <asp:Label ID="lblBadgeLaboral" runat="server" CssClass="badge-seccion" Text="Pendiente" />
                                        <span class="seccion-flecha">▾</span>
                                    </div>
                                </div>
                                <div class="seccion-body">
                                    <div class="field">
                                        <asp:TextBox ID="txtSituacionLaboral" runat="server"
                                            TextMode="MultiLine" Rows="4"
                                            placeholder="Ocupación actual, estabilidad laboral, ambiente de trabajo, conflictos, satisfacción..."
                                            ClientIDMode="Static"
                                            oninput="actualizarBadge(this, 'lblBadgeLaboral')" />
                                    </div>
                                </div>
                            </div>

                       
                            <div class="seccion-colapsable">
                                <div class="seccion-header" onclick="toggleSeccion(this)">
                                    <div class="seccion-header-left">
                                        <span class="seccion-icono">⚡</span>
                                        <span class="seccion-titulo">Eventos traumáticos relevantes</span>
                                    </div>
                                    <div class="seccion-header-right">
                                        <asp:Label ID="lblBadgeTrauma" runat="server" CssClass="badge-seccion" Text="Pendiente" />
                                        <span class="seccion-flecha">▾</span>
                                    </div>
                                </div>
                                <div class="seccion-body">
                                    <div class="field">
                                        <asp:TextBox ID="txtEventosTraumaticos" runat="server"
                                            TextMode="MultiLine" Rows="4"
                                            placeholder="Pérdidas significativas, abuso, accidentes, situaciones de violencia, duelos no elaborados..."
                                            ClientIDMode="Static"
                                            oninput="actualizarBadge(this, 'lblBadgeTrauma')" />
                                    </div>
                                </div>
                            </div>

                         
                            <div class="form-actions">
                                <a href="FormRegistroPaciente.aspx" class="btn-secondary">Volver a pacientes</a>
                                <asp:Button ID="btnGuardar" runat="server"
                                    Text="Guardar historial clínico"
                                    CssClass="btn-primary"
                                    OnClick="btnGuardar_Click" />
                            </div>

                        </div>
                    </div>

              
                    <div class="historial-info-col">

                    
                        <div class="content-card progreso-card">
                            <p class="progreso-titulo">Completitud del historial</p>
                            <div class="progreso-barra-wrap">
                                <div class="progreso-barra">
                                    <div class="progreso-fill" id="progresoFill" style="width: 0%"></div>
                                </div>
                                <span class="progreso-pct" id="progresoPct">0%</span>
                            </div>
                            <div class="progreso-items">
                                <div class="progreso-item" id="pi-habitos">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">Hábitos nocivos</span>
                                </div>
                                <div class="progreso-item" id="pi-contexto">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">Contexto familiar</span>
                                </div>
                                <div class="progreso-item" id="pi-antfam">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">Antecedentes familiares</span>
                                </div>
                                <div class="progreso-item" id="pi-antmed">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">Antecedentes médicos</span>
                                </div>
                                <div class="progreso-item" id="pi-laboral">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">Situación laboral</span>
                                </div>
                                <div class="progreso-item" id="pi-trauma">
                                    <span class="pi-icono pi-pendiente">○</span>
                                    <span class="pi-label">Eventos traumáticos</span>
                                </div>
                            </div>
                        </div>

                   
                        <div class="content-card aviso-card">
                            <div class="aviso-icon">🔒</div>
                            <p class="aviso-titulo">Datos encriptados</p>
                            <p class="aviso-texto">El historial clínico se encripta con AES antes de persistirse, cumpliendo la Ley 25.326.</p>
                        </div>

                    
                        <div class="content-card accesos-card">
                            <p class="accesos-titulo">Accesos rápidos</p>
                            <a href='FormRealizarConsulta.aspx' class="acceso-item">
                                🗒️ <span>Nueva consulta</span>
                            </a>
                            <a href='FormResumenIA.aspx' class="acceso-item">
                                🤖 <span>Generar resumen IA</span>
                            </a>
                            <a href='FormLineaTemporal.aspx' class="acceso-item">
                                📅 <span>Ver línea temporal</span>
                            </a>
                        </div>

                    </div>

                </div>
            </div>
        </div>

    </form>

    <script type="text/javascript">
       
        function toggleSeccion(header) {
            var seccion = header.parentElement;
            var body    = seccion.querySelector('.seccion-body');
            var flecha  = header.querySelector('.seccion-flecha');
            var abierto = seccion.classList.contains('abierto');

            if (abierto) {
                seccion.classList.remove('abierto');
                body.style.maxHeight  = '0';
                body.style.paddingTop = '0';
                flecha.textContent    = '▾';
            } else {
                seccion.classList.add('abierto');
                body.style.maxHeight  = body.scrollHeight + 'px';
                body.style.paddingTop = '14px';
                flecha.textContent    = '▴';
            }
        }

       
        function actualizarBadge(textarea, badgeId) {
            var badge = document.getElementById(badgeId);
            if (!badge) return;
            var tieneContenido = textarea.value.trim().length > 0;
            badge.textContent = tieneContenido ? 'Completado' : 'Pendiente';
            badge.className   = tieneContenido ? 'badge-seccion completado' : 'badge-seccion';
            actualizarProgreso();
        }

       
        function actualizarProgreso() {
            var campos = [
                { id: 'txtHabitosNocivos',         piId: 'pi-habitos'  },
                { id: 'txtContextoFamiliar',        piId: 'pi-contexto' },
                { id: 'txtAntecedentesFamiliares',  piId: 'pi-antfam'   },
                { id: 'txtAntecedentesMedicos',     piId: 'pi-antmed'   },
                { id: 'txtSituacionLaboral',        piId: 'pi-laboral'  },
                { id: 'txtEventosTraumaticos',      piId: 'pi-trauma'   }
            ];

            var completados = 0;
            campos.forEach(function (c) {
                var el = document.getElementById(c.id);
                var pi = document.getElementById(c.piId);
                if (!el || !pi) return;
                var ok = el.value.trim().length > 0;
                if (ok) completados++;
                var icono = pi.querySelector('.pi-icono');
                if (icono) {
                    icono.textContent = ok ? '●' : '○';
                    icono.className   = ok ? 'pi-icono pi-ok' : 'pi-icono pi-pendiente';
                }
            });

            var pct  = Math.round((completados / campos.length) * 100);
            var fill = document.getElementById('progresoFill');
            var pctEl= document.getElementById('progresoPct');
            if (fill)  fill.style.width  = pct + '%';
            if (pctEl) pctEl.textContent = pct + '%';
        }

      
        window.addEventListener('DOMContentLoaded', function () {
            actualizarProgreso();

            
            var campos = [
                'txtHabitosNocivos', 'txtContextoFamiliar',
                'txtAntecedentesFamiliares', 'txtAntecedentesMedicos',
                'txtSituacionLaboral', 'txtEventosTraumaticos'
            ];
            campos.forEach(function (id) {
                var el = document.getElementById(id);
                if (el && el.value.trim().length > 0) {
                    var seccion = el.closest('.seccion-colapsable');
                    if (seccion && !seccion.classList.contains('abierto')) {
                        toggleSeccion(seccion.querySelector('.seccion-header'));
                    }
                }
            });
        });
    </script>
</body>
</html>
