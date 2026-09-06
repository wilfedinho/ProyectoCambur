using BE;
using System;
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
                string query = "INSERT INTO MensajeContacto (nombre, email, asunto, mensaje, fecha_envio) " +
                               "VALUES (@nombre, @email, @asunto, @mensaje, @fecha_envio); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@nombre", mensaje.Nombre);
                    comando.Parameters.AddWithValue("@email", mensaje.Email);
                    comando.Parameters.AddWithValue("@asunto", mensaje.Asunto);
                    comando.Parameters.AddWithValue("@mensaje", mensaje.Mensaje);
                    comando.Parameters.AddWithValue("@fecha_envio", mensaje.FechaEnvio);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    mensaje.IdMensaje = idGenerado;
                    return idGenerado;
                }
            }
        }
    }
}