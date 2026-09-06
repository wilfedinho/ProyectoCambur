using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BackupDAL
    {
        public string ObtenerNombreBaseDatos()
        {
            return GestorConexion.GestorCone.ObtenerNombreBaseDatos();
        }

        #region Carpeta de backups (la propia instancia de SQL Server, garantizado escribible por ella)

        public string ObtenerCarpetaBackups()
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                using (SqlCommand comando = new SqlCommand(
                    "SELECT CAST(SERVERPROPERTY('InstanceDefaultBackupPath') AS NVARCHAR(260))", cone))
                {
                    object resultado = comando.ExecuteScalar();
                    string carpeta = resultado == DBNull.Value || resultado == null ? null : resultado.ToString();

                    if (string.IsNullOrWhiteSpace(carpeta))
                    {
                        return null;
                    }

                    if (!carpeta.EndsWith("\\"))
                    {
                        carpeta += "\\";
                    }

                    return carpeta;
                }
            }
        }

        #endregion

        #region Generar backup

        public void EjecutarBackup(string nombreBaseDatos, string rutaCompleta)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "BACKUP DATABASE " + EscaparIdentificador(nombreBaseDatos) +
                               " TO DISK = @ruta WITH INIT, CHECKSUM";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.CommandTimeout = 0; 
                    comando.Parameters.AddWithValue("@ruta", rutaCompleta);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Listar backups disponibles

        public List<BackupDisponible> ListarDisponibles(string nombreBaseDatos, string carpeta)
        {
            List<string> nombresArchivo = new List<string>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                string query =
                    "IF OBJECT_ID('tempdb..#archivosBackup') IS NOT NULL DROP TABLE #archivosBackup; " +
                    "CREATE TABLE #archivosBackup (Subdirectory NVARCHAR(512), Depth INT, IsFile BIT); " +
                    "INSERT INTO #archivosBackup EXEC master.dbo.xp_dirtree @carpeta, 1, 1; " +
                    "SELECT Subdirectory FROM #archivosBackup WHERE IsFile = 1 " +
                    "AND (Subdirectory LIKE 'SistemaCambur[_]%.bak' OR Subdirectory LIKE 'Backup[_]%.bak') " +
                    "ORDER BY Subdirectory DESC;";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@carpeta", carpeta);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            nombresArchivo.Add(reader["Subdirectory"].ToString());
                        }
                    }
                }
            }

            List<BackupDisponible> disponibles = new List<BackupDisponible>();

            foreach (string nombreArchivo in nombresArchivo)
            {
                BackupDisponible info = LeerCabeceraBackup(carpeta + nombreArchivo, nombreArchivo);
                if (info != null)
                {
                    info.CoincideBaseDatos = string.Equals(info.NombreBaseDatosOrigen, nombreBaseDatos, StringComparison.OrdinalIgnoreCase);
                    disponibles.Add(info);
                }
            }

            return disponibles;
        }
        private BackupDisponible LeerCabeceraBackup(string rutaCompleta, string nombreArchivo)
        {
            try
            {
                using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
                {
                    cone.Open();
                    using (SqlCommand comando = new SqlCommand("RESTORE HEADERONLY FROM DISK = @ruta", cone))
                    {
                        comando.Parameters.AddWithValue("@ruta", rutaCompleta);
                        using (SqlDataReader reader = comando.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new BackupDisponible
                                {
                                    NombreArchivo = nombreArchivo,
                                    Fecha = Convert.ToDateTime(reader["BackupStartDate"]),
                                    TamanioBytes = Convert.ToInt64(Convert.ToDouble(reader["BackupSize"])),
                                    NombreBaseDatosOrigen = reader["DatabaseName"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                return null;
            }

            return null;
        }

        #endregion

        #region Restaurar backup
        public void EjecutarRestore(string nombreBaseDatos, string rutaCompleta)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexionMaster())
            {
                cone.Open();
                using (SqlCommand comando = new SqlCommand("dbo.sp_CamburRestaurarBackup", cone))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.CommandTimeout = 0;
                    comando.Parameters.AddWithValue("@NombreBaseDatos", nombreBaseDatos);
                    comando.Parameters.AddWithValue("@RutaArchivo", rutaCompleta);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Historial de operaciones (propio del módulo, no confundir con Bitacora general)

        public void AltaHistorial(OperacionBackupRestore operacion)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO HistorialBackupRestore " +
                               "(tipo_operacion, nombre_archivo, fecha_operacion, resultado, detalle_error, usuario) " +
                               "VALUES (@tipo, @archivo, @fecha, @resultado, @detalle, @usuario)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@tipo", operacion.TipoOperacion);
                    comando.Parameters.AddWithValue("@archivo", operacion.NombreArchivo);
                    comando.Parameters.AddWithValue("@fecha", operacion.FechaOperacion);
                    comando.Parameters.AddWithValue("@resultado", operacion.Resultado);
                    comando.Parameters.AddWithValue("@detalle", (object)operacion.DetalleError ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@usuario", (object)operacion.Usuario ?? DBNull.Value);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public List<OperacionBackupRestore> ObtenerHistorial(int cantidad)
        {
            List<OperacionBackupRestore> lista = new List<OperacionBackupRestore>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT TOP (@cantidad) id_historial, tipo_operacion, nombre_archivo, fecha_operacion, resultado, detalle_error, usuario " +
                               "FROM HistorialBackupRestore ORDER BY fecha_operacion DESC";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@cantidad", cantidad);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new OperacionBackupRestore
                            {
                                IdHistorial = Convert.ToInt32(reader["id_historial"]),
                                TipoOperacion = reader["tipo_operacion"].ToString(),
                                NombreArchivo = reader["nombre_archivo"].ToString(),
                                FechaOperacion = Convert.ToDateTime(reader["fecha_operacion"]),
                                Resultado = reader["resultado"].ToString(),
                                DetalleError = reader["detalle_error"] == DBNull.Value ? null : reader["detalle_error"].ToString(),
                                Usuario = reader["usuario"] == DBNull.Value ? null : reader["usuario"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        #endregion

        private string EscaparIdentificador(string nombre)
        {
            return "[" + nombre.Replace("]", "]]") + "]";
        }
    }
}