using BE;
using System;
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
        public void InvalidarTokensVigentesDe(int idProfesional)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE TokenRecuperacion SET usado = @usado WHERE id_profesional = @id_profesional AND usado = 0";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idProfesional);
                    comando.Parameters.AddWithValue("@usado", true);
                    comando.ExecuteNonQuery();
                }
            }
        }

        private TokenRecuperacion ArmarToken(SqlDataReader reader)
        {
            return new TokenRecuperacion(
                Convert.ToInt32(reader["id_token"]),
                Convert.ToInt32(reader["id_profesional"]),
                reader["token_hash"].ToString(),
                Convert.ToDateTime(reader["fecha_generacion"]),
                Convert.ToDateTime(reader["fecha_expiracion"]),
                Convert.ToBoolean(reader["usado"])
            );
        }
    }
}