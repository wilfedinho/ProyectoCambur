using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormModificarConsulta : System.Web.UI.Page
{
    // =========================================================
    // CONSTANTE: plazo máximo de modificación en días (CUN06)
    // =========================================================
    private const int PLAZO_MAX_DIAS = 3;

    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarConsultaDemo();
        }
    }

    // =========================================================
    // PROFESIONAL LOGUEADO (demo)
    // TODO: reemplazar por Session["Profesional"]
    // =========================================================
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }

    // =========================================================
    // CARGA DE LA CONSULTA A MODIFICAR (demo)
    // En producción: leer id de Request.QueryString["id"]
    // TODO: reemplazar por:
    //   int idConsulta = Convert.ToInt32(Request.QueryString["id"]);
    //   BE.Consulta c  = BLL.ConsultaBLL.ObtenerPorId(idConsulta);
    //   Validar que c.IdProfesional == (int)Session["IdProfesional"]
    //   Los campos vienen desencriptados desde la BLL (AES decrypt)
    // =========================================================
    private void CargarConsultaDemo()
    {
        // -- Datos de solo lectura de la consulta --
        string nombrePaciente = "Martín González";
        string inicialesPac = "MG";
        DateTime fechaConsulta = DateTime.Today.AddDays(-1); // ayer → dentro del plazo
        int duracion = 50;
        string modalidad = "Presencial";
        DateTime fechaCreacion = fechaConsulta;
        DateTime? ultimaMod = null; // nunca modificada

        // -- Calcular días restantes para editar --
        DateTime fechaLimite = fechaCreacion.AddDays(PLAZO_MAX_DIAS);
        int diasRestantes = (int)(fechaLimite - DateTime.Today).TotalDays;
        bool dentroDelPlazo = diasRestantes > 0;

        if (!dentroDelPlazo)
        {
            // Mostrar panel bloqueado
            pnlBloqueado.Visible = true;
            pnlFormulario.Visible = false;
            lblMensajeBloqueado.Text =
                "La consulta del " + fechaConsulta.ToString("dd/MM/yyyy") +
                " ya no puede editarse. El plazo de " + PLAZO_MAX_DIAS +
                " días desde su creación venció el " + fechaLimite.ToString("dd/MM/yyyy") + ".";
            return;
        }

        // Mostrar formulario editable
        pnlBloqueado.Visible = false;
        pnlFormulario.Visible = true;

        // -- Header de solo lectura --
        lblPacienteIniciales.Text = inicialesPac;
        lblPacienteNombre.Text = nombrePaciente;
        lblFechaConsulta.Text = fechaConsulta.ToString("dd/MM/yyyy");
        lblDuracionConsulta.Text = duracion + " minutos";
        lblModalidadConsulta.Text = modalidad;

        // -- Badge de plazo --
        if (diasRestantes == 1)
        {
            lblBadgePlazo.Text = "⚠️ Último día para editar";
            lblBadgePlazo.CssClass = "badge-plazo-urgente";
        }
        else
        {
            lblBadgePlazo.Text = "✓ Editable hasta el " + fechaLimite.ToString("dd/MM/yyyy");
            lblBadgePlazo.CssClass = "badge-plazo-ok";
        }

        // -- Card de plazo (columna derecha) --
        lblDiasRestantes.Text = diasRestantes.ToString();
        lblFechaLimite.Text = "Límite: " + fechaLimite.ToString("dddd dd/MM/yyyy");

        // Porcentaje de tiempo consumido para la barra
        double pctConsumido = Math.Round(
            ((double)(PLAZO_MAX_DIAS - diasRestantes) / PLAZO_MAX_DIAS) * 100, 0);
        double pctRestante = 100 - pctConsumido;

        // Se inyecta el width del fill vía style inline
        // (el Label tiene CssClass="plazo-fill", y el style se agrega aquí)
        lblPlazoFill.Style["width"] = pctRestante + "%";
        lblPlazoFill.Style["background"] = diasRestantes == 1 ? "#F4A261" : "#2A9D8F";

        // -- Historial de cambios --
        lblFechaCreacion.Text = fechaCreacion.ToString("dd/MM/yyyy HH:mm");
        lblUltimaModificacion.Text = ultimaMod.HasValue
            ? ultimaMod.Value.ToString("dd/MM/yyyy HH:mm")
            : "Sin modificaciones previas";

        // -- Contenido clínico (demo: datos desencriptados) --
        txtObjetivos.Text = "Trabajar la regulación emocional ante situaciones de conflicto laboral.";
        txtObservaciones.Text = "El paciente llega visiblemente ansioso. Refiere una semana de alta tensión en el trabajo por un conflicto con su supervisor. Se observa hipervigilancia y dificultad para concentrarse durante la sesión.";
        txtHipotesis.Text = "Posible activación de esquema de incompetencia frente a figuras de autoridad.";
        txtIntervenciones.Text = "Técnica de reestructuración cognitiva sobre la interpretación del conflicto. Ejercicio de respiración diafragmática al inicio de la sesión.";
        txtEvolucion.Text = "El paciente logra identificar el pensamiento automático asociado. Al cierre refiere sentirse más tranquilo.";
        txtDiagnostico.Text = "Episodio de ansiedad situacional con activación de creencias nucleares de incompetencia.";
        txtTratamiento.Text = "Continuar con TCC. Proponer registro de pensamientos automáticos para la próxima semana.";
    }

    // =========================================================
    // EVENTO BOTÓN GUARDAR CAMBIOS
    // =========================================================
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        // Validar que siga dentro del plazo (doble chequeo server-side)
        // TODO: en producción:
        //   int idConsulta       = Convert.ToInt32(Request.QueryString["id"]);
        //   BE.Consulta original = BLL.ConsultaBLL.ObtenerPorId(idConsulta);
        //   if ((DateTime.Today - original.FechaCreacion).TotalDays >= PLAZO_MAX_DIAS)
        //   { MostrarError("El plazo de edición venció."); return; }
        //
        //   Verificar que pertenece al profesional logueado:
        //   if (original.IdProfesional != (int)Session["IdProfesional"])
        //   { MostrarError("No tenés permisos para modificar esta consulta."); return; }

        // Validar que al menos un campo tenga contenido
        bool alguienCompleto =
            !string.IsNullOrWhiteSpace(txtObjetivos.Text) ||
            !string.IsNullOrWhiteSpace(txtObservaciones.Text) ||
            !string.IsNullOrWhiteSpace(txtHipotesis.Text) ||
            !string.IsNullOrWhiteSpace(txtIntervenciones.Text) ||
            !string.IsNullOrWhiteSpace(txtEvolucion.Text) ||
            !string.IsNullOrWhiteSpace(txtDiagnostico.Text) ||
            !string.IsNullOrWhiteSpace(txtTratamiento.Text);

        if (!alguienCompleto)
        {
            MostrarError("Completá al menos un campo antes de guardar los cambios.");
            return;
        }

        // TODO: reemplazar por:
        //   int idConsulta        = Convert.ToInt32(Request.QueryString["id"]);
        //   BE.Consulta c         = BLL.ConsultaBLL.ObtenerPorId(idConsulta);
        //   c.Objetivos           = txtObjetivos.Text.Trim();       // BLL re-encripta con AES
        //   c.Observaciones       = txtObservaciones.Text.Trim();   // BLL re-encripta con AES
        //   c.Hipotesis           = txtHipotesis.Text.Trim();       // BLL re-encripta con AES
        //   c.Intervenciones      = txtIntervenciones.Text.Trim();  // BLL re-encripta con AES
        //   c.Evolucion           = txtEvolucion.Text.Trim();       // BLL re-encripta con AES
        //   c.Diagnostico         = txtDiagnostico.Text.Trim();     // BLL re-encripta con AES
        //   c.Tratamiento         = txtTratamiento.Text.Trim();     // BLL re-encripta con AES
        //   c.FechaUltimaModif    = DateTime.Now;
        //   bool ok = BLL.ConsultaBLL.Modificar(c);
        //   if (ok) { MostrarExito("Consulta actualizada correctamente."); }
        //   else      MostrarError("No fue posible guardar los cambios.");

        // DEMO: simular guardado exitoso
        lblUltimaModificacion.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        MostrarExito("Consulta actualizada correctamente. Los cambios fueron re-encriptados.");
    }

    // =========================================================
    // HELPERS
    // =========================================================
    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }

    private void MostrarExito(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-success";
        lblMensaje.Visible = true;
    }
}
