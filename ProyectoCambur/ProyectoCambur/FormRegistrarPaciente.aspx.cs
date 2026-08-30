using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using GUI;

public partial class FormRegistrarPaciente : PaginaBase
{
    private class FilaPaciente
    {
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public int Edad { get; set; }
        public string EstadoCivil { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = System.Web.UI.UnobtrusiveValidationMode.None;

        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        GestorPermiso gestorPermiso = new GestorPermiso();
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_registrar_paciente"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
            ModoAlta();
            CargarGrilla();
        }
    }

    private void AplicarTraducciones()
    {
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("seccion_gestion_clinica");
        lblHeaderPagina.Text = Traducir("nav_registrar_paciente");

        lblEtiquetaNombre.Text = Traducir("lbl_nombre");
        rfvNombre.ErrorMessage = Traducir("error_nombre_obligatorio");
        lblEtiquetaApellido.Text = Traducir("lbl_apellido");
        rfvApellido.ErrorMessage = Traducir("error_apellido_obligatorio");
        lblEtiquetaDni.Text = Traducir("lbl_dni");
        rfvDni.ErrorMessage = Traducir("error_dni_obligatorio");
        revDni.ErrorMessage = Traducir("error_formato_dni");
        lblEtiquetaFechaNacimiento.Text = Traducir("lbl_fecha_nacimiento");
        rfvFecha.ErrorMessage = Traducir("error_fecha_nacimiento_obligatoria");
        lblEtiquetaOcupacion.Text = Traducir("lbl_ocupacion");
        lblEtiquetaEstadoCivil.Text = Traducir("lbl_estado_civil");
        rfvEstado.ErrorMessage = Traducir("error_estado_civil_obligatorio");
        lblEtiquetaSexo.Text = Traducir("lbl_sexo");
        lblEtiquetaEmail.Text = Traducir("lbl_correo");
        revEmail.ErrorMessage = Traducir("error_formato_email");
        lblEtiquetaTelefono.Text = Traducir("lbl_telefono");

        lblTituloListado.Text = Traducir("titulo_pacientes_registrados");
        gvPacientes.EmptyDataText = Traducir("empty_pacientes");
    }

    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        if (!rbMasculino.Checked && !rbFemenino.Checked && !rbNoEspecifica.Checked)
        {
            MostrarError(Traducir("error_sexo_obligatorio"));
            return;
        }

        DateTime fechaNacimiento;
        if (!DateTime.TryParse(txtFechaNacimiento.Text, out fechaNacimiento))
        {
            MostrarError(Traducir("error_fecha_nacimiento_obligatoria"));
            return;
        }
        string sexo = rbMasculino.Checked ? "Masculino" : (rbFemenino.Checked ? "Femenino" : "Otro");

        Paciente nuevoPaciente = new Paciente();
        nuevoPaciente.IdPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        nuevoPaciente.Nombre = txtNombre.Text.Trim();
        nuevoPaciente.Apellido = txtApellido.Text.Trim();
        nuevoPaciente.DNI = txtDni.Text.Trim();
        nuevoPaciente.FechaNacimiento = fechaNacimiento;
        nuevoPaciente.Ocupacion = txtOcupacion.Text.Trim();
        nuevoPaciente.EstadoCivil = ddlEstadoCivil.SelectedValue;
        nuevoPaciente.Email = txtEmail.Text.Trim();
        nuevoPaciente.Telefono = txtTelefono.Text.Trim();
        nuevoPaciente.Sexo = sexo;

        GestorPaciente gestorPaciente = new GestorPaciente();
        try
        {
            gestorPaciente.Alta(nuevoPaciente);
            MostrarExito(string.Format(Traducir("msg_paciente_registrado"), nuevoPaciente.Nombre + " " + nuevoPaciente.Apellido));
            LimpiarFormulario();
            CargarGrilla();
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int idPaciente = Convert.ToInt32(e.CommandArgument);
        GestorPaciente gestorPaciente = new GestorPaciente();

        try
        {
            switch (e.CommandName)
            {
                case "DarBaja":
                    gestorPaciente.Baja(idPaciente);
                    MostrarExito(Traducir("msg_paciente_baja"));
                    break;

                case "Reactivar":
                    gestorPaciente.Activar(idPaciente);
                    MostrarExito(Traducir("msg_paciente_reactivado"));
                    break;
            }
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }

        CargarGrilla();
    }

    protected void gvPacientes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType != DataControlRowType.DataRow) return;

        FilaPaciente fila = e.Row.DataItem as FilaPaciente;
        if (fila == null) return;

        Label lblEstadoPaciente = e.Row.FindControl("lblEstadoPaciente") as Label;
        if (lblEstadoPaciente != null)
        {
            lblEstadoPaciente.Text = fila.Activo ? Traducir("estado_disponible") : Traducir("estado_desactivado");
            lblEstadoPaciente.CssClass = fila.Activo ? "badge-estado activo" : "badge-estado inactivo";
        }

        LinkButton lbBaja = e.Row.FindControl("lbBaja") as LinkButton;
        if (lbBaja != null) lbBaja.Text = "🚫 " + Traducir("btn_dar_baja");

        LinkButton lbReactivar = e.Row.FindControl("lbReactivar") as LinkButton;
        if (lbReactivar != null) lbReactivar.Text = "✅ " + Traducir("btn_reactivar");
    }

    private void CargarGrilla()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        int idPropio = GestorSesion.PsicologoActual.IdPsicologo;
        List<Paciente> propios = gestorPaciente.ObtenerPorPsicologo(idPropio, soloActivos: false);

        List<FilaPaciente> filas = propios.Select(p => new FilaPaciente
        {
            IdPaciente = p.IdPaciente,
            NombreCompleto = p.Nombre + " " + p.Apellido,
            Dni = p.DNI,
            Edad = CalcularEdad(p.FechaNacimiento),
            EstadoCivil = p.EstadoCivil,
            FechaRegistro = p.FechaRegistro,
            Activo = p.Activo
        }).OrderByDescending(f => f.FechaRegistro).ToList();

        gvPacientes.DataSource = filas;
        gvPacientes.DataBind();

        lblBadgeActivos.Text = filas.Count(f => f.Activo) + " " + Traducir("badge_activos_sufijo");
        lblBadgeActivos.Visible = true;
        lblBadgeInactivos.Text = filas.Count(f => !f.Activo) + " " + Traducir("badge_inactivos_sufijo");
        lblBadgeInactivos.Visible = true;
    }

    private int CalcularEdad(DateTime fechaNacimiento)
    {
        int edad = DateTime.Now.Year - fechaNacimiento.Year;
        if (fechaNacimiento.Date > DateTime.Now.AddYears(-edad)) edad--;
        return edad;
    }

    private void ModoAlta()
    {
        lblFormTitulo.Text = Traducir("titulo_nuevo_paciente");
        lblFormSubtitulo.Text = Traducir("subtitulo_registrar_paciente");
        btnRegistrar.Text = Traducir("btn_registrar_paciente_form");
        LimpiarFormulario();
    }

    private void LimpiarFormulario()
    {
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtDni.Text = string.Empty;
        txtFechaNacimiento.Text = string.Empty;
        txtOcupacion.Text = string.Empty;
        ddlEstadoCivil.SelectedIndex = 0;
        rbMasculino.Checked = false;
        rbFemenino.Checked = false;
        rbNoEspecifica.Checked = false;
        txtEmail.Text = string.Empty;
        txtTelefono.Text = string.Empty;
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