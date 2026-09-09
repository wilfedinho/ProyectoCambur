using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using GUI;
public partial class FormMaestroProfesional : PaginaBase
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
        if (!new GestorPermiso().TienePermiso(psicologoActual.RolPermiso, "acceder_abm_profesionales"))
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
        lblMenuLandingSidebar.Text = Traducir("nav_landing");
        ddlFiltroEstado.Items.FindByValue("TODOS").Text = Traducir("opt_todos");
        ddlFiltroEstado.Items.FindByValue("ACTIVOS").Text = Traducir("opt_activos");
        ddlFiltroEstado.Items.FindByValue("INACTIVOS").Text = Traducir("opt_desactivados");
        lblTituloListado.Text = Traducir("titulo_profesionales_registrados");
        lblEtiquetaMostrar.Text = Traducir("lbl_mostrar");
        gvProfesionales.Columns[0].HeaderText = Traducir("col_profesional");
        gvProfesionales.Columns[1].HeaderText = Traducir("col_dni");
        gvProfesionales.Columns[2].HeaderText = Traducir("col_email");
        gvProfesionales.Columns[3].HeaderText = Traducir("col_idioma");
        gvProfesionales.Columns[4].HeaderText = Traducir("lbl_rol_plan");
        gvProfesionales.Columns[5].HeaderText = Traducir("col_registrado");
        gvProfesionales.Columns[6].HeaderText = Traducir("col_estado");
        gvProfesionales.Columns[7].HeaderText = Traducir("col_acciones");
        gvProfesionales.EmptyDataText = Traducir("empty_profesionales");
        ddlRol.Items.FindByValue("").Text = Traducir("opt_seleccionar");
        ddlRol.Items.FindByValue("Basico").Text = Traducir("rol_psicologo_basico");
        ddlRol.Items.FindByValue("Profesional").Text = Traducir("rol_psicologo_profesional");
        ddlRol.Items.FindByValue("Premium").Text = Traducir("rol_psicologo_premium");

        btnModoGestion.Text = "👥 " + Traducir("modo_gestion");
        btnModoSerializar.Text = "🗄️ " + Traducir("modo_serializar_xml");
        btnModoDeserializar.Text = "📥 " + Traducir("modo_deserializar_xml");

        lblTituloSerializar.Text = Traducir("titulo_serializar_profesionales");
        lblSubtituloSerializar.Text = Traducir("subtitulo_serializar_profesionales");
        gvSeleccionSerializar.Columns[0].HeaderText = Traducir("col_seleccionar");
        gvSeleccionSerializar.Columns[1].HeaderText = Traducir("col_profesional");
        gvSeleccionSerializar.Columns[2].HeaderText = Traducir("col_dni");
        gvSeleccionSerializar.Columns[3].HeaderText = Traducir("col_email");
        gvSeleccionSerializar.Columns[4].HeaderText = Traducir("lbl_rol_plan");
        gvSeleccionSerializar.Columns[5].HeaderText = Traducir("col_registrado");
        gvSeleccionSerializar.EmptyDataText = Traducir("empty_profesionales");
        btnExportarSeleccionados.Text = "⬇️ " + Traducir("btn_exportar_seleccionados");
        btnVolverGestionSerializar.Text = Traducir("btn_volver_gestion");

        lblTituloDeserializar.Text = Traducir("titulo_deserializar_profesionales");
        lblSubtituloDeserializar.Text = Traducir("subtitulo_deserializar_profesionales");
        lblEtiquetaArchivoXml.Text = Traducir("lbl_archivo_xml");
        lblBtnElegirArchivo.Text = Traducir("btn_elegir_archivo");
        lblNombreArchivoXml.Text = Traducir("lbl_ningun_archivo_seleccionado");
        btnCargarXml.Text = "📤 " + Traducir("btn_cargar_xml");
        btnVolverGestionDeserializar.Text = Traducir("btn_volver_gestion");
        lblTituloVistaPreviaXml.Text = Traducir("titulo_vista_previa_xml");
        gvProfesionalesXml.Columns[0].HeaderText = Traducir("col_profesional");
        gvProfesionalesXml.Columns[1].HeaderText = Traducir("col_dni");
        gvProfesionalesXml.Columns[2].HeaderText = Traducir("col_email");
        gvProfesionalesXml.Columns[3].HeaderText = Traducir("lbl_rol_plan");
        gvProfesionalesXml.Columns[4].HeaderText = Traducir("col_registrado");
        gvProfesionalesXml.Columns[5].HeaderText = Traducir("col_estado");
        gvProfesionalesXml.EmptyDataText = Traducir("empty_vista_previa_xml");
    }

    private void CambiarModo(string modo)
    {
        hdnModo.Value = modo;
        pnlModoGestion.Visible = modo == "GESTION";
        pnlModoSerializar.Visible = modo == "SERIALIZAR";
        pnlModoDeserializar.Visible = modo == "DESERIALIZAR";
        btnModoGestion.CssClass = modo == "GESTION" ? "btn-modo active" : "btn-modo";
        btnModoSerializar.CssClass = modo == "SERIALIZAR" ? "btn-modo active" : "btn-modo";
        btnModoDeserializar.CssClass = modo == "DESERIALIZAR" ? "btn-modo active" : "btn-modo";
    }

    protected void btnModoGestion_Click(object sender, EventArgs e)
    {
        CambiarModo("GESTION");
        ModoAlta();
        CargarGrilla();
    }

    protected void btnModoSerializar_Click(object sender, EventArgs e)
    {
        CambiarModo("SERIALIZAR");
        lblMensajeSerializar.Visible = false;
        CargarGrillaSerializar();
    }

    protected void btnModoDeserializar_Click(object sender, EventArgs e)
    {
        CambiarModo("DESERIALIZAR");
        lblMensajeDeserializar.Visible = false;
        gvProfesionalesXml.Visible = false;
        gvProfesionalesXml.DataSource = null;
        gvProfesionalesXml.DataBind();
    }

    private void CargarGrillaSerializar()
    {
        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        int idPropio = GestorSesion.PsicologoActual.IdPsicologo;
        List<Psicologo> todos = gestorPsicologo.ObtenerTodos().Where(p => p.IdPsicologo != idPropio).ToList();
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
        gvSeleccionSerializar.DataSource = filas;
        gvSeleccionSerializar.DataBind();
    }

    protected void btnExportarSeleccionados_Click(object sender, EventArgs e)
    {
        lblMensajeSerializar.Visible = false;

        List<int> idsSeleccionados = new List<int>();
        foreach (GridViewRow fila in gvSeleccionSerializar.Rows)
        {
            if (fila.RowType != DataControlRowType.DataRow) continue;
            CheckBox chkSeleccionar = fila.FindControl("chkSeleccionar") as CheckBox;
            if (chkSeleccionar != null && chkSeleccionar.Checked)
            {
                idsSeleccionados.Add(Convert.ToInt32(gvSeleccionSerializar.DataKeys[fila.RowIndex].Value));
            }
        }

        string rutaTemp = null;
        try
        {
            string carpetaTemp = Server.MapPath("~/App_Data/ExportacionesTemp");
            if (!Directory.Exists(carpetaTemp))
            {
                Directory.CreateDirectory(carpetaTemp);
            }
            string nombreArchivo = "Profesionales_Cambur_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".xml";
            rutaTemp = Path.Combine(carpetaTemp, nombreArchivo);

            Psicologo psicologoActual = GestorSesion.PsicologoActual;
            WSProfesionales servicioProfesionales = new WSProfesionales();
            servicioProfesionales.SerializarProfesionales(rutaTemp, idsSeleccionados, psicologoActual.Nombre + " " + psicologoActual.Apellido);

            byte[] contenidoXml = File.ReadAllBytes(rutaTemp);

            Response.Clear();
            Response.ContentType = "application/xml";
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + nombreArchivo + "\"");
            Response.AddHeader("Content-Length", contenidoXml.Length.ToString());
            Response.BinaryWrite(contenidoXml);
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (ExcepcionTraducible ex)
        {
            lblMensajeSerializar.Text = TraducirExcepcion(ex);
            lblMensajeSerializar.CssClass = "server-error";
            lblMensajeSerializar.Visible = true;
        }
        finally
        {
            if (rutaTemp != null && File.Exists(rutaTemp))
            {
                File.Delete(rutaTemp);
            }
        }
    }

    protected void btnCargarXml_Click(object sender, EventArgs e)
    {
        lblMensajeDeserializar.Visible = false;
        gvProfesionalesXml.Visible = false;

        if (!fuArchivoXml.HasFile)
        {
            MostrarErrorDeserializar(Traducir("error_xml_ningun_archivo"));
            return;
        }

        string rutaTemp = null;
        try
        {
            string carpetaTemp = Server.MapPath("~/App_Data/ExportacionesTemp");
            if (!Directory.Exists(carpetaTemp))
            {
                Directory.CreateDirectory(carpetaTemp);
            }
            rutaTemp = Path.Combine(carpetaTemp, Guid.NewGuid().ToString("N") + ".xml");
            fuArchivoXml.SaveAs(rutaTemp);

            WSProfesionales servicioProfesionales = new WSProfesionales();
            List<ProfesionalXmlVista> profesionales = servicioProfesionales.DeserializarProfesionales(rutaTemp);

            gvProfesionalesXml.DataSource = profesionales;
            gvProfesionalesXml.DataBind();
            gvProfesionalesXml.Visible = true;

            lblMensajeDeserializar.Text = string.Format(Traducir("msg_xml_cargado_exitosamente"), profesionales.Count);
            lblMensajeDeserializar.CssClass = "server-success";
            lblMensajeDeserializar.Visible = true;
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarErrorDeserializar(TraducirExcepcion(ex));
        }
        catch (Exception)
        {
            MostrarErrorDeserializar(Traducir("error_xml_formato_invalido"));
        }
        finally
        {
            if (rutaTemp != null && File.Exists(rutaTemp))
            {
                File.Delete(rutaTemp);
            }
        }
    }

    private void MostrarErrorDeserializar(string msg)
    {
        lblMensajeDeserializar.Text = msg;
        lblMensajeDeserializar.CssClass = "server-error";
        lblMensajeDeserializar.Visible = true;
    }

    private void CargarGrilla()
    {
        GestorPsicologo gestorPsicologo = new GestorPsicologo();
        int idPropio = GestorSesion.PsicologoActual.IdPsicologo;
        List<Psicologo> todos = gestorPsicologo.ObtenerTodos().Where(p => p.IdPsicologo != idPropio).ToList();
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
        TraducirFilasGrilla();
        List<Psicologo> universoCompleto = gestorPsicologo.ObtenerTodos().Where(p => p.IdPsicologo != idPropio).ToList();
        lblBadgeActivos.Text = universoCompleto.Count(p => p.Activo) + " " + Traducir("badge_activos_sufijo");
        lblBadgeActivos.Visible = true;
        lblBadgeInactivos.Text = universoCompleto.Count(p => !p.Activo) + " " + Traducir("badge_inactivos_sufijo");
        lblBadgeInactivos.Visible = true;
    }

    private void TraducirFilasGrilla()
    {
        foreach (GridViewRow fila in gvProfesionales.Rows)
        {
            if (fila.RowType != DataControlRowType.DataRow) continue;
            LinkButton lbModificar = fila.FindControl("lbModificar") as LinkButton;
            if (lbModificar != null) lbModificar.Text = "✏️ " + Traducir("btn_modificar");
            LinkButton lbBaja = fila.FindControl("lbBaja") as LinkButton;
            if (lbBaja != null) lbBaja.Text = "🚫 " + Traducir("btn_dar_baja");
            LinkButton lbReactivar = fila.FindControl("lbReactivar") as LinkButton;
            if (lbReactivar != null) lbReactivar.Text = "✅ " + Traducir("btn_reactivar");
            LinkButton lbDeshabilitar = fila.FindControl("lbDeshabilitar") as LinkButton;
            if (lbDeshabilitar != null) lbDeshabilitar.Text = "⛔ " + Traducir("btn_deshabilitar");
            LinkButton lbHabilitar = fila.FindControl("lbHabilitar") as LinkButton;
            if (lbHabilitar != null) lbHabilitar.Text = "✅ " + Traducir("btn_habilitar");
            LinkButton lbDesbloquear = fila.FindControl("lbDesbloquear") as LinkButton;
            if (lbDesbloquear != null) lbDesbloquear.Text = "🔓 " + Traducir("btn_desbloquear");
        }
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
                nuevoPsicologo.Idioma = ddlIdioma.SelectedValue;
                nuevoPsicologo.RolPermiso = ddlRol.SelectedValue;
                gestorPsicologo.AltaPorAdministrador(nuevoPsicologo);
                MostrarExito(string.Format(Traducir("msg_profesional_registrado"), nuevoPsicologo.Nombre + " " + nuevoPsicologo.Apellido));
            }
            else
            {
                int idPsicologo = Convert.ToInt32(hdnIdPsicologo.Value);
                Psicologo psicologoModificado = gestorPsicologo.BuscarPorId(idPsicologo);
                if (psicologoModificado == null)
                {
                    MostrarError(Traducir("msg_profesional_no_existe"));
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
                MostrarExito(string.Format(Traducir("msg_profesional_modificado"), psicologoModificado.Nombre + " " + psicologoModificado.Apellido));
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
        CargarGrilla();
    }
    private static readonly Dictionary<string, IComandoAccion> ComandosProfesional =
        new Dictionary<string, IComandoAccion>
        {
            { "DarBaja", new ComandoDarBajaProfesional() },
            { "Reactivar", new ComandoReactivarProfesional() },
            { "Deshabilitar", new ComandoDeshabilitarProfesional() },
            { "Habilitar", new ComandoHabilitarProfesional() },
            { "Desbloquear", new ComandoDesbloquearProfesional() },
        };

    private static readonly Dictionary<string, string> MensajesExitoProfesional =
        new Dictionary<string, string>
        {
            { "DarBaja", "msg_profesional_baja" },
            { "Reactivar", "msg_profesional_reactivado" },
            { "Deshabilitar", "msg_profesional_deshabilitado" },
            { "Habilitar", "msg_profesional_habilitado" },
            { "Desbloquear", "msg_profesional_desbloqueado" },
        };

    protected void gvProfesionales_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int idPsicologo = Convert.ToInt32(e.CommandArgument);

        if (e.CommandName == "Modificar")
        {
            CargarFormularioParaEdicion(idPsicologo);
            return;
        }

        try
        {
            IComandoAccion comando;
            if (ComandosProfesional.TryGetValue(e.CommandName, out comando))
            {
                comando.Ejecutar(idPsicologo);
                MostrarExito(Traducir(MensajesExitoProfesional[e.CommandName]));
            }
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
        CargarGrilla();
    }

    private void ModoAlta()
    {
        hdnIdPsicologo.Value = "0";
        lblFormTitulo.Text = Traducir("titulo_nuevo_profesional");
        btnGuardar.Text = Traducir("btn_registrar_profesional");
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
            MostrarError(Traducir("msg_profesional_no_existe_grilla"));
            CargarGrilla();
            return;
        }
        hdnIdPsicologo.Value = psicologo.IdPsicologo.ToString();
        lblFormTitulo.Text = Traducir("titulo_modificar_profesional");
        btnGuardar.Text = Traducir("btn_guardar_cambios");
        btnCancelarEdicion.Text = Traducir("btn_cancelar_edicion");
        btnCancelarEdicion.Visible = true;
        pnlAvisoContrasena.Visible = false;
        txtNombre.Text = psicologo.Nombre;
        txtApellido.Text = psicologo.Apellido;
        txtDni.Text = psicologo.Dni;
        txtEmail.Text = psicologo.Email;
        ddlIdioma.SelectedValue = psicologo.Idioma;
        ddlRol.SelectedValue = psicologo.RolPermiso;
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