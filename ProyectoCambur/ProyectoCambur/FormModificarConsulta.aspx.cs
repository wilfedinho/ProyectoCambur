using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormModificarConsulta : System.Web.UI.Page
{
    private const int PLAZO_MAX_DIAS = 3;
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarConsultaDemo();
        }
    }
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }
    private void CargarConsultaDemo()
    {
   
        string nombrePaciente = "Martín González";
        string inicialesPac = "MG";
        DateTime fechaConsulta = DateTime.Today.AddDays(-1); 
        int duracion = 50;
        string modalidad = "Presencial";
        DateTime fechaCreacion = fechaConsulta;
        DateTime? ultimaMod = null; 

        DateTime fechaLimite = fechaCreacion.AddDays(PLAZO_MAX_DIAS);
        int diasRestantes = (int)(fechaLimite - DateTime.Today).TotalDays;
        bool dentroDelPlazo = diasRestantes > 0;

        if (!dentroDelPlazo)
        {

            pnlBloqueado.Visible = true;
            pnlFormulario.Visible = false;
            lblMensajeBloqueado.Text =
                "La consulta del " + fechaConsulta.ToString("dd/MM/yyyy") +
                " ya no puede editarse. El plazo de " + PLAZO_MAX_DIAS +
                " días desde su creación venció el " + fechaLimite.ToString("dd/MM/yyyy") + ".";
            return;
        }
        pnlBloqueado.Visible = false;
        pnlFormulario.Visible = true;

        lblPacienteIniciales.Text = inicialesPac;
        lblPacienteNombre.Text = nombrePaciente;
        lblFechaConsulta.Text = fechaConsulta.ToString("dd/MM/yyyy");
        lblDuracionConsulta.Text = duracion + " minutos";
        lblModalidadConsulta.Text = modalidad;
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
        lblDiasRestantes.Text = diasRestantes.ToString();
        lblFechaLimite.Text = "Límite: " + fechaLimite.ToString("dddd dd/MM/yyyy");
        double pctConsumido = Math.Round(
            ((double)(PLAZO_MAX_DIAS - diasRestantes) / PLAZO_MAX_DIAS) * 100, 0);
        double pctRestante = 100 - pctConsumido;
        lblPlazoFill.Style["width"] = pctRestante + "%";
        lblPlazoFill.Style["background"] = diasRestantes == 1 ? "#F4A261" : "#2A9D8F";
        lblFechaCreacion.Text = fechaCreacion.ToString("dd/MM/yyyy HH:mm");
        lblUltimaModificacion.Text = ultimaMod.HasValue
            ? ultimaMod.Value.ToString("dd/MM/yyyy HH:mm")
            : "Sin modificaciones previas";
        txtObjetivos.Text = "Trabajar la regulación emocional ante situaciones de conflicto laboral.";
        txtObservaciones.Text = "El paciente llega visiblemente ansioso. Refiere una semana de alta tensión en el trabajo por un conflicto con su supervisor. Se observa hipervigilancia y dificultad para concentrarse durante la sesión.";
        txtHipotesis.Text = "Posible activación de esquema de incompetencia frente a figuras de autoridad.";
        txtIntervenciones.Text = "Técnica de reestructuración cognitiva sobre la interpretación del conflicto. Ejercicio de respiración diafragmática al inicio de la sesión.";
        txtEvolucion.Text = "El paciente logra identificar el pensamiento automático asociado. Al cierre refiere sentirse más tranquilo.";
        txtDiagnostico.Text = "Episodio de ansiedad situacional con activación de creencias nucleares de incompetencia.";
        txtTratamiento.Text = "Continuar con TCC. Proponer registro de pensamientos automáticos para la próxima semana.";
    }
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
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
        lblUltimaModificacion.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        MostrarExito("Consulta actualizada correctamente. Los cambios fueron re-encriptados.");
    }
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
