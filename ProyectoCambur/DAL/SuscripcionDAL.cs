using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class SuscripcionDAL
    {
        #region Operaciones Suscripcion

        public int Alta(Suscripcion nuevaSuscripcion)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO Suscripcion (id_profesional, [plan], estado, fecha_inicio, fecha_fin, precio, id_pago_externo, ultimos_cuatro_tarjeta, digito_verificador) " +
                               "VALUES (@id_profesional, @plan, @estado, @fecha_inicio, @fecha_fin, @precio, @id_pago_externo, @ultimos_cuatro_tarjeta, @digito_verificador); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", nuevaSuscripcion.IdProfesional);
                    comando.Parameters.AddWithValue("@plan", nuevaSuscripcion.Plan.ToString());
                    comando.Parameters.AddWithValue("@estado", nuevaSuscripcion.Estado.ToString());
                    comando.Parameters.AddWithValue("@fecha_inicio", nuevaSuscripcion.FechaInicio);
                    comando.Parameters.AddWithValue("@fecha_fin", (object)nuevaSuscripcion.FechaFin ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@precio", nuevaSuscripcion.Precio);
                    comando.Parameters.AddWithValue("@id_pago_externo", (object)nuevaSuscripcion.IdPagoExterno ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@ultimos_cuatro_tarjeta", (object)nuevaSuscripcion.UltimosCuatroTarjeta ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)nuevaSuscripcion.DigitoVerificador ?? DBNull.Value);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    nuevaSuscripcion.IdSuscripcion = idGenerado;
                    return idGenerado;
                }
            }
        }

        #endregion

        #region Busquedas Suscripcion
        public Suscripcion BuscarPorId(int idSuscripcion)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Suscripcion WHERE id_suscripcion = @id_suscripcion";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_suscripcion", idSuscripcion);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarSuscripcion(reader);
                        }
                    }
                }
            }

            return null;
        }

        public Suscripcion BuscarActivaDe(int idProfesional)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT TOP 1 * FROM Suscripcion WHERE id_profesional = @id_profesional AND estado = @estado ORDER BY fecha_inicio DESC";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idProfesional);
                    comando.Parameters.AddWithValue("@estado", EstadoSuscripcion.Activa.ToString());
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarSuscripcion(reader);
                        }
                    }
                }
            }

            return null;
        }

        private Suscripcion ArmarSuscripcion(SqlDataReader reader)
        {
            return new Suscripcion(
                Convert.ToInt32(reader["id_suscripcion"]),
                Convert.ToInt32(reader["id_profesional"]),
                (PlanSuscripcion)Enum.Parse(typeof(PlanSuscripcion), reader["plan"].ToString()),
                (EstadoSuscripcion)Enum.Parse(typeof(EstadoSuscripcion), reader["estado"].ToString()),
                Convert.ToDateTime(reader["fecha_inicio"]),
                reader["fecha_fin"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["fecha_fin"]),
                Convert.ToDecimal(reader["precio"]),
                reader["id_pago_externo"] == DBNull.Value ? null : reader["id_pago_externo"].ToString(),
                reader["ultimos_cuatro_tarjeta"] == DBNull.Value ? null : reader["ultimos_cuatro_tarjeta"].ToString(),
                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            );
        }

        public List<Suscripcion> ObtenerTodas()
        {
            List<Suscripcion> lista = new List<Suscripcion>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Suscripcion ORDER BY id_suscripcion";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarSuscripcion(reader));
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
                string query = "SELECT digito_verificador FROM Suscripcion ORDER BY id_suscripcion";
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

        public void ActualizarDVH(int idSuscripcion, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Suscripcion SET digito_verificador = @digito_verificador WHERE id_suscripcion = @id_suscripcion";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_suscripcion", idSuscripcion);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}