using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class HistorialClinicoDAL
    {
        #region Operaciones HistorialClinico

        public int Alta(HistorialClinico nuevoHistorial)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO HistorialClinico (id_paciente, habitos_nocivos, contexto_familiar, antecedentes_familiares, antecedentes_medicos, situacion_laboral, eventos_traumaticos, fecha_registro, digito_verificador) " +
                               "VALUES (@id_paciente, @habitos_nocivos, @contexto_familiar, @antecedentes_familiares, @antecedentes_medicos, @situacion_laboral, @eventos_traumaticos, @fecha_registro, @digito_verificador); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", nuevoHistorial.IdPaciente);
                    comando.Parameters.AddWithValue("@habitos_nocivos", (object)nuevoHistorial.HabitosNocivos ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@contexto_familiar", (object)nuevoHistorial.ContextoFamiliar ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@antecedentes_familiares", (object)nuevoHistorial.AntecedentesFamiliares ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@antecedentes_medicos", (object)nuevoHistorial.AntecedentesMedicos ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@situacion_laboral", (object)nuevoHistorial.SituacionLaboral ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@eventos_traumaticos", (object)nuevoHistorial.EventosTraumaticos ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@fecha_registro", nuevoHistorial.FechaRegistro);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)nuevoHistorial.DigitoVerificador ?? DBNull.Value);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    nuevoHistorial.IdHistorial = idGenerado;
                    return idGenerado;
                }
            }
        }

        public void Modificar(HistorialClinico historialModificado)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE HistorialClinico SET habitos_nocivos = @habitos_nocivos, contexto_familiar = @contexto_familiar, " +
                               "antecedentes_familiares = @antecedentes_familiares, antecedentes_medicos = @antecedentes_medicos, " +
                               "situacion_laboral = @situacion_laboral, eventos_traumaticos = @eventos_traumaticos, " +
                               "digito_verificador = @digito_verificador " +
                               "WHERE id_historial = @id_historial";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_historial", historialModificado.IdHistorial);
                    comando.Parameters.AddWithValue("@habitos_nocivos", (object)historialModificado.HabitosNocivos ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@contexto_familiar", (object)historialModificado.ContextoFamiliar ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@antecedentes_familiares", (object)historialModificado.AntecedentesFamiliares ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@antecedentes_medicos", (object)historialModificado.AntecedentesMedicos ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@situacion_laboral", (object)historialModificado.SituacionLaboral ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@eventos_traumaticos", (object)historialModificado.EventosTraumaticos ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)historialModificado.DigitoVerificador ?? DBNull.Value);
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
                string query = "SELECT digito_verificador FROM HistorialClinico ORDER BY id_historial";
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

        public void ActualizarDVH(int idHistorial, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE HistorialClinico SET digito_verificador = @digito_verificador WHERE id_historial = @id_historial";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_historial", idHistorial);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Busquedas HistorialClinico

        public HistorialClinico BuscarPorId(int idHistorial)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM HistorialClinico WHERE id_historial = @id_historial";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_historial", idHistorial);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarHistorial(reader);
                        }
                    }
                }
            }

            return null;
        }
        public HistorialClinico BuscarPorPaciente(int idPaciente)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM HistorialClinico WHERE id_paciente = @id_paciente";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", idPaciente);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarHistorial(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<HistorialClinico> ObtenerTodos()
        {
            List<HistorialClinico> lista = new List<HistorialClinico>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM HistorialClinico ORDER BY id_historial";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarHistorial(reader));
                    }
                }
            }

            return lista;
        }

        private HistorialClinico ArmarHistorial(SqlDataReader reader)
        {
            return new HistorialClinico(
                Convert.ToInt32(reader["id_historial"]),
                Convert.ToInt32(reader["id_paciente"]),
                reader["habitos_nocivos"] == DBNull.Value ? null : reader["habitos_nocivos"].ToString(),
                reader["contexto_familiar"] == DBNull.Value ? null : reader["contexto_familiar"].ToString(),
                reader["antecedentes_familiares"] == DBNull.Value ? null : reader["antecedentes_familiares"].ToString(),
                reader["antecedentes_medicos"] == DBNull.Value ? null : reader["antecedentes_medicos"].ToString(),
                reader["situacion_laboral"] == DBNull.Value ? null : reader["situacion_laboral"].ToString(),
                reader["eventos_traumaticos"] == DBNull.Value ? null : reader["eventos_traumaticos"].ToString(),
                Convert.ToDateTime(reader["fecha_registro"]),
                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            );
        }

        #endregion
    }
}