using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace SERVICIOS
{
    public class GestorPermiso
    {
        private readonly PermisoDAL permisoDAL = new PermisoDAL();

        #region Chequeo de permisos (uso desde las paginas)
        public bool TienePermiso(string nombrePerfil, string nombrePermisoSimple)
        {
            PermisoCompuesto perfil = permisoDAL.LeerPerfilConEstructura(nombrePerfil);
            if (perfil == null)
            {
                return false;
            }

            return perfil.Contiene(nombrePermisoSimple);
        }
        public List<string> ObtenerPermisosEfectivos(string nombrePerfil)
        {
            List<string> resultado = new List<string>();
            PermisoCompuesto perfil = permisoDAL.LeerPerfilConEstructura(nombrePerfil);
            if (perfil == null)
            {
                return resultado;
            }

            foreach (PermisoSimple simple in perfil.ObtenerTodosLosPermisosSimples())
            {
                resultado.Add(simple.ObtenerNombre());
            }
            return resultado;
        }

        #endregion

        #region Lecturas

        public PermisoCompuesto LeerPerfilConEstructura(string nombrePerfil)
        {
            return permisoDAL.LeerPerfilConEstructura(nombrePerfil);
        }

        public PermisoCompuesto LeerFamiliaConEstructura(string nombreFamilia)
        {
            return permisoDAL.LeerFamiliaConEstructura(nombreFamilia);
        }

        public List<PermisoCompuesto> ObtenerTodasLasFamilias()
        {
            return permisoDAL.LeerTodasLasFamiliasConEstructura();
        }

        public List<string> ObtenerNombresPermisosSimples()
        {
            return permisoDAL.ObtenerNombresPermisosSimples();
        }

        public List<string> ObtenerNombresFamilias()
        {
            return permisoDAL.ObtenerNombresFamilias();
        }

        public List<string> ObtenerNombresPerfiles()
        {
            return permisoDAL.ObtenerNombresPerfiles();
        }

        #endregion

        #region Altas

        public void AltaPermisoSimple(string nombre)
        {
            ValidarNombre(nombre);

            if (permisoDAL.PermisoSimpleExiste(nombre))
            {
                throw new ExcepcionTraducible("error_permiso_simple_duplicado");
            }

            permisoDAL.InsertarPermisoSimple(nombre);
        }

        public void AltaFamilia(string nombre)
        {
            ValidarNombre(nombre);

            if (permisoDAL.FamiliaExiste(nombre))
            {
                throw new ExcepcionTraducible("error_familia_duplicada");
            }

            permisoDAL.InsertarFamilia(new PermisoCompuesto(nombre));
        }

        public void AltaPerfil(string nombre)
        {
            ValidarNombre(nombre);

            if (permisoDAL.PerfilExiste(nombre))
            {
                throw new ExcepcionTraducible("error_perfil_duplicado");
            }

            permisoDAL.InsertarPerfil(new PermisoCompuesto(nombre));
        }
        public void AgregarElementoAFamilia(string nombreFamilia, string nombreElemento)
        {
            if (nombreFamilia == nombreElemento)
            {
                throw new ExcepcionTraducible("error_familia_no_puede_incluirse_a_si_misma");
            }

            ValidarNoGeneraDuplicado(nombreFamilia, nombreElemento, esPerfil: false);

            bool agregado = permisoDAL.AgregarElementoAFamilia(nombreFamilia, nombreElemento);
            if (!agregado)
            {
                throw new ExcepcionTraducible("error_elemento_no_encontrado");
            }
        }

        public void AgregarElementoAPerfil(string nombrePerfil, string nombreElemento)
        {
            ValidarNoGeneraDuplicado(nombrePerfil, nombreElemento, esPerfil: true);

            bool agregado = permisoDAL.AgregarElementoAPerfil(nombrePerfil, nombreElemento);
            if (!agregado)
            {
                throw new ExcepcionTraducible("error_elemento_no_encontrado");
            }
        }
        private void ValidarNoGeneraDuplicado(string nombreRaiz, string nombreElemento, bool esPerfil)
        {
            PermisoCompuesto raiz = esPerfil
                ? permisoDAL.LeerPerfilConEstructura(nombreRaiz)
                : permisoDAL.LeerFamiliaConEstructura(nombreRaiz);

            if (raiz == null) return;

            HashSet<string> permisosYaPresentes = new HashSet<string>();
            foreach (PermisoSimple simple in raiz.ObtenerTodosLosPermisosSimples())
            {
                permisosYaPresentes.Add(simple.ObtenerNombre());
            }

            if (permisoDAL.PermisoSimpleExiste(nombreElemento))
            {
                if (permisosYaPresentes.Contains(nombreElemento))
                {
                    throw new ExcepcionTraducible("error_permiso_ya_incluido", nombreElemento, nombreRaiz);
                }
                return;
            }

            if (permisoDAL.FamiliaExiste(nombreElemento))
            {
                PermisoCompuesto candidata = permisoDAL.LeerFamiliaConEstructura(nombreElemento);
                if (candidata != null)
                {
                    foreach (PermisoSimple simple in candidata.ObtenerTodosLosPermisosSimples())
                    {
                        if (permisosYaPresentes.Contains(simple.ObtenerNombre()))
                        {
                            throw new ExcepcionTraducible("error_familia_genera_permisos_duplicados", nombreElemento, nombreRaiz);
                        }
                    }
                }
            }
        }

        #endregion

        #region Bajas

        public void QuitarElementoDeFamilia(string nombreFamilia, string nombreElemento, bool elementoEsFamilia)
        {
            if (elementoEsFamilia)
            {
                permisoDAL.EliminarRelacionFamiliaFamilia(nombreFamilia, nombreElemento);
            }
            else
            {
                permisoDAL.EliminarRelacionPermisoSimpleFamilia(nombreFamilia, nombreElemento);
            }
        }

        public void QuitarElementoDePerfil(string nombrePerfil, string nombreElemento, bool elementoEsFamilia)
        {
            if (elementoEsFamilia)
            {
                permisoDAL.EliminarRelacionPerfilFamilia(nombrePerfil, nombreElemento);
            }
            else
            {
                permisoDAL.EliminarRelacionPermisoSimplePerfil(nombrePerfil, nombreElemento);
            }
        }
        public void BorrarFamilia(string nombreFamilia)
        {
            if (permisoDAL.FamiliaEstaAsignadaAPerfil(nombreFamilia))
            {
                throw new ExcepcionTraducible("error_familia_asignada_a_perfil", nombreFamilia);
            }

            if (permisoDAL.FamiliaEstaAnidadaEnOtra(nombreFamilia))
            {
                throw new ExcepcionTraducible("error_familia_anidada_en_otra", nombreFamilia);
            }

            permisoDAL.BorrarFamilia(nombreFamilia);
        }
        public void BorrarPerfil(string nombrePerfil)
        {
            if (permisoDAL.PerfilEstaAsignado(nombrePerfil))
            {
                throw new ExcepcionTraducible("error_perfil_asignado_a_profesional", nombrePerfil);
            }

            permisoDAL.BorrarPerfil(nombrePerfil);
        }

        #endregion

        #region Validaciones

        private void ValidarNombre(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ExcepcionTraducible("error_nombre_permiso_obligatorio");
            }
        }

        #endregion
    }
}