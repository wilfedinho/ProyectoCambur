using BE;
using BLL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using GUI;

public partial class FormExportarReporte : PaginaBase
{
    private readonly Dictionary<string, List<DocumentoExportable>> documentosPorTipo = new Dictionary<string, List<DocumentoExportable>>();
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
        if (!gestorPermiso.TienePermiso(psicologoActual.RolPermiso, "acceder_exportar_reporte"))
        {
            DenegarAcceso();
            return;
        }

        AplicarTraducciones();

        if (!IsPostBack)
        {
            CargarComboPacientes();
            if (ddlPaciente.Items.Count > 0)
            {
                CargarDocumentosDisponibles(int.Parse(ddlPaciente.SelectedValue));
            }
            CargarExportacionesRecientes();
        }
    }

    private void AplicarTraducciones()
    {
        lblMenuCerrarSesionSidebar.Text = Traducir("menu_cerrar_sesion");
        lblHeaderTitulo.Text = Traducir("nav_exportar_reporte");

        lblCardTitulo.Text = Traducir("titulo_exportar_reporte");
        lblCardSubtitulo.Text = Traducir("subtitulo_exportar_reporte");
        lblSeccionPaciente.Text = Traducir("lbl_paciente");
        lblEtiquetaPaciente.Text = Traducir("lbl_paciente");
        lblSeccionTipoDoc.Text = Traducir("lbl_tipo_documento");
        lblElegirDocumento.Text = Traducir("lbl_elegir_documento");

        lblTipoResumen.Text = Traducir("doc_resumen_clinico");
        lblTipoDerivacion.Text = Traducir("doc_informe_derivacion");
        lblTipoPerfil.Text = Traducir("doc_perfil_evolutivo");

        lnkVolver.Text = Traducir("btn_volver");
        btnExportar.Text = Traducir("btn_exportar_pdf");

        lblExportacionesRecientesTitulo.Text = Traducir("titulo_exportaciones_recientes");
        lblSinExportaciones.Text = Traducir("msg_sin_exportaciones");

        lblAvisoFormatoTitulo.Text = Traducir("titulo_aviso_formato_pdf");
        lblAvisoFormatoTexto.Text = Traducir("texto_aviso_formato_pdf");
        lblAvisoProteccionTitulo.Text = Traducir("titulo_aviso_proteccion_datos");
        lblAvisoProteccionTexto.Text = Traducir("texto_aviso_proteccion_datos");
    }

    private void CargarComboPacientes()
    {
        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorPaciente gestorPaciente = new GestorPaciente();
        List<Paciente> pacientes = gestorPaciente.ObtenerPorPsicologo(psicologoActual.IdPsicologo);

        ddlPaciente.Items.Clear();
        foreach (Paciente p in pacientes.OrderBy(p => p.Apellido).ThenBy(p => p.Nombre))
        {
            ddlPaciente.Items.Add(new ListItem(p.Apellido + ", " + p.Nombre, p.IdPaciente.ToString()));
        }
    }

    public bool DisponibleResumen { get; private set; }
    public bool DisponibleDerivacion { get; private set; }
    public bool DisponiblePerfil { get; private set; }

    public string DocumentosDisponiblesJson
    {
        get
        {
            Dictionary<string, List<Dictionary<string, object>>> mapa = new Dictionary<string, List<Dictionary<string, object>>>();

            foreach (string tipo in new[] { GestorExportacion.TIPO_RESUMEN, GestorExportacion.TIPO_DERIVACION, GestorExportacion.TIPO_PERFIL })
            {
                List<Dictionary<string, object>> lista = new List<Dictionary<string, object>>();
                List<DocumentoExportable> documentos;

                if (documentosPorTipo.TryGetValue(tipo, out documentos))
                {
                    foreach (DocumentoExportable doc in documentos)
                    {
                        Dictionary<string, object> fila = new Dictionary<string, object>();
                        fila["id"] = doc.IdDocumento;
                        fila["fecha"] = doc.Fecha.ToString("dd/MM/yyyy HH:mm");
                        fila["detalle"] = doc.Detalle ?? "";
                        lista.Add(fila);
                    }
                }

                mapa[tipo] = lista;
            }

            JavaScriptSerializer serializador = new JavaScriptSerializer();
            return serializador.Serialize(mapa);
        }
    }

    private void CargarDocumentosDisponibles(int idPaciente)
    {
        GestorExportacion gestorExportacion = new GestorExportacion();
        int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;

        DisponibleResumen = gestorExportacion.DocumentoDisponible(idPsicologo, idPaciente, GestorExportacion.TIPO_RESUMEN);
        DisponibleDerivacion = gestorExportacion.DocumentoDisponible(idPsicologo, idPaciente, GestorExportacion.TIPO_DERIVACION);
        DisponiblePerfil = gestorExportacion.DocumentoDisponible(idPsicologo, idPaciente, GestorExportacion.TIPO_PERFIL);

        documentosPorTipo.Clear();
        documentosPorTipo[GestorExportacion.TIPO_RESUMEN] = DisponibleResumen
            ? gestorExportacion.ObtenerDocumentosDisponibles(idPsicologo, idPaciente, GestorExportacion.TIPO_RESUMEN)
            : new List<DocumentoExportable>();
        documentosPorTipo[GestorExportacion.TIPO_DERIVACION] = DisponibleDerivacion
            ? gestorExportacion.ObtenerDocumentosDisponibles(idPsicologo, idPaciente, GestorExportacion.TIPO_DERIVACION)
            : new List<DocumentoExportable>();
        documentosPorTipo[GestorExportacion.TIPO_PERFIL] = DisponiblePerfil
            ? gestorExportacion.ObtenerDocumentosDisponibles(idPsicologo, idPaciente, GestorExportacion.TIPO_PERFIL)
            : new List<DocumentoExportable>();

        lblEstadoResumen.Text = DisponibleResumen ? Traducir("lbl_disponible") : Traducir("lbl_no_disponible");
        lblEstadoResumen.CssClass = DisponibleResumen ? "doc-badge doc-badge-ok" : "doc-badge doc-badge-no-disponible";

        if (DisponibleResumen)
        {
            GestorResumenClinico gestorResumen = new GestorResumenClinico();
            ResumenClinico ultimo = gestorResumen.ObtenerPorPaciente(idPaciente).OrderByDescending(r => r.FechaGeneracion).FirstOrDefault();
            lblFechaResumen.Text = ultimo != null
                ? string.Format(Traducir("lbl_generado_el"), ultimo.FechaGeneracion.ToString("dd/MM/yyyy"))
                : "";
        }
        else
        {
            lblFechaResumen.Text = Traducir("lbl_sin_documento");
        }

        if (DisponibleDerivacion)
        {
            lblEstadoDerivacion.Text = Traducir("lbl_disponible");
            lblEstadoDerivacion.CssClass = "doc-badge doc-badge-ok";

            GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
            InformeDerivacion ultimo = gestorInforme.ObtenerPorPaciente(idPaciente)
                .Where(i => i.Estado == EstadoInforme.Auditado)
                .OrderByDescending(i => i.FechaAuditoria)
                .FirstOrDefault();
            lblFechaDerivacion.Text = ultimo != null && ultimo.FechaAuditoria.HasValue
                ? string.Format(Traducir("lbl_generado_el"), ultimo.FechaAuditoria.Value.ToString("dd/MM/yyyy"))
                : "";
        }
        else if (gestorExportacion.DocumentoPendienteAuditoria(idPaciente))
        {
            lblEstadoDerivacion.Text = Traducir("lbl_pendiente_auditoria");
            lblEstadoDerivacion.CssClass = "doc-badge doc-badge-pendiente";
            lblFechaDerivacion.Text = Traducir("lbl_sin_documento");
        }
        else
        {
            lblEstadoDerivacion.Text = Traducir("lbl_no_disponible");
            lblEstadoDerivacion.CssClass = "doc-badge doc-badge-no-disponible";
            lblFechaDerivacion.Text = Traducir("lbl_sin_documento");
        }

        lblEstadoPerfil.Text = DisponiblePerfil ? Traducir("lbl_disponible") : Traducir("lbl_no_disponible");
        lblEstadoPerfil.CssClass = DisponiblePerfil ? "doc-badge doc-badge-ok" : "doc-badge doc-badge-no-disponible";

        if (DisponiblePerfil)
        {
            GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
            PerfilPaciente ultimo = gestorPerfil.ObtenerPorPaciente(idPaciente).OrderByDescending(p => p.FechaGeneracion).FirstOrDefault();
            lblFechaPerfil.Text = ultimo != null
                ? string.Format(Traducir("lbl_generado_el"), ultimo.FechaGeneracion.ToString("dd/MM/yyyy"))
                : "";
        }
        else
        {
            lblFechaPerfil.Text = Traducir("lbl_sin_documento");
        }

        string tipoAutoSeleccionado;
        if (DisponibleResumen)
        {
            tipoAutoSeleccionado = GestorExportacion.TIPO_RESUMEN;
        }
        else if (DisponibleDerivacion)
        {
            tipoAutoSeleccionado = GestorExportacion.TIPO_DERIVACION;
        }
        else if (DisponiblePerfil)
        {
            tipoAutoSeleccionado = GestorExportacion.TIPO_PERFIL;
        }
        else
        {
            tipoAutoSeleccionado = "";
        }

        hfTipoSeleccionado.Value = tipoAutoSeleccionado;

        List<DocumentoExportable> documentosDelTipo;
        hfDocumentoSeleccionado.Value = !string.IsNullOrEmpty(tipoAutoSeleccionado)
            && documentosPorTipo.TryGetValue(tipoAutoSeleccionado, out documentosDelTipo)
            && documentosDelTipo.Count > 0
                ? documentosDelTipo[0].IdDocumento.ToString()
                : "";
    }

    protected string ClaseSeleccionado(string tipo)
    {
        return hfTipoSeleccionado.Value == tipo ? "seleccionado" : "";
    }

    protected string CheckIcono(string tipo)
    {
        return hfTipoSeleccionado.Value == tipo ? "●" : "○";
    }

    private void CargarExportacionesRecientes()
    {
        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorExportacion gestorExportacion = new GestorExportacion();
        GestorPaciente gestorPaciente = new GestorPaciente();

        List<Bitacora> eventos = gestorExportacion.ObtenerExportacionesRecientes(psicologoActual.IdPsicologo);

        DataTable dt = new DataTable();
        dt.Columns.Add("Icono", typeof(string));
        dt.Columns.Add("Tipo", typeof(string));
        dt.Columns.Add("Fecha", typeof(DateTime));

        foreach (Bitacora b in eventos)
        {
            dt.Rows.Add("🤖", Traducir("doc_resumen_clinico"), b.FechaEvento);
        }

        if (dt.Rows.Count > 0)
        {
            rptExportaciones.DataSource = dt;
            rptExportaciones.DataBind();
            lblSinExportaciones.Visible = false;
        }
        else
        {
            rptExportaciones.DataSource = null;
            rptExportaciones.DataBind();
            lblSinExportaciones.Visible = true;
        }
    }

    protected void ddlPaciente_SelectedIndexChanged(object sender, EventArgs e)
    {
        int idPaciente;
        if (int.TryParse(ddlPaciente.SelectedValue, out idPaciente))
        {
            CargarDocumentosDisponibles(idPaciente);
        }
        lblMensaje.Visible = false;
    }

    protected void btnExportar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        string tipo = hfTipoSeleccionado.Value;
        if (string.IsNullOrEmpty(tipo))
        {
            MostrarError(Traducir("error_documento_no_disponible"));
            return;
        }

        int idPaciente;
        if (!int.TryParse(ddlPaciente.SelectedValue, out idPaciente))
        {
            MostrarError(Traducir("error_consulta_sin_paciente"));
            return;
        }

        int? idDocumento = null;
        int idDocumentoParseado;
        if (int.TryParse(hfDocumentoSeleccionado.Value, out idDocumentoParseado))
        {
            idDocumento = idDocumentoParseado;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;
        GestorExportacion gestorExportacion = new GestorExportacion();

        try
        {
            string nombreArchivo;
            byte[] pdf = gestorExportacion.Generar(psicologoActual.IdPsicologo, idPaciente, tipo, idDocumento, out nombreArchivo);

            Response.Clear();
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment; filename=\"" + nombreArchivo + "\"");
            Response.AddHeader("Content-Length", pdf.Length.ToString());
            Response.BinaryWrite(pdf);
            Response.Flush();
            HttpContext.Current.ApplicationInstance.CompleteRequest();
        }
        catch (ExcepcionTraducible ex)
        {
            MostrarError(TraducirExcepcion(ex));
        }
        catch (Exception)
        {
            MostrarError(Traducir("error_generacion_pdf"));
        }
    }

    private string ObtenerIniciales(string nombre, string apellido)
    {
        string i1 = !string.IsNullOrEmpty(nombre) ? nombre.Substring(0, 1) : "";
        string i2 = !string.IsNullOrEmpty(apellido) ? apellido.Substring(0, 1) : "";
        return (i1 + i2).ToUpper();
    }

    private void MostrarError(string mensaje)
    {
        lblMensaje.Text = mensaje;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }
}