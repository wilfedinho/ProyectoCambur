using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ConsultaDAL
    {
        #region Operaciones Consulta

        public int Alta(Consulta nuevaConsulta)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO Consulta (id_paciente, id_profesional, fecha_consulta, tiempo_consulta, objetivos, observaciones, hipotesis, intervenciones, evolucion_observada, diagnostico, tratamiento, fecha_registro, digito_verificador) " +
                               "VALUES (@id_paciente, @id_profesional, @fecha_consulta, @tiempo_consulta, @objetivos, @observaciones, @hipotesis, @intervenciones, @evolucion_observada, @diagnostico, @tratamiento, @fecha_registro, @digito_verificador); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", nuevaConsulta.IdPaciente);
                    comando.Parameters.AddWithValue("@id_profesional", nuevaConsulta.IdPsicologo);
                    comando.Parameters.AddWithValue("@fecha_consulta", nuevaConsulta.FechaConsulta);
                    comando.Parameters.AddWithValue("@tiempo_consulta", nuevaConsulta.TiempoConsulta);
                    comando.Parameters.AddWithValue("@objetivos", (object)nuevaConsulta.Objetivos ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@observaciones", (object)nuevaConsulta.Observaciones ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@hipotesis", (object)nuevaConsulta.Hipotesis ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@intervenciones", (object)nuevaConsulta.Intervenciones ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@evolucion_observada", (object)nuevaConsulta.EvolucionObservada ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@diagnostico", (object)nuevaConsulta.Diagnostico ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@tratamiento", (object)nuevaConsulta.Tratamiento ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@fecha_registro", nuevaConsulta.FechaRegistro);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)nuevaConsulta.DigitoVerificador ?? DBNull.Value);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    nuevaConsulta.IdConsulta = idGenerado;
                    return idGenerado;
                }
            }
        }

        public void Modificar(Consulta consultaModificada)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Consulta SET id_paciente = @id_paciente, id_profesional = @id_profesional, fecha_consulta = @fecha_consulta, " +
                               "tiempo_consulta = @tiempo_consulta, objetivos = @objetivos, observaciones = @observaciones, hipotesis = @hipotesis, " +
                               "intervenciones = @intervenciones, evolucion_observada = @evolucion_observada, diagnostico = @diagnostico, " +
                               "tratamiento = @tratamiento, fecha_modificacion = @fecha_modificacion, digito_verificador = @digito_verificador " +
                               "WHERE id_consulta = @id_consulta";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_consulta", consultaModificada.IdConsulta);
                    comando.Parameters.AddWithValue("@id_paciente", consultaModificada.IdPaciente);
                    comando.Parameters.AddWithValue("@id_profesional", consultaModificada.IdPsicologo);
                    comando.Parameters.AddWithValue("@fecha_consulta", consultaModificada.FechaConsulta);
                    comando.Parameters.AddWithValue("@tiempo_consulta", consultaModificada.TiempoConsulta);
                    comando.Parameters.AddWithValue("@objetivos", (object)consultaModificada.Objetivos ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@observaciones", (object)consultaModificada.Observaciones ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@hipotesis", (object)consultaModificada.Hipotesis ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@intervenciones", (object)consultaModificada.Intervenciones ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@evolucion_observada", (object)consultaModificada.EvolucionObservada ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@diagnostico", (object)consultaModificada.Diagnostico ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@tratamiento", (object)consultaModificada.Tratamiento ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@fecha_modificacion", (object)consultaModificada.FechaModificacion ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)consultaModificada.DigitoVerificador ?? DBNull.Value);
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
                string query = "SELECT digito_verificador FROM Consulta ORDER BY id_consulta";
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

        public void ActualizarDVH(int idConsulta, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Consulta SET digito_verificador = @digito_verificador WHERE id_consulta = @id_consulta";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_consulta", idConsulta);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Busquedas Consulta

        public Consulta BuscarPorId(int idConsulta)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Consulta WHERE id_consulta = @id_consulta";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_consulta", idConsulta);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarConsulta(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<Consulta> ObtenerPorPaciente(int idPaciente)
        {
            List<Consulta> lista = new List<Consulta>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Consulta WHERE id_paciente = @id_paciente ORDER BY fecha_consulta DESC";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", idPaciente);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(ArmarConsulta(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public List<Consulta> ObtenerTodas()
        {
            List<Consulta> lista = new List<Consulta>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Consulta ORDER BY fecha_consulta DESC";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarConsulta(reader));
                    }
                }
            }

            return lista;
        }

        private Consulta ArmarConsulta(SqlDataReader reader)
        {
            return new Consulta(
                Convert.ToInt32(reader["id_consulta"]),
                Convert.ToInt32(reader["id_paciente"]),
                Convert.ToInt32(reader["id_profesional"]),
                Convert.ToDateTime(reader["fecha_consulta"]),
                Convert.ToInt32(reader["tiempo_consulta"]),
                reader["objetivos"] == DBNull.Value ? null : reader["objetivos"].ToString(),
                reader["observaciones"] == DBNull.Value ? null : reader["observaciones"].ToString(),
                reader["hipotesis"] == DBNull.Value ? null : reader["hipotesis"].ToString(),
                reader["intervenciones"] == DBNull.Value ? null : reader["intervenciones"].ToString(),
                reader["evolucion_observada"] == DBNull.Value ? null : reader["evolucion_observada"].ToString(),
                reader["diagnostico"] == DBNull.Value ? null : reader["diagnostico"].ToString(),
                reader["tratamiento"] == DBNull.Value ? null : reader["tratamiento"].ToString(),
                Convert.ToDateTime(reader["fecha_registro"]),
                reader["fecha_modificacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["fecha_modificacion"]),
                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            );
        }

        #endregion
    }
}