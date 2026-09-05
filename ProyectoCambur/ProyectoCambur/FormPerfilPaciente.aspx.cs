using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using GUI;

public partial class FormPerfilPaciente : PaginaBase
{
    public string ModelosInfoJson
    {
        get
        {
            Dictionary<string, object> info = new Dictionary<string, object>
            {
                { "BIGFIVE", new Dictionary<string, string> { { "nombre", Traducir("modelo_bigfive_nombre") }, { "icono", "🌐" }, { "desc", Traducir("modelo_bigfive_desc") } } },
                { "COPE", new Dictionary<string, string> { { "nombre", Traducir("modelo_cope_nombre") }, { "icono", "🛡️" }, { "desc", Traducir("modelo_cope_desc") } } },
                { "AUTOEFICACIA", new Dictionary<string, string> { { "nombre", Traducir("modelo_autoeficacia_nombre") }, { "icono", "⚡" }, { "desc", Traducir("modelo_autoeficacia_desc") } } },
                { "APEGO", new Dictionary<string, string> { { "nombre", Traducir("modelo_apego_nombre") }, { "icono", "🔗" }, { "desc", Traducir("modelo_apego_desc") } } },
                { "VALORES", new Dictionary<string, string> { { "nombre", Traducir("modelo_valores_nombre") }, { "icono", "🌱" }, { "desc", Traducir("modelo_valores_desc") } } }
            };

            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Serialize(info);
        }
    }
    public string JsonSeleccionarModeloAlerta
    {
        get
        {
            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Serialize(Traducir("msg_seleccionar_modelo_alerta"));
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
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
    private void AplicarTraducciones()
    {
        lblTaglineSidebar.Text = Traducir("tagline_panel_gestion");
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderSeccion.Text = Traducir("seccion_perfilacion");

        lblEtiquetaPacientePerfil.Text = Traducir("lbl_paciente");

        lblAvisoPerfil.Text = Traducir("ia_aviso_perfil");

        lblSeccionSeleccionarModelo.Text = Traducir("seccion_seleccionar_modelo");
        lblHintSeleccionarModelo.Text = Traducir("hint_seleccionar_modelo");

        lblModeloBigFiveNombre.Text = Traducir("modelo_bigfive_nombre");
        lblModeloBigFiveDesc.Text = Traducir("modelo_bigfive_desc");

        lblModeloCopeNombre.Text = Traducir("modelo_cope_nombre");
        lblModeloCopeDesc.Text = Traducir("modelo_cope_desc");

        lblModeloAutoeficaciaNombre.Text = Traducir("modelo_autoeficacia_nombre");
        lblModeloAutoeficaciaDesc.Text = Traducir("modelo_autoeficacia_desc");

        lblModeloApegoNombre.Text = Traducir("modelo_apego_nombre");
        lblModeloApegoDesc.Text = Traducir("modelo_apego_desc");

        lblModeloValoresNombre.Text = Traducir("modelo_valores_nombre");
        lblModeloValoresDesc.Text = Traducir("modelo_valores_desc");

        lnkCancelar.Text = Traducir("btn_cancelar");
        btnGenerar.Text = Traducir("btn_generar_perfil");

        lblTituloModeloSeleccionado.Text = Traducir("titulo_modelo_seleccionado");
        lblNingunModeloSeleccionado.Text = Traducir("txt_ningun_modelo_seleccionado");

        lblTituloPerfilesAnteriores.Text = Traducir("titulo_perfiles_anteriores");
        lblSinPerfiles.Text = Traducir("sin_perfiles_anteriores");

        lblAvisoTituloDatosEncriptados.Text = Traducir("aviso_titulo_datos_encriptados");
        lblAvisoTextoDatosEncriptados.Text = Traducir("aviso_texto_datos_encriptados_perfil");

        lblCargaTitulo.Text = Traducir("carga_titulo_perfil_ia");
        lblCargaSubtitulo.Text = Traducir("carga_subtitulo_perfil_ia");

        lblTituloPerfilGenerado.Text = Traducir("titulo_perfil_generado");
        btnNuevoPerfil.Text = Traducir("btn_nuevo_perfil");

        lblAvisoIABadgePerfil.Text = Traducir("aviso_ia_badge_perfil");

        lblSeccionDescripcionGeneral.Text = Traducir("seccion_descripcion_general_perfil");
        lblSeccionDimensionesEvaluadas.Text = Traducir("seccion_dimensiones_evaluadas");
        lblSeccionPatronesIdentificados.Text = Traducir("seccion_patrones_identificados");
        lblSeccionConsideracionesTratamiento.Text = Traducir("seccion_consideraciones_tratamiento");

        lblNotaPiePerfil.Text = Traducir("nota_pie_perfil");

        lblTituloDetallesPerfil.Text = Traducir("titulo_detalles_perfil");
        lblMetaLabelPaciente.Text = Traducir("lbl_paciente");
        lblMetaLabelModelo.Text = Traducir("lbl_modelo_utilizado");
        lblMetaLabelConsultas.Text = Traducir("lbl_consultas_analizadas");
        lblMetaLabelFecha.Text = Traducir("lbl_generado");

        lblTituloAccionesRelacionadas.Text = Traducir("titulo_acciones_relacionadas");
        lblAccesoExportarPdf.Text = Traducir("acceso_exportar_pdf");
        lblAccesoResumenIA.Text = Traducir("acceso_resumen_ia");
        lblAccesoGenerarDerivacion.Text = Traducir("acceso_generar_derivacion");

        lblAvisoTituloPerfilEncriptado.Text = Traducir("aviso_titulo_perfil_encriptado");
        lblAvisoTextoPerfilEncriptado.Text = Traducir("aviso_texto_perfil_encriptado");
    }

    private void MostrarEstado(int estado)
    {
        pnlSeleccion.Visible = (estado == 1);
        pnlResultado.Visible = (estado == 2);
        lblHeaderTitulo.Text = estado == 1
            ? Traducir("nav_perfilacion_paciente")
            : Traducir("titulo_perfil_generado");
    }

    private void CargarComboPacientes()
    {
        GestorPaciente gestorPaciente = new GestorPaciente();
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;
        List<Paciente> propios = gestorPaciente.ObtenerPorPsicologo(idPsicologo);

        ddlPacientePerfil.Items.Clear();
        ddlPacientePerfil.Items.Add(new ListItem(Traducir("opt_seleccionar"), ""));
        foreach (Paciente p in propios.OrderBy(x => x.Apellido))
        {
            ddlPacientePerfil.Items.Add(new ListItem(p.Nombre + " " + p.Apellido, p.IdPaciente.ToString()));
        }
    }

    protected void ddlPacientePerfil_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        hfModeloSeleccionado.Value = string.Empty;
        MostrarEstado(1);

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
        lblPacienteEdad.Text = CalcularEdad(paciente.FechaNacimiento) + " " + Traducir("lbl_anios");
        lblPacienteConsultas.Text = TextoCantidadConsultas(cantidadConsultas);
    }

    private string TextoCantidadConsultas(int cantidad)
    {
        return cantidad + " " + (cantidad == 1 ? Traducir("txt_consulta_registrada_singular") : Traducir("txt_consultas_registradas_plural"));
    }
    private void CargarPerfilesAnteriores(int idPaciente)
    {
        GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
        List<PerfilPaciente> perfiles = gestorPerfil.ObtenerPorPaciente(idPaciente);

        DataTable dt = new DataTable();
        dt.Columns.Add("IdPerfil", typeof(int));
        dt.Columns.Add("Modelo", typeof(string));
        dt.Columns.Add("Fecha", typeof(DateTime));

        foreach (PerfilPaciente perfil in perfiles)
        {
            SeccionesPerfilPaciente secciones = gestorPerfil.ObtenerSecciones(perfil);
            dt.Rows.Add(perfil.IdPerfil, secciones != null ? secciones.NombreModelo : "Modelo", perfil.FechaGeneracion);
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
    protected void rptPerfilesAnteriores_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        if (e.CommandName != "VerPerfil") return;

        lblMensaje.Visible = false;
        int idPerfil = Convert.ToInt32(e.CommandArgument);
        MostrarPerfilGenerado(idPerfil);
        MostrarEstado(2);
    }

    protected void btnGenerar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (string.IsNullOrEmpty(ddlPacientePerfil.SelectedValue))
        {
            MostrarError(Traducir("error_seleccionar_paciente_perfil"));
            return;
        }

        string modelo = hfModeloSeleccionado.Value;
        if (string.IsNullOrEmpty(modelo))
        {
            MostrarError(Traducir("msg_seleccionar_modelo_alerta"));
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

        string nombrePaciente = ddlPacientePerfil.SelectedItem != null ? ddlPacientePerfil.SelectedItem.Text : string.Empty;

        lblResultadoMeta.Text = Traducir("lbl_paciente") + ": " + nombrePaciente + " · " + secciones.NombreModelo;
        lblModeloUsado.Text = "🧠 " + Traducir("lbl_modelo_utilizado") + ": " + secciones.NombreModelo;
        lblDescripcionGeneral.Text = secciones.Descripcion;
        lblDimensiones.Text = secciones.Dimensiones;
        lblPatrones.Text = secciones.Patrones;
        lblConsideraciones.Text = secciones.Consideraciones;

        GestorConsulta gestorConsulta = new GestorConsulta();
        int cantidadConsultas = gestorConsulta.ObtenerPorPaciente(perfil.IdPaciente).Count;

        lblMetaPaciente.Text = nombrePaciente;
        lblMetaModelo.Text = secciones.NombreModelo;
        lblMetaConsultas.Text = cantidadConsultas + " " + (cantidadConsultas == 1 ? Traducir("txt_consulta_analizada_singular") : Traducir("txt_consultas_analizadas_plural"));
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