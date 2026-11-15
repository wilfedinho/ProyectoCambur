using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class ContactoDAL
    {
        public int Alta(MensajeContacto mensaje)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO MensajeContacto (nombre, email, asunto, mensaje, fecha_envio, digito_verificador) " +
                               "VALUES (@nombre, @email, @asunto, @mensaje, @fecha_envio, @digito_verificador); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@nombre", mensaje.Nombre);
                    comando.Parameters.AddWithValue("@email", mensaje.Email);
                    comando.Parameters.AddWithValue("@asunto", mensaje.Asunto);
                    comando.Parameters.AddWithValue("@mensaje", mensaje.Mensaje);
                    comando.Parameters.AddWithValue("@fecha_envio", mensaje.FechaEnvio);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)mensaje.DigitoVerificador ?? DBNull.Value);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    mensaje.IdMensaje = idGenerado;
                    return idGenerado;
                }
            }
        }

        #region Busquedas

        public MensajeContacto BuscarPorId(int idMensaje)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM MensajeContacto WHERE id_mensaje = @id_mensaje";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_mensaje", idMensaje);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarMensaje(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<MensajeContacto> ObtenerTodos()
        {
            List<MensajeContacto> lista = new List<MensajeContacto>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM MensajeContacto ORDER BY id_mensaje";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarMensaje(reader));
                    }
                }
            }

            return lista;
        }

        private MensajeContacto ArmarMensaje(SqlDataReader reader)
        {
            return new MensajeContacto
            {
                IdMensaje = Convert.ToInt32(reader["id_mensaje"]),
                Nombre = reader["nombre"].ToString(),
                Email = reader["email"].ToString(),
                Asunto = reader["asunto"].ToString(),
                Mensaje = reader["mensaje"].ToString(),
                FechaEnvio = Convert.ToDateTime(reader["fecha_envio"]),
                DigitoVerificador = reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            };
        }

        #endregion

        #region Digito Verificador (DVH)

        public List<string> ObtenerListaDVH()
        {
            List<string> lista = new List<string>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT digito_verificador FROM MensajeContacto ORDER BY id_mensaje";
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

        public void ActualizarDVH(int idMensaje, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE MensajeContacto SET digito_verificador = @digito_verificador WHERE id_mensaje = @id_mensaje";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_mensaje", idMensaje);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}