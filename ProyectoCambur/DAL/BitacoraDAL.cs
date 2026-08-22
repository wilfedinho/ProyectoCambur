using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class BitacoraDAL
    {
        #region Alta

        public void Alta(Bitacora nuevoEvento)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "INSERT INTO Bitacora (usuario, modulo, descripcion, criticidad, fecha_evento) " +
                               "VALUES (@usuario, @modulo, @descripcion, @criticidad, @fecha_evento)";
                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddWithValue("@usuario", nuevoEvento.Usuario);
                    comando.Parameters.AddWithValue("@modulo", nuevoEvento.Modulo);
                    comando.Parameters.AddWithValue("@descripcion", nuevoEvento.Descripcion);
                    comando.Parameters.AddWithValue("@criticidad", nuevoEvento.Criticidad);
                    comando.Parameters.AddWithValue("@fecha_evento", nuevoEvento.FechaEvento);
                    comando.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Combos de filtros (solo lo que realmente esta registrado)

        public List<string> ObtenerModulosDistintos()
        {
            return ObtenerDistintos("SELECT DISTINCT modulo FROM Bitacora ORDER BY modulo", "modulo");
        }

        public List<string> ObtenerUsuariosDistintos()
        {
            return ObtenerDistintos("SELECT DISTINCT usuario FROM Bitacora ORDER BY usuario", "usuario");
        }

        public List<int> ObtenerCriticidadesDistintas()
        {
            List<int> lista = new List<int>();
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                string query = "SELECT DISTINCT criticidad FROM Bitacora ORDER BY criticidad";
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(Convert.ToInt32(reader["criticidad"]));
                    }
                }
            }
            return lista;
        }

        private List<string> ObtenerDistintos(string query, string columna)
        {
            List<string> lista = new List<string>();
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(reader[columna].ToString());
                    }
                }
            }
            return lista;
        }

        #endregion

        #region Busqueda filtrada (multi-filtro combinable)

       
        public List<Bitacora> ObtenerPorFiltros(DateTime? fechaInicio, DateTime? fechaFin, string modulo, string usuario, int? criticidad)
        {
            List<Bitacora> lista = new List<Bitacora>();

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                string query = "SELECT * FROM Bitacora WHERE 1=1";
                List<SqlParameter> parametros = new List<SqlParameter>();
                bool hayFiltros = false;

                if (fechaInicio.HasValue)
                {
                    query += " AND fecha_evento >= @fecha_inicio";
                    parametros.Add(new SqlParameter("@fecha_inicio", fechaInicio.Value.Date));
                    hayFiltros = true;
                }

                if (fechaFin.HasValue)
                {
                    query += " AND fecha_evento < @fecha_fin";
                    parametros.Add(new SqlParameter("@fecha_fin", fechaFin.Value.Date.AddDays(1)));
                    hayFiltros = true;
                }

                if (!string.IsNullOrEmpty(modulo))
                {
                    query += " AND modulo = @modulo";
                    parametros.Add(new SqlParameter("@modulo", modulo));
                    hayFiltros = true;
                }

                if (!string.IsNullOrEmpty(usuario))
                {
                    query += " AND usuario = @usuario";
                    parametros.Add(new SqlParameter("@usuario", usuario));
                    hayFiltros = true;
                }

                if (criticidad.HasValue)
                {
                    query += " AND criticidad = @criticidad";
                    parametros.Add(new SqlParameter("@criticidad", criticidad.Value));
                    hayFiltros = true;
                }

                if (!hayFiltros)
                {
                    query += " AND fecha_evento >= @fecha_default";
                    parametros.Add(new SqlParameter("@fecha_default", DateTime.Now.Date.AddDays(-2)));
                }

                query += " ORDER BY fecha_evento DESC";

                using (SqlCommand comando = new SqlCommand(query, cone))
                {
                    comando.Parameters.AddRange(parametros.ToArray());
                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Bitacora(
                                Convert.ToInt32(reader["id_bitacora"]),
                                reader["usuario"].ToString(),
                                reader["modulo"].ToString(),
                                reader["descripcion"].ToString(),
                                Convert.ToInt32(reader["criticidad"]),
                                Convert.ToDateTime(reader["fecha_evento"])
                            ));
                        }
                    }
                }
            }

            return lista;
        }

        #endregion
    }
}