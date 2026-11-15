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
                reader["descripcion"] == DBNull.Value ? null : reader["descripcion"].ToString(),
                reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
            );
        }

        #region Digito Verificador (DVH)

        public List<string> ObtenerListaDVH()
        {
            List<string> lista = new List<string>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT digito_verificador FROM ModeloEvaluacion ORDER BY id_modelo";
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

        public void ActualizarDVH(int idModelo, string dvh)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "UPDATE ModeloEvaluacion SET digito_verificador = @digito_verificador WHERE id_modelo = @id_modelo";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@id_modelo", idModelo);
                    comando.Parameters.AddWithValue("@digito_verificador", dvh);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}