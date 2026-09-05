using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class ModeloEvaluacionDAL
    {
        public List<ModeloEvaluacion> ObtenerTodos()
        {
            List<ModeloEvaluacion> lista = new List<ModeloEvaluacion>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM ModeloEvaluacion ORDER BY id_modelo";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(ArmarModeloEvaluacion(reader));
                    }
                }
            }

            return lista;
        }

        public ModeloEvaluacion BuscarPorId(int idModelo)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT * FROM ModeloEvaluacion WHERE id_modelo = @id_modelo";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_modelo", idModelo);
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ArmarModeloEvaluacion(reader);
                        }
                    }
                }
            }

            return null;
        }

        private ModeloEvaluacion ArmarModeloEvaluacion(SqlDataReader reader)
        {
            return new ModeloEvaluacion(
                Convert.ToInt32(reader["id_modelo"]),
                reader["nombre"].ToString(),
                reader["descripcion"] == DBNull.Value ? null : reader["descripcion"].ToString()
            );
        }
    }
}