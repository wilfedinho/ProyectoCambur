using System;
using System.Data.SqlClient;

namespace DAL
{
    public class DigitoVerificadorDAL
    {
        public string ObtenerDVV(string nombreTabla)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT dvv FROM DigitoVerificador WHERE tabla = @tabla";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@tabla", nombreTabla);
                    object resultado = comando.ExecuteScalar();
                    return (resultado == null || resultado == DBNull.Value) ? null : resultado.ToString();
                }
            }
        }

        public int ObtenerCR(string nombreTabla)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT cr FROM DigitoVerificador WHERE tabla = @tabla";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@tabla", nombreTabla);
                    object resultado = comando.ExecuteScalar();
                    return (resultado == null || resultado == DBNull.Value) ? 0 : Convert.ToInt32(resultado);
                }
            }
        }

       
        public void ActualizarDVV(string nombreTabla, string nuevoDVV, int nuevoCR)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = @"MERGE INTO DigitoVerificador AS destino
                                  USING (SELECT @tabla AS tabla, @dvv AS dvv, @cr AS cr) AS origen
                                  ON destino.tabla = origen.tabla
                                  WHEN MATCHED THEN
                                      UPDATE SET dvv = origen.dvv, cr = origen.cr
                                  WHEN NOT MATCHED THEN
                                      INSERT (tabla, dvv, cr) VALUES (origen.tabla, origen.dvv, origen.cr);";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@tabla", nombreTabla);
                    comando.Parameters.AddWithValue("@dvv", nuevoDVV);
                    comando.Parameters.AddWithValue("@cr", nuevoCR);
                    comando.ExecuteNonQuery();
                }
            }
        }

        
        public int CalcularCount(string nombreTabla)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT COUNT(*) FROM " + nombreTabla;
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    return (int)comando.ExecuteScalar();
                }
            }
        }
    }
}