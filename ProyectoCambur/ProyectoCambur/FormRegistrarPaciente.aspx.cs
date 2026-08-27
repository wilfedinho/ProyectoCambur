using System;
using System.Data;
using System.Web.UI.WebControls;

public partial class FormRegistrarPaciente : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarFormularioDemo();
            CargarTablaPacientesDemo();
        }
    }
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }
    private void CargarFormularioDemo()
    {
        txtNombre.Text = "Martín";
        txtApellido.Text = "González";
        txtFechaNacimiento.Text = "1990-06-15";
        txtOcupacion.Text = "Docente";
        ddlEstadoCivil.SelectedValue = "SOL";
        rbMasculino.Checked = true;
        txtEmail.Text = "martin.gonzalez@email.com";
        txtTelefono.Text = "11-2345-6789";
    }
    private void CargarTablaPacientesDemo()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("IdPaciente", typeof(int));
        dt.Columns.Add("NombreCompleto", typeof(string));
        dt.Columns.Add("Edad", typeof(int));
        dt.Columns.Add("EstadoCivil", typeof(string));
        dt.Columns.Add("FechaRegistro", typeof(DateTime));
        dt.Columns.Add("Activo", typeof(bool));

        dt.Rows.Add(1, "Martín González", 33, "Soltero/a", new DateTime(2025, 3, 10), true);
        dt.Rows.Add(2, "Sofía Ramírez", 28, "En pareja", new DateTime(2025, 5, 22), true);
        dt.Rows.Add(3, "Carlos Ibáñez", 45, "Casado/a", new DateTime(2025, 8, 1), true);
        dt.Rows.Add(4, "Valentina Moreno", 31, "Divorciada", new DateTime(2025, 11, 14), false);
        dt.Rows.Add(5, "Facundo Pérez", 27, "Soltero/a", new DateTime(2026, 1, 9), true);

        gvPacientes.DataSource = dt;
        gvPacientes.DataBind();
        int activos = 0;
        int inactivos = 0;
        foreach (DataRow row in dt.Rows)
        {
            if ((bool)row["Activo"]) activos++;
            else inactivos++;
        }

        lblBadgeActivos.Text = activos + " activos";
        lblBadgeActivos.Visible = true;

        lblBadgeInactivos.Text = inactivos + " inactivos";
        lblBadgeInactivos.Visible = true;
    }
    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        if (!rbMasculino.Checked && !rbFemenino.Checked && !rbNoEspecifica.Checked)
        {
            MostrarError("Seleccioná el sexo del paciente.");
            return;
        }

        string nombre = txtNombre.Text.Trim();
        string apellido = txtApellido.Text.Trim();
        string fechaStr = txtFechaNacimiento.Text;

        DateTime fechaNac;
        if (!DateTime.TryParse(fechaStr, out fechaNac))
        {
            MostrarError("La fecha de nacimiento ingresada no es válida.");
            return;
        }
        bool ok = SimularRegistroDemo(nombre);

        if (ok)
        {
            MostrarExito("Paciente \"" + nombre + " " + apellido + "\" registrado correctamente.");
            LimpiarFormulario();
            CargarTablaPacientesDemo();
        }
        else
        {
            MostrarError("Ya existe un paciente con esos datos en tu entorno clínico.");
        }
    }
    protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int idPaciente = Convert.ToInt32(e.CommandArgument);

        switch (e.CommandName)
        {
            case "Modificar":
            
                MostrarExito("Demo: Modificar paciente ID " + idPaciente + ". (Redirigir a FormModificarPaciente.aspx)");
                break;

            case "DarBaja":
            
                MostrarExito("Demo: Paciente ID " + idPaciente + " dado de baja correctamente.");
                CargarTablaPacientesDemo();
                break;

            case "Reactivar":
         
                MostrarExito("Demo: Paciente ID " + idPaciente + " reactivado correctamente.");
                CargarTablaPacientesDemo();
                break;
        }
    }
    private bool SimularRegistroDemo(string nombre)
    {
        if (nombre.ToLower() == "duplicado") return false;
        return true;
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

    private void LimpiarFormulario()
    {
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtFechaNacimiento.Text = string.Empty;
        txtOcupacion.Text = string.Empty;
        ddlEstadoCivil.SelectedIndex = 0;
        rbMasculino.Checked = false;
        rbFemenino.Checked = false;
        rbNoEspecifica.Checked = false;
        txtEmail.Text = string.Empty;
        txtTelefono.Text = string.Empty;
    }
}