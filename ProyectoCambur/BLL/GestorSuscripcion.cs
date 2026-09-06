using BE;
using DAL;
using SERVICIOS;
using System;
using System.Linq;

namespace BLL
{
    public class GestorSuscripcion
    {
        private const string TABLA = "Suscripcion";
        private readonly IPasarelaPago pasarelaPago;

        public GestorSuscripcion() : this(new PasarelaMercadoPago())
        {
        }

        public GestorSuscripcion(IPasarelaPago pasarelaPagoAUsar)
        {
            pasarelaPago = pasarelaPagoAUsar;
        }

        #region Consultas

        public Suscripcion ObtenerActivaDe(int idPsicologo)
        {
            return new SuscripcionDAL().BuscarActivaDe(idPsicologo);
        }

        public InfoPlan ObtenerPlanDe(Suscripcion suscripcion)
        {
            if (suscripcion == null) return null;
            return CatalogoPlanes.Planes.FirstOrDefault(p => p.Plan == suscripcion.Plan);
        }

        public UsoPeriodo ObtenerUso(int idPsicologo, DateTime desde)
        {
            UsoPeriodo uso = new UsoPeriodo();

            GestorConsulta gestorConsulta = new GestorConsulta();
            uso.Consultas = gestorConsulta.ObtenerPorPsicologo(idPsicologo).Count(c => c.FechaConsulta >= desde);

            GestorResumenClinico gestorResumen = new GestorResumenClinico();
            uso.ResumenesIA = gestorResumen.ObtenerPorPsicologo(idPsicologo).Count(r => r.FechaGeneracion >= desde);

            GestorInformeDerivacion gestorInforme = new GestorInformeDerivacion();
            uso.Derivaciones = gestorInforme.ObtenerPorPsicologo(idPsicologo).Count(i => i.FechaGeneracion >= desde);

            GestorPerfilPaciente gestorPerfil = new GestorPerfilPaciente();
            uso.Perfiles = gestorPerfil.ObtenerPorPsicologo(idPsicologo).Count(p => p.FechaGeneracion >= desde);

            return uso;
        }

        #endregion

        #region CUN13 - Cambio de plan / medio de pago
        public Psicologo CambiarPlan(int idPsicologo, int idPlan, string tokenTarjeta, string paymentMethodId)
        {
            return ProcesarPago(idPsicologo, idPlan, tokenTarjeta, paymentMethodId, permitirMismoPlan: false, soloMedioPago: false);
        }
        public Psicologo ActualizarMedioPago(int idPsicologo, string tokenTarjeta, string paymentMethodId)
        {
            Suscripcion activa = ObtenerActivaDe(idPsicologo);
            if (activa == null)
            {
                throw new ExcepcionTraducible("error_suscripcion_no_activa");
            }

            InfoPlan planActual = CatalogoPlanes.Planes.FirstOrDefault(p => p.Plan == activa.Plan);
            if (planActual == null)
            {
                throw new ExcepcionTraducible("error_suscripcion_plan_invalido");
            }

            return ProcesarPago(idPsicologo, planActual.IdPlan, tokenTarjeta, paymentMethodId, permitirMismoPlan: true, soloMedioPago: true);
        }

        private Psicologo ProcesarPago(int idPsicologo, int idPlan, string tokenTarjeta, string paymentMethodId, bool permitirMismoPlan, bool soloMedioPago)
        {
            if (string.IsNullOrWhiteSpace(tokenTarjeta) || string.IsNullOrWhiteSpace(paymentMethodId))
            {
                throw new ExcepcionTraducible("error_pago_timeout");
            }

            InfoPlan plan = CatalogoPlanes.ObtenerPorId(idPlan);
            if (plan == null)
            {
                throw new ExcepcionTraducible("error_suscripcion_plan_invalido");
            }

            PsicologoDAL psicologoDAL = new PsicologoDAL();
            Psicologo psicologo = psicologoDAL.BuscarPorId(idPsicologo);
            if (psicologo == null)
            {
                throw new ExcepcionTraducible("error_profesional_no_encontrado");
            }

            SuscripcionDAL suscripcionDAL = new SuscripcionDAL();
            Suscripcion activa = suscripcionDAL.BuscarActivaDe(idPsicologo);

            if (!permitirMismoPlan && activa != null && activa.Plan == plan.Plan)
            {
                throw new ExcepcionTraducible("error_suscripcion_mismo_plan");
            }

            DatosPago datosPago = new DatosPago
            {
                TokenTarjeta = tokenTarjeta,
                PaymentMethodId = paymentMethodId,
                Monto = plan.Precio,
                Descripcion = "Cambur — Suscripción plan " + plan.NombreComercial,
                EmailPagador = psicologo.Email,
                DniPagador = (psicologo.Dni ?? string.Empty).Replace(".", "")
            };

            ResultadoPago resultadoPago = pasarelaPago.CrearPago(datosPago);
            if (!resultadoPago.Aprobado)
            {
                throw new ExcepcionTraducible("error_pago_rechazado", resultadoPago.MotivoRechazo);
            }

            DateTime ahora = DateTime.Now;
            DigitoVerificador digitoVerificador = new DigitoVerificador();

            Suscripcion nuevaSuscripcion = new Suscripcion
            {
                IdProfesional = idPsicologo,
                Plan = plan.Plan,
                Estado = EstadoSuscripcion.Activa,
                FechaInicio = ahora,
                FechaFin = ahora.AddMonths(1),
                Precio = plan.Precio,
                IdPagoExterno = resultadoPago.IdPagoExterno,
                UltimosCuatroTarjeta = resultadoPago.UltimosCuatroTarjeta
            };

            bool cambiaRol = !string.Equals(psicologo.RolPermiso, plan.RolPermiso, StringComparison.Ordinal);
            int? idSuscripcionAnterior = activa != null ? (int?)activa.IdSuscripcion : null;
            int? idProfesionalCambioRol = cambiaRol ? (int?)idPsicologo : null;
            string nuevoRolPermisoParam = cambiaRol ? plan.RolPermiso : null;
            int idNuevaSuscripcion = suscripcionDAL.ProcesarPagoTransaccional(
                idSuscripcionAnterior,
                ahora,
                nuevaSuscripcion,
                idProfesionalCambioRol,
                nuevoRolPermisoParam);

            nuevaSuscripcion.IdSuscripcion = idNuevaSuscripcion;
            if (activa != null)
            {
                activa.Estado = EstadoSuscripcion.Vencida;
                activa.FechaFin = ahora;
                digitoVerificador.ActualizarDVH(activa, TABLA);
            }

            digitoVerificador.ActualizarDVH(nuevaSuscripcion, TABLA);

            if (cambiaRol)
            {
                psicologo.RolPermiso = plan.RolPermiso;
                digitoVerificador.ActualizarDVH(psicologo, "Profesional");
            }

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(
                EventosBitacora.MOD_SUSCRIPCION,
                soloMedioPago ? EventosBitacora.DESC_ACTUALIZAR_MEDIO_PAGO : EventosBitacora.DESC_MODIF_SUSCRIPCION,
                soloMedioPago ? EventosBitacora.CRIT_ACTUALIZAR_MEDIO_PAGO : EventosBitacora.CRIT_MODIF_SUSCRIPCION);

            return psicologo;
        }

        #endregion

        #region Cancelación / Reactivación

        public void Cancelar(int idPsicologo)
        {
            SuscripcionDAL suscripcionDAL = new SuscripcionDAL();
            Suscripcion activa = suscripcionDAL.BuscarActivaDe(idPsicologo);
            if (activa == null)
            {
                throw new ExcepcionTraducible("error_suscripcion_no_activa");
            }

            suscripcionDAL.ActualizarEstadoYFin(activa.IdSuscripcion, EstadoSuscripcion.Cancelada, activa.FechaFin);
            activa.Estado = EstadoSuscripcion.Cancelada;

            new DigitoVerificador().ActualizarDVH(activa, TABLA);
            new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_SUSCRIPCION, EventosBitacora.DESC_CANCELAR_SUSCRIPCION, EventosBitacora.CRIT_CANCELAR_SUSCRIPCION);
        }
        public void Reactivar(int idPsicologo)
        {
            SuscripcionDAL suscripcionDAL = new SuscripcionDAL();
            Suscripcion ultima = suscripcionDAL.BuscarUltimaDe(idPsicologo);

            if (ultima == null || ultima.Estado != EstadoSuscripcion.Cancelada)
            {
                throw new ExcepcionTraducible("error_suscripcion_no_cancelada");
            }

            if (ultima.FechaFin.HasValue && ultima.FechaFin.Value < DateTime.Now)
            {
                throw new ExcepcionTraducible("error_suscripcion_periodo_vencido");
            }

            suscripcionDAL.ActualizarEstadoYFin(ultima.IdSuscripcion, EstadoSuscripcion.Activa, ultima.FechaFin);
            ultima.Estado = EstadoSuscripcion.Activa;

            new DigitoVerificador().ActualizarDVH(ultima, TABLA);
            new GestorBitacora().RegistrarEvento(EventosBitacora.MOD_SUSCRIPCION, EventosBitacora.DESC_REACTIVAR_SUSCRIPCION, EventosBitacora.CRIT_REACTIVAR_SUSCRIPCION);
        }

        #endregion
    }
}