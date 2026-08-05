# 🎨 Cambur — Design System & Contexto de Desarrollo Frontend

> Documento de referencia para la generación de nuevas pantallas del sistema **Cambur**.  
> Adjuntarlo al inicio de cada nueva sesión para mantener coherencia visual y técnica con las pantallas ya desarrolladas.

---

## 1. Stack tecnológico

| Capa | Tecnología |
|---|---|
| Backend | C# — ASP.NET WebForms (.NET Framework) |
| Archivos de pantalla | `.aspx` (markup) + `.aspx.cs` (code-behind) |
| Estilos | CSS3 externo en carpeta `EstilosPaginas/` |
| Interactividad | JavaScript vanilla puro (sin frameworks) |
| Fuentes | Google Fonts (importadas desde CDN) |
| Gráficos | CSS puro (divs proporcionales — sin librerías) |
| IA / API externa | Python / FastAPI con OpenRouter (módulo separado) |

### Reglas estrictas de frontend
- ❌ No usar React, Vue, Angular ni ningún framework JS
- ❌ No usar Bootstrap ni librerías CSS externas
- ❌ No usar jQuery
- ✅ Solo HTML5 + CSS3 + JavaScript vanilla
- ✅ Una sola importación permitida: Google Fonts via `@import url(...)`
- ✅ Todo el CSS va en archivo `.css` separado en `EstilosPaginas/`
- ✅ Todo el JS va en `<script>` al final del `.aspx`, antes de `</body>`

---

## 2. Estructura de archivos

```
📁 Proyecto/
├── FormNombrePantalla.aspx           ← Markup + controles ASP.NET
├── FormNombrePantalla.aspx.cs        ← Code-behind en C#
└── EstilosPaginas/
    ├── Shared.css                    ← CSS base de toda la app (SIEMPRE incluir)
    └── FormNombrePantalla.css        ← CSS específico de la pantalla
```

### Referencia de CSS en el `<head>` (SIEMPRE este orden)
```html
<link href="EstilosPaginas/Shared.css"             rel="stylesheet" type="text/css"/>
<link href="EstilosPaginas/FormNombrePantalla.css"  rel="stylesheet" type="text/css"/>
```

### Excepción: pantallas públicas (Login, Registro)
Las pantallas **sin sidebar** (Login, Registro de Profesional) **no incluyen** `Shared.css`.  
Tienen su propio CSS autocontenido con las variables de la paleta.

---

## 3. Paleta de colores

```css
:root {
    /* Fondos */
    --bg-paper:      #F2EEE8;   /* Fondo general — blanco papel cálido */
    --surface:       #FFFFFF;   /* Cards, paneles, formularios */

    /* Sidebar */
    --sidebar-dark:  #1B2A3B;   /* Fondo principal del sidebar */
    --sidebar-mid:   #243A4E;   /* Hover y elementos activos del sidebar */
    --sidebar-dark2: #162330;   /* Círculos decorativos del sidebar */

    /* Tipografía */
    --text-main:     #1B2A3B;   /* Texto principal */
    --text-sub:      #3D566E;   /* Texto secundario / labels */
    --text-muted:    #6B8BA4;   /* Texto atenuado */
    --text-light:    #9AABBF;   /* Separadores de sección, eyebrows */

    /* Acento */
    --accent:        #E8455A;   /* Coral/rojo — botones IA, badges, elementos destacados */

    /* Bordes */
    --border:        #DDD7CE;   /* Bordes generales */

    /* Semánticos */
    --success:       #2A9D8F;   /* Verde — éxito, activo */
    --success-bg:    #EAF7F5;
    --success-border:#9FE1CB;
    --success-text:  #085041;

    --warning:       #F4A261;   /* Naranja — advertencia */

    /* Fuentes */
    --font-title:    'Barlow Condensed', sans-serif;
    --font-body:     'DM Sans', sans-serif;

    /* Dimensiones estructurales */
    --sidebar-w:     240px;
    --header-h:      64px;
}
```

### Reglas de uso del color
- El **coral** `#E8455A` es el **único acento**. Usarlo para: botones de IA, badge "Más elegido", ítem activo del sidebar (borde izquierdo), eyebrows de sección, asteriscos de campos obligatorios.
- El **azul pizarra** `#1B2A3B` es el color principal: sidebar, botones primarios, texto de títulos.
- El **blanco papel** `#F2EEE8` es el fondo general de la app (nunca blanco puro como fondo).
- El **verde** `#2A9D8F` solo para estados positivos: badges "Activo", mensajes de éxito, barras de progreso completadas.
- Nunca mezclar el coral con el verde en el mismo componente.

---

## 4. Tipografía

| Uso | Fuente | Peso | Tamaño referencial |
|---|---|---|---|
| Logotipo CAMBUR | Barlow Condensed | 800 | 30–42px |
| Títulos de pantalla / card | Barlow Condensed | 700 | 22–34px |
| Títulos hero (Login, Registro) | Barlow Condensed | 800 | 40–44px |
| Números grandes (KPIs, stats) | Barlow Condensed | 700 | 30–44px |
| Cuerpo / labels / inputs | DM Sans | 400 | 13–14px |
| Labels de campo | DM Sans | 500 | 11–12px |
| Eyebrows / separadores | DM Sans | 500 | 10–11px, `letter-spacing: 2–2.5px`, `text-transform: uppercase` |
| Notas / legales | DM Sans | 300–400 | 11–12px |

```html
<!-- Importar siempre en el CSS (no en el HTML) -->
@import url('https://fonts.googleapis.com/css2?family=Barlow+Condensed:wght@700;800&family=DM+Sans:wght@300;400;500&display=swap');
```

---

## 5. Layout de la aplicación

### 5.1 Layout base de pantallas internas (con sesión)

```
┌─────────────────────────────────────────────────────────┐
│  SIDEBAR (240px fijo)  │  HEADER (64px sticky)          │
│  #1B2A3B               │  #FFFFFF / border-bottom        │
│                        ├────────────────────────────────│
│  Logo CAMBUR           │                                 │
│  Nav items             │   CONTENIDO (.page-content)     │
│  ...                   │   padding: 32px                 │
│  [footer]              │                                 │
│  Mi Suscripción        │                                 │
│  Cerrar sesión         │                                 │
└────────────────────────┴────────────────────────────────┘
```

**Clases base en `Shared.css`:**
```css
#form1      { display: flex; width: 100%; }
.sidebar    { width: 240px; position: fixed; height: 100vh; background: #1B2A3B; }
.main-wrap  { margin-left: 240px; flex: 1; display: flex; flex-direction: column; }
.top-header { height: 64px; background: #FFF; border-bottom: 0.5px solid var(--border); position: sticky; top: 0; }
.page-content { padding: 32px; flex: 1; }
```

### 5.2 Layout de pantallas públicas (Login / Registro)

Card flotante centrado en la pantalla, dividido en dos mitades:

```
┌─────────────────────────────────────────────────────────┐
│              FONDO #F2EEE8 con gradientes sutiles        │
│  ┌───────────────────────────────────────────────────┐  │
│  │  MITAD OSCURA (42%)      │  MITAD CLARA (58%)     │  │
│  │  #1B2A3B                 │  #FFFFFF               │  │
│  │                          │                        │  │
│  │  Logo CAMBUR             │  Formulario            │  │
│  │  Texto hero              │  Campos + botón        │  │
│  │  Pills informativos      │  Links de navegación   │  │
│  │  Círculos decorativos    │                        │  │
│  └──────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────┘
```

**Dimensiones:** `max-width: 880px`, `min-height: 520px`, `border-radius: 16px`  
**Sombra:** `box-shadow: 0 24px 64px rgba(27,42,59,0.18), 0 4px 16px rgba(27,42,59,0.10)`

---

## 6. Componentes del sidebar

### Items de navegación (con emojis outline)
```html
<a href="FormDashboard.aspx"         class="nav-item">🏠 Dashboard</a>
<a href="FormRegistroPaciente.aspx"  class="nav-item">👤 Pacientes</a>
<a href="FormRealizarConsulta.aspx"  class="nav-item">🗒️ Consultas</a>
<a href="FormHistorialClinico.aspx"  class="nav-item">📋 Historial Clínico</a>
<a href="FormResumenIA.aspx"         class="nav-item">🤖 Resumen IA</a>
<a href="FormLineaTemporal.aspx"     class="nav-item">📅 Línea Temporal</a>
<a href="FormInformeDerivacion.aspx" class="nav-item">📤 Derivaciones</a>
<a href="FormPerfilPaciente.aspx"    class="nav-item">🧠 Perfilación</a>
<a href="FormExportarReporte.aspx"   class="nav-item">💾 Exportar</a>
```
```html
<!-- Footer del sidebar -->
<a href="FormSuscripcion.aspx" class="nav-item">💳 Mi Suscripción</a>
<a href="FormLogout.aspx"      class="nav-item nav-logout">🚪 Cerrar sesión</a>
```

**Sidebar Administrador** (pantallas admin) usa la misma estructura con ítems distintos:
```html
<a href="FormAuditoriaBitacora.aspx"  class="nav-item">📜 Bitácora</a>
<a href="FormBackupRestore.aspx"      class="nav-item">💾 Backup / Restore</a>
<a href="FormDigitoVerificador.aspx"  class="nav-item">🔢 Dígito Verificador</a>
<a href="FormGestionIdiomas.aspx"     class="nav-item">🌐 Gestionar Idiomas</a>
<a href="FormABMProfesionales.aspx"   class="nav-item">👤 ABM Profesionales</a>
<a href="FormABMPacientes.aspx"       class="nav-item">🧑‍⚕️ ABM Pacientes</a>
<a href="FormABMConsultas.aspx"       class="nav-item">🗒️ ABM Consultas</a>
```

Y el tagline del logo cambia a:
```html
<div class="tagline-admin">Panel Administrador</div>
```
Con color: `color: var(--accent)` (en lugar del gris habitual)

---

## 7. Componentes reutilizables

### 7.1 Content Card
Contenedor blanco principal para secciones de contenido.
```css
.content-card {
    background: var(--surface);
    border: 0.5px solid var(--border);
    border-radius: 10px;
    padding: 28px 32px;
}
.mt-24 { margin-top: 24px; }
```

### 7.2 Separador de sección
```css
.section-sep {
    font-size: 10px;
    letter-spacing: 2.5px;
    text-transform: uppercase;
    color: var(--text-light);
    font-weight: 500;
    margin: 24px 0 14px 0;
    display: flex;
    align-items: center;
    gap: 12px;
}
.section-sep::after { content: ''; flex: 1; height: 0.5px; background: var(--border); }
```

### 7.3 Campos de formulario
```css
.field { display: flex; flex-direction: column; gap: 5px; }
.field label { font-size: 12px; font-weight: 500; color: var(--text-sub); letter-spacing: 0.2px; }
.field label sup { color: var(--accent); font-size: 11px; }

/* Inputs, selects, textareas */
.field input, .field select, .field textarea {
    background: #FAFAF8;
    border: 0.5px solid var(--border);
    border-radius: 6px;
    padding: 10px 14px;
    font-size: 14px;
    color: var(--text-main);
    font-family: var(--font-body);
    outline: none;
    width: 100%;
    transition: border-color 0.15s;
}
.field input:focus, .field select:focus, .field textarea:focus {
    border-color: var(--text-main);
    background: var(--surface);
}
```

### 7.4 Grillas de campos
```css
.grid-2 { display: grid; grid-template-columns: 1fr 1fr;       gap: 14px; }
.grid-3 { display: grid; grid-template-columns: 1fr 1fr 1fr;   gap: 14px; }
.grid-4 { display: grid; grid-template-columns: repeat(4,1fr); gap: 14px; }
```

### 7.5 Botones principales
```css
/* Primario: azul oscuro */
.btn-primary {
    background: var(--text-main);
    color: var(--bg-paper);
    border: none; border-radius: 6px;
    padding: 11px 28px;
    font-size: 14px; font-weight: 500;
    cursor: pointer; transition: background 0.15s;
}
.btn-primary:hover { background: var(--sidebar-mid); }

/* Secundario: borde neutro */
.btn-secondary {
    background: transparent;
    color: var(--text-sub);
    border: 0.5px solid var(--border);
    border-radius: 6px;
    padding: 11px 24px;
    font-size: 14px; font-weight: 400;
    cursor: pointer;
}

/* Éxito: verde */
.btn-success { background: var(--success); color: #fff; border: none; border-radius: 6px; padding: 11px 24px; }

/* Peligro: coral */
.btn-danger  { background: transparent; color: #993C1D; border: 0.5px solid #F5C4B3; border-radius: 6px; padding: 10px 20px; }
.btn-danger-solid { background: var(--accent); color: #fff; border: none; border-radius: 6px; padding: 11px 24px; }
```

### 7.6 Mensajes servidor
```css
/* Error */
.server-error {
    background: #FEF0F0; border: 0.5px solid #F5C4B3;
    border-radius: 6px; padding: 10px 16px;
    font-size: 13px; color: #993C1D; display: block; margin-bottom: 20px;
}

/* Éxito */
.server-success {
    background: var(--success-bg); border: 0.5px solid var(--success-border);
    border-radius: 6px; padding: 10px 16px;
    font-size: 13px; color: var(--success-text); display: block; margin-bottom: 20px;
}
```

### 7.7 Badges de estado
```css
/* Activo — verde */
.badge-estado.activo   { background: var(--success-bg); border: 0.5px solid var(--success-border); color: var(--success-text); }

/* Inactivo — gris */
.badge-estado.inactivo { background: #F1EFE8; border: 0.5px solid #D3D1C7; color: #5F5E5A; }

/* Conteo activos */
.badge-activos   { background: var(--success-bg); border: 0.5px solid var(--success-border); color: var(--success-text); }

/* Conteo inactivos */
.badge-inactivos { background: #FEF0F0; border: 0.5px solid #F5C4B3; color: #993C1D; }

/* Todos los badges */
.badge-estado, .badge-activos, .badge-inactivos {
    font-size: 11–12px; font-weight: 500;
    border-radius: 100px; padding: 3–5px 10–14px;
    display: inline-block; white-space: nowrap;
}
```

### 7.8 Tabla de datos (GridView)
```css
.data-table          { width: 100%; border-collapse: collapse; font-size: 13px; }
.data-table th       { padding: 10px 14px; font-size: 11px; letter-spacing: 1.5px; text-transform: uppercase; color: var(--text-muted); border-bottom: 0.5px solid var(--border); background: #FAFAF8; }
.data-table td       { padding: 13px 14px; color: var(--text-main); border-bottom: 0.5px solid #EDE8E0; }
.table-row-alt td    { background: #FDFCFA; }
.th-left             { text-align: left !important; }
.th-centro           { text-align: center !important; }
.td-centro           { text-align: center; color: var(--text-sub); }
.td-acciones         { text-align: right; white-space: nowrap; }
.empty-row td        { text-align: center; color: var(--text-muted); padding: 40px 0; font-style: italic; }
```

### 7.9 Botones de acción en tabla
```css
.tbl-btn             { font-size: 12px; font-weight: 500; border-radius: 5px; padding: 5px 11px; display: inline-block; margin-left: 6px; border: 0.5px solid transparent; transition: opacity 0.12s; text-decoration: none; }
.tbl-btn-ver         { color: var(--text-sub); border-color: var(--border); background: var(--surface); }
.tbl-btn-mod         { color: #0C447C; border-color: #B5D4F4; background: #E6F1FB; }
.tbl-btn-baja        { color: #993C1D; border-color: #F5C4B3; background: #FEF0F0; }
.tbl-btn-reactivar   { color: var(--success-text); border-color: var(--success-border); background: var(--success-bg); }
.tbl-btn-hidden      { display: none !important; }
```

### 7.10 Overlay de carga (pantallas con IA)
```css
.carga-overlay {
    position: fixed; inset: 0;
    background: rgba(27, 42, 59, 0.55);
    z-index: 200; display: flex;
    align-items: center; justify-content: center;
}
.carga-card { background: var(--surface); border-radius: 12px; padding: 40px 48px; text-align: center; min-width: 320px; }
.carga-spinner { width: 44px; height: 44px; border: 3px solid #EDE8E0; border-top-color: var(--text-main); border-radius: 50%; margin: 0 auto 20px; animation: spin 0.8s linear infinite; }
@keyframes spin { to { transform: rotate(360deg); } }
.carga-titulo    { font-family: var(--font-title); font-size: 20px; font-weight: 700; color: var(--text-main); margin-bottom: 8px; }
.carga-subtitulo { font-size: 13px; color: var(--text-muted); line-height: 1.6; }
```

### 7.11 Aviso de IA (badge amarillo resultado)
```css
.ia-badge-resultado {
    background: #FAEEDA; border: 0.5px solid #FAC775;
    border-left: 3px solid #EF9F27; border-radius: 6px;
    padding: 10px 14px; font-size: 12px; color: #633806;
    margin-bottom: 20px; line-height: 1.6;
}
```

### 7.12 Aviso informativo azul
```css
.ia-badge-aviso {
    background: #F0F4FA; border: 0.5px solid #B5D4F4;
    border-left: 3px solid #378ADD; border-radius: 6px;
    padding: 11px 16px; font-size: 13px; color: #0C447C; line-height: 1.6;
}
```

### 7.13 Accesos rápidos (aside)
```css
.acceso-item {
    display: flex; align-items: center; gap: 8px;
    padding: 9px 10px; border-radius: 6px;
    font-size: 13px; color: var(--text-sub);
    transition: background 0.12s; text-decoration: none;
}
.acceso-item:hover { background: var(--bg-paper); color: var(--text-main); }
```

---

## 8. Layouts de pantallas (patrones recurrentes)

### Layout 2 columnas (principal + aside 240px)
Usado en: Registrar Paciente, Historial Clínico, Perfilación, Exportar, Suscripción, etc.
```css
.pantalla-layout {
    display: grid;
    grid-template-columns: 1fr 240px;
    gap: 20px;
    align-items: start;
}
```

### Layout 2 columnas (formulario 70% + info 30%)
Usado en: Realizar Consulta, Modificar Consulta.
```css
.consulta-layout {
    display: grid;
    grid-template-columns: 1fr 280px;
    gap: 20px;
    align-items: start;
}
```

### Layout 2 columnas simétricas
Usado en: Backup/Restore.
```css
.br-layout {
    display: grid;
    grid-template-columns: 1fr 1fr;
    gap: 20px;
    align-items: start;
}
```

### Layout resultado IA (main + aside 260px)
Usado en: Resumen IA, Informe de Derivación, Perfilación resultado.
```css
.resultado-layout {
    display: grid;
    grid-template-columns: 1fr 260px;
    gap: 20px;
    align-items: start;
}
```

---

## 9. Controles ASP.NET WebForms — tabla de conversión

| HTML generado | Control ASP.NET |
|---|---|
| `<input type="text">` | `<asp:TextBox runat="server" />` |
| `<input type="password">` | `<asp:TextBox TextMode="Password" runat="server" />` |
| `<input type="email">` | `<asp:TextBox TextMode="Email" runat="server" />` |
| `<input type="date">` | `<asp:TextBox TextMode="Date" runat="server" />` |
| `<input type="number">` | `<asp:TextBox TextMode="Number" runat="server" />` |
| `<textarea>` | `<asp:TextBox TextMode="MultiLine" Rows="4" runat="server" />` |
| `<select>` | `<asp:DropDownList runat="server">` |
| `<input type="checkbox">` | `<asp:CheckBox runat="server" />` |
| `<input type="radio">` | `<asp:RadioButton GroupName="grupo" runat="server" />` |
| `<button type="submit">` | `<asp:Button OnClick="btn_Click" runat="server" />` |
| `<a>` con postback | `<asp:LinkButton CommandName="accion" runat="server" />` |
| `<span id="msg">` | `<asp:Label runat="server" />` |
| `<input type="hidden">` | `<asp:HiddenField runat="server" />` |
| `<table>` de datos | `<asp:GridView runat="server" />` |
| Lista repetida | `<asp:Repeater runat="server" />` |
| Sección condicional | `<asp:Panel Visible="false" runat="server" />` |
| Validación requerida | `<asp:RequiredFieldValidator runat="server" />` |
| Validación regex | `<asp:RegularExpressionValidator runat="server" />` |
| Validar comparación | `<asp:CompareValidator runat="server" />` |

### Atributos importantes
- `ClientIDMode="Static"` → para poder referenciar el control desde JS por su ID
- `AutoPostBack="true"` → en DropDownList para disparar postback al cambiar
- `ValidationGroup="vgNombre"` → agrupar validadores por formulario
- `CausesValidation="false"` → en botones secundarios que no deben validar

---

## 10. Patrón del Code-Behind

### Estructura estándar de cada `.aspx.cs`

```csharp
public partial class FormNombrePantalla : System.Web.UI.Page
{
    // ── PAGE LOAD ──────────────────────────────────────────
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarDatosDemo();        // Pre-relleno para demo
        }
    }

    // ── PROFESIONAL LOGUEADO (demo) ────────────────────────
    // TODO: reemplazar por Session["Profesional"]
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text         = "LM";
    }

    // ── DATOS DEMO ─────────────────────────────────────────
    // TODO: reemplazar por llamadas a BLL real
    private void CargarDatosDemo() { ... }

    // ── EVENTOS DE BOTONES ─────────────────────────────────
    protected void btnAccion_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        if (!Page.IsValid) return;
        // lógica...
        MostrarExito("Operación realizada correctamente.");
    }

    // ── HELPERS ────────────────────────────────────────────
    private void MostrarError(string msg)
    {
        lblMensaje.Text     = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible  = true;
    }

    private void MostrarExito(string msg)
    {
        lblMensaje.Text     = msg;
        lblMensaje.CssClass = "server-success";
        lblMensaje.Visible  = true;
    }
}
```

### Convenciones de comentarios TODO
```csharp
// TODO: reemplazar por Session["Profesional"]
// TODO: reemplazar por BLL.PacienteBLL.ObtenerPorId(id)
// TODO: reemplazar por BLL.ConsultaBLL.Registrar(consulta)
// Datos clínicos → se encripta con AES en BLL antes de persistir
// BLL.DigitoVerificadorBLL.RecalcularPorProfesional(idProfesional)
// BLL.BitacoraBLL.Registrar(id, "Módulo", "descripción", criticidad: 3)
```

---

## 11. Responsive — breakpoints y adaptaciones

### Media queries recomendados
```css
/* Tablet */
@media (max-width: 1024px) {
    .grid-4 { grid-template-columns: 1fr 1fr; }
    .kpi-grid-4 { grid-template-columns: 1fr 1fr; }
}

/* Layout con aside → columna única */
@media (max-width: 860px) {
    .pantalla-layout,
    .resultado-layout,
    .consulta-layout { grid-template-columns: 1fr; }
    .pantalla-aside  { order: -1; } /* El aside va arriba en mobile */
}

/* Sidebar colapsable */
@media (max-width: 768px) {
    .sidebar         { width: 200px; }
    .main-wrap       { margin-left: 200px; }
    .grid-3          { grid-template-columns: 1fr 1fr; }
}

/* Mobile */
@media (max-width: 600px) {
    .sidebar         { display: none; } /* En producción usar hamburger menu */
    .main-wrap       { margin-left: 0; }
    .grid-2,
    .grid-3          { grid-template-columns: 1fr; }
    .page-content    { padding: 16px; }
    .login-card      { flex-direction: column; }
    .card-brand      { min-height: 200px; width: 100%; }
    .kpi-grid-4      { grid-template-columns: 1fr 1fr; }
    .dash-main-row   { grid-template-columns: 1fr; }
}
```

### Reglas generales de responsive
- Todos los `grid-template-columns` deben colapsar a `1fr` en mobile
- Los asides de 240px pasan a ancho completo (orden arriba o abajo según contexto)
- Los cards del login colapsan: columna izquierda arriba, formulario abajo
- Las tablas de datos usan `overflow-x: auto` en el `.table-wrap`
- Los botones de acción en mobile: `width: 100%`
- El sidebar en mobile debe estar oculto o reemplazado por menú hamburguesa

---

## 12. Pantallas desarrolladas

### Pantallas de usuario (profesional)

| Archivo | Caso de uso | Descripción |
|---|---|---|
| `FormRegistroProfesional.aspx` | CUN01 | Registro público — card doble columna |
| `FormLogin.aspx` | CUS01 | Login — card centrado con branding |
| `FormLogout.aspx` | CUS02 | Proceso de cierre de sesión |
| `FormDashboard.aspx` | CUN11 | Dashboard operativo con KPIs y gráfico CSS |
| `FormRegistroPaciente.aspx` | CUN02 | ABM de pacientes con tabla y acciones |
| `FormRealizarConsulta.aspx` | CUN03 | Formulario clínico + card lateral paciente |
| `FormHistorialClinico.aspx` | CUN04 | Secciones colapsables + barra de progreso |
| `FormResumenIA.aspx` | CUN05 | Flujo 3 estados: filtros → consultas → resultado |
| `FormModificarConsulta.aspx` | CUN06 | Edición con control de plazo (3 días) |
| `FormLineaTemporal.aspx` | CUN07 | Timeline vertical alternado con filtros |
| `FormInformeDerivacion.aspx` | CUN08+09 | Generación y auditoría del informe IA |
| `FormPerfilPaciente.aspx` | CUN10 | Selector de modelos + resultado IA |
| `FormExportarReporte.aspx` | CUN12 | Selector de documento + preview con blur |
| `FormSuscripcion.aspx` | CUN13 | Plan activo + comparativa + modal cancelación |
| `FormCambiarIdioma.aspx` | CUS03 | Grilla de idiomas con selección visual |
| `FormCambiarClave.aspx` | CUS04 | Cambio de contraseña con SHA-256 y checklist |

### Pantallas de administrador

| Archivo | Caso de uso | Descripción |
|---|---|---|
| `FormAuditoriaBitacora.aspx` | CUS05 | Bitácora filtrable con paginación y detalle |
| `FormBackupRestore.aspx` | CUS06 | Backup + Restore con confirmación multi-paso |
| `FormDigitoVerificador.aspx` | CUS07 | Verificación con 3 estados: inicial, OK, error |
| `FormGestionIdiomas.aspx` | CUS11 | ABM idiomas + edición inline de traducciones |

---

## 13. Paleta de íconos (emojis en sidebar)

| Sección | Emoji |
|---|---|
| Dashboard | 🏠 |
| Pacientes | 👤 |
| Consultas | 🗒️ |
| Historial Clínico | 📋 |
| Resumen IA | 🤖 |
| Línea Temporal | 📅 |
| Derivaciones | 📤 |
| Perfilación | 🧠 |
| Exportar | 💾 |
| Mi Suscripción | 💳 |
| Cerrar sesión | 🚪 |
| Bitácora | 📜 |
| Backup/Restore | 💾 |
| Dígito Verificador | 🔢 |
| Gestionar Idiomas | 🌐 |
| ABM Profesionales | 👤 |
| ABM Pacientes | 🧑‍⚕️ |
| ABM Consultas | 🗒️ |

---

## 14. Convenciones de nombrado

| Elemento | Convención | Ejemplo |
|---|---|---|
| Páginas | `Form` + PascalCase | `FormRegistroPaciente.aspx` |
| Labels | `lbl` + PascalCase | `lblNombreProfesional` |
| TextBox | `txt` + PascalCase | `txtFechaNacimiento` |
| DropDownList | `ddl` + PascalCase | `ddlEstadoCivil` |
| Button | `btn` + PascalCase | `btnRegistrar` |
| LinkButton | `lb` + PascalCase | `lbVerDetalle` |
| CheckBox | `chk` + PascalCase | `chkConsulta` |
| RadioButton | `rb` + PascalCase | `rbMasculino` |
| HiddenField | `hf` + PascalCase | `hfPlanSeleccionado` |
| GridView | `gv` + PascalCase | `gvPacientes` |
| Repeater | `rpt` + PascalCase | `rptTimeline` |
| Panel | `pnl` + PascalCase | `pnlResultadoOk` |
| Validators | `rfv/rev/cv` + Campo | `rfvNombre`, `revEmail` |
| ValidationGroup | `vg` + Nombre | `vgRegistro`, `vgLogin` |
| CSS clases | kebab-case | `.content-card`, `.btn-primary` |
| CSS variables | `--` kebab-case | `--bg-paper`, `--accent` |

---

## 15. Seguridad y datos clínicos

### Principios implementados
- **Contraseñas:** SHA-256 en `ComputarSHA256()` dentro del code-behind (nunca en JS)
- **Datos clínicos:** comentados con `// → BLL encripta con AES` en cada campo sensible
- **Sesión:** `Session["IdProfesional"]`, `Session["Profesional"]`, `Session["Email"]`
- **Bitácora:** `BLL.BitacoraBLL.Registrar(id, módulo, descripción, criticidad)` en cada operación importante
- **Dígito verificador:** recalcular tras cada operación de escritura
- **Bloqueo por intentos:** máximo 3 intentos → bloqueo de 10 minutos (implementado en `FormLogin`)
- **Ley 25.326:** mencionada en todos los formularios públicos y avisos clínicos

### Criticidades de bitácora
| Nivel | Descripción |
|---|---|
| 1 — Alta | Login, Logout, cambio de contraseña, backup, restore, bloqueo de cuenta |
| 2 — Media | Generación IA, derivaciones, cambio de plan, actualización de datos |
| 3 — Baja | Registro de paciente, consulta, historial, exportación, configuración |

---

## 16. Modo demo

Todas las pantallas arrancan con `DEMO_MODE` activado:
- El `Page_Load` con `!IsPostBack` pre-rellena los controles desde el code-behind
- Los métodos de demo están marcados con comentarios `// TODO: reemplazar por BLL.XXX`
- Para activar el backend real: reemplazar los métodos demo por llamadas a BLL y quitar los pre-rellenos
- La contraseña demo siempre es: `Demo2026@` (email: `lucia@consultorio.com`)
