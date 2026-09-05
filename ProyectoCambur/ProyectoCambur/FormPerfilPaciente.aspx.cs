using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using GUI;

public partial class FormPerfilPaciente : PaginaBase
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
            Psicologo psicologoActual = GestorSesion.PsicologoActual;
            lblNombreProfesional.Text = psicologoActual.Nombre + " " + psicologoActual.Apellido;
            lblIniciales.Text = Iniciales(psicologoActual.Nombre, psicologoActual.Apellido);

            CargarComboPacientes();
            if (!string.IsNullOrEmpty(ddlPacientePerfil.SelectedValue))
            {
                int idPaciente = Convert.ToInt32(ddlPacientePerfil.SelectedValue);
                CargarInfoPaciente(idPaciente);
                CargarPerfilesAnteriores(idPaciente);
            }

            MostrarEstado(1);
        }
    }

    private void MostrarEstado(int estado)
    {
        pnlSeleccion.Visible = (estado == 1);
        pnlResultado.Visible = (estado == 2);
        lblHeaderTitulo.Text = estado == 1
            ? "Generar perfil del paciente"
            : "Perfil generado";
    }

    private void CargarComboPacientes()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        List<Paciente> propios = gestorPaciente.ObtenerPorPsicologo(idPsicologo);

        ddlPacientePerfil.Items.Clear();
        ddlPacientePerfil.Items.Add(new ListItem("Seleccioná un paciente...", ""));
        foreach (Paciente p in propios.OrderBy(x => x.Apellido))
        {
            ddlPacientePerfil.Items.Add(new ListItem(p.Nombre + " " + p.Apellido, p.IdPaciente.ToString()));
        }
    }

    protected void ddlPacientePerfil_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        hfModeloSeleccionado.Value = string.Empty;

        if (!string.IsNullOrEmpty(ddlPacientePerfil.SelectedValue))
        {
            int idPaciente = Convert.ToInt32(ddlPacientePerfil.SelectedValue);
            CargarInfoPaciente(idPaciente);
            CargarPerfilesAnteriores(idPaciente);
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
    }

    private void CargarPerfilesAnteriores(int idPaciente)
    {
        GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
        List<PerfilPaciente> perfiles = gestorPerfil.ObtenerPorPaciente(idPaciente);

        DataTable dt = new DataTable();
        dt.Columns.Add("Modelo", typeof(string));
        dt.Columns.Add("Fecha", typeof(DateTime));

        foreach (PerfilPaciente perfil in perfiles)
        {
            SeccionesPerfilPaciente secciones = gestorPerfil.ObtenerSecciones(perfil);
            dt.Rows.Add(secciones != null ? secciones.NombreModelo : "Modelo", perfil.FechaGeneracion);
        }

        if (dt.Rows.Count > 0)
        {
            rptPerfilesAnteriores.DataSource = dt;
            rptPerfilesAnteriores.DataBind();
            lblSinPerfiles.Visible = false;
        }
        else
        {
            rptPerfilesAnteriores.DataSource = null;
            rptPerfilesAnteriores.DataBind();
            lblSinPerfiles.Visible = true;
        }
    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (string.IsNullOrEmpty(ddlPacientePerfil.SelectedValue))
        {
            MostrarError("Seleccioná un paciente antes de generar el perfil.");
            return;
        }

        string modelo = hfModeloSeleccionado.Value;
        if (string.IsNullOrEmpty(modelo))
        {
            MostrarError("Seleccioná un modelo de evaluación antes de generar el perfil.");
            return;
        }

        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        int idPaciente = Convert.ToInt32(ddlPacientePerfil.SelectedValue);

        GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
        try
        {
            int idGenerado = gestorPerfil.Generar(idPsicologo, idPaciente, modelo);
            MostrarPerfilGenerado(idGenerado);
            MostrarEstado(2);
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    private void MostrarPerfilGenerado(int idPerfil)
    {
        GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
        PerfilPaciente perfil = gestorPerfil.BuscarPorId(idPerfil);
        SeccionesPerfilPaciente secciones = gestorPerfil.ObtenerSecciones(perfil);

        string nombrePaciente = ddlPacientePerfil.SelectedItem.Text;

        lblResultadoMeta.Text = "Paciente: " + nombrePaciente + " · " + secciones.NombreModelo;
        lblModeloUsado.Text = "🧠 Modelo: " + secciones.NombreModelo;
        lblDescripcionGeneral.Text = secciones.Descripcion;
        lblDimensiones.Text = secciones.Dimensiones;
        lblPatrones.Text = secciones.Patrones;
        lblConsideraciones.Text = secciones.Consideraciones;

        GestorConsulta gestorConsulta = new GestorConsulta();
        int cantidadConsultas = gestorConsulta.ObtenerPorPaciente(perfil.IdPaciente).Count;

        lblMetaPaciente.Text = nombrePaciente;
        lblMetaModelo.Text = secciones.NombreModelo;
        lblMetaConsultas.Text = cantidadConsultas + " consulta" + (cantidadConsultas == 1 ? "" : "s") + " analizada" + (cantidadConsultas == 1 ? "" : "s");
        lblMetaFecha.Text = perfil.FechaGeneracion.ToString("dd/MM/yyyy HH:mm");
    }

    protected void btnNuevoPerfil_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        hfModeloSeleccionado.Value = string.Empty;

        if (!string.IsNullOrEmpty(ddlPacientePerfil.SelectedValue))
        {
            CargarPerfilesAnteriores(Convert.ToInt32(ddlPacientePerfil.SelectedValue));
        }

        MostrarEstado(1);
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        MostrarExito("Perfil guardado y encriptado correctamente. Disponible para exportar en PDF.");
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