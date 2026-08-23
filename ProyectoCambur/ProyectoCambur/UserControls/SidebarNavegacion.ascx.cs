using SERVICIOS;
using System;
using System.Text;
using System.Web;


public partial class SidebarNavegacion : System.Web.UI.UserControl
{
 
    public string PaginaActual { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado) return;

        GUI.PaginaBase paginaBase = Page as GUI.PaginaBase;
        string rol = GestorSesion.PsicologoActual.RolPermiso;

        StringBuilder html = new StringBuilder();

   
        AgregarItem(html, paginaBase, "menu_inicio", "🏠", DestinoMenuSegunRol(rol), "inicio");

        if (rol == "Web Master")
        {
            AgregarSeccion(html, paginaBase, "seccion_herramientas_sistema");
            AgregarItem(html, paginaBase, "nav_auditoria_bitacora", "📜", "FormAuditoriaBitacora.aspx", "bitacora");
            AgregarItem(html, paginaBase, "nav_backup_restore", "💾", "FormBackupRestore.aspx", "backup");
            AgregarItem(html, paginaBase, "nav_digito_verificador", "🔐", "FormDigitoVerificador.aspx", "integridad");
        }
        else if (rol == "Administrador")
        {
            AgregarSeccion(html, paginaBase, "seccion_gestion_sistema");
            AgregarItem(html, paginaBase, "nav_abm_profesionales", "👥", "FormMaestroProfesional.aspx", "profesionales");
         
            AgregarItem(html, paginaBase, "nav_abm_pacientes", "🧑‍⚕️", "FormMaestroPaciente.aspx", "pacientes");
            AgregarItem(html, paginaBase, "nav_abm_consultas", "🗒️", "FormMaestroConsulta.aspx", "consultas");
            AgregarItem(html, paginaBase, "nav_gestionar_idiomas", "🌐", "FormGestionIdiomas.aspx", "idiomas");
            AgregarItem(html, paginaBase, "nav_gestionar_permisos", "🛡️", "FormGestionPermisos.aspx", "permisos");
        }
        else
        {
     
            AgregarSeccion(html, paginaBase, "seccion_gestion_clinica");
            AgregarItem(html, paginaBase, "nav_registrar_paciente", "👤", "FormRegistrarPaciente.aspx", "pacientes");
            AgregarItem(html, paginaBase, "nav_realizar_consulta", "🗒️", "FormRealizarConsulta.aspx", "consulta_nueva");
            AgregarItem(html, paginaBase, "nav_generar_historial", "📋", "FormHistorialClinico.aspx", "historial");
            AgregarItem(html, paginaBase, "nav_modificar_consulta", "✏️", "FormModificarConsulta.aspx", "consulta_modificar");
            AgregarItem(html, paginaBase, "nav_linea_temporal", "📅", "FormLineaTemporal.aspx", "linea_temporal");

            AgregarSeccion(html, paginaBase, "seccion_modulo_ia");
            AgregarItem(html, paginaBase, "nav_resumen_ia", "🤖", "FormResumenIA.aspx", "resumen_ia");
            AgregarItem(html, paginaBase, "nav_informe_derivacion", "📤", "FormInformeDerivacion.aspx", "informe_derivacion");
            AgregarItem(html, paginaBase, "nav_auditoria_informe", "✅", "FormInformeDerivacion.aspx", "auditoria_informe");
            AgregarItem(html, paginaBase, "nav_perfilacion_paciente", "🧠", "FormPerfilPaciente.aspx", "perfilacion");

            AgregarSeccion(html, paginaBase, "seccion_operativo_config");
            AgregarItem(html, paginaBase, "nav_dashboard", "📊", "FormDashboard.aspx", "dashboard");
            AgregarItem(html, paginaBase, "nav_exportar_reporte", "💾", "FormExportarReporte.aspx", "exportar");
            AgregarItem(html, paginaBase, "nav_gestionar_suscripcion", "💳", "FormSuscripcion.aspx", "suscripcion");
        }

        litNav.Text = html.ToString();
    }

    private string DestinoMenuSegunRol(string rol)
    {
        switch (rol)
        {
            case "Administrador": return "FormMenuAdministrador.aspx";
            case "Web Master": return "FormMenuWebMaster.aspx";
            default: return "FormMenuProfesional.aspx";
        }
    }

    private void AgregarSeccion(StringBuilder html, GUI.PaginaBase paginaBase, string clave)
    {
        string texto = paginaBase != null ? paginaBase.Traducir(clave) : clave;
        html.Append("<div class=\"nav-seccion-titulo\">").Append(HttpUtility.HtmlEncode(texto)).Append("</div>");
    }

    private void AgregarItem(StringBuilder html, GUI.PaginaBase paginaBase, string clave, string icono, string url, string clavePagina)
    {
        string texto = paginaBase != null ? paginaBase.Traducir(clave) : clave;
        string cssActivo = (PaginaActual == clavePagina) ? " active" : "";

        html.Append("<a href=\"").Append(HttpUtility.HtmlEncode(url)).Append("\" class=\"nav-item").Append(cssActivo).Append("\">")
            .Append("<span>").Append(icono).Append("</span> ")
            .Append(HttpUtility.HtmlEncode(texto))
            .Append("</a>");
    }
}