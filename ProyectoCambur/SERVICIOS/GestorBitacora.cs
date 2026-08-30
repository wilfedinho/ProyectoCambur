using BE;
using DAL;
using System;
using System.Collections.Generic;

namespace SERVICIOS
{
    public class GestorBitacora
    {

        public void RegistrarEvento(string modulo, string descripcion, int criticidad)
        {
            if (!GestorSesion.EstaAutenticado) return;

            RegistrarEvento(GestorSesion.PsicologoActual.Email, modulo, descripcion, criticidad);
        }
        public void RegistrarEvento(string emailUsuario, string modulo, string descripcion, int criticidad)
        {
            Bitacora evento = new Bitacora(0, emailUsuario, modulo, descripcion, criticidad, DateTime.Now);
            new BitacoraDAL().Alta(evento);
            new DigitoVerificador().ActualizarDVH(evento, "Bitacora");
        }

        public List<Bitacora> ObtenerPorFiltros(DateTime? fechaInicio, DateTime? fechaFin, string modulo, string usuario, int? criticidad)
        {
            return new BitacoraDAL().ObtenerPorFiltros(fechaInicio, fechaFin, modulo, usuario, criticidad);
        }

        public List<string> ObtenerModulosRegistrados()
        {
            return new BitacoraDAL().ObtenerModulosDistintos();
        }

        public List<string> ObtenerUsuariosRegistrados()
        {
            return new BitacoraDAL().ObtenerUsuariosDistintos();
        }

        public List<int> ObtenerCriticidadesRegistradas()
        {
            return new BitacoraDAL().ObtenerCriticidadesDistintas();
        }
    }
}