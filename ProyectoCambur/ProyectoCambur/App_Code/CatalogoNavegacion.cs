using System.Collections.Generic;

namespace GUI
{
    public class ItemMenu
    {
        public string Icono;
        public string Url;
        public string ClaveTitulo;
        public string ClaveDescripcion;

        public ItemMenu(string icono, string url, string claveTitulo, string claveDescripcion)
        {
            Icono = icono;
            Url = url;
            ClaveTitulo = claveTitulo;
            ClaveDescripcion = claveDescripcion;
        }
    }
    public static class CatalogoNavegacion
    {
        private static readonly Dictionary<string, ItemMenu> Items = new Dictionary<string, ItemMenu>
        {
            { "acceder_auditoria_bitacora",   new ItemMenu("📜", "FormAuditoriaBitacora.aspx",   "nav_auditoria_bitacora",   "tile_bitacora_desc") },
            { "acceder_backup_restore",       new ItemMenu("💾", "FormBackupRestore.aspx",       "nav_backup_restore",      "tile_backup_desc") },
            { "acceder_digito_verificador",   new ItemMenu("🔐", "FormDigitoVerificador.aspx",   "nav_digito_verificador",  "tile_integridad_desc") },
            { "acceder_abm_profesionales",    new ItemMenu("👥", "FormMaestroProfesional.aspx",  "nav_abm_profesionales",   "tile_profesionales_desc") },
            { "acceder_abm_pacientes",        new ItemMenu("🧑‍⚕️", "FormMaestroPaciente.aspx",   "nav_abm_pacientes",       "tile_abm_pacientes_desc") },
            { "acceder_abm_consultas",        new ItemMenu("🗒️", "FormMaestroConsulta.aspx",     "nav_abm_consultas",       "tile_abm_consultas_desc") },
            { "acceder_gestionar_idiomas",    new ItemMenu("🌐", "FormGestionIdiomas.aspx",      "nav_gestionar_idiomas",   "tile_idiomas_desc") },
            { "acceder_gestionar_permisos",   new ItemMenu("🛡️", "FormGestionPermisos.aspx",     "nav_gestionar_permisos",  "tile_permisos_desc") },
            { "acceder_registrar_paciente",   new ItemMenu("👤", "FormRegistrarPaciente.aspx",   "nav_registrar_paciente",  "tile_registrar_paciente_desc") },
            { "acceder_realizar_consulta",    new ItemMenu("🗒️", "FormRealizarConsulta.aspx",    "nav_realizar_consulta",   "tile_realizar_consulta_desc") },
            { "acceder_generar_historial",    new ItemMenu("📋", "FormHistorialClinico.aspx",    "nav_generar_historial",   "tile_generar_historial_desc") },
            { "acceder_modificar_consulta",   new ItemMenu("✏️", "FormModificarConsulta.aspx",   "nav_modificar_consulta",  "tile_modificar_consulta_desc") },
            { "acceder_linea_temporal",       new ItemMenu("📅", "FormLineaTemporal.aspx",       "nav_linea_temporal",      "tile_linea_temporal_desc") },
            { "acceder_resumen_ia",           new ItemMenu("🤖", "FormResumenIA.aspx",           "nav_resumen_ia",          "tile_resumen_ia_desc") },
            { "acceder_informe_derivacion",   new ItemMenu("📤", "FormInformeDerivacion.aspx",   "nav_informe_derivacion",  "tile_informe_derivacion_desc") },
            { "acceder_auditoria_informe",    new ItemMenu("✅", "FormInformeDerivacion.aspx",   "nav_auditoria_informe",   "tile_auditoria_informe_desc") },
            { "acceder_perfilacion_paciente", new ItemMenu("🧠", "FormPerfilPaciente.aspx",      "nav_perfilacion_paciente","tile_perfilacion_desc") },
            { "acceder_dashboard",            new ItemMenu("📊", "FormDashboard.aspx",           "nav_dashboard",           "tile_dashboard_desc") },
            { "acceder_exportar_reporte",     new ItemMenu("💾", "FormExportarReporte.aspx",     "nav_exportar_reporte",    "tile_exportar_desc") },
            { "acceder_gestionar_suscripcion",new ItemMenu("💳", "FormSuscripcion.aspx",         "nav_gestionar_suscripcion","tile_suscripcion_desc") },
        };
        private static readonly Dictionary<string, string> Secciones = new Dictionary<string, string>
        {
            { "HerramientasSistema",   "seccion_herramientas_sistema" },
            { "GestionSistema",        "seccion_gestion_sistema" },
            { "GestionClinica",        "seccion_gestion_clinica" },
            { "ModuloIA",              "seccion_modulo_ia" },
            { "OperativoConfiguracion","seccion_operativo_config" },
        };

        public static ItemMenu ObtenerInfo(string nombrePermisoSimple)
        {
            return Items.TryGetValue(nombrePermisoSimple, out ItemMenu info) ? info : null;
        }

        public static string ObtenerClaveSeccion(string nombreFamilia)
        {
            return Secciones.TryGetValue(nombreFamilia, out string clave) ? clave : null;
        }
    }
}