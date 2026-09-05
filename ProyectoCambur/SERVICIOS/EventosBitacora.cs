namespace SERVICIOS
{

    public static class EventosBitacora
    {

        public const string MOD_AUTENTICACION = "Autenticación";
        public const string MOD_PROFESIONALES = "Profesionales";
        public const string MOD_PACIENTES = "Pacientes";
        public const string MOD_CONSULTAS = "Consultas";
        public const string MOD_HISTORIAL_CLINICO = "Historial Clínico";
        public const string MOD_MODULO_IA = "Módulo IA";
        public const string MOD_REPORTES = "Reportes";
        public const string MOD_CONFIGURACION = "Configuración";
        public const string MOD_SUSCRIPCION = "Suscripción";
        public const string MOD_ADMINISTRACION = "Administración";
        public const string MOD_GESTION_IDIOMAS = "Gestión de Idiomas";
        public const string DESC_INICIO_SESION = "Inicio de Sesión";
        public const int CRIT_INICIO_SESION = 2;
        public const string DESC_CIERRE_SESION = "Cierre de sesión";
        public const int CRIT_CIERRE_SESION = 3;
        public const string DESC_DESBLOQUEO_MANUAL = "Desbloqueo manual de cuenta";
        public const int CRIT_DESBLOQUEO_MANUAL = 1;
        public const string DESC_SOLICITUD_RECUPERACION_CLAVE = "Solicitud de recuperación de clave por olvido";
        public const int CRIT_SOLICITUD_RECUPERACION_CLAVE = 2;
        public const string DESC_RESTABLECIMIENTO_CLAVE = "Restablecimiento de clave vía email de recuperación";
        public const int CRIT_RESTABLECIMIENTO_CLAVE = 1;
        public const string DESC_REGISTRO_PROFESIONAL = "Registro de nuevo profesional";
        public const int CRIT_REGISTRO_PROFESIONAL = 2;
        public const string DESC_REGISTRO_PACIENTE = "Registro de nuevo paciente";
        public const int CRIT_REGISTRO_PACIENTE = 2;
        public const string DESC_MODIF_PACIENTE = "Modificación de datos de paciente";
        public const int CRIT_MODIF_PACIENTE = 2;
        public const string DESC_REGISTRO_CONSULTA = "Registro de nueva consulta clínica";
        public const int CRIT_REGISTRO_CONSULTA = 2;
        public const string DESC_MODIF_CONSULTA = "Modificación de consulta clínica";
        public const int CRIT_MODIF_CONSULTA = 2;
        public const string DESC_INCORP_HISTORIAL = "Incorporación de historial clínico";
        public const int CRIT_INCORP_HISTORIAL = 2;
        public const string DESC_MODIF_HISTORIAL = "Modificación de historial clínico";
        public const int CRIT_MODIF_HISTORIAL = 2;
        public const string DESC_RESUMEN_IA = "Generación de resumen clínico asistido por IA";
        public const int CRIT_RESUMEN_IA = 2;
        public const string DESC_INFORME_DERIVACION = "Generación de informe de derivación";
        public const int CRIT_INFORME_DERIVACION = 2;
        public const string DESC_AUDITORIA_INFORME = "Auditoría de informe de derivación";
        public const int CRIT_AUDITORIA_INFORME = 2;
        public const string DESC_DESCARTE_INFORME = "Descarte de informe de derivación";
        public const int CRIT_DESCARTE_INFORME = 2;
        public const string DESC_PERFIL_EVOLUTIVO = "Generación de perfil evolutivo del paciente";
        public const int CRIT_PERFIL_EVOLUTIVO = 2;
        public const string DESC_EXPORTAR_PDF = "Exportación de reporte PDF";
        public const int CRIT_EXPORTAR_PDF = 3;
        public const string DESC_CAMBIO_CLAVE = "Cambio de clave de acceso";
        public const int CRIT_CAMBIO_CLAVE = 1;
        public const string DESC_CAMBIO_IDIOMA = "Cambio de idioma de interfaz";
        public const int CRIT_CAMBIO_IDIOMA = 3;
        public const string DESC_MODIF_SUSCRIPCION = "Modificación de plan de suscripción";
        public const int CRIT_MODIF_SUSCRIPCION = 2;
        public const string DESC_CANCELAR_SUSCRIPCION = "Cancelación de suscripción";
        public const int CRIT_CANCELAR_SUSCRIPCION = 1;
        public const string DESC_ACTUALIZAR_MEDIO_PAGO = "Actualización de medio de pago";
        public const int CRIT_ACTUALIZAR_MEDIO_PAGO = 2;
        public const string DESC_REACTIVAR_SUSCRIPCION = "Reactivación de suscripción cancelada";
        public const int CRIT_REACTIVAR_SUSCRIPCION = 2;
        public const string DESC_ALTA_SUSCRIPCION = "Alta de suscripción (pago aprobado por Mercado Pago)";
        public const int CRIT_ALTA_SUSCRIPCION = 2;
        public const string DESC_CONSULTA_BITACORA = "Consulta de bitácora";
        public const int CRIT_CONSULTA_BITACORA = 3;
        public const string DESC_GENERAR_BACKUP = "Generación de backup";
        public const int CRIT_GENERAR_BACKUP = 1;
        public const string DESC_RESTAURAR_BACKUP = "Restauración de backup";
        public const int CRIT_RESTAURAR_BACKUP = 1;
        public const string DESC_RECALCULO_DVH = "Recálculo de dígitos verificadores";
        public const int CRIT_RECALCULO_DVH = 1;
        public const string DESC_ALTA_IDIOMA = "Alta de nuevo idioma";
        public const int CRIT_ALTA_IDIOMA = 2;
        public const string DESC_MODIF_TRADUCCION = "Modificación de traducción";
        public const int CRIT_MODIF_TRADUCCION = 3;
        public const string DESC_BAJA_IDIOMA = "Baja lógica de idioma";
        public const int CRIT_BAJA_IDIOMA = 2;
    }
}