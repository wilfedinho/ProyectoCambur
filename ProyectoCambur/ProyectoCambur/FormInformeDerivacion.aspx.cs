using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using GUI;

public partial class FormInformeDerivacion : PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
            lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");

            CargarComboPacientes();
            if (!string.IsNullOrEmpty(ddlPacienteDerivacion.SelectedValue))
            {
                CargarInfoPaciente(Convert.ToInt32(ddlPacienteDerivacion.SelectedValue));
            }

            MostrarEstado(1);
        }
    }
    private void MostrarEstado(int estado)
    {
        pnlFormulario.Visible = (estado == 1);
        pnlAuditoria.Visible = (estado == 2);
        lblHeaderTitulo.Text = estado == 1 ? "Generar informe" : "Auditoría del informe";

        ucSidebarNavegacion.PaginaActual = estado == 1 ? "acceder_informe_derivacion" : "acceder_auditoria_informe";
        ucSidebarNavegacion.RenderizarNavegacion();
    }

    private void CargarComboPacientes()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        List<Paciente> propios = gestorPaciente.ObtenerPorPsicologo(idPsicologo);

        ddlPacienteDerivacion.Items.Clear();
        ddlPacienteDerivacion.Items.Add(new ListItem("Seleccioná un paciente...", ""));
        foreach (Paciente p in propios.OrderBy(x => x.Apellido))
        {
            ddlPacienteDerivacion.Items.Add(new ListItem(p.Nombre + " " + p.Apellido, p.IdPaciente.ToString()));
        }
    }

    protected void ddlPacienteDerivacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        if (!string.IsNullOrEmpty(ddlPacienteDerivacion.SelectedValue))
        {
            CargarInfoPaciente(Convert.ToInt32(ddlPacienteDerivacion.SelectedValue));
        }
    }

    private void CargarInfoPaciente(int idPaciente)
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
        if (paciente == null) return;

        GestorConsulta gestorConsulta = new GestorConsulta();
        int cantidadConsultas = gestorConsulta.ObtenerPorPaciente(idPaciente).Count;

        lblPacienteIniciales.Text = Iniciales(paciente.Nombre, paciente.Apellido);
        lblPacienteNombre.Text = paciente.Nombre + " " + paciente.Apellido;
        lblPacienteEdad.Text = CalcularEdad(paciente.FechaNacimiento) + " años";
        lblPacienteConsultas.Text = cantidadConsultas + " consulta" + (cantidadConsultas == 1 ? "" : "s") + " registrada" + (cantidadConsultas == 1 ? "" : "s");

        GestorHistorialClinico gestorHistorial = new GestorHistorialClinico();
        bool tieneHistorial = gestorHistorial.BuscarPorPaciente(idPaciente) != null;

        lblAvisoIA.Text = (cantidadConsultas > 0 ? cantidadConsultas + " consulta" + (cantidadConsultas == 1 ? "" : "s") + " registrada" + (cantidadConsultas == 1 ? "" : "s") : "Sin consultas registradas") +
                           (tieneHistorial ? " · Historial clínico completo" : " · Sin historial clínico") +
                           " · Evolución observada";
        lblInfoConsultas.Text = cantidadConsultas + " consulta" + (cantidadConsultas == 1 ? "" : "s") + " registrada" + (cantidadConsultas == 1 ? "" : "s");
    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idPaciente = Convert.ToInt32(ddlPacienteDerivacion.SelectedValue);
        string especialidad = ddlEspecialidad.SelectedItem.Text;
        string profDestino = txtProfDestino.Text.Trim();
        string institucion = txtInstitucion.Text.Trim();
        string motivo = txtMotivo.Text.Trim();

        GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
        try
        {
            int idGenerado = gestorInforme.Generar(idPsicologo, idPaciente, especialidad, profDestino, institucion, motivo);
            hdnIdInforme.Value = idGenerado.ToString();
            MostrarInformeGenerado(idGenerado);
            MostrarEstado(2);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    private void MostrarInformeGenerado(int idInforme)
    {
        GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
        InformeDerivacion informe = gestorInforme.BuscarPorId(idInforme);
        SeccionesInformeDerivacion secciones = gestorInforme.ObtenerSecciones(informe);

        txtSintesisDiagnostica.Text = secciones.SintesisDiagnostica;
        txtAndamiajes.Text = secciones.Andamiajes;
        txtObjetivos.Text = secciones.Objetivos;
        txtModalidadTrabajo.Text = secciones.ModalidadTrabajo;
        txtMotivoDerivacion.Text = secciones.MotivoDerivacion;
        txtFirma.Text = string.Empty;

        lblMetaPaciente.Text = ddlPacienteDerivacion.SelectedItem.Text;
        lblMetaEspecialidad.Text = secciones.EspecialidadDerivacion;
        lblMetaDestino.Text = secciones.ProfesionalDestinatario + (!string.IsNullOrEmpty(secciones.Institucion) ? " — " + secciones.Institucion : "");
        lblMetaFecha.Text = informe.FechaGeneracion.ToString("dd/MM/yyyy HH:mm");

        lblAuditoriaMeta.Text = "Derivación a " + secciones.EspecialidadDerivacion + " · " + secciones.ProfesionalDestinatario;
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

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idInforme = Convert.ToInt32(hdnIdInforme.Value);

        GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
        try
        {
            gestorInforme.Auditar(idPsicologo, idInforme,
                txtSintesisDiagnostica.Text.Trim(), txtAndamiajes.Text.Trim(), txtObjetivos.Text.Trim(),
                txtModalidadTrabajo.Text.Trim(), txtMotivoDerivacion.Text.Trim(), txtFirma.Text.Trim());

            MostrarExito("Informe validado y firmado por " + txtFirma.Text.Trim() +
                         ". El documento está disponible para exportar en PDF.");
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void btnGuardarBorrador_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idInforme = Convert.ToInt32(hdnIdInforme.Value);

        GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
        try
        {
            gestorInforme.GuardarBorrador(idPsicologo, idInforme,
                txtSintesisDiagnostica.Text.Trim(), txtAndamiajes.Text.Trim(), txtObjetivos.Text.Trim(),
                txtModalidadTrabajo.Text.Trim(), txtMotivoDerivacion.Text.Trim());

            MostrarExito("Borrador guardado. Podés continuar la revisión más tarde desde la sección Derivaciones.");
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void btnDescartar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idInforme = Convert.ToInt32(hdnIdInforme.Value);

        GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
        try
        {
            gestorInforme.Descartar(idPsicologo, idInforme);
            LimpiarFormulario();
            MostrarEstado(1);
            MostrarExito("El informe fue descartado correctamente.");
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    private void LimpiarFormulario()
    {
        ddlEspecialidad.SelectedIndex = 0;
        txtProfDestino.Text = string.Empty;
        txtInstitucion.Text = string.Empty;
        txtMotivo.Text = string.Empty;
        hdnIdInforme.Value = string.Empty;
    }

    private string Iniciales(string nombre, string apellido)
    {
        string i1 = !string.IsNullOrWhiteSpace(nombre) ? nombre.Trim().Substring(0, 1).ToUpper() : "";
        string i2 = !string.IsNullOrWhiteSpace(apellido) ? apellido.Trim().Substring(0, 1).ToUpper() : "";
        return i1 + i2;
    }

    private int CalcularEdad(DateTime fechaNacimiento)
    {
        int edad = DateTime.Today.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;
        return edad;
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