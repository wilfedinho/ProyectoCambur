using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

public partial class FormRealizarConsulta : System.Web.UI.Page
{
    private class PacienteDemo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Iniciales { get; set; }
        public int Edad { get; set; }
        public string Ocupacion { get; set; }
        public string EstadoCivil { get; set; }
        public int Consultas { get; set; }
        public string UltimaConsulta { get; set; }
    }

    private List<PacienteDemo> ObtenerPacientesDemo()
    {
        return new List<PacienteDemo>
        {
            new PacienteDemo { Id=1, Nombre="Martín González",   Iniciales="MG", Edad=33, Ocupacion="Docente",      EstadoCivil="Soltero/a",  Consultas=12, UltimaConsulta="15/04/2026" },
            new PacienteDemo { Id=2, Nombre="Sofía Ramírez",     Iniciales="SR", Edad=28, Ocupacion="Diseñadora",   EstadoCivil="En pareja",  Consultas=7,  UltimaConsulta="02/05/2026" },
            new PacienteDemo { Id=3, Nombre="Carlos Ibáñez",     Iniciales="CI", Edad=45, Ocupacion="Contador",     EstadoCivil="Casado/a",   Consultas=20, UltimaConsulta="08/05/2026" },
            new PacienteDemo { Id=4, Nombre="Valentina Moreno",  Iniciales="VM", Edad=31, Ocupacion="Enfermera",    EstadoCivil="Divorciada", Consultas=5,  UltimaConsulta="10/03/2026" },
            new PacienteDemo { Id=5, Nombre="Facundo Pérez",     Iniciales="FP", Edad=27, Ocupacion="Estudiante",   EstadoCivil="Soltero/a",  Consultas=3,  UltimaConsulta="28/04/2026" },
        };
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarDropdownPacientes();
            CargarFormularioDemo();
            ActualizarCardPaciente(1); 
            CargarUltimasConsultasDemo(1);
        }
    }
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }
    private void CargarDropdownPacientes()
    {
        ddlPaciente.Items.Clear();
        ddlPaciente.Items.Add(new ListItem("Seleccioná un paciente...", ""));

        foreach (var p in ObtenerPacientesDemo())
            ddlPaciente.Items.Add(new ListItem(p.Nombre, p.Id.ToString()));
        if (ddlPaciente.Items.Count > 1)
            ddlPaciente.Items[1].Selected = true;
    }
    private void CargarFormularioDemo()
    {
        txtFechaConsulta.Text = DateTime.Today.ToString("yyyy-MM-dd");
        txtDuracion.Text = "50";
        ddlModalidad.SelectedValue = "PRE";
        txtObjetivos.Text = "Trabajar la regulación emocional ante situaciones de conflicto laboral.";
        txtObservaciones.Text = "El paciente llega visiblemente ansioso. Refiere una semana de alta tensión en el trabajo por un conflicto con su supervisor. Se observa hipervigilancia y dificultad para concentrarse durante la sesión.";
        txtHipotesis.Text = "Posible activación de esquema de incompetencia frente a figuras de autoridad.";
        txtIntervenciones.Text = "Técnica de reestructuración cognitiva sobre la interpretación del conflicto. Ejercicio de respiración diafragmática al inicio de la sesión.";
        txtEvolucion.Text = "El paciente logra identificar el pensamiento automático asociado. Al cierre refiere sentirse más tranquilo.";
        txtDiagnostico.Text = "Episodio de ansiedad situacional con activación de creencias nucleares de incompetencia.";
        txtTratamiento.Text = "Continuar con TCC. Proponer registro de pensamientos automáticos para la próxima semana.";
    }
    private void ActualizarCardPaciente(int idPaciente)
    {
        var pacientes = ObtenerPacientesDemo();
        var p = pacientes.Find(x => x.Id == idPaciente);

        if (p == null)
        {
            lblPacienteNombre.Text = "Seleccioná un paciente";
            lblPacienteIniciales.Text = "--";
            lblPacienteEdad.Text = "";
            lblPacienteOcupacion.Text = "";
            lblPacienteEstado.Text = "";
            lblTotalConsultas.Text = "--";
            lblUltimaConsulta.Text = "--";
            return;
        }

        lblPacienteIniciales.Text = p.Iniciales;
        lblPacienteNombre.Text = p.Nombre;
        lblPacienteEdad.Text = p.Edad + " años";
        lblPacienteOcupacion.Text = p.Ocupacion;
        lblPacienteEstado.Text = p.EstadoCivil;
        lblTotalConsultas.Text = p.Consultas.ToString();
        lblUltimaConsulta.Text = p.UltimaConsulta;
    }
    private void CargarUltimasConsultasDemo(int idPaciente)
    {
        var consultas = new DataTable();
        consultas.Columns.Add("Fecha", typeof(DateTime));
        consultas.Columns.Add("Resumen", typeof(string));

        if (idPaciente == 1)
        {
            consultas.Rows.Add(new DateTime(2026, 4, 15), "Regulación emocional ante conflicto laboral.");
            consultas.Rows.Add(new DateTime(2026, 3, 28), "Trabajo sobre creencias de incompetencia.");
            consultas.Rows.Add(new DateTime(2026, 3, 10), "Psicoeducación sobre ansiedad y TCC.");
        }
        else if (idPaciente == 2)
        {
            consultas.Rows.Add(new DateTime(2026, 5, 2), "Exploración de dinámicas relacionales.");
            consultas.Rows.Add(new DateTime(2026, 4, 14), "Trabajo sobre apego ansioso.");
        }
        else if (idPaciente == 3)
        {
            consultas.Rows.Add(new DateTime(2026, 5, 8), "Cierre de ciclo de duelo.");
            consultas.Rows.Add(new DateTime(2026, 4, 22), "Reestructuración cognitiva.");
            consultas.Rows.Add(new DateTime(2026, 4, 7), "Manejo del estrés crónico.");
        }

        if (consultas.Rows.Count > 0)
        {
            rptUltimasConsultas.DataSource = consultas;
            rptUltimasConsultas.DataBind();
            lblSinConsultas.Visible = false;
        }
        else
        {
            rptUltimasConsultas.DataSource = null;
            rptUltimasConsultas.DataBind();
            lblSinConsultas.Visible = true;
        }
    }
    protected void ddlPaciente_SelectedIndexChanged(object sender, EventArgs e)
    {
        int idPaciente = 0;
        if (int.TryParse(ddlPaciente.SelectedValue, out idPaciente) && idPaciente > 0)
        {
            ActualizarCardPaciente(idPaciente);
            CargarUltimasConsultasDemo(idPaciente);
        }
        else
        {
            ActualizarCardPaciente(0);
        }
    }
    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;
        int idPaciente = 0;
        if (!int.TryParse(ddlPaciente.SelectedValue, out idPaciente) || idPaciente == 0)
        {
            MostrarError("Debés seleccionar un paciente para registrar la consulta.");
            return;
        }
        DateTime fechaConsulta;
        if (!DateTime.TryParse(txtFechaConsulta.Text, out fechaConsulta))
        {
            MostrarError("La fecha ingresada no es válida.");
            return;
        }
        int duracion;
        if (!int.TryParse(txtDuracion.Text, out duracion) || duracion <= 0)
        {
            MostrarError("La duración debe ser un número positivo de minutos.");
            return;
        }
        string modalidad = ddlModalidad.SelectedValue;
        string objetivos = txtObjetivos.Text.Trim();
        string observaciones = txtObservaciones.Text.Trim();
        string hipotesis = txtHipotesis.Text.Trim();
        string intervenciones = txtIntervenciones.Text.Trim();
        string evolucion = txtEvolucion.Text.Trim();
        string diagnostico = txtDiagnostico.Text.Trim();
        string tratamiento = txtTratamiento.Text.Trim();
        string nombrePaciente = ddlPaciente.SelectedItem.Text;
        MostrarExito("Consulta del " + fechaConsulta.ToString("dd/MM/yyyy")
            + " registrada correctamente para " + nombrePaciente + ". Los datos fueron encriptados.");
        LimpiarContenidoClinico();
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
    private void LimpiarContenidoClinico()
    {
        txtObjetivos.Text = string.Empty;
        txtObservaciones.Text = string.Empty;
        txtHipotesis.Text = string.Empty;
        txtIntervenciones.Text = string.Empty;
        txtEvolucion.Text = string.Empty;
        txtDiagnostico.Text = string.Empty;
        txtTratamiento.Text = string.Empty;
    }
}