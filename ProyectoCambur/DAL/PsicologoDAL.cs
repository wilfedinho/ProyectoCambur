using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    
    public class PsicologoDAL
    {
        #region Operaciones Psicologo

        public int Alta(Psicologo nuevoPsicologo)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO Profesional (nombre, apellido, dni, email, contrasena, idioma, rol_permiso, activo, is_habilitado, is_bloqueado, intentos, hora_ultima_sesion, fecha_registro, digito_verificador) " +
                               "VALUES (@nombre, @apellido, @dni, @email, @contrasena, @idioma, @rol_permiso, @activo, @is_habilitado, @is_bloqueado, @intentos, @hora_ultima_sesion, @fecha_registro, @digito_verificador); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@nombre", nuevoPsicologo.Nombre);
                    comando.Parameters.AddWithValue("@apellido", nuevoPsicologo.Apellido);
                    comando.Parameters.AddWithValue("@dni", nuevoPsicologo.Dni);
                    comando.Parameters.AddWithValue("@email", nuevoPsicologo.Email);
                    comando.Parameters.AddWithValue("@contrasena", nuevoPsicologo.Contrasena);
                    comando.Parameters.AddWithValue("@idioma", nuevoPsicologo.Idioma);
                    comando.Parameters.AddWithValue("@rol_permiso", nuevoPsicologo.RolPermiso);
                    comando.Parameters.AddWithValue("@activo", nuevoPsicologo.Activo);
                    comando.Parameters.AddWithValue("@is_habilitado", true);
                    comando.Parameters.AddWithValue("@is_bloqueado", false);
                    comando.Parameters.AddWithValue("@intentos", 0);
                    comando.Parameters.AddWithValue("@hora_ultima_sesion", DateTime.Now);
                    comando.Parameters.AddWithValue("@fecha_registro", nuevoPsicologo.FechaRegistro);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)nuevoPsicologo.DigitoVerificador ?? DBNull.Value);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    nuevoPsicologo.IdPsicologo = idGenerado;
                    nuevoPsicologo.IsHabilitado = true;
                    nuevoPsicologo.IsBloqueado = false;
                    nuevoPsicologo.Intentos = 0;
                    return idGenerado;
                }
            }
        }

        public void Baja(int idPsicologo)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Profesional SET activo = @activo WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idPsicologo);
                    comando.Parameters.AddWithValue("@activo", false);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Activar(int idPsicologo)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Profesional SET activo = @activo WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idPsicologo);
                    comando.Parameters.AddWithValue("@activo", true);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Modificar(Psicologo psicologoModificado)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Profesional SET nombre = @nombre, apellido = @apellido, dni = @dni, email = @email, " +
                               "idioma = @idioma, rol_permiso = @rol_permiso, digito_verificador = @digito_verificador " +
                               "WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", psicologoModificado.IdPsicologo);
                    comando.Parameters.AddWithValue("@nombre", psicologoModificado.Nombre);
                    comando.Parameters.AddWithValue("@apellido", psicologoModificado.Apellido);
                    comando.Parameters.AddWithValue("@dni", psicologoModificado.Dni);
                    comando.Parameters.AddWithValue("@email", psicologoModificado.Email);
                    comando.Parameters.AddWithValue("@idioma", psicologoModificado.Idioma);
                    comando.Parameters.AddWithValue("@rol_permiso", psicologoModificado.RolPermiso);
                    comando.Parameters.AddWithValue("@digito_verificador", (object)psicologoModificado.DigitoVerificador ?? DBNull.Value);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void CambiarContrasena(int idPsicologo, string nuevaContrasenaHash)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Profesional SET contrasena = @contrasena WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idPsicologo);
                    comando.Parameters.AddWithValue("@contrasena", nuevaContrasenaHash);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Control de acceso (intentos, bloqueo, habilitado)

        public void ActualizarIntentos(int idPsicologo, int intentos, DateTime horaUltimaSesion)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Profesional SET intentos = @intentos, hora_ultima_sesion = @hora_ultima_sesion WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idPsicologo);
                    comando.Parameters.AddWithValue("@intentos", intentos);
                    comando.Parameters.AddWithValue("@hora_ultima_sesion", horaUltimaSesion);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Bloquear(int idPsicologo)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Profesional SET is_bloqueado = @is_bloqueado WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idPsicologo);
                    comando.Parameters.AddWithValue("@is_bloqueado", true);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Desbloquear(int idPsicologo, string nuevaContrasenaHash)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Profesional SET is_bloqueado = @is_bloqueado, intentos = @intentos, contrasena = @contrasena WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idPsicologo);
                    comando.Parameters.AddWithValue("@is_bloqueado", false);
                    comando.Parameters.AddWithValue("@intentos", 0);
                    comando.Parameters.AddWithValue("@contrasena", nuevaContrasenaHash);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Habilitar(int idPsicologo)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Profesional SET is_habilitado = @is_habilitado WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idPsicologo);
                    comando.Parameters.AddWithValue("@is_habilitado", true);
                    comando.ExecuteNonQuery();
                }
            }
        }

        public void Deshabilitar(int idPsicologo)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Profesional SET is_habilitado = @is_habilitado WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idPsicologo);
                    comando.Parameters.AddWithValue("@is_habilitado", false);
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
                string query = "SELECT digito_verificador FROM Profesional ORDER BY id_profesional";
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

        public void ActualizarDVH(int idPsicologo, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE Profesional SET digito_verificador = @digito_verificador WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idPsicologo);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Busquedas Psicologo

        public Psicologo BuscarPorId(int idPsicologo)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Profesional WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idPsicologo);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarPsicologo(reader);
                        }
                    }
                }
            }

            return null;
        }

        public Psicologo BuscarPorEmail(string email)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Profesional WHERE email = @email";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@email", email);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarPsicologo(reader);
                        }
                    }
                }
            }

            return null;
        }

        public bool ExisteEmail(string email)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT COUNT(1) FROM Profesional WHERE email = @email";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@email", email);
                    int cantidad = Convert.ToInt32(comando.ExecuteScalar());
                    return cantidad > 0;
                }
            }
        }

        public List<Psicologo> ObtenerTodos()
        {
            List<Psicologo> listaPsicologos = new List<Psicologo>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM Profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaPsicologos.Add(ArmarPsicologo(reader));
                        }
                    }
                }
            }

            return listaPsicologos;
        }

        private Psicologo ArmarPsicologo(SqlDataReader reader)
        {
            return new Psicologo(
                Convert.ToInt32(reader["id_profesional"]),
                reader["nombre"].ToString(),
                reader["apellido"].ToString(),
                reader["dni"].ToString(),
                reader["email"].ToString(),
                reader["contrasena"].ToString(),
                reader["idioma"].ToString(),
                reader["rol_permiso"].ToString(),
                Convert.ToBoolean(reader["activo"]),
                Convert.ToBoolean(reader["is_habilitado"]),
                Convert.ToBoolean(reader["is_bloqueado"]),
                Convert.ToInt32(reader["intentos"]),
                Convert.ToDateTime(reader["hora_ultima_sesion"]),
                Convert.ToDateTime(reader["fecha_registro"]),
                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            );
        }

        #endregion
    }
}