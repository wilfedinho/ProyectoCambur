using BE;
using DAL;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace BLL
{
    public class GestorBackup
    {
        public const string TIPO_BACKUP = "BACKUP";
        public const string TIPO_RESTORE = "RESTORE";
        private static readonly Regex PatronNombreArchivo = new Regex(@"^(?:SistemaCambur|Backup)_\d{8}_\d{6}\.bak$", RegexOptions.Compiled);

        private readonly BackupDAL backupDAL = new BackupDAL();
        public string ObtenerCarpetaDestino()
        {
            return backupDAL.ObtenerCarpetaBackups();
        }
        #region Generar backup
        public string GenerarBackup()
        {
            string nombreBaseDatos = backupDAL.ObtenerNombreBaseDatos();
            string carpeta = ObtenerCarpetaOLanzarError();

            string nombreArchivo = "SistemaCambur_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".bak";
            string rutaCompleta = carpeta + nombreArchivo;

            try
            {
                backupDAL.EjecutarBackup(nombreBaseDatos, rutaCompleta);
            }
            catch (SqlException ex)
            {
                RegistrarOperacion(TIPO_BACKUP, nombreArchivo, exitoso: false, detalleError: ex.Message, tamanioBytes: null);
                throw new ExcepcionTraducible(EsErrorDePermiso(ex) ? "error_backup_permiso_carpeta" : "error_backup_fallo", ex.Message);
            }
            long? tamanioBytes = backupDAL.ObtenerTamanioBackup(rutaCompleta);

            RegistrarOperacion(TIPO_BACKUP, nombreArchivo, exitoso: true, detalleError: null, tamanioBytes: tamanioBytes);
            return nombreArchivo;
        }

        #endregion

        #region Listar backups disponibles

        public List<BackupDisponible> ListarDisponibles()
        {
            string nombreBaseDatos = backupDAL.ObtenerNombreBaseDatos();
            string carpeta = ObtenerCarpetaOLanzarError();

            List<OperacionBackupRestore> registrados;
            try
            {
                registrados = backupDAL.ObtenerBackupsRegistrados();
            }
            catch (SqlException ex)
            {
                throw new ExcepcionTraducible(EsErrorDePermiso(ex) ? "error_backup_permiso_carpeta" : "error_backup_listado_fallo", ex.Message);
            }

            List<BackupDisponible> disponibles = new List<BackupDisponible>();

            foreach (OperacionBackupRestore registro in registrados)
            {
                string rutaCompleta = carpeta + registro.NombreArchivo;
                bool existe;

                try
                {
                    existe = backupDAL.ArchivoExiste(rutaCompleta);
                }
                catch (SqlException ex)
                {
                    throw new ExcepcionTraducible(EsErrorDePermiso(ex) ? "error_backup_permiso_carpeta" : "error_backup_listado_fallo", ex.Message);
                }

                if (!existe)
                {
                    continue;
                }

                disponibles.Add(new BackupDisponible
                {
                    NombreArchivo = registro.NombreArchivo,
                    Fecha = registro.FechaOperacion,
                    TamanioBytes = registro.TamanioBytes ?? 0,
                    NombreBaseDatosOrigen = nombreBaseDatos,
                    CoincideBaseDatos = true
                });
            }

            return disponibles.OrderByDescending(b => b.Fecha).ToList();
        }

        #endregion

        #region Restaurar backup
        public void RestaurarBackup(string nombreArchivo)
        {
            if (string.IsNullOrWhiteSpace(nombreArchivo) || !PatronNombreArchivo.IsMatch(nombreArchivo))
            {
                throw new ExcepcionTraducible("error_backup_archivo_invalido");
            }

            string nombreBaseDatos = backupDAL.ObtenerNombreBaseDatos();
            string carpeta = ObtenerCarpetaOLanzarError();

            List<BackupDisponible> disponibles = ListarDisponibles();
            BackupDisponible elegido = disponibles.FirstOrDefault(b => b.NombreArchivo == nombreArchivo);

            if (elegido == null)
            {
                throw new ExcepcionTraducible("error_backup_archivo_invalido");
            }

            string rutaCompleta = carpeta + nombreArchivo;
            List<OperacionBackupRestore> registradosAntes = backupDAL.ObtenerTodos();

            try
            {
                backupDAL.EjecutarRestore(nombreBaseDatos, rutaCompleta);
            }
            catch (SqlException ex)
            {
                RegistrarOperacion(TIPO_RESTORE, nombreArchivo, exitoso: false, detalleError: ex.Message, tamanioBytes: null);
                throw new ExcepcionTraducible(EsErrorDePermiso(ex) ? "error_restore_permiso_carpeta" : "error_restore_fallo", ex.Message);
            }

            ReincorporarHistorialPerdido(registradosAntes);
            RegistrarOperacion(TIPO_RESTORE, nombreArchivo, exitoso: true, detalleError: null, tamanioBytes: null);
        }

        private void ReincorporarHistorialPerdido(List<OperacionBackupRestore> registradosAntes)
        {
            try
            {
                HashSet<string> yaRegistrados = new HashSet<string>(
                    backupDAL.ObtenerBackupsRegistrados().Select(o => o.NombreArchivo),
                    StringComparer.OrdinalIgnoreCase);

                var backupsAPerderse = registradosAntes
                    .Where(o => o.TipoOperacion == TIPO_BACKUP
                                && o.Resultado != null
                                && o.Resultado.StartsWith("Completado", StringComparison.OrdinalIgnoreCase)
                                && !yaRegistrados.Contains(o.NombreArchivo))
                    .OrderBy(o => o.FechaOperacion);

                foreach (OperacionBackupRestore operacion in backupsAPerderse)
                {
                    OperacionBackupRestore reincorporada = new OperacionBackupRestore
                    {
                        TipoOperacion = operacion.TipoOperacion,
                        NombreArchivo = operacion.NombreArchivo,
                        FechaOperacion = operacion.FechaOperacion,
                        Resultado = operacion.Resultado,
                        DetalleError = operacion.DetalleError,
                        Usuario = operacion.Usuario,
                        TamanioBytes = operacion.TamanioBytes
                    };

                    backupDAL.AltaHistorial(reincorporada);
                    new DigitoVerificador().ActualizarDVH(reincorporada, "HistorialBackupRestore");
                }
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Historial

        public List<OperacionBackupRestore> ObtenerHistorial(int cantidad = 10)
        {
            return backupDAL.ObtenerHistorial(cantidad);
        }

        #endregion

        #region Utilidades privadas

        private string ObtenerCarpetaOLanzarError()
        {
            string carpeta = backupDAL.ObtenerCarpetaBackups();
            if (string.IsNullOrWhiteSpace(carpeta))
            {
                throw new ExcepcionTraducible("error_backup_carpeta_no_configurada");
            }
            return carpeta;
        }
        private bool EsErrorDePermiso(SqlException ex)
        {
            if (ex.Number == 3201 || ex.Number == 3013 || ex.Number == 297)
            {
                return true;
            }
            return ex.Message.IndexOf("Access is denied", StringComparison.OrdinalIgnoreCase) >= 0
                || ex.Message.IndexOf("Acceso denegado", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void RegistrarOperacion(string tipo, string nombreArchivo, bool exitoso, string detalleError, long? tamanioBytes)
        {
            string usuario = GestorSesion.EstaAutenticado ? GestorSesion.PsicologoActual.Email : null;

            try
            {
                OperacionBackupRestore operacion = new OperacionBackupRestore
                {
                    TipoOperacion = tipo,
                    NombreArchivo = nombreArchivo,
                    FechaOperacion = DateTime.Now,
                    Resultado = exitoso ? "Completado correctamente" : "Error: " + detalleError,
                    DetalleError = detalleError,
                    Usuario = usuario,
                    TamanioBytes = tamanioBytes
                };
                backupDAL.AltaHistorial(operacion);
                new DigitoVerificador().ActualizarDVH(operacion, "HistorialBackupRestore");
            }
            catch (Exception)
            {
            }

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(
                EventosBitacora.MOD_ADMINISTRACION,
                tipo == TIPO_BACKUP ? EventosBitacora.DESC_GENERAR_BACKUP : EventosBitacora.DESC_RESTAURAR_BACKUP,
                tipo == TIPO_BACKUP ? EventosBitacora.CRIT_GENERAR_BACKUP : EventosBitacora.CRIT_RESTAURAR_BACKUP);
        }

        #endregion
    }
}