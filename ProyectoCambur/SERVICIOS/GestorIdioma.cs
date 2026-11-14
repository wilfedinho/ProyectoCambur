using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SERVICIOS
{
    public class GestorIdioma
    {

        private const string IDIOMA_REFERENCIA = "Español";
        private const int LARGO_MAXIMO_CODIGO_ISO = 5;

        private readonly ITraductorAutomatico traductor;

        public GestorIdioma() : this(new TraductorResx())
        {
        }


        public GestorIdioma(ITraductorAutomatico traductorAutomatico)
        {
            traductor = traductorAutomatico;
        }

        #region Operaciones Idioma


        public void Alta(Idioma nuevoIdioma)
        {
            AltaInterna(nuevoIdioma, traductor);
        }

        public void AltaPersonalizado(string nombreIdioma)
        {
            nombreIdioma = (nombreIdioma ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(nombreIdioma))
            {
                throw new ExcepcionTraducible("error_nombre_idioma_obligatorio");
            }

            IdiomaDAL idiomaDAL = new IdiomaDAL();
            if (idiomaDAL.ExisteIdioma(nombreIdioma))
            {
                throw new ExcepcionTraducible("error_idioma_duplicado");
            }

            string codigoIsoGenerado = GenerarCodigoIsoPersonalizado(nombreIdioma, idiomaDAL);
            Idioma nuevoIdiomaPersonalizado = new Idioma(nombreIdioma, codigoIsoGenerado, true, false);

            AltaInterna(nuevoIdiomaPersonalizado, new TraductorIdentidad());
        }

        private void AltaInterna(Idioma nuevoIdioma, ITraductorAutomatico traductorAUsar)
        {
            ValidarDatosIdioma(nuevoIdioma);

            IdiomaDAL idiomaDAL = new IdiomaDAL();
            if (idiomaDAL.ExisteIdioma(nuevoIdioma.NombreIdioma))
            {
                throw new ExcepcionTraducible("error_idioma_duplicado");
            }

            Idioma idiomaReferencia = idiomaDAL.BuscarPorNombre(IDIOMA_REFERENCIA);
            if (idiomaReferencia == null)
            {
                throw new ExcepcionTraducible("error_idioma_referencia_no_encontrado", IDIOMA_REFERENCIA);
            }

            TraduccionDAL traduccionDAL = new TraduccionDAL();
            List<Traduccion> clavesReferencia = traduccionDAL.ObtenerTodasLasClaves(IDIOMA_REFERENCIA);

            if (clavesReferencia.Count == 0)
            {
                throw new ExcepcionTraducible("error_idioma_referencia_sin_traducciones");
            }
            idiomaDAL.Alta(nuevoIdioma);
            new DigitoVerificador().ActualizarDVH(nuevoIdioma, "Idioma");

            List<string> claves = clavesReferencia.Select(t => t.Clave).ToList();
            Dictionary<string, string> traducciones = traductorAUsar.Traducir(claves, nuevoIdioma.CodigoIso);

            List<Traduccion> traduccionesNuevas = new List<Traduccion>();
            foreach (Traduccion original in clavesReferencia)
            {
                string texto;
                bool encontrada = traducciones.TryGetValue(original.Clave, out texto) && !string.IsNullOrWhiteSpace(texto);

                traduccionesNuevas.Add(new Traduccion(
                    0,
                    nuevoIdioma.NombreIdioma,
                    original.Clave,
                    encontrada ? texto : original.Texto,
                    !encontrada
                ));
            }

            traduccionDAL.AltaMasiva(traduccionesNuevas);
            DigitoVerificador digitoVerificador = new DigitoVerificador();
            foreach (Traduccion insertada in traduccionDAL.ObtenerTodasPorIdioma(nuevoIdioma.NombreIdioma))
            {
                digitoVerificador.ActualizarDVH(insertada, "Traduccion");
            }

            new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_GESTION_IDIOMAS, EventosBitacora.DESC_ALTA_IDIOMA, EventosBitacora.CRIT_ALTA_IDIOMA);
        }

        public void Activar(string nombreIdioma)
        {
            IdiomaDAL idiomaDAL = new IdiomaDAL();
            idiomaDAL.Activar(nombreIdioma);
            RecalcularDVHDeIdioma(idiomaDAL, nombreIdioma);
        }


        public void Desactivar(string nombreIdioma)
        {
            IdiomaDAL idiomaDAL = new IdiomaDAL();

            if (idiomaDAL.ExisteProfesionalUsandoIdioma(nombreIdioma))
            {
                idiomaDAL.ActualizarIsOcupadoCache(nombreIdioma, true);
                throw new ExcepcionTraducible("error_idioma_en_uso", nombreIdioma);
            }

            idiomaDAL.ActualizarIsOcupadoCache(nombreIdioma, false);
            idiomaDAL.Desactivar(nombreIdioma);
            RecalcularDVHDeIdioma(idiomaDAL, nombreIdioma);
            new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_GESTION_IDIOMAS, EventosBitacora.DESC_BAJA_IDIOMA, EventosBitacora.CRIT_BAJA_IDIOMA);
        }

        private void RecalcularDVHDeIdioma(IdiomaDAL idiomaDAL, string nombreIdioma)
        {
            Idioma idiomaActualizado = idiomaDAL.BuscarPorNombre(nombreIdioma);
            if (idiomaActualizado != null)
            {
                new DigitoVerificador().ActualizarDVH(idiomaActualizado, "Idioma");
            }
        }

        #endregion

        #region Operaciones Traduccion

        public void ModificarTraduccion(int idTraduccion, string nuevoTexto)
        {
            if (string.IsNullOrWhiteSpace(nuevoTexto))
            {
                throw new ExcepcionTraducible("error_traduccion_vacia");
            }

            TraduccionDAL traduccionDAL = new TraduccionDAL();
            traduccionDAL.ModificarTexto(idTraduccion, nuevoTexto);

            Traduccion actualizada = traduccionDAL.BuscarPorId(idTraduccion);
            if (actualizada != null)
            {
                new DigitoVerificador().ActualizarDVH(actualizada, "Traduccion");
            }

            new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_GESTION_IDIOMAS, EventosBitacora.DESC_MODIF_TRADUCCION, EventosBitacora.CRIT_MODIF_TRADUCCION);
        }

        public List<Traduccion> ObtenerTraduccionesDe(string nombreIdioma)
        {
            TraduccionDAL traduccionDAL = new TraduccionDAL();
            return traduccionDAL.ObtenerTodasPorIdioma(nombreIdioma);
        }

        public List<Traduccion> ObtenerPendientesDe(string nombreIdioma)
        {
            TraduccionDAL traduccionDAL = new TraduccionDAL();
            return traduccionDAL.ObtenerPendientes(nombreIdioma);
        }

        #endregion

        #region Busquedas Idioma

        public List<Idioma> ObtenerTodos()
        {
            IdiomaDAL idiomaDAL = new IdiomaDAL();
            return idiomaDAL.ObtenerTodos();
        }

        public Idioma BuscarPorNombre(string nombreIdioma)
        {
            IdiomaDAL idiomaDAL = new IdiomaDAL();
            return idiomaDAL.BuscarPorNombre(nombreIdioma);
        }

        #endregion

        #region Validaciones

        private void ValidarDatosIdioma(Idioma idioma)
        {
            if (string.IsNullOrWhiteSpace(idioma.NombreIdioma))
            {
                throw new ExcepcionTraducible("error_nombre_idioma_obligatorio");
            }

            if (string.IsNullOrWhiteSpace(idioma.CodigoIso) || idioma.CodigoIso.Length > LARGO_MAXIMO_CODIGO_ISO)
            {
                throw new ExcepcionTraducible("error_codigo_iso_obligatorio");
            }
        }
        private string GenerarCodigoIsoPersonalizado(string nombreIdioma, IdiomaDAL idiomaDAL)
        {
            string baseCodigo = new string(nombreIdioma
                .Where(char.IsLetterOrDigit)
                .Select(char.ToLowerInvariant)
                .ToArray());

            if (string.IsNullOrEmpty(baseCodigo))
            {
                baseCodigo = "idm";
            }

            if (baseCodigo.Length > LARGO_MAXIMO_CODIGO_ISO)
            {
                baseCodigo = baseCodigo.Substring(0, LARGO_MAXIMO_CODIGO_ISO);
            }

            List<string> codigosExistentes = idiomaDAL.ObtenerTodos()
                .Select(i => (i.CodigoIso ?? string.Empty).ToLowerInvariant())
                .ToList();

            if (!codigosExistentes.Contains(baseCodigo))
            {
                return baseCodigo;
            }

            string raiz = baseCodigo.Length >= LARGO_MAXIMO_CODIGO_ISO
                ? baseCodigo.Substring(0, LARGO_MAXIMO_CODIGO_ISO - 1)
                : baseCodigo;

            for (int sufijo = 1; sufijo <= 9; sufijo++)
            {
                string candidato = raiz + sufijo;
                if (!codigosExistentes.Contains(candidato))
                {
                    return candidato;
                }
            }
            return raiz + new Random().Next(0, 10);
        }

        #endregion
    }
}