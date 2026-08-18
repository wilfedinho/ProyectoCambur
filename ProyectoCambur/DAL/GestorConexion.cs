using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DAL
{
    public class GestorConexion
    {
        private static GestorConexion Instancia;

        public static GestorConexion GestorCone
        {
            get
            {
                if (Instancia == null) { Instancia = new GestorConexion(); }
                return Instancia;
            }
        }

        private GestorConexion()
        {
        }

        public string ObtenerStringConexion()
        {
            try
            {
                ConnectionStringSettings config = ConfigurationManager.ConnectionStrings["Cambur"];
                if (config == null || string.IsNullOrWhiteSpace(config.ConnectionString))
                    throw new InvalidOperationException("No se encontro el ConnectionString 'Cambur' en el Web.config.");

                return config.ConnectionString;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la cadena de conexion: " + ex.Message, ex);
            }
        }

        public SqlConnection DevolverConexion()
        {
            return new SqlConnection(ObtenerStringConexion());
        }
    }
}