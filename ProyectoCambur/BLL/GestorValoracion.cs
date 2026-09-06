using BE;
using DAL;
using SERVICIOS;
using System;

namespace BLL
{
    public class GestorValoracion
    {
        private readonly ValoracionDAL valoracionDAL = new ValoracionDAL();
        private readonly GestorSuscripcion gestorSuscripcion = new GestorSuscripcion();

        public ResumenValoraciones ObtenerResumen()
        {
            return valoracionDAL.ObtenerResumen();
        }

        public System.Collections.Generic.List<ValoracionServicio> ObtenerTestimonios(int cantidad = 12)
        {
            return valoracionDAL.ObtenerTestimonios(cantidad);
        }

        public ValoracionServicio ObtenerValoracionDe(int idProfesional)
        {
            return valoracionDAL.BuscarPorProfesional(idProfesional);
        }
        public ValoracionServicio EnviarValoracion(int idProfesional, int puntuacion, string comentario)
        {
            if (puntuacion < 1 || puntuacion > 5)
            {
                throw new ExcepcionTraducible("error_valoracion_puntuacion_invalida");
            }

            Suscripcion activa = gestorSuscripcion.ObtenerActivaDe(idProfesional);
            if (activa == null)
            {
                throw new ExcepcionTraducible("error_valoracion_sin_suscripcion_activa");
            }

            string comentarioLimpio = string.IsNullOrWhiteSpace(comentario) ? null : comentario.Trim();
            if (comentarioLimpio != null && comentarioLimpio.Length > 500)
            {
                comentarioLimpio = comentarioLimpio.Substring(0, 500);
            }

            ValoracionServicio existente = valoracionDAL.BuscarPorProfesional(idProfesional);

            if (existente != null)
            {
                existente.Plan = activa.Plan;
                existente.Puntuacion = puntuacion;
                existente.Comentario = comentarioLimpio;
                existente.FechaValoracion = DateTime.Now;
                valoracionDAL.Actualizar(existente);

                RegistrarEvento();
                return existente;
            }

            ValoracionServicio nueva = new ValoracionServicio
            {
                IdProfesional = idProfesional,
                Plan = activa.Plan,
                Puntuacion = puntuacion,
                Comentario = comentarioLimpio,
                FechaValoracion = DateTime.Now
            };
            valoracionDAL.Alta(nueva);

            RegistrarEvento();
            return nueva;
        }

        private void RegistrarEvento()
        {
            new GestorBitacora().RegistrarEvento(
                EventosBitacora.MOD_LANDING,
                EventosBitacora.DESC_VALORACION_SERVICIO,
                EventosBitacora.CRIT_VALORACION_SERVICIO);
        }
    }
}