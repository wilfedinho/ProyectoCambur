using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PerfilPacienteDAL
    {
        #region Operaciones PerfilPaciente

        public int Alta(PerfilPaciente nuevoPerfil)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO PerfilPaciente (id_paciente, id_profesional, id_modelo, resultado, fecha_generacion, digito_verificador) " +
                               "VALUES (@id_paciente, @id_profesional, @id_modelo, @resultado, @fecha_generacion, @digito_verificador); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", nuevoPerfil.IdPaciente);
                    comando.Parameters.AddWithValue("@id_profesional", nuevoPerfil.IdProfesional);
                    comando.Parameters.AddWithValue("@id_modelo", nuevoPerfil.IdModelo);
                    comando.Parameters.AddWithValue("@resultado", (object)nuevoPerfil.Resultado ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@fecha_generacion", nuevoPerfil.FechaGeneracion);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)nuevoPerfil.DigitoVerificador ?? DBNull.Value);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    nuevoPerfil.IdPerfil = idGenerado;
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
                string query = "SELECT digito_verificador FROM PerfilPaciente ORDER BY id_perfil";
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

        public void ActualizarDVH(int idPerfil, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE PerfilPaciente SET digito_verificador = @digito_verificador WHERE id_perfil = @id_perfil";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_perfil", idPerfil);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Busquedas PerfilPaciente

        public PerfilPaciente BuscarPorId(int idPerfil)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM PerfilPaciente WHERE id_perfil = @id_perfil";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_perfil", idPerfil);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarPerfilPaciente(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<PerfilPaciente> ObtenerPorPaciente(int idPaciente)
        {
            List<PerfilPaciente> lista = new List<PerfilPaciente>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM PerfilPaciente WHERE id_paciente = @id_paciente ORDER BY fecha_generacion DESC";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", idPaciente);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(ArmarPerfilPaciente(reader));
                        }
                    }
                }
            }

            return lista;
        }

        public List<PerfilPaciente> ObtenerTodos()
        {
            List<PerfilPaciente> lista = new List<PerfilPaciente>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM PerfilPaciente ORDER BY fecha_generacion DESC";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarPerfilPaciente(reader));
                    }
                }
            }

            return lista;
        }

        private PerfilPaciente ArmarPerfilPaciente(SqlDataReader reader)
        {
            return new PerfilPaciente(
                Convert.ToInt32(reader["id_perfil"]),
                Convert.ToInt32(reader["id_paciente"]),
                Convert.ToInt32(reader["id_profesional"]),
                Convert.ToInt32(reader["id_modelo"]),
                reader["resultado"] == DBNull.Value ? null : reader["resultado"].ToString(),
                Convert.ToDateTime(reader["fecha_generacion"]),
                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            );
        }

        #endregion
    }
}