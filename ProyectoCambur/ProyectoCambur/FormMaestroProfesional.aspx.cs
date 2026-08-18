using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

public partial class FormMaestroProfesional : System.Web.UI.Page
{
    
    private class FilaProfesional
    {
        public int IdPsicologo { get; set; }
        public string NombreCompleto { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Idioma { get; set; }
        public string RolPermiso { get; set; }
        public DateTime FechaRegistro { get; set; }
        public bool Activo { get; set; }
        public bool IsHabilitado { get; set; }
        public bool IsBloqueado { get; set; }
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

      
        if (psicologoActual.RolPermiso != "Administrador" && psicologoActual.RolPermiso != "Web Master")
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            lblNombreProfesional.Text = psicologoActual.Nombre + " " + psicologoActual.Apellido;
            lblRolActual.Text = psicologoActual.RolPermiso;
            lblIniciales.Text = ObtenerIniciales(psicologoActual.Nombre, psicologoActual.Apellido);
            lblTaglineSidebar.Text = psicologoActual.RolPermiso == "Web Master" ? "Panel Técnico" : "Panel de Gestión";
            lnkVolverMenu.NavigateUrl = psicologoActual.RolPermiso == "Web Master" ? "FormMenuWebMaster.aspx" : "FormMenuAdministrador.aspx";

            ModoAlta();
            CargarGrilla();
        }
    }

    
    private void CargarGrilla()
    {
        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        int idPropio = GestorSesion.PsicologoActual.IdPsicologo;

        List<Psicologo> todos = gestorPsicologo.ObtenerTodos()
            .Where(p => p.IdPsicologo != idPropio)
            .ToList();

        string filtro = ddlFiltroEstado.SelectedValue;
        if (filtro == "ACTIVOS")
        {
            todos = todos.Where(p => p.Activo).ToList();
        }
        else if (filtro == "INACTIVOS")
        {
            todos = todos.Where(p => !p.Activo).ToList();
        }

        List<FilaProfesional> filas = todos.Select(p => new FilaProfesional
        {
            IdPsicologo = p.IdPsicologo,
            NombreCompleto = p.Nombre + " " + p.Apellido,
            Dni = p.Dni,
            Email = p.Email,
            Idioma = p.Idioma,
            RolPermiso = p.RolPermiso,
            FechaRegistro = p.FechaRegistro,
            Activo = p.Activo,
            IsHabilitado = p.IsHabilitado,
            IsBloqueado = p.IsBloqueado
        }).ToList();

        gvProfesionales.DataSource = filas;
        gvProfesionales.DataBind();

        
        List<Psicologo> universoCompleto = gestorPsicologo.ObtenerTodos()
            .Where(p => p.IdPsicologo != idPropio)
            .ToList();

        lblBadgeActivos.Text = universoCompleto.Count(p => p.Activo) + " activos";
        lblBadgeActivos.Visible = true;
        lblBadgeInactivos.Text = universoCompleto.Count(p => !p.Activo) + " inactivos";
        lblBadgeInactivos.Visible = true;
    }

   
    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        bool esAlta = hdnIdPsicologo.Value == "0";

        if (!Page.IsValid) return;

        GestorPsicologo gestorPsicologo = new GestorPsicologo();

        try
        {
            if (esAlta)
            {
                Psicologo nuevoPsicologo = new Psicologo();
                nuevoPsicologo.Nombre = txtNombre.Text.Trim();
                nuevoPsicologo.Apellido = txtApellido.Text.Trim();
                nuevoPsicologo.Dni = txtDni.Text.Trim();
                nuevoPsicologo.Email = txtEmail.Text.Trim().ToLower();
              
                nuevoPsicologo.Contrasena = nuevoPsicologo.Dni + nuevoPsicologo.Email;
                nuevoPsicologo.Idioma = ddlIdioma.SelectedValue;
                nuevoPsicologo.RolPermiso = ddlRol.SelectedValue;

                gestorPsicologo.Alta(nuevoPsicologo);
                MostrarExito("Profesional \"" + nuevoPsicologo.Nombre + " " + nuevoPsicologo.Apellido +
                    "\" registrado correctamente. Contraseña inicial: DNI+Email.");
            }
            else
            {
                int idPsicologo = Convert.ToInt32(hdnIdPsicologo.Value);
                Psicologo psicologoModificado = gestorPsicologo.BuscarPorId(idPsicologo);
                if (psicologoModificado == null)
                {
                    MostrarError("El profesional que intentás modificar ya no existe.");
                    ModoAlta();
                    CargarGrilla();
                    return;
                }

                psicologoModificado.Nombre = txtNombre.Text.Trim();
                psicologoModificado.Apellido = txtApellido.Text.Trim();
                psicologoModificado.Dni = txtDni.Text.Trim();
                psicologoModificado.Email = txtEmail.Text.Trim().ToLower();
                psicologoModificado.Idioma = ddlIdioma.SelectedValue;
                psicologoModificado.RolPermiso = ddlRol.SelectedValue;
              

                gestorPsicologo.Modificar(psicologoModificado);
                MostrarExito("Profesional \"" + psicologoModificado.Nombre + " " + psicologoModificado.Apellido + "\" modificado correctamente.");
            }

            ModoAlta();
            CargarGrilla();
        }
        catch (ArgumentException ex)
        {
            MostrarError(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            MostrarError(ex.Message);
        }
    }

   
    protected void btnCancelarEdicion_Click(object sender, EventArgs e)
    {
        ModoAlta();
    }


    protected void ddlFiltroEstado_SelectedIndexChanged(object sender, EventArgs e)
    {
        CargarGrilla();
    }

   
    protected void gvProfesionales_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int idPsicologo = Convert.ToInt32(e.CommandArgument);
        GestorPsicologo gestorPsicologo = new GestorPsicologo();

        switch (e.CommandName)
        {
            case "Modificar":
                CargarFormularioParaEdicion(idPsicologo);
                return; 

            case "DarBaja":
                gestorPsicologo.Baja(idPsicologo);
                MostrarExito("Profesional dado de baja correctamente.");
                break;

            case "Reactivar":
                gestorPsicologo.Activar(idPsicologo);
                MostrarExito("Profesional reactivado correctamente.");
                break;

            case "Deshabilitar":
                gestorPsicologo.Deshabilitar(idPsicologo);
                MostrarExito("Profesional deshabilitado. Ya no va a poder iniciar sesión.");
                break;

            case "Habilitar":
                gestorPsicologo.Habilitar(idPsicologo);
                MostrarExito("Profesional habilitado correctamente.");
                break;

            case "Desbloquear":
                gestorPsicologo.Desbloquear(idPsicologo);
                MostrarExito("Profesional desbloqueado. Su contraseña se reseteó a DNI+Email; debería cambiarla en su próximo ingreso.");
                break;
        }

        CargarGrilla();
    }

  
    private void ModoAlta()
    {
        hdnIdPsicologo.Value = "0";
        lblFormTitulo.Text = "Nuevo profesional";
        btnGuardar.Text = "Registrar profesional";
        btnCancelarEdicion.Visible = false;
        pnlAvisoContrasena.Visible = true;

        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtDni.Text = string.Empty;
        txtEmail.Text = string.Empty;
        ddlIdioma.SelectedIndex = 0;
        ddlRol.SelectedIndex = 0;
    }

    private void CargarFormularioParaEdicion(int idPsicologo)
    {
        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        Psicologo psicologo = gestorPsicologo.BuscarPorId(idPsicologo);
        if (psicologo == null)
        {
            MostrarError("El profesional seleccionado ya no existe.");
            CargarGrilla();
            return;
        }

        hdnIdPsicologo.Value = psicologo.IdPsicologo.ToString();
        lblFormTitulo.Text = "Modificar profesional";
        btnGuardar.Text = "Guardar cambios";
        btnCancelarEdicion.Visible = true;
        pnlAvisoContrasena.Visible = false;

        txtNombre.Text = psicologo.Nombre;
        txtApellido.Text = psicologo.Apellido;
        txtDni.Text = psicologo.Dni;
        txtEmail.Text = psicologo.Email;
        ddlIdioma.SelectedValue = psicologo.Idioma;
        ddlRol.SelectedValue = psicologo.RolPermiso;
    }

    
    private string ObtenerIniciales(string nombre, string apellido)
    {
        string inicialNombre = string.IsNullOrEmpty(nombre) ? "" : nombre.Substring(0, 1);
        string inicialApellido = string.IsNullOrEmpty(apellido) ? "" : apellido.Substring(0, 1);
        return (inicialNombre + inicialApellido).ToUpper();
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