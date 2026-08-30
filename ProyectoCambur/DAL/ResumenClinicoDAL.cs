using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ResumenClinicoDAL
    {
        #region Operaciones ResumenClinico

        public int Alta(ResumenClinico nuevoResumen)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO ResumenClinico (id_paciente, id_profesional, contenido, rango_desde, rango_hasta, fecha_generacion, digito_verificador) " +
                               "VALUES (@id_paciente, @id_profesional, @contenido, @rango_desde, @rango_hasta, @fecha_generacion, @digito_verificador); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", nuevoResumen.IdPaciente);
                    comando.Parameters.AddWithValue("@id_profesional", nuevoResumen.IdProfesional);
                    comando.Parameters.AddWithValue("@contenido", (object)nuevoResumen.Contenido ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@rango_desde", nuevoResumen.RangoDesde);
                    comando.Parameters.AddWithValue("@rango_hasta", nuevoResumen.RangoHasta);
                    comando.Parameters.AddWithValue("@fecha_generacion", nuevoResumen.FechaGeneracion);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)nuevoResumen.DigitoVerificador ?? DBNull.Value);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    nuevoResumen.IdResumen = idGenerado;
                    return idGenerado;
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
                string query = "SELECT digito_verificador FROM ResumenClinico ORDER BY id_resumen";
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

        public void ActualizarDVH(int idResumen, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE ResumenClinico SET digito_verificador = @digito_verificador WHERE id_resumen = @id_resumen";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_resumen", idResumen);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Busquedas ResumenClinico

        public ResumenClinico BuscarPorId(int idResumen)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM ResumenClinico WHERE id_resumen = @id_resumen";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_resumen", idResumen);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarResumenClinico(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<ResumenClinico> ObtenerPorPaciente(int idPaciente)
        {
            List<ResumenClinico> lista = new List<ResumenClinico>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM ResumenClinico WHERE id_paciente = @id_paciente ORDER BY fecha_generacion DESC";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", idPaciente);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(ArmarResumenClinico(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public List<ResumenClinico> ObtenerTodos()
        {
            List<ResumenClinico> lista = new List<ResumenClinico>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM ResumenClinico ORDER BY fecha_generacion DESC";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarResumenClinico(reader));
                    }
                }
            }

            return lista;
        }

        private ResumenClinico ArmarResumenClinico(SqlDataReader reader)
        {
            return new ResumenClinico(
                Convert.ToInt32(reader["id_resumen"]),
                Convert.ToInt32(reader["id_paciente"]),
                Convert.ToInt32(reader["id_profesional"]),
                reader["contenido"] == DBNull.Value ? null : reader["contenido"].ToString(),
                Convert.ToDateTime(reader["rango_desde"]),
                Convert.ToDateTime(reader["rango_hasta"]),
                Convert.ToDateTime(reader["fecha_generacion"]),
                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            );
        }

        #endregion
    }
}