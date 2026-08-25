using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

public partial class FormMaestroPaciente : GUI.PaginaBase
{
    private class FilaPaciente
    {
        public int IdPaciente { get; set; }
        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public string NombrePsicologo { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        GestorPermiso gestorPermiso = new GestorPermiso();
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_abm_pacientes"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");

            CargarComboPsicologos();
            ModoAlta();
            CargarGrilla();
        }
    }

    private void AplicarTraducciones()
    {
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("header_administrador");
        lblHeaderPagina.Text = Traducir("nav_abm_pacientes");

        lblSubtituloForm.Text = Traducir("subtitulo_abm_paciente");
        lblSeccionVinculo.Text = Traducir("seccion_vinculo_profesional");
        lblEtiquetaPsicologo.Text = Traducir("lbl_psicologo_asignado");
        lblHintPsicologo.Text = Traducir("hint_psicologo_asignado");
        rfvPsicologo.ErrorMessage = Traducir("error_paciente_sin_profesional");

        lblSeccionDatos.Text = Traducir("seccion_datos_paciente");
        lblEtiquetaNombre.Text = Traducir("lbl_nombre");
        rfvNombre.ErrorMessage = Traducir("error_nombre_obligatorio");
        lblEtiquetaApellido.Text = Traducir("lbl_apellido");
        rfvApellido.ErrorMessage = Traducir("error_apellido_obligatorio");
        lblEtiquetaDni.Text = Traducir("lbl_dni");
        revDni.ErrorMessage = Traducir("error_formato_dni");
        lblEtiquetaFechaNacimiento.Text = Traducir("lbl_fecha_nacimiento");
        rfvFechaNacimiento.ErrorMessage = Traducir("error_fecha_nacimiento_obligatoria");
        lblEtiquetaSexo.Text = Traducir("lbl_sexo");
        lblEtiquetaEstadoCivil.Text = Traducir("lbl_estado_civil");
        lblEtiquetaOcupacion.Text = Traducir("lbl_ocupacion");
        lblEtiquetaEmail.Text = Traducir("lbl_correo");
        revEmail.ErrorMessage = Traducir("error_formato_email");
        lblEtiquetaTelefono.Text = Traducir("lbl_telefono");

        lblTituloListado.Text = Traducir("titulo_pacientes_registrados");
        lblEtiquetaMostrar.Text = Traducir("lbl_mostrar");
        ddlFiltroEstado.Items.FindByValue("TODOS").Text = Traducir("opt_todos");
        ddlFiltroEstado.Items.FindByValue("ACTIVOS").Text = Traducir("opt_activos");
        ddlFiltroEstado.Items.FindByValue("INACTIVOS").Text = Traducir("opt_desactivados");

        gvPacientes.Columns[0].HeaderText = Traducir("col_paciente");
        gvPacientes.Columns[1].HeaderText = Traducir("lbl_dni");
        gvPacientes.Columns[2].HeaderText = Traducir("lbl_psicologo_asignado");
        gvPacientes.Columns[3].HeaderText = Traducir("col_email");
        gvPacientes.Columns[4].HeaderText = Traducir("col_registrado");
        gvPacientes.Columns[5].HeaderText = Traducir("col_estado");
        gvPacientes.Columns[6].HeaderText = Traducir("col_acciones");
        gvPacientes.EmptyDataText = Traducir("empty_pacientes");
    }
    private void CargarComboPsicologos()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        List<Psicologo> clinicos = gestorPaciente.ObtenerPsicologosClinicos();

        ddlPsicologo.Items.Clear();
        ddlPsicologo.Items.Add(new ListItem(Traducir("opt_seleccionar"), ""));

        foreach (Psicologo p in clinicos.OrderBy(x => x.Apellido))
        {
            ddlPsicologo.Items.Add(new ListItem(p.Nombre + " " + p.Apellido + " (" + p.Email + ")", p.IdPsicologo.ToString()));
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        bool esAlta = hdnIdPaciente.Value == "0";

        if (!Page.IsValid) return;

        GestorPaciente gestorPaciente = new GestorPaciente();

        try
        {
            DateTime fechaNacimiento;
            DateTime.TryParse(txtFechaNacimiento.Text, out fechaNacimiento);

            if (esAlta)
            {
                Paciente nuevoPaciente = new Paciente();
                nuevoPaciente.IdPsicologo = Convert.ToInt32(ddlPsicologo.SelectedValue);
                nuevoPaciente.Nombre = txtNombre.Text.Trim();
                nuevoPaciente.Apellido = txtApellido.Text.Trim();
                nuevoPaciente.DNI = txtDni.Text.Trim();
                nuevoPaciente.FechaNacimiento = fechaNacimiento;
                nuevoPaciente.Ocupacion = txtOcupacion.Text.Trim();
                nuevoPaciente.EstadoCivil = txtEstadoCivil.Text.Trim();
                nuevoPaciente.Email = txtEmail.Text.Trim();
                nuevoPaciente.Telefono = txtTelefono.Text.Trim();
                nuevoPaciente.Sexo = ddlSexo.SelectedValue;

                gestorPaciente.Alta(nuevoPaciente);
                MostrarExito(string.Format(Traducir("msg_paciente_registrado"), nuevoPaciente.Nombre + " " + nuevoPaciente.Apellido));
            }
            else
            {
                int idPaciente = Convert.ToInt32(hdnIdPaciente.Value);
                Paciente pacienteModificado = gestorPaciente.BuscarPorId(idPaciente);
                if (pacienteModificado == null)
                {
                    MostrarError(Traducir("msg_paciente_no_existe"));
                    ModoAlta();
                    CargarGrilla();
                    return;
                }

                pacienteModificado.IdPsicologo = Convert.ToInt32(ddlPsicologo.SelectedValue);
                pacienteModificado.Nombre = txtNombre.Text.Trim();
                pacienteModificado.Apellido = txtApellido.Text.Trim();
                pacienteModificado.DNI = txtDni.Text.Trim();
                pacienteModificado.FechaNacimiento = fechaNacimiento;
                pacienteModificado.Ocupacion = txtOcupacion.Text.Trim();
                pacienteModificado.EstadoCivil = txtEstadoCivil.Text.Trim();
                pacienteModificado.Email = txtEmail.Text.Trim();
                pacienteModificado.Telefono = txtTelefono.Text.Trim();
                pacienteModificado.Sexo = ddlSexo.SelectedValue;

                gestorPaciente.Modificar(pacienteModificado);
                MostrarExito(string.Format(Traducir("msg_paciente_modificado"), pacienteModificado.Nombre + " " + pacienteModificado.Apellido));
            }

            ModoAlta();
            CargarGrilla();
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
    }

    protected void btnCancelarEdicion_Click(object sender, EventArgs e)
    {
        ModoAlta();
    }

    protected void ddlFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvPacientes.PageIndex = 0;
        CargarGrilla();
    }

    protected void gvPacientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPacientes.PageIndex = e.NewPageIndex;
        CargarGrilla();
    }

    protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int idPaciente = Convert.ToInt32(e.CommandArgument);
        GestorPaciente gestorPaciente = new GestorPaciente();

        switch (e.CommandName)
        {
            case "Modificar":
                CargarFormularioParaEdicion(idPaciente);
                return;

            case "DarBaja":
                gestorPaciente.Baja(idPaciente);
                MostrarExito(Traducir("msg_paciente_baja"));
                break;

            case "Reactivar":
                gestorPaciente.Activar(idPaciente);
                MostrarExito(Traducir("msg_paciente_reactivado"));
                break;
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

        LinkButton lbModificar = e.Row.FindControl("lbModificar") as LinkButton;
        if (lbModificar != null) lbModificar.Text = "✏️ " + Traducir("btn_modificar");

        LinkButton lbBaja = e.Row.FindControl("lbBaja") as LinkButton;
        if (lbBaja != null) lbBaja.Text = "🚫 " + Traducir("btn_dar_baja");

        LinkButton lbReactivar = e.Row.FindControl("lbReactivar") as LinkButton;
        if (lbReactivar != null) lbReactivar.Text = "✅ " + Traducir("btn_reactivar");
    }

    private void CargarGrilla()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        GestorPsicologo gestorPsicologo = new GestorPsicologo();

        List<Paciente> todos = gestorPaciente.ObtenerTodos();
        List<Psicologo> todosLosPsicologos = gestorPsicologo.ObtenerTodos();
        Dictionary<int, string> nombresPorId = todosLosPsicologos.ToDictionary(p => p.IdPsicologo, p => p.Nombre + " " + p.Apellido);

        string filtro = ddlFiltroEstado.SelectedValue;
        if (filtro == "ACTIVOS")
        {
            todos = todos.Where(p => p.Activo).ToList();
        }
        else if (filtro == "INACTIVOS")
        {
            todos = todos.Where(p => !p.Activo).ToList();
        }

        List<FilaPaciente> filas = todos.Select(p => new FilaPaciente
        {
            IdPaciente = p.IdPaciente,
            NombreCompleto = p.Nombre + " " + p.Apellido,
            Dni = p.DNI,
            NombrePsicologo = nombresPorId.ContainsKey(p.IdPsicologo) ? nombresPorId[p.IdPsicologo] : "—",
            Email = p.Email,
            FechaRegistro = p.FechaRegistro,
            Activo = p.Activo
        }).ToList();

        gvPacientes.DataSource = filas;
        gvPacientes.DataBind();
        List<Paciente> universoCompleto = gestorPaciente.ObtenerTodos();
        lblBadgeActivos.Text = universoCompleto.Count(p => p.Activo) + " " + Traducir("badge_activos_sufijo");
        lblBadgeActivos.Visible = true;
        lblBadgeInactivos.Text = universoCompleto.Count(p => !p.Activo) + " " + Traducir("badge_inactivos_sufijo");
        lblBadgeInactivos.Visible = true;
    }

    private void ModoAlta()
    {
        hdnIdPaciente.Value = "0";
        lblFormTitulo.Text = Traducir("titulo_nuevo_paciente");
        btnGuardar.Text = Traducir("btn_registrar_paciente_form");
        btnCancelarEdicion.Visible = false;
        ddlPsicologo.SelectedIndex = 0;
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtDni.Text = string.Empty;
        txtFechaNacimiento.Text = string.Empty;
        ddlSexo.SelectedIndex = 0;
        txtEstadoCivil.Text = string.Empty;
        txtOcupacion.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtTelefono.Text = string.Empty;
    }

    private void CargarFormularioParaEdicion(int idPaciente)
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        Paciente paciente = gestorPaciente.BuscarPorId(idPaciente);
        if (paciente == null)
        {
            MostrarError(Traducir("msg_paciente_no_existe"));
            CargarGrilla();
            return;
        }
        hdnIdPaciente.Value = paciente.IdPaciente.ToString();
        lblFormTitulo.Text = Traducir("titulo_modificar_paciente");
        btnGuardar.Text = Traducir("btn_guardar_cambios");
        btnCancelarEdicion.Visible = true;
        if (ddlPsicologo.Items.FindByValue(paciente.IdPsicologo.ToString()) == null)
        {
            GestorPsicologo gestorPsicologo = new GestorPsicologo();
            Psicologo psicologoActual = gestorPsicologo.BuscarPorId(paciente.IdPsicologo);
            if (psicologoActual != null)
            {
                ddlPsicologo.Items.Add(new ListItem(
                    psicologoActual.Nombre + " " + psicologoActual.Apellido + " (" + psicologoActual.Email + ") ⚠",
                    psicologoActual.IdPsicologo.ToString()));
            }
        }

        ddlPsicologo.SelectedValue = paciente.IdPsicologo.ToString();
        txtNombre.Text = paciente.Nombre;
        txtApellido.Text = paciente.Apellido;
        txtDni.Text = paciente.DNI;
        txtFechaNacimiento.Text = paciente.FechaNacimiento.ToString("yyyy-MM-dd");
        ddlSexo.SelectedValue = paciente.Sexo;
        txtEstadoCivil.Text = paciente.EstadoCivil;
        txtOcupacion.Text = paciente.Ocupacion;
        txtEmail.Text = paciente.Email;
        txtTelefono.Text = paciente.Telefono;
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