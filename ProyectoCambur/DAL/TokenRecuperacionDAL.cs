using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class TokenRecuperacionDAL
    {
        public int Alta(TokenRecuperacion nuevoToken)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO TokenRecuperacion (id_profesional, token_hash, fecha_generacion, fecha_expiracion, usado) " +
                               "VALUES (@id_profesional, @token_hash, @fecha_generacion, @fecha_expiracion, @usado); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", nuevoToken.IdProfesional);
                    comando.Parameters.AddWithValue("@token_hash", nuevoToken.TokenHash);
                    comando.Parameters.AddWithValue("@fecha_generacion", nuevoToken.FechaGeneracion);
                    comando.Parameters.AddWithValue("@fecha_expiracion", nuevoToken.FechaExpiracion);
                    comando.Parameters.AddWithValue("@usado", nuevoToken.Usado);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    nuevoToken.IdToken = idGenerado;
                    return idGenerado;
                }
            }
        }

        public TokenRecuperacion BuscarPorHash(string tokenHash)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM TokenRecuperacion WHERE token_hash = @token_hash";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@token_hash", tokenHash);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarToken(reader);
                        }
                    }
                }
            }

            return null;
        }

        public void MarcarUsado(int idToken)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE TokenRecuperacion SET usado = @usado WHERE id_token = @id_token";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_token", idToken);
                    comando.Parameters.AddWithValue("@usado", true);
                    comando.ExecuteNonQuery();
                }
            }
        }
        public List<TokenRecuperacion> BuscarVigentesDe(int idProfesional)
        {
            List<TokenRecuperacion> lista = new List<TokenRecuperacion>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM TokenRecuperacion WHERE id_profesional = @id_profesional AND usado = 0";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idProfesional);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(ArmarToken(reader));
                        }
                    }
                }
            }

            return lista;
        }

        #region Digito Verificador (DVH)

        public List<string> ObtenerListaDVH()
        {
            List<string> lista = new List<string>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT digito_verificador FROM TokenRecuperacion ORDER BY id_token";
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

        public void ActualizarDVH(int idToken, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE TokenRecuperacion SET digito_verificador = @digito_verificador WHERE id_token = @id_token";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_token", idToken);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public List<TokenRecuperacion> ObtenerTodos()
        {
            List<TokenRecuperacion> lista = new List<TokenRecuperacion>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM TokenRecuperacion ORDER BY id_token";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarToken(reader));
                    }
                }
            }

            return lista;
        }

        #endregion

        private TokenRecuperacion ArmarToken(SqlDataReader reader)
        {
            return new TokenRecuperacion(
                Convert.ToInt32(reader["id_token"]),
                Convert.ToInt32(reader["id_profesional"]),
                reader["token_hash"].ToString(),
                Convert.ToDateTime(reader["fecha_generacion"]),
                Convert.ToDateTime(reader["fecha_expiracion"]),
                Convert.ToBoolean(reader["usado"]),
                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            );
        }
    }
}