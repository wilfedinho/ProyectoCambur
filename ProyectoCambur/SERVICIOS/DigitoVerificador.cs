using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICIOS
{
    public class DigitoVerificador
    {
        private static readonly List<string> TablasControladas = new List<string> { "Profesional", "Paciente", "Consulta" };

        #region Calculo de DVH (por registro)

        public string CalcularDVH(object entidad)
        {
            StringBuilder sb = new StringBuilder();

            if (entidad is Psicologo psicologo)
            {
                sb.Append(psicologo.Nombre);
                sb.Append(psicologo.Apellido);
                sb.Append(psicologo.Dni);
                sb.Append(psicologo.Email);
                sb.Append(psicologo.Contrasena);
                sb.Append(psicologo.Idioma);
                sb.Append(psicologo.RolPermiso);
                sb.Append(psicologo.Activo);
                sb.Append(psicologo.IsHabilitado);
            }

            if (entidad is Paciente paciente)
            {
                sb.Append(paciente.Nombre);
                sb.Append(paciente.Apellido);
                sb.Append(paciente.DNI);
                sb.Append(paciente.FechaNacimiento);
                sb.Append(paciente.Ocupacion);
                sb.Append(paciente.EstadoCivil);
                sb.Append(paciente.Email);
                sb.Append(paciente.Telefono);
                sb.Append(paciente.Sexo);
                sb.Append(paciente.IdPsicologo);
                sb.Append(paciente.Activo);
            }

            if (entidad is Consulta consulta)
            {
                sb.Append(consulta.IdPaciente);
                sb.Append(consulta.IdPsicologo);
                sb.Append(consulta.FechaConsulta);
                sb.Append(consulta.TiempoConsulta);
                sb.Append(consulta.FechaRegistro);
                sb.Append(consulta.Objetivos);
                sb.Append(consulta.Observaciones);
                sb.Append(consulta.Hipotesis);
                sb.Append(consulta.Intervenciones);
                sb.Append(consulta.EvolucionObservada);
                sb.Append(consulta.Diagnostico);
                sb.Append(consulta.Tratamiento);
            }

            return Cifrador.GestorCifrador.EncriptarIrreversible(sb.ToString());
        }

        #endregion

        #region Calculo de DVV (por tabla)

        public string CalcularDVV(string nombreTabla)
        {
            StringBuilder sb = new StringBuilder();

            if (nombreTabla == "Profesional")
            {
                PsicologoDAL psicologoDAL = new PsicologoDAL();
                foreach (string dvh in psicologoDAL.ObtenerListaDVH())
                {
                    sb.Append(dvh);
                }
            }

            if (nombreTabla == "Paciente")
            {
                PacienteDAL pacienteDAL = new PacienteDAL();
                foreach (string dvh in pacienteDAL.ObtenerListaDVH())
                {
                    sb.Append(dvh);
                }
            }

            if (nombreTabla == "Consulta")
            {
                ConsultaDAL consultaDAL = new ConsultaDAL();
                foreach (string dvh in consultaDAL.ObtenerListaDVH())
                {
                    sb.Append(dvh);
                }
            }

            return Cifrador.GestorCifrador.EncriptarIrreversible(sb.ToString());
        }

        #endregion

        #region Actualizacion (se llama despues de cada alta/modificacion legitima)

        public void ActualizarDVH(object entidad, string nombreTabla)
        {
            string dvh = CalcularDVH(entidad);

            if (nombreTabla == "Profesional" && entidad is Psicologo psicologo)
            {
                PsicologoDAL psicologoDAL = new PsicologoDAL();
                psicologoDAL.ActualizarDVH(psicologo.IdPsicologo, dvh);
                psicologo.DigitoVerificador = dvh;
            }

            if (nombreTabla == "Paciente" && entidad is Paciente paciente)
            {
                PacienteDAL pacienteDAL = new PacienteDAL();
                pacienteDAL.ActualizarDVH(paciente.IdPaciente, dvh);
                paciente.DigitoVerificador = dvh;
            }

            if (nombreTabla == "Consulta" && entidad is Consulta consulta)
            {
                ConsultaDAL consultaDAL = new ConsultaDAL();
                consultaDAL.ActualizarDVH(consulta.IdConsulta, dvh);
                consulta.DigitoVerificador = dvh;
            }

            ActualizarDVV(nombreTabla);
        }

        public void ActualizarDVV(string nombreTabla)
        {
            string dvv = CalcularDVV(nombreTabla);
            DigitoVerificadorDAL digitoVerificadorDAL = new DigitoVerificadorDAL();
            int cr = digitoVerificadorDAL.CalcularCount(nombreTabla);

            digitoVerificadorDAL.ActualizarDVV(nombreTabla, dvv, cr);
        }

        #endregion

        #region Verificacion de integridad

        public bool VerificarIntegridadDVH(object entidad)
        {
            if (entidad is Psicologo || entidad is Paciente || entidad is Consulta)
            {
                return CalcularDVH(entidad) == ObtenerDigitoVerificadorDe(entidad);
            }

            return false;
        }

        private string ObtenerDigitoVerificadorDe(object entidad)
        {
            if (entidad is Psicologo psicologo) return psicologo.DigitoVerificador;
            if (entidad is Paciente paciente) return paciente.DigitoVerificador;
            if (entidad is Consulta consulta) return consulta.DigitoVerificador;
            return null;
        }

        public bool VerificarIntegridadDVV(string nombreTabla)
        {
            DigitoVerificadorDAL digitoVerificadorDAL = new DigitoVerificadorDAL();
            return CalcularDVV(nombreTabla) == digitoVerificadorDAL.ObtenerDVV(nombreTabla);
        }

        public List<InconsistenciaDetectada> VerificarIntegridadTodasLasTablas()
        {
            List<InconsistenciaDetectada> inconsistencias = new List<InconsistenciaDetectada>();
            DigitoVerificadorDAL digitoVerificadorDAL = new DigitoVerificadorDAL();

            foreach (string tabla in TablasControladas)
            {
                if (tabla == "Profesional")
                {
                    PsicologoDAL psicologoDAL = new PsicologoDAL();
                    List<Psicologo> psicologos = psicologoDAL.ObtenerTodos();

                    bool huboInconsistenciaDeRegistro = false;

                    foreach (Psicologo psicologo in psicologos)
                    {
                        if (!VerificarIntegridadDVH(psicologo))
                        {
                            huboInconsistenciaDeRegistro = true;
                            inconsistencias.Add(new InconsistenciaDetectada(
                                "dvh_registro_inconsistente",
                                psicologo.Nombre + " " + psicologo.Apellido,
                                psicologo.Email
                            ));
                        }
                    }

                    AgregarInconsistenciasDeConteo(inconsistencias, digitoVerificadorDAL, tabla, huboInconsistenciaDeRegistro);
                }

                if (tabla == "Paciente")
                {
                    PacienteDAL pacienteDAL = new PacienteDAL();
                    List<Paciente> pacientes = pacienteDAL.ObtenerTodos();

                    bool huboInconsistenciaDeRegistro = false;

                    foreach (Paciente paciente in pacientes)
                    {
                        if (!VerificarIntegridadDVH(paciente))
                        {
                            huboInconsistenciaDeRegistro = true;
                            inconsistencias.Add(new InconsistenciaDetectada(
                                "dvh_registro_inconsistente_paciente",
                                paciente.Nombre + " " + paciente.Apellido,
                                paciente.DNI
                            ));
                        }
                    }

                    AgregarInconsistenciasDeConteo(inconsistencias, digitoVerificadorDAL, tabla, huboInconsistenciaDeRegistro);
                }

                if (tabla == "Consulta")
                {
                    ConsultaDAL consultaDAL = new ConsultaDAL();
                    List<Consulta> consultas = consultaDAL.ObtenerTodas();

                    bool huboInconsistenciaDeRegistro = false;

                    foreach (Consulta consulta in consultas)
                    {
                        if (!VerificarIntegridadDVH(consulta))
                        {
                            huboInconsistenciaDeRegistro = true;
                            inconsistencias.Add(new InconsistenciaDetectada(
                                "dvh_registro_inconsistente_consulta",
                                consulta.IdConsulta,
                                consulta.FechaConsulta
                            ));
                        }
                    }

                    AgregarInconsistenciasDeConteo(inconsistencias, digitoVerificadorDAL, tabla, huboInconsistenciaDeRegistro);
                }
            }

            return inconsistencias;
        }

        private void AgregarInconsistenciasDeConteo(List<InconsistenciaDetectada> inconsistencias, DigitoVerificadorDAL digitoVerificadorDAL, string tabla, bool huboInconsistenciaDeRegistro)
        {
            int cantidadReal = digitoVerificadorDAL.CalcularCount(tabla);
            int cantidadRegistrada = digitoVerificadorDAL.ObtenerCR(tabla);

            if (cantidadReal < cantidadRegistrada)
            {
                int faltantes = cantidadRegistrada - cantidadReal;
                inconsistencias.Add(new InconsistenciaDetectada("dvh_faltan_registros_tabla", tabla, faltantes));
            }
            else if (cantidadReal > cantidadRegistrada)
            {
                int sobrantes = cantidadReal - cantidadRegistrada;
                inconsistencias.Add(new InconsistenciaDetectada("dvh_registros_de_mas_tabla", tabla, sobrantes));
            }
            else if (!huboInconsistenciaDeRegistro && !VerificarIntegridadDVV(tabla))
            {
                inconsistencias.Add(new InconsistenciaDetectada("dvh_alteracion_no_asociada_tabla", tabla));
            }
        }

        public bool ExisteAlgunaInconsistencia()
        {
            return VerificarIntegridadTodasLasTablas().Count > 0;
        }

        #endregion

        #region Recalculo total (uso administrativo / puesta al dia inicial)

        public void RecalcularTodo()
        {
            foreach (string tabla in TablasControladas)
            {
                if (tabla == "Profesional")
                {
                    PsicologoDAL psicologoDAL = new PsicologoDAL();
                    List<Psicologo> psicologos = psicologoDAL.ObtenerTodos();

                    if (psicologos.Count == 0)
                    {
                        ActualizarDVV(tabla);
                    }

                    foreach (Psicologo psicologo in psicologos)
                    {
                        ActualizarDVH(psicologo, tabla);
                    }
                }

                if (tabla == "Paciente")
                {
                    PacienteDAL pacienteDAL = new PacienteDAL();
                    List<Paciente> pacientes = pacienteDAL.ObtenerTodos();

                    if (pacientes.Count == 0)
                    {
                        ActualizarDVV(tabla);
                    }

                    foreach (Paciente paciente in pacientes)
                    {
                        ActualizarDVH(paciente, tabla);
                    }
                }

                if (tabla == "Consulta")
                {
                    ConsultaDAL consultaDAL = new ConsultaDAL();
                    List<Consulta> consultas = consultaDAL.ObtenerTodas();

                    if (consultas.Count == 0)
                    {
                        ActualizarDVV(tabla);
                    }

                    foreach (Consulta consulta in consultas)
                    {
                        ActualizarDVH(consulta, tabla);
                    }
                }
            }

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_ADMINISTRACION, EventosBitacora.DESC_RECALCULO_DVH, EventosBitacora.CRIT_RECALCULO_DVH);
        }

        #endregion
    }
}