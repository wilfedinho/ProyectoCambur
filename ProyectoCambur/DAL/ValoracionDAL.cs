using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace DAL
{
    public class ValoracionDAL
    {
        public ValoracionServicio BuscarPorProfesional(int idProfesional)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM ValoracionServicio WHERE id_profesional = @id_profesional";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", idProfesional);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarValoracion(reader, conNombreProfesional: false);
                        }
                    }
                }
            }

            return null;
        }

        public int Alta(ValoracionServicio valoracion)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO ValoracionServicio (id_profesional, [plan], puntuacion, comentario, fecha_valoracion) " +
                               "VALUES (@id_profesional, @plan, @puntuacion, @comentario, @fecha_valoracion); " +
                               "SELECT CAST(SCOPE_IDENTITY() AS INT)";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_profesional", valoracion.IdProfesional);
                    comando.Parameters.AddWithValue("@plan", valoracion.Plan.ToString());
                    comando.Parameters.AddWithValue("@puntuacion", valoracion.Puntuacion);
                    comando.Parameters.AddWithValue("@comentario", (object)valoracion.Comentario ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@fecha_valoracion", valoracion.FechaValoracion);

                    int idGenerado = Convert.ToInt32(comando.ExecuteScalar());
                    valoracion.IdValoracion = idGenerado;
                    return idGenerado;
                }
            }
        }

        public void Actualizar(ValoracionServicio valoracion)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE ValoracionServicio SET [plan] = @plan, puntuacion = @puntuacion, comentario = @comentario, fecha_valoracion = @fecha_valoracion " +
                               "WHERE id_valoracion = @id_valoracion";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_valoracion", valoracion.IdValoracion);
                    comando.Parameters.AddWithValue("@plan", valoracion.Plan.ToString());
                    comando.Parameters.AddWithValue("@puntuacion", valoracion.Puntuacion);
                    comando.Parameters.AddWithValue("@comentario", (object)valoracion.Comentario ?? DBNull.Value);
                    comando.Parameters.AddWithValue("@fecha_valoracion", valoracion.FechaValoracion);
                    comando.ExecuteNonQuery();
                }
            }
        }
        public List<ValoracionServicio> ObtenerTestimonios(int cantidad)
        {
            List<ValoracionServicio> lista = new List<ValoracionServicio>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT TOP (@cantidad) v.*, p.nombre AS nombre_profesional, p.apellido AS apellido_profesional " +
                               "FROM ValoracionServicio v " +
                               "INNER JOIN Profesional p ON p.id_profesional = v.id_profesional " +
                               "WHERE v.comentario IS NOT NULL AND LTRIM(RTRIM(v.comentario)) <> '' " +
                               "ORDER BY v.fecha_valoracion DESC";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@cantidad", cantidad);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(ArmarValoracion(reader, conNombreProfesional: true));
                        }
                    }
                }
            }

            return lista;
        }

        public ResumenValoraciones ObtenerResumen()
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT COUNT(*) AS cantidad, ISNULL(AVG(CAST(puntuacion AS DECIMAL(4,2))), 0) AS promedio FROM ValoracionServicio";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ResumenValoraciones
                        {
                            Cantidad = Convert.ToInt32(reader["cantidad"]),
                            Promedio = Convert.ToDouble(reader["promedio"])
                        };
                    }
                }
            }

            return new ResumenValoraciones { Cantidad = 0, Promedio = 0 };
        }

        private ValoracionServicio ArmarValoracion(SqlDataReader reader, bool conNombreProfesional)
        {
            ValoracionServicio valoracion = new ValoracionServicio
            {
                IdValoracion = Convert.ToInt32(reader["id_valoracion"]),
                IdProfesional = Convert.ToInt32(reader["id_profesional"]),
                Plan = (PlanSuscripcion)Enum.Parse(typeof(PlanSuscripcion), reader["plan"].ToString()),
                Puntuacion = Convert.ToInt32(reader["puntuacion"]),
                Comentario = reader["comentario"] == DBNull.Value ? null : reader["comentario"].ToString(),
                FechaValoracion = Convert.ToDateTime(reader["fecha_valoracion"])
            };

            if (conNombreProfesional)
            {
                valoracion.NombreProfesional = reader["nombre_profesional"].ToString();
                valoracion.ApellidoProfesional = reader["apellido_profesional"].ToString();
            }

            return valoracion;
        }
    }
}