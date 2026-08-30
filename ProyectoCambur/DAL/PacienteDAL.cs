using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class PacienteDAL
    {
        #region Operaciones Paciente
        public bool ExisteDuplicadoPorDni(int idProfesional, string dni, int? idPacienteExcluir = null)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT COUNT(1) FROM Paciente WHERE id_profesional = @id_profesional AND dni = @dni";
                if (idPacienteExcluir.HasValue)
                {
                    query += " AND id_paciente <> @id_paciente_excluir";
                }
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idProfesional);
                    comando.Parameters.AddWithValue("@dni", (object)dni ?? string.Empty);
                    if (idPacienteExcluir.HasValue)
                    {
                        comando.Parameters.AddWithValue("@id_paciente_excluir", idPacienteExcluir.Value);
                    }
                    return Convert.ToInt32(comando.ExecuteScalar()) > 0;
                }
            }
        }
        public bool ExisteDuplicadoPorEmail(int idProfesional, string email, int? idPacienteExcluir = null)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT COUNT(1) FROM Paciente WHERE id_profesional = @id_profesional AND email = @email";
                if (idPacienteExcluir.HasValue)
                {
                    query += " AND id_paciente <> @id_paciente_excluir";
                }
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idProfesional);
                    comando.Parameters.AddWithValue("@email", email);
                    if (idPacienteExcluir.HasValue)
                    {
                        comando.Parameters.AddWithValue("@id_paciente_excluir", idPacienteExcluir.Value);
                    }
                    return Convert.ToInt32(comando.ExecuteScalar()) > 0;
                }
            }
        }

        public int Alta(Paciente nuevoPaciente)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO Paciente (id_profesional, nombre, apellido, dni, fecha_nacimiento, ocupacion, estado_civil, email, telefono, sexo, activo, fecha_registro, digito_verificador) " +
                               "VALUES (@IdProfesional, @Nombre, @Apellido, @dni, @FechaNacimiento, @Ocupacion, @EstadoCivil, @Email, @Telefono, @Sexo, @Activo, @FechaRegistro, @DigitoVerificador); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@IdProfesional", nuevoPaciente.IdPsicologo);
                    comando.Parameters.AddWithValue("@Nombre", nuevoPaciente.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", nuevoPaciente.Apellido);
                    comando.Parameters.AddWithValue("@dni", nuevoPaciente.DNI);
                    comando.Parameters.AddWithValue("@FechaNacimiento", nuevoPaciente.FechaNacimiento);
                    comando.Parameters.AddWithValue("@Ocupacion", (object)nuevoPaciente.Ocupacion ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@EstadoCivil", (object)nuevoPaciente.EstadoCivil ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Email", (object)nuevoPaciente.Email ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Telefono", (object)nuevoPaciente.Telefono ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Sexo", (object)nuevoPaciente.Sexo ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Activo", true);
                    comando.Parameters.AddWithValue("@FechaRegistro", nuevoPaciente.FechaRegistro);
                    comando.Parameters.AddWithValue("@DigitoVerificador", (object)nuevoPaciente.DigitoVerificador ?? DBNull.Value);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    nuevoPaciente.IdPaciente = idGenerado;
                    nuevoPaciente.Activo = true;
                    return idGenerado;
                }
            }
        }

        public void Baja(int idPaciente)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Paciente SET activo = @Activo WHERE id_paciente = @IdPaciente";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@IdPaciente", idPaciente);
                    comando.Parameters.AddWithValue("@Activo", false);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Activar(int idPaciente)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Paciente SET activo = @Activo WHERE id_paciente = @IdPaciente";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@IdPaciente", idPaciente);
                    comando.Parameters.AddWithValue("@Activo", true);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Modificar(Paciente pacienteModificado)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Paciente SET id_profesional = @IdProfesional, nombre = @Nombre, apellido = @Apellido, dni = @dni, fecha_nacimiento = @FechaNacimiento, " +
                               "ocupacion = @Ocupacion, estado_civil = @EstadoCivil, email = @Email, telefono = @Telefono, sexo = @Sexo, " +
                               "digito_verificador = @DigitoVerificador WHERE id_paciente = @IdPaciente";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@IdPaciente", pacienteModificado.IdPaciente);
                    comando.Parameters.AddWithValue("@IdProfesional", pacienteModificado.IdPsicologo);
                    comando.Parameters.AddWithValue("@Nombre", pacienteModificado.Nombre);
                    comando.Parameters.AddWithValue("@Apellido", pacienteModificado.Apellido);
                    comando.Parameters.AddWithValue("@dni", pacienteModificado.DNI);
                    comando.Parameters.AddWithValue("@FechaNacimiento", pacienteModificado.FechaNacimiento);
                    comando.Parameters.AddWithValue("@Ocupacion", (object)pacienteModificado.Ocupacion ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@EstadoCivil", (object)pacienteModificado.EstadoCivil ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Email", (object)pacienteModificado.Email ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Telefono", (object)pacienteModificado.Telefono ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@Sexo", (object)pacienteModificado.Sexo ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@DigitoVerificador", (object)pacienteModificado.DigitoVerificador ?? DBNull.Value);
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
                string query = "SELECT digito_verificador FROM Paciente ORDER BY id_paciente";
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

        public void ActualizarDVH(int idPaciente, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Paciente SET digito_verificador = @digito_verificador WHERE id_paciente = @id_paciente";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_paciente", idPaciente);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Busquedas Paciente

        public Paciente BuscarPorId(int idPaciente)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Paciente WHERE id_paciente = @IdPaciente";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@IdPaciente", idPaciente);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarPaciente(reader);
                        }
                    }
                }
            }

            return null;
        }

        public List<Paciente> ObtenerPorProfesional(int idProfesional, bool soloActivos = true)
        {
            List<Paciente> listaPacientes = new List<Paciente>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Paciente WHERE id_profesional = @IdProfesional" + (soloActivos ? " AND activo = 1" : "");
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@IdProfesional", idProfesional);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaPacientes.Add(ArmarPaciente(reader));
                        }
                    }
                }
            }

            return listaPacientes;
        }

        public List<Paciente> ObtenerTodos()
        {
            List<Paciente> listaPacientes = new List<Paciente>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Paciente";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaPacientes.Add(ArmarPaciente(reader));
                        }
                    }
                }
            }

            return listaPacientes;
        }

        private Paciente ArmarPaciente(SqlDataReader reader)
        {
            return new Paciente(
                Convert.ToInt32(reader["id_paciente"]),
                Convert.ToInt32(reader["id_profesional"]),
                reader["nombre"].ToString(),
                reader["apellido"].ToString(),
                reader["dni"].ToString(),
                Convert.ToDateTime(reader["fecha_nacimiento"]),
                reader["ocupacion"] == DBNull.Value ? null : reader["ocupacion"].ToString(),
                reader["estado_civil"] == DBNull.Value ? null : reader["estado_civil"].ToString(),
                reader["email"] == DBNull.Value ? null : reader["email"].ToString(),
                reader["telefono"] == DBNull.Value ? null : reader["telefono"].ToString(),
                reader["sexo"] == DBNull.Value ? null : reader["sexo"].ToString(),
                Convert.ToBoolean(reader["activo"]),
                Convert.ToDateTime(reader["fecha_registro"]),
                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            );
        }

        #endregion
    }
}