using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class InformeDerivacionDAL
    {
        #region Operaciones InformeDerivacion

        public int Alta(InformeDerivacion nuevoInforme)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO InformeDerivacion (id_paciente, id_profesional, contenido, estado, fecha_generacion, fecha_auditoria, digito_verificador) " +
                               "VALUES (@id_paciente, @id_profesional, @contenido, @estado, @fecha_generacion, @fecha_auditoria, @digito_verificador); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", nuevoInforme.IdPaciente);
                    comando.Parameters.AddWithValue("@id_profesional", nuevoInforme.IdProfesional);
                    comando.Parameters.AddWithValue("@contenido", (object)nuevoInforme.Contenido ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@estado", nuevoInforme.Estado.ToString());
                    comando.Parameters.AddWithValue("@fecha_generacion", nuevoInforme.FechaGeneracion);
                    comando.Parameters.AddWithValue("@fecha_auditoria", (object)nuevoInforme.FechaAuditoria ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)nuevoInforme.DigitoVerificador ?? DBNull.Value);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    nuevoInforme.IdInforme = idGenerado;
                    return idGenerado;
                }
            }
        }
        public void ActualizarContenidoYEstado(int idInforme, string contenido, EstadoInforme estado, DateTime? fechaAuditoria)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE InformeDerivacion SET contenido = @contenido, estado = @estado, fecha_auditoria = @fecha_auditoria WHERE id_informe = @id_informe";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_informe", idInforme);
                    comando.Parameters.AddWithValue("@contenido", (object)contenido ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@estado", estado.ToString());
                    comando.Parameters.AddWithValue("@fecha_auditoria", (object)fechaAuditoria ?? DBNull.Value);
                    comando.ExecuteNonQuery();
                }
            }
        }
        public void Baja(int idInforme)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "DELETE FROM InformeDerivacion WHERE id_informe = @id_informe";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_informe", idInforme);
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
                string query = "SELECT digito_verificador FROM InformeDerivacion ORDER BY id_informe";
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

        public void ActualizarDVH(int idInforme, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE InformeDerivacion SET digito_verificador = @digito_verificador WHERE id_informe = @id_informe";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_informe", idInforme);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Busquedas InformeDerivacion

        public InformeDerivacion BuscarPorId(int idInforme)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM InformeDerivacion WHERE id_informe = @id_informe";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_informe", idInforme);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarInformeDerivacion(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<InformeDerivacion> ObtenerPorPaciente(int idPaciente)
        {
            List<InformeDerivacion> lista = new List<InformeDerivacion>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM InformeDerivacion WHERE id_paciente = @id_paciente ORDER BY fecha_generacion DESC";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", idPaciente);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(ArmarInformeDerivacion(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public List<InformeDerivacion> ObtenerTodos()
        {
            List<InformeDerivacion> lista = new List<InformeDerivacion>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM InformeDerivacion ORDER BY fecha_generacion DESC";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarInformeDerivacion(reader));
                    }
                }
            }

            return lista;
        }

        private InformeDerivacion ArmarInformeDerivacion(SqlDataReader reader)
        {
            return new InformeDerivacion(
                Convert.ToInt32(reader["id_informe"]),
                Convert.ToInt32(reader["id_paciente"]),
                Convert.ToInt32(reader["id_profesional"]),
                reader["contenido"] == DBNull.Value ? null : reader["contenido"].ToString(),
                (EstadoInforme)Enum.Parse(typeof(EstadoInforme), reader["estado"].ToString()),
                Convert.ToDateTime(reader["fecha_generacion"]),
                reader["fecha_auditoria"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["fecha_auditoria"]),
                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            );
        }

        #endregion
    }
}