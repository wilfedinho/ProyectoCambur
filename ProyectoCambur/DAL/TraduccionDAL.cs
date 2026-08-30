using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class TraduccionDAL
    {
        #region Alta masiva (SqlBulkCopy)

        public void AltaMasiva(List<Traduccion> traducciones)
        {
            if (traducciones == null || traducciones.Count == 0) return;

            DataTable tabla = new DataTable();
            tabla.Columns.Add("idioma", typeof(string));
            tabla.Columns.Add("clave", typeof(string));
            tabla.Columns.Add("texto", typeof(string));
            tabla.Columns.Add("pendiente", typeof(bool));

            foreach (Traduccion t in traducciones)
            {
                tabla.Rows.Add(t.Idioma, t.Clave, t.Texto, t.Pendiente);
            }

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cone))
                {
                    bulkCopy.DestinationTableName = "Traduccion";
                    bulkCopy.BatchSize = 500;
                    bulkCopy.BulkCopyTimeout = 120;

                    bulkCopy.ColumnMappings.Add("idioma", "idioma");
                    bulkCopy.ColumnMappings.Add("clave", "clave");
                    bulkCopy.ColumnMappings.Add("texto", "texto");
                    bulkCopy.ColumnMappings.Add("pendiente", "pendiente");


                    bulkCopy.WriteToServer(tabla);
                }
            }
        }

        #endregion

        #region Operaciones Traduccion

        public void ModificarTexto(int idTraduccion, string nuevoTexto)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                string query = "UPDATE Traduccion SET texto = @texto, pendiente = 0 WHERE id_traduccion = @id_traduccion";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_traduccion", idTraduccion);
                    comando.Parameters.AddWithValue("@texto", nuevoTexto);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Digito Verificador (DVH)
        public List<string> ObtenerListaDVH()
        {
            List<string> lista = new List<string>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT digito_verificador FROM Traduccion ORDER BY id_traduccion";
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

        public void ActualizarDVH(int idTraduccion, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Traduccion SET digito_verificador = @digito_verificador WHERE id_traduccion = @id_traduccion";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_traduccion", idTraduccion);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public Traduccion BuscarPorId(int idTraduccion)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Traduccion WHERE id_traduccion = @id_traduccion";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_traduccion", idTraduccion);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Traduccion(
                                Convert.ToInt32(reader["id_traduccion"]),
                                reader["idioma"].ToString(),
                                reader["clave"].ToString(),
                                reader["texto"].ToString(),
                                Convert.ToBoolean(reader["pendiente"]),
                                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
                            );
                        }
                    }
                }
            }

            return null;
        }

        public List<Traduccion> ObtenerTodas()
        {
            List<Traduccion> lista = new List<Traduccion>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Traduccion ORDER BY id_traduccion";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Traduccion(
                            Convert.ToInt32(reader["id_traduccion"]),
                            reader["idioma"].ToString(),
                            reader["clave"].ToString(),
                            reader["texto"].ToString(),
                            Convert.ToBoolean(reader["pendiente"]),
                            reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
                        ));
                    }
                }
            }

            return lista;
        }

        #endregion

        #region Operaciones Traduccion (borrado)

        public void EliminarTraduccionesDeIdioma(string nombreIdioma)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "DELETE FROM Traduccion WHERE idioma = @idioma";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@idioma", nombreIdioma);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Busquedas Traduccion


        public Dictionary<string, string> ObtenerTraduccionesDeIdioma(string nombreIdioma)
        {
            Dictionary<string, string> diccionario = new Dictionary<string, string>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT clave, texto FROM Traduccion WHERE idioma = @idioma";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@idioma", nombreIdioma);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string clave = reader["clave"].ToString();
                            if (!diccionario.ContainsKey(clave))
                            {
                                diccionario.Add(clave, reader["texto"].ToString());
                            }
                        }
                    }
                }
            }

            return diccionario;
        }


        public List<Traduccion> ObtenerTodasLasClaves(string nombreIdiomaReferencia)
        {
            List<Traduccion> lista = new List<Traduccion>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Traduccion WHERE idioma = @idioma";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@idioma", nombreIdiomaReferencia);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Traduccion(
                                Convert.ToInt32(reader["id_traduccion"]),
                                reader["idioma"].ToString(),
                                reader["clave"].ToString(),
                                reader["texto"].ToString(),
                                Convert.ToBoolean(reader["pendiente"])
                            ));
                        }
                    }
                }
            }

            return lista;
        }

        public List<Traduccion> ObtenerPendientes(string nombreIdioma)
        {
            List<Traduccion> lista = new List<Traduccion>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Traduccion WHERE idioma = @idioma AND pendiente = 1 ORDER BY clave";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@idioma", nombreIdioma);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Traduccion(
                                Convert.ToInt32(reader["id_traduccion"]),
                                reader["idioma"].ToString(),
                                reader["clave"].ToString(),
                                reader["texto"].ToString(),
                                Convert.ToBoolean(reader["pendiente"])
                            ));
                        }
                    }
                }
            }

            return lista;
        }

        public List<Traduccion> ObtenerTodasPorIdioma(string nombreIdioma)
        {
            return ObtenerTodasLasClaves(nombreIdioma);
        }

        #endregion
    }
}