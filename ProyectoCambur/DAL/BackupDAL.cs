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

        public List<OperacionBackupRestore> ObtenerBackupsRegistrados()
        {
            List<OperacionBackupRestore> lista = new List<OperacionBackupRestore>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM HistorialBackupRestore " +
                               "WHERE tipo_operacion = @tipo AND resultado LIKE 'Completado%' " +
                               "ORDER BY fecha_operacion DESC";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@tipo", "BACKUP");
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(ArmarOperacion(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public bool ArchivoExiste(string rutaCompleta)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                using (SqlCommand comando = new SqlCommand(
                    "SELECT file_exists FROM sys.dm_os_file_exists(@ruta)", cone))
                {
                    comando.Parameters.AddWithValue("@ruta", rutaCompleta);
                    object resultado = comando.ExecuteScalar();
                    return resultado != null && resultado != DBNull.Value && Convert.ToInt32(resultado) == 1;
                }
            }
        }

        public long? ObtenerTamanioBackup(string rutaCompleta)
        {
            try
            {
                using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
                {
                    cone.Open();
                    string query = @"
                        SELECT TOP 1 bs.backup_size
                        FROM msdb.dbo.backupset bs
                        JOIN msdb.dbo.backupmediafamily mf ON mf.media_set_id = bs.media_set_id
                        WHERE mf.physical_device_name = @ruta
                        ORDER BY bs.backup_finish_date DESC";

                    using (SqlCommand comando = new SqlCommand(query, cone))
                    {
                        comando.Parameters.AddWithValue("@ruta", rutaCompleta);
                        object resultado = comando.ExecuteScalar();
                        return resultado == null || resultado == DBNull.Value ? (long?)null : Convert.ToInt64(Convert.ToDouble(resultado));
                    }
                }
            }
            catch (SqlException)
            {
                return null;
            }
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

        public int AltaHistorial(OperacionBackupRestore operacion)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO HistorialBackupRestore " +
                               "(tipo_operacion, nombre_archivo, fecha_operacion, resultado, detalle_error, usuario, digito_verificador, tamanio_bytes) " +
                               "VALUES (@tipo, @archivo, @fecha, @resultado, @detalle, @usuario, @digito_verificador, @tamanio_bytes); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@tipo", operacion.TipoOperacion);
                    comando.Parameters.AddWithValue("@archivo", operacion.NombreArchivo);
                    comando.Parameters.AddWithValue("@fecha", operacion.FechaOperacion);
                    comando.Parameters.AddWithValue("@resultado", operacion.Resultado);
                    comando.Parameters.AddWithValue("@detalle", (object)operacion.DetalleError ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@usuario", (object)operacion.Usuario ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)operacion.DigitoVerificador ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@tamanio_bytes", (object)operacion.TamanioBytes ?? DBNull.Value);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    operacion.IdHistorial = idGenerado;
                    return idGenerado;
                }
            }
        }

        public OperacionBackupRestore BuscarPorId(int idHistorial)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM HistorialBackupRestore WHERE id_historial = @id_historial";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_historial", idHistorial);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarOperacion(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<OperacionBackupRestore> ObtenerTodos()
        {
            List<OperacionBackupRestore> lista = new List<OperacionBackupRestore>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM HistorialBackupRestore ORDER BY id_historial";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarOperacion(reader));
                    }
                }
            }

            return lista;
        }

        private OperacionBackupRestore ArmarOperacion(SqlDataReader reader)
        {
            return new OperacionBackupRestore
            {
                IdHistorial = Convert.ToInt32(reader["id_historial"]),
                TipoOperacion = reader["tipo_operacion"].ToString(),
                NombreArchivo = reader["nombre_archivo"].ToString(),
                FechaOperacion = Convert.ToDateTime(reader["fecha_operacion"]),
                Resultado = reader["resultado"].ToString(),
                DetalleError = reader["detalle_error"] == DBNull.Value ? null : reader["detalle_error"].ToString(),
                Usuario = reader["usuario"] == DBNull.Value ? null : reader["usuario"].ToString(),
                DigitoVerificador = reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString(),
                TamanioBytes = reader["tamanio_bytes"] == DBNull.Value ? (long?)null : Convert.ToInt64(reader["tamanio_bytes"])
            };
        }

        public List<OperacionBackupRestore> ObtenerHistorial(int cantidad)
        {
            List<OperacionBackupRestore> lista = new List<OperacionBackupRestore>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT TOP (@cantidad) id_historial, tipo_operacion, nombre_archivo, fecha_operacion, resultado, detalle_error, usuario, digito_verificador, tamanio_bytes " +
                               "FROM HistorialBackupRestore ORDER BY fecha_operacion DESC";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@cantidad", cantidad);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(ArmarOperacion(reader));
                        }
                    }
                }
            }

            return lista;
        }

        #endregion

        #region Digito Verificador (DVH)

        public List<string> ObtenerListaDVH()
        {
            List<string> lista = new List<string>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT digito_verificador FROM HistorialBackupRestore ORDER BY id_historial";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(reader["digito_verificador"] == DBNull.Value ? string.Empty : reader["digito_verificador"].ToString());
                    }
                }
            }

            return lista;
        }

        public void ActualizarDVH(int idHistorial, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE HistorialBackupRestore SET digito_verificador = @digito_verificador WHERE id_historial = @id_historial";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_historial", idHistorial);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        private string EscaparIdentificador(string nombre)
        {
            return "[" + nombre.Replace("]", "]]") + "]";
        }
    }
}