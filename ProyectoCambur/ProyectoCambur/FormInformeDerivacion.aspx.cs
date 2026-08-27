using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class FormInformeDerivacion : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarPacienteDemo();
            MostrarEstado(1);
        }
    }

    private void MostrarEstado(int estado)
    {
        pnlFormulario.Visible = (estado == 1);
        pnlAuditoria.Visible = (estado == 2);
        lblHeaderTitulo.Text = estado == 1 ? "Generar informe" : "Auditoría del informe";
    }

    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }


    private void CargarPacienteDemo()
    {
        lblPacienteIniciales.Text = "MG";
        lblPacienteNombre.Text = "Martín González";
        lblPacienteEdad.Text = "33 años";
        lblPacienteConsultas.Text = "12 consultas registradas";


        ddlEspecialidad.SelectedValue = "PSI";
        txtProfDestino.Text = "Dr. Hernán Acosta";
        txtInstitucion.Text = "Centro de Salud Mental Belgrano";
        txtMotivo.Text = "Se solicita interconsulta psiquiátrica para evaluación de posible medicación " +
                                        "complementaria al tratamiento psicoterapéutico en curso, dado el nivel de " +
                                        "activación ansiosa persistente que no cede completamente con TCC.";

        lblAvisoIA.Text = "Últimas 12 consultas · Historial clínico completo · Evolución observada";
        lblInfoConsultas.Text = "Últimas 12 consultas registradas";
    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        string especialidad = ddlEspecialidad.SelectedItem.Text;
        string profDestino = txtProfDestino.Text.Trim();
        string institucion = txtInstitucion.Text.Trim();
        string motivo = txtMotivo.Text.Trim();
        CargarInformeDemo(especialidad, profDestino, motivo);

        lblMetaPaciente.Text = "Martín González";
        lblMetaEspecialidad.Text = especialidad;
        lblMetaDestino.Text = profDestino + (institucion != "" ? " — " + institucion : "");
        lblMetaFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

   
        lblAuditoriaMeta.Text = "Derivación a " + especialidad + " · " + profDestino;

        MostrarEstado(2);
    }

    private void CargarInformeDemo(string especialidad, string profDestino, string motivo)
    {
        txtSintesisDiagnostica.Text =
            "El paciente presenta un cuadro de ansiedad generalizada con activación de esquemas " +
            "cognitivos de incompetencia e insuficiencia, asociados a conflictos relacionales con " +
            "figuras de autoridad. Se observan patrones de hipervigilancia en entornos evaluativos " +
            "y dificultades moderadas en la regulación emocional ante situaciones de conflicto laboral. " +
            "El diagnóstico de trabajo se enmarca dentro del espectro ansioso con componentes de " +
            "personalidad de tipo dependiente.";

        txtAndamiajes.Text =
            "• Terapia Cognitivo Conductual (TCC) como marco principal de intervención.\n" +
            "• Reestructuración cognitiva aplicada a creencias nucleares de incompetencia.\n" +
            "• Técnica de la flecha descendente para identificación de esquemas.\n" +
            "• Registro de pensamientos automáticos como tarea intersesión.\n" +
            "• Técnicas de regulación emocional: respiración diafragmática y mindfulness básico.\n" +
            "• Psicoeducación sobre el modelo ABC del pensamiento (primeras sesiones).";

        txtObjetivos.Text =
            "• Reducir el nivel de activación ansiosa basal del paciente.\n" +
            "• Fortalecer la tolerancia a la frustración ante situaciones evaluativas.\n" +
            "• Trabajar la autonomía emocional respecto de figuras de autoridad.\n" +
            "• Consolidar estrategias de regulación emocional ante conflictos relacionales.\n" +
            "• Evaluar necesidad de soporte farmacológico complementario.";

        txtModalidadTrabajo.Text =
            "Psicoterapia individual de frecuencia semanal, modalidad presencial, " +
            "con sesiones de 50 minutos. Se incorporó una sesión de seguimiento telefónico " +
            "ante episodio de crisis aguda durante el tratamiento.";

        txtMotivoDerivacion.Text = motivo;
    }

    protected void btnValidar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        if (string.IsNullOrWhiteSpace(txtSintesisDiagnostica.Text) &&
            string.IsNullOrWhiteSpace(txtAndamiajes.Text))
        {
            MostrarError("El informe no puede estar vacío. Completá al menos la síntesis diagnóstica.");
            return;
        }

        MostrarExito("Informe validado y firmado por " + txtFirma.Text.Trim() +
                     ". El documento está disponible para exportar en PDF.");
    }

    protected void btnGuardarBorrador_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        MostrarExito("Borrador guardado. Podés continuar la revisión más tarde desde la sección Derivaciones.");
    }

    protected void btnDescartar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        LimpiarFormulario();
        MostrarEstado(1);
        MostrarExito("El informe fue descartado correctamente.");
    }

    private void LimpiarFormulario()
    {
        ddlEspecialidad.SelectedIndex = 0;
        txtProfDestino.Text = string.Empty;
        txtInstitucion.Text = string.Empty;
        txtMotivo.Text = string.Empty;
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
