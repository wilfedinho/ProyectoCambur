using BE;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class PermisoDAL
    {
        #region Construccion del arbol completo
        private void CargarUniversoCompleto(SqlConnection cone, out Dictionary<string, PermisoSimple> simples, out Dictionary<string, PermisoCompuesto> familias)
        {
            simples = new Dictionary<string, PermisoSimple>();
            familias = new Dictionary<string, PermisoCompuesto>();

            using (SqlCommand cmd = new SqlCommand("SELECT nombre_permiso_simple FROM PermisoSimple", cone))
            using (SqlDataReader lector = cmd.ExecuteReader())
            {
                while (lector.Read())
                {
                    string nombre = lector["nombre_permiso_simple"].ToString();
                    simples[nombre] = new PermisoSimple(nombre);
                }
            }

            using (SqlCommand cmd = new SqlCommand("SELECT nombre_familia FROM Familia", cone))
            using (SqlDataReader lector = cmd.ExecuteReader())
            {
                while (lector.Read())
                {
                    string nombre = lector["nombre_familia"].ToString();
                    familias[nombre] = new PermisoCompuesto(nombre);
                }
            }

            using (SqlCommand cmd = new SqlCommand("SELECT nombre_familia, nombre_permiso_simple FROM PermisoSimple_Familia", cone))
            using (SqlDataReader lector = cmd.ExecuteReader())
            {
                while (lector.Read())
                {
                    string nombreFamilia = lector["nombre_familia"].ToString();
                    string nombreSimple = lector["nombre_permiso_simple"].ToString();

                    if (familias.TryGetValue(nombreFamilia, out PermisoCompuesto familia) &&
                        simples.TryGetValue(nombreSimple, out PermisoSimple simple))
                    {
                        familia.Agregar(simple);
                    }
                }
            }

            using (SqlCommand cmd = new SqlCommand("SELECT nombre_familia_incluye, nombre_familia_incluida FROM Familia_Familia", cone))
            using (SqlDataReader lector = cmd.ExecuteReader())
            {
                while (lector.Read())
                {
                    string padre = lector["nombre_familia_incluye"].ToString();
                    string hija = lector["nombre_familia_incluida"].ToString();

                    if (familias.TryGetValue(padre, out PermisoCompuesto familiaPadre) &&
                        familias.TryGetValue(hija, out PermisoCompuesto familiaHija))
                    {
                        familiaPadre.Agregar(familiaHija);
                    }
                }
            }
        }

        #endregion

        #region Lectura de arboles completos

        public PermisoCompuesto LeerPerfilConEstructura(string nombrePerfil)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                using (SqlCommand cmdExiste = new SqlCommand("SELECT COUNT(1) FROM Perfil WHERE nombre_perfil = @nombre", cone))
                {
                    cmdExiste.Parameters.AddWithValue("@nombre", nombrePerfil);
                    if (Convert.ToInt32(cmdExiste.ExecuteScalar()) == 0)
                    {
                        return null;
                    }
                }

                Dictionary<string, PermisoSimple> simples;
                Dictionary<string, PermisoCompuesto> familias;
                CargarUniversoCompleto(cone, out simples, out familias);

                PermisoCompuesto perfil = new PermisoCompuesto(nombrePerfil);

                using (SqlCommand cmd = new SqlCommand("SELECT nombre_permiso_simple FROM PermisoSimple_Perfil WHERE nombre_perfil = @nombre", cone))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombrePerfil);
                    using (SqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            string nombreSimple = lector["nombre_permiso_simple"].ToString();
                            if (simples.TryGetValue(nombreSimple, out PermisoSimple simple))
                            {
                                perfil.Agregar(simple);
                            }
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT nombre_familia FROM Perfil_Familia WHERE nombre_perfil = @nombre", cone))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombrePerfil);
                    using (SqlDataReader lector = cmd.ExecuteReader())
                    {
                        while (lector.Read())
                        {
                            string nombreFamilia = lector["nombre_familia"].ToString();
                            if (familias.TryGetValue(nombreFamilia, out PermisoCompuesto familia))
                            {
                                perfil.Agregar(familia);
                            }
                        }
                    }
                }

                return perfil;
            }
        }

        public PermisoCompuesto LeerFamiliaConEstructura(string nombreFamilia)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                Dictionary<string, PermisoSimple> simples;
                Dictionary<string, PermisoCompuesto> familias;
                CargarUniversoCompleto(cone, out simples, out familias);

                return familias.TryGetValue(nombreFamilia, out PermisoCompuesto familiaEncontrada) ? familiaEncontrada : null;
            }
        }

        public List<PermisoCompuesto> LeerTodasLasFamiliasConEstructura()
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                Dictionary<string, PermisoSimple> simples;
                Dictionary<string, PermisoCompuesto> familias;
                CargarUniversoCompleto(cone, out simples, out familias);

                return familias.Values.ToList();
            }
        }

        #endregion

        #region Listados simples (sin estructura, solo nombres)

        public List<string> ObtenerNombresPermisosSimples()
        {
            return ObtenerNombres("SELECT nombre_permiso_simple FROM PermisoSimple", "nombre_permiso_simple");
        }

        public List<string> ObtenerNombresFamilias()
        {
            return ObtenerNombres("SELECT nombre_familia FROM Familia", "nombre_familia");
        }

        public List<string> ObtenerNombresPerfiles()
        {
            return ObtenerNombres("SELECT nombre_perfil FROM Perfil", "nombre_perfil");
        }

        private List<string> ObtenerNombres(string query, string columna)
        {
            List<string> lista = new List<string>();
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                using (SqlCommand cmd = new SqlCommand(query, cone))
                using (SqlDataReader lector = cmd.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        lista.Add(lector[columna].ToString());
                    }
                }
            }
            return lista;
        }

        #endregion

        #region Altas

        public void InsertarPermisoSimple(string nombre)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                using (SqlCommand cmd = new SqlCommand("INSERT INTO PermisoSimple (nombre_permiso_simple) VALUES (@nombre)", cone))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void InsertarFamilia(PermisoCompuesto nuevaFamilia)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT INTO Familia (nombre_familia) VALUES (@nombre)", cone))
                {
                    cmd.Parameters.AddWithValue("@nombre", nuevaFamilia.ObtenerNombre());
                    cmd.ExecuteNonQuery();
                }

                foreach (Permiso hijo in nuevaFamilia.ObtenerHijos())
                {
                    InsertarRelacionDesdeFamilia(cone, nuevaFamilia.ObtenerNombre(), hijo);
                }
            }
        }

        public void InsertarPerfil(PermisoCompuesto nuevoPerfil)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT INTO Perfil (nombre_perfil) VALUES (@nombre)", cone))
                {
                    cmd.Parameters.AddWithValue("@nombre", nuevoPerfil.ObtenerNombre());
                    cmd.ExecuteNonQuery();
                }

                foreach (Permiso hijo in nuevoPerfil.ObtenerHijos())
                {
                    InsertarRelacionDesdePerfil(cone, nuevoPerfil.ObtenerNombre(), hijo);
                }
            }
        }

        private void InsertarRelacionDesdeFamilia(SqlConnection cone, string nombreFamilia, Permiso hijo)
        {
            if (hijo is PermisoSimple)
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO PermisoSimple_Familia (nombre_familia, nombre_permiso_simple) VALUES (@familia, @simple)", cone))
                {
                    cmd.Parameters.AddWithValue("@familia", nombreFamilia);
                    cmd.Parameters.AddWithValue("@simple", hijo.ObtenerNombre());
                    cmd.ExecuteNonQuery();
                }
            }
            else if (hijo is PermisoCompuesto)
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Familia_Familia (nombre_familia_incluye, nombre_familia_incluida) VALUES (@padre, @hija)", cone))
                {
                    cmd.Parameters.AddWithValue("@padre", nombreFamilia);
                    cmd.Parameters.AddWithValue("@hija", hijo.ObtenerNombre());
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void InsertarRelacionDesdePerfil(SqlConnection cone, string nombrePerfil, Permiso hijo)
        {
            if (hijo is PermisoSimple)
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO PermisoSimple_Perfil (nombre_perfil, nombre_permiso_simple) VALUES (@perfil, @simple)", cone))
                {
                    cmd.Parameters.AddWithValue("@perfil", nombrePerfil);
                    cmd.Parameters.AddWithValue("@simple", hijo.ObtenerNombre());
                    cmd.ExecuteNonQuery();
                }
            }
            else if (hijo is PermisoCompuesto)
            {
                using (SqlCommand cmd = new SqlCommand("INSERT INTO Perfil_Familia (nombre_perfil, nombre_familia) VALUES (@perfil, @familia)", cone))
                {
                    cmd.Parameters.AddWithValue("@perfil", nombrePerfil);
                    cmd.Parameters.AddWithValue("@familia", hijo.ObtenerNombre());
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public bool AgregarElementoAFamilia(string nombreFamilia, string nombreIncluido, out string tablaAfectada)
        {
            return AgregarElemento(nombreFamilia, nombreIncluido,
                "INSERT INTO PermisoSimple_Familia (nombre_familia, nombre_permiso_simple) VALUES (@padre, @hijo)", "PermisoSimple_Familia",
                "INSERT INTO Familia_Familia (nombre_familia_incluye, nombre_familia_incluida) VALUES (@padre, @hijo)", "Familia_Familia",
                out tablaAfectada);
        }

        public bool AgregarElementoAPerfil(string nombrePerfil, string nombreIncluido, out string tablaAfectada)
        {
            return AgregarElemento(nombrePerfil, nombreIncluido,
                "INSERT INTO PermisoSimple_Perfil (nombre_perfil, nombre_permiso_simple) VALUES (@padre, @hijo)", "PermisoSimple_Perfil",
                "INSERT INTO Perfil_Familia (nombre_perfil, nombre_familia) VALUES (@padre, @hijo)", "Perfil_Familia",
                out tablaAfectada);
        }

        private bool AgregarElemento(string nombrePadre, string nombreIncluido, string queryPermisoSimple, string tablaPermisoSimple, string queryFamilia, string tablaFamilia, out string tablaAfectada)
        {
            tablaAfectada = null;

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                using (SqlCommand cmdVerifSimple = new SqlCommand("SELECT COUNT(1) FROM PermisoSimple WHERE nombre_permiso_simple = @nombre", cone))
                {
                    cmdVerifSimple.Parameters.AddWithValue("@nombre", nombreIncluido);
                    if (Convert.ToInt32(cmdVerifSimple.ExecuteScalar()) > 0)
                    {
                        using (SqlCommand cmd = new SqlCommand(queryPermisoSimple, cone))
                        {
                            cmd.Parameters.AddWithValue("@padre", nombrePadre);
                            cmd.Parameters.AddWithValue("@hijo", nombreIncluido);
                            cmd.ExecuteNonQuery();
                        }
                        tablaAfectada = tablaPermisoSimple;
                        return true;
                    }
                }

                using (SqlCommand cmdVerifFamilia = new SqlCommand("SELECT COUNT(1) FROM Familia WHERE nombre_familia = @nombre", cone))
                {
                    cmdVerifFamilia.Parameters.AddWithValue("@nombre", nombreIncluido);
                    if (Convert.ToInt32(cmdVerifFamilia.ExecuteScalar()) > 0)
                    {
                        using (SqlCommand cmd = new SqlCommand(queryFamilia, cone))
                        {
                            cmd.Parameters.AddWithValue("@padre", nombrePadre);
                            cmd.Parameters.AddWithValue("@hijo", nombreIncluido);
                            cmd.ExecuteNonQuery();
                        }
                        tablaAfectada = tablaFamilia;
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion

        #region Bajas

        public void EliminarRelacionPermisoSimpleFamilia(string nombreFamilia, string nombrePermisoSimple)
        {
            EjecutarNonQuery("DELETE FROM PermisoSimple_Familia WHERE nombre_familia = @a AND nombre_permiso_simple = @b", nombreFamilia, nombrePermisoSimple);
        }

        public void EliminarRelacionFamiliaFamilia(string familiaPadre, string familiaHija)
        {
            EjecutarNonQuery("DELETE FROM Familia_Familia WHERE nombre_familia_incluye = @a AND nombre_familia_incluida = @b", familiaPadre, familiaHija);
        }

        public void EliminarRelacionPermisoSimplePerfil(string nombrePerfil, string nombrePermisoSimple)
        {
            EjecutarNonQuery("DELETE FROM PermisoSimple_Perfil WHERE nombre_perfil = @a AND nombre_permiso_simple = @b", nombrePerfil, nombrePermisoSimple);
        }

        public void EliminarRelacionPerfilFamilia(string nombrePerfil, string nombreFamilia)
        {
            EjecutarNonQuery("DELETE FROM Perfil_Familia WHERE nombre_perfil = @a AND nombre_familia = @b", nombrePerfil, nombreFamilia);
        }

        private void EjecutarNonQuery(string query, string valorA, string valorB)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                using (SqlCommand cmd = new SqlCommand(query, cone))
                {
                    cmd.Parameters.AddWithValue("@a", valorA);
                    cmd.Parameters.AddWithValue("@b", valorB);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void BorrarFamilia(string nombreFamilia)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                EjecutarEnConexion(cone, "DELETE FROM Familia_Familia WHERE nombre_familia_incluye = @nombre OR nombre_familia_incluida = @nombre", nombreFamilia);
                EjecutarEnConexion(cone, "DELETE FROM PermisoSimple_Familia WHERE nombre_familia = @nombre", nombreFamilia);
                EjecutarEnConexion(cone, "DELETE FROM Perfil_Familia WHERE nombre_familia = @nombre", nombreFamilia);
                EjecutarEnConexion(cone, "DELETE FROM Familia WHERE nombre_familia = @nombre", nombreFamilia);
            }
        }

        public void BorrarPerfil(string nombrePerfil)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();

                EjecutarEnConexion(cone, "DELETE FROM PermisoSimple_Perfil WHERE nombre_perfil = @nombre", nombrePerfil);
                EjecutarEnConexion(cone, "DELETE FROM Perfil_Familia WHERE nombre_perfil = @nombre", nombrePerfil);
                EjecutarEnConexion(cone, "DELETE FROM Perfil WHERE nombre_perfil = @nombre", nombrePerfil);
            }
        }

        private void EjecutarEnConexion(SqlConnection cone, string query, string nombre)
        {
            using (SqlCommand cmd = new SqlCommand(query, cone))
            {
                cmd.Parameters.AddWithValue("@nombre", nombre);
                cmd.ExecuteNonQuery();
            }
        }

        #endregion

        #region Validaciones de integridad

        public bool PermisoSimpleExiste(string nombre)
        {
            return Existe("SELECT COUNT(1) FROM PermisoSimple WHERE nombre_permiso_simple = @nombre", nombre);
        }

        public bool FamiliaExiste(string nombre)
        {
            return Existe("SELECT COUNT(1) FROM Familia WHERE nombre_familia = @nombre", nombre);
        }

        public bool PerfilExiste(string nombre)
        {
            return Existe("SELECT COUNT(1) FROM Perfil WHERE nombre_perfil = @nombre", nombre);
        }

        private bool Existe(string query, string nombre)
        {
            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                using (SqlCommand cmd = new SqlCommand(query, cone))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }


        public bool PerfilEstaAsignado(string nombrePerfil)
        {
            return Existe("SELECT COUNT(1) FROM Profesional WHERE rol_permiso = @nombre", nombrePerfil);
        }


        public bool FamiliaEstaAsignadaAPerfil(string nombreFamilia)
        {
            return Existe("SELECT COUNT(1) FROM Perfil_Familia WHERE nombre_familia = @nombre", nombreFamilia);
        }

        public bool FamiliaEstaAnidadaEnOtra(string nombreFamilia)
        {
            return Existe("SELECT COUNT(1) FROM Familia_Familia WHERE nombre_familia_incluida = @nombre", nombreFamilia);
        }

        #endregion

        #region Digito Verificador (DVH/DVV) — tablas de la familia de Permisos
        public class FilaPermiso
        {
            public string Clave1 { get; set; }
            public string Clave2 { get; set; }
            public string DigitoVerificador { get; set; }
        }

        private static void ObtenerNombresColumnas(string nombreTabla, out string tabla, out string columna1, out string columna2)
        {
            switch (nombreTabla)
            {
                case "PermisoSimple": tabla = "PermisoSimple"; columna1 = "nombre_permiso_simple"; columna2 = null; break;
                case "Familia": tabla = "Familia"; columna1 = "nombre_familia"; columna2 = null; break;
                case "Perfil": tabla = "Perfil"; columna1 = "nombre_perfil"; columna2 = null; break;
                case "PermisoSimple_Familia": tabla = "PermisoSimple_Familia"; columna1 = "nombre_familia"; columna2 = "nombre_permiso_simple"; break;
                case "Familia_Familia": tabla = "Familia_Familia"; columna1 = "nombre_familia_incluye"; columna2 = "nombre_familia_incluida"; break;
                case "PermisoSimple_Perfil": tabla = "PermisoSimple_Perfil"; columna1 = "nombre_perfil"; columna2 = "nombre_permiso_simple"; break;
                case "Perfil_Familia": tabla = "Perfil_Familia"; columna1 = "nombre_perfil"; columna2 = "nombre_familia"; break;
                default: tabla = null; columna1 = null; columna2 = null; break;
            }
        }
        public List<FilaPermiso> ObtenerFilas(string nombreTabla)
        {
            List<FilaPermiso> lista = new List<FilaPermiso>();

            string tabla, columna1, columna2;
            ObtenerNombresColumnas(nombreTabla, out tabla, out columna1, out columna2);
            if (tabla == null) return lista;

            string query = columna2 == null
                ? string.Format("SELECT {0} AS clave1, digito_verificador FROM {1} ORDER BY {0}", columna1, tabla)
                : string.Format("SELECT {0} AS clave1, {1} AS clave2, digito_verificador FROM {2} ORDER BY {0}, {1}", columna1, columna2, tabla);

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                using (SqlCommand comando = new SqlCommand(query, cone))
                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new FilaPermiso
                        {
                            Clave1 = reader["clave1"].ToString(),
                            Clave2 = columna2 == null ? null : reader["clave2"].ToString(),
                            DigitoVerificador = reader["digito_verificador"] == DBNull.Value ? null : reader["digito_verificador"].ToString()
                        });
                    }
                }
            }

            return lista;
        }
        public List<string> ObtenerListaDVH(string nombreTabla)
        {
            List<string> lista = new List<string>();
            foreach (FilaPermiso fila in ObtenerFilas(nombreTabla))
            {
                lista.Add(fila.DigitoVerificador ?? string.Empty);
            }
            return lista;
        }
        public void ActualizarDVH(string nombreTabla, string clave1, string clave2, string dvh)
        {
            string tabla, columna1, columna2;
            ObtenerNombresColumnas(nombreTabla, out tabla, out columna1, out columna2);
            if (tabla == null) return;

            string query = columna2 == null
                ? string.Format("UPDATE {0} SET digito_verificador = @dvh WHERE {1} = @clave1", tabla, columna1)
                : string.Format("UPDATE {0} SET digito_verificador = @dvh WHERE {1} = @clave1 AND {2} = @clave2", tabla, columna1, columna2);

            using (SqlConnection cone = GestorConexion.GestorCone.DevolverConexion())
            {
                cone.Open();
                using (SqlCommand cmd = new SqlCommand(query, cone))
                {
                    cmd.Parameters.AddWithValue("@dvh", dvh);
                    cmd.Parameters.AddWithValue("@clave1", clave1);
                    if (columna2 != null)
                    {
                        cmd.Parameters.AddWithValue("@clave2", clave2);
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}