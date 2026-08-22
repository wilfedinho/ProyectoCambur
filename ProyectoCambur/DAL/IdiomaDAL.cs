using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class IdiomaDAL
    {
        #region Operaciones Idioma

        public void Alta(Idioma nuevoIdioma)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO Idioma (nombre_idioma, codigo_iso, is_disponible, is_ocupado) " +
                               "VALUES (@nombre_idioma, @codigo_iso, @is_disponible, @is_ocupado)";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@nombre_idioma", nuevoIdioma.NombreIdioma);
                    comando.Parameters.AddWithValue("@codigo_iso", nuevoIdioma.CodigoIso);
                    comando.Parameters.AddWithValue("@is_disponible", true);
                    comando.Parameters.AddWithValue("@is_ocupado", false);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Activar(string nombreIdioma)
        {
            CambiarDisponibilidad(nombreIdioma, true);
        }

        public void Desactivar(string nombreIdioma)
        {
            CambiarDisponibilidad(nombreIdioma, false);
        }

        private void CambiarDisponibilidad(string nombreIdioma, bool disponible)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Idioma SET is_disponible = @is_disponible WHERE nombre_idioma = @nombre_idioma";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@nombre_idioma", nombreIdioma);
                    comando.Parameters.AddWithValue("@is_disponible", disponible);
                    comando.ExecuteNonQuery();
                }
            }
        }

        
        public void ActualizarIsOcupadoCache(string nombreIdioma, bool ocupado)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Idioma SET is_ocupado = @is_ocupado WHERE nombre_idioma = @nombre_idioma";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@nombre_idioma", nombreIdioma);
                    comando.Parameters.AddWithValue("@is_ocupado", ocupado);
                    comando.ExecuteNonQuery();
                }
            }
        }

        
        public bool ExisteProfesionalUsandoIdioma(string nombreIdioma)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT COUNT(1) FROM Profesional WHERE idioma = @idioma AND activo = 1";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@idioma", nombreIdioma);
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }
        }

        #endregion

        #region Busquedas Idioma

        public bool ExisteIdioma(string nombreIdioma)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT COUNT(1) FROM Idioma WHERE nombre_idioma = @nombre_idioma";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@nombre_idioma", nombreIdioma);
                    return Convert.ToInt32(comando.ExecuteScalar()) > 0;
                }
            }
        }

        public Idioma BuscarPorNombre(string nombreIdioma)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Idioma WHERE nombre_idioma = @nombre_idioma";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@nombre_idioma", nombreIdioma);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarIdioma(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<Idioma> ObtenerTodos()
        {
            List<Idioma> lista = new List<Idioma>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Idioma ORDER BY nombre_idioma";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarIdioma(reader));
                    }
                }
            }

            return lista;
        }

        private Idioma ArmarIdioma(SqlDataReader reader)
        {
            return new Idioma(
                reader["nombre_idioma"].ToString(),
                reader["codigo_iso"] == DBNull.Value ? null : reader["codigo_iso"].ToString(),
                Convert.ToBoolean(reader["is_disponible"]),
                Convert.ToBoolean(reader["is_ocupado"])
            );
        }

        #endregion
    }
}