using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class FormResumenIA : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarDropdownPacientes();
            CargarFiltrosFechaDemo();
            MostrarEstado(1); 
        }
    }
    private void MostrarEstado(int estado)
    {
        pnlFiltros.Visible = (estado == 1);
        pnlConsultas.Visible = (estado == 2);
        pnlResumen.Visible = (estado == 3);
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
        ddlPaciente.Items.Add(new ListItem("Martín González", "1"));
        ddlPaciente.Items.Add(new ListItem("Sofía Ramírez", "2"));
        ddlPaciente.Items.Add(new ListItem("Carlos Ibáñez", "3"));
        ddlPaciente.Items.Add(new ListItem("Valentina Moreno", "4"));
        ddlPaciente.Items.Add(new ListItem("Facundo Pérez", "5"));
        ddlPaciente.Items[1].Selected = true;
    }
    private void CargarFiltrosFechaDemo()
    {
        txtFechaDesde.Text = new DateTime(2026, 1, 1).ToString("yyyy-MM-dd");
        txtFechaHasta.Text = DateTime.Today.ToString("yyyy-MM-dd");
    }
    protected void btnBuscar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        int idPaciente = 0;
        if (!int.TryParse(ddlPaciente.SelectedValue, out idPaciente) || idPaciente == 0)
        {
            MostrarError("Seleccioná un paciente válido.");
            return;
        }

        DateTime desde, hasta;
        if (!DateTime.TryParse(txtFechaDesde.Text, out desde) ||
            !DateTime.TryParse(txtFechaHasta.Text, out hasta))
        {
            MostrarError("Las fechas ingresadas no son válidas.");
            return;
        }

        if (desde > hasta)
        {
            MostrarError("La fecha de inicio debe ser anterior a la fecha de fin.");
            return;
        }
        DataTable consultas = ObtenerConsultasDemo(idPaciente, desde, hasta);

        if (consultas.Rows.Count == 0)
        {
            MostrarError("No se encontraron consultas registradas para ese paciente en el período seleccionado.");
            return;
        }
        rptConsultas.DataSource = consultas;
        rptConsultas.DataBind();
        lblCantConsultas.Text = consultas.Rows.Count + " consultas";
        lblCantConsultas.Visible = true;
        lblRangoBusqueda.Text = desde.ToString("dd/MM/yyyy") + " al " + hasta.ToString("dd/MM/yyyy");

        MostrarEstado(2);
    }
    protected void btnVolver_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        MostrarEstado(1);
    }
    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        string nombrePaciente = ddlPaciente.SelectedItem.Text;
        string periodoTexto = txtFechaDesde.Text != string.Empty
            ? DateTime.Parse(txtFechaDesde.Text).ToString("dd/MM/yyyy") + " al " +
              DateTime.Parse(txtFechaHasta.Text).ToString("dd/MM/yyyy")
            : "período seleccionado";
        CargarResumenDemo(nombrePaciente, periodoTexto);
        MostrarEstado(3);
    }
    protected void btnNuevoResumen_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        CargarFiltrosFechaDemo();
        MostrarEstado(1);
    }
    protected void btnGuardarResumen_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        MostrarExito("Resumen guardado y encriptado correctamente. Disponible para exportar en PDF.");
    }
    private DataTable ObtenerConsultasDemo(int idPaciente, DateTime desde, DateTime hasta)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("IdConsulta", typeof(int));
        dt.Columns.Add("Fecha", typeof(DateTime));
        dt.Columns.Add("Duracion", typeof(int));
        dt.Columns.Add("Modalidad", typeof(string));
        dt.Columns.Add("ModalidadCss", typeof(string));
        dt.Columns.Add("ResumenObjetivos", typeof(string));
        var todasConsultas = new List<object[]>
        {
            new object[]{ 101, new DateTime(2026,4,15), 50, "Presencial", "presencial", "Regulación emocional ante conflicto laboral con supervisor." },
            new object[]{ 102, new DateTime(2026,3,28), 50, "Presencial", "presencial", "Trabajo sobre creencias nucleares de incompetencia." },
            new object[]{ 103, new DateTime(2026,3,10), 45, "Virtual",    "virtual",    "Psicoeducación sobre ansiedad y modelo TCC." },
            new object[]{ 104, new DateTime(2026,2,20), 50, "Presencial", "presencial", "Exploración de historia de apego con figura paterna." },
            new object[]{ 105, new DateTime(2026,1,30), 50, "Presencial", "presencial", "Registro de pensamientos automáticos negativos." },
            new object[]{ 106, new DateTime(2026,1,14), 40, "Telefónica", "telefonica", "Seguimiento breve post-crisis de ansiedad." },
        };

        foreach (var c in todasConsultas)
        {
            DateTime fechaConsulta = (DateTime)c[1];
            if (fechaConsulta >= desde && fechaConsulta <= hasta)
                dt.Rows.Add(c[0], c[1], c[2], c[3], c[4], c[5]);
        }

        return dt;
    }
    private void CargarResumenDemo(string nombrePaciente, string periodo)
    {
        lblResumenMeta.Text = "Paciente: " + nombrePaciente + " · " + periodo;
        lblMetaPaciente.Text = nombrePaciente;
        lblMetaPeriodo.Text = periodo;
        lblMetaConsultas.Text = "6 consultas analizadas";
        lblMetaFecha.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        lblContextoGeneral.Text =
            "Durante el período analizado, el paciente atravesó una etapa de alta carga emocional " +
            "vinculada principalmente a conflictos en su entorno laboral y a la activación de patrones " +
            "relacionales establecidos en la infancia. Las sesiones mostraron un trabajo consistente " +
            "en el marco de la Terapia Cognitivo Conductual, con foco en la identificación y " +
            "reestructuración de pensamientos automáticos negativos.";

        lblEvolucion.Text =
            "Se observa una progresión positiva en la capacidad del paciente para identificar " +
            "sus pensamientos automáticos sin actuar impulsivamente sobre ellos. En las primeras " +
            "sesiones del período predominaba la reactividad emocional; hacia el cierre del período " +
            "el paciente logra mayor distancia cognitiva ante situaciones de conflicto. " +
            "La tolerancia a la frustración mostró mejoras moderadas pero sostenidas.";

        lblTemasRecurrentes.Text =
            "• Conflicto con figuras de autoridad (supervisor laboral, padre).\n" +
            "• Creencias nucleares de incompetencia e insuficiencia.\n" +
            "• Dificultades en la regulación emocional ante críticas externas.\n" +
            "• Patrón de hipervigilancia en contextos evaluativos.\n" +
            "• Comparación negativa con pares en entornos laborales.";

        lblIntervenciones.Text =
            "• Reestructuración cognitiva sobre creencias de incompetencia (3 sesiones).\n" +
            "• Registro de pensamientos automáticos como tarea entre sesiones.\n" +
            "• Ejercicios de respiración diafragmática al inicio de cada sesión.\n" +
            "• Psicoeducación sobre el modelo ABC del pensamiento (sesión 3).\n" +
            "• Técnica de la flecha descendente para identificación de creencias nucleares.";

        lblObservaciones.Text =
            "El vínculo terapéutico se consolida de manera positiva. El paciente muestra " +
            "adherencia a las tareas entre sesiones y refiere percibir cambios concretos en " +
            "su vida cotidiana. Se sugiere continuar profundizando el trabajo sobre los esquemas " +
            "relacionales de apego en las próximas sesiones, dado que emergen como sustrato " +
            "de los conflictos actuales con figuras de autoridad.";
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
