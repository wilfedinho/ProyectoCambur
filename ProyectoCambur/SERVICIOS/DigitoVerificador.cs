using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICIOS
{
    public class DigitoVerificador
    {
        private static readonly List<string> TablasControladas = new List<string> {
            "Profesional", "Paciente", "Consulta", "Suscripcion", "Traduccion", "TokenRecuperacion", "Bitacora", "Idioma",
            "PermisoSimple", "Familia", "Perfil", "PermisoSimple_Familia", "Familia_Familia", "PermisoSimple_Perfil", "Perfil_Familia"
        };
        private static readonly HashSet<string> TablasFamiliaPermiso = new HashSet<string> {
            "PermisoSimple", "Familia", "Perfil", "PermisoSimple_Familia", "Familia_Familia", "PermisoSimple_Perfil", "Perfil_Familia"
        };

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

            if (entidad is Suscripcion suscripcion)
            {
                sb.Append(suscripcion.IdProfesional);
                sb.Append(suscripcion.Plan);
                sb.Append(suscripcion.Estado);
                sb.Append(suscripcion.FechaInicio);
                sb.Append(suscripcion.FechaFin);
                sb.Append(suscripcion.Precio);
                sb.Append(suscripcion.IdPagoExterno);
                sb.Append(suscripcion.UltimosCuatroTarjeta);
            }

            if (entidad is Traduccion traduccion)
            {
                sb.Append(traduccion.Idioma);
                sb.Append(traduccion.Clave);
                sb.Append(traduccion.Texto);
                sb.Append(traduccion.Pendiente);
            }

            if (entidad is TokenRecuperacion token)
            {
                sb.Append(token.IdProfesional);
                sb.Append(token.TokenHash);
                sb.Append(token.FechaGeneracion);
                sb.Append(token.FechaExpiracion);
                sb.Append(token.Usado);
            }

            if (entidad is Bitacora bitacora)
            {
                sb.Append(bitacora.Usuario);
                sb.Append(bitacora.Modulo);
                sb.Append(bitacora.Descripcion);
                sb.Append(bitacora.Criticidad);
                sb.Append(bitacora.FechaEvento);
            }

            if (entidad is Idioma idioma)
            {
                sb.Append(idioma.NombreIdioma);
                sb.Append(idioma.CodigoIso);
                sb.Append(idioma.IsDisponible);
            }

            return Cifrador.GestorCifrador.EncriptarIrreversible(sb.ToString());
        }
        public string CalcularDVHClave(string clave1, string clave2 = null)
        {
            string contenido = clave2 == null ? clave1 : clave1 + "|" + clave2;
            return Cifrador.GestorCifrador.EncriptarIrreversible(contenido);
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

            if (nombreTabla == "Suscripcion")
            {
                SuscripcionDAL suscripcionDAL = new SuscripcionDAL();
                foreach (string dvh in suscripcionDAL.ObtenerListaDVH())
                {
                    sb.Append(dvh);
                }
            }

            if (nombreTabla == "Traduccion")
            {
                TraduccionDAL traduccionDAL = new TraduccionDAL();
                foreach (string dvh in traduccionDAL.ObtenerListaDVH())
                {
                    sb.Append(dvh);
                }
            }

            if (nombreTabla == "TokenRecuperacion")
            {
                TokenRecuperacionDAL tokenDAL = new TokenRecuperacionDAL();
                foreach (string dvh in tokenDAL.ObtenerListaDVH())
                {
                    sb.Append(dvh);
                }
            }

            if (nombreTabla == "Bitacora")
            {
                BitacoraDAL bitacoraDAL = new BitacoraDAL();
                foreach (string dvh in bitacoraDAL.ObtenerListaDVH())
                {
                    sb.Append(dvh);
                }
            }

            if (nombreTabla == "Idioma")
            {
                IdiomaDAL idiomaDAL = new IdiomaDAL();
                foreach (string dvh in idiomaDAL.ObtenerListaDVH())
                {
                    sb.Append(dvh);
                }
            }

            if (TablasFamiliaPermiso.Contains(nombreTabla))
            {
                PermisoDAL permisoDAL = new PermisoDAL();
                foreach (string dvh in permisoDAL.ObtenerListaDVH(nombreTabla))
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
            object entidadPersistida = ReleerEntidadPersistida(entidad, nombreTabla) ?? entidad;
            string dvh = CalcularDVH(entidadPersistida);

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

            if (nombreTabla == "Suscripcion" && entidad is Suscripcion suscripcion)
            {
                SuscripcionDAL suscripcionDAL = new SuscripcionDAL();
                suscripcionDAL.ActualizarDVH(suscripcion.IdSuscripcion, dvh);
                suscripcion.DigitoVerificador = dvh;
            }

            if (nombreTabla == "Traduccion" && entidad is Traduccion traduccion)
            {
                TraduccionDAL traduccionDAL = new TraduccionDAL();
                traduccionDAL.ActualizarDVH(traduccion.IdTraduccion, dvh);
                traduccion.DigitoVerificador = dvh;
            }

            if (nombreTabla == "TokenRecuperacion" && entidad is TokenRecuperacion token)
            {
                TokenRecuperacionDAL tokenDAL = new TokenRecuperacionDAL();
                tokenDAL.ActualizarDVH(token.IdToken, dvh);
                token.DigitoVerificador = dvh;
            }

            if (nombreTabla == "Bitacora" && entidad is Bitacora bitacora)
            {
                BitacoraDAL bitacoraDAL = new BitacoraDAL();
                bitacoraDAL.ActualizarDVH(bitacora.IdBitacora, dvh);
                bitacora.DigitoVerificador = dvh;
            }

            if (nombreTabla == "Idioma" && entidad is Idioma idioma)
            {
                IdiomaDAL idiomaDAL = new IdiomaDAL();
                idiomaDAL.ActualizarDVH(idioma.NombreIdioma, dvh);
                idioma.DigitoVerificador = dvh;
            }
            ActualizarDVV(nombreTabla);
        }
        private object ReleerEntidadPersistida(object entidad, string nombreTabla)
        {
            if (nombreTabla == "Profesional" && entidad is Psicologo psicologo && psicologo.IdPsicologo > 0)
            {
                return new PsicologoDAL().BuscarPorId(psicologo.IdPsicologo);
            }

            if (nombreTabla == "Paciente" && entidad is Paciente paciente && paciente.IdPaciente > 0)
            {
                return new PacienteDAL().BuscarPorId(paciente.IdPaciente);
            }

            if (nombreTabla == "Consulta" && entidad is Consulta consulta && consulta.IdConsulta > 0)
            {
                return new ConsultaDAL().BuscarPorId(consulta.IdConsulta);
            }

            if (nombreTabla == "Suscripcion" && entidad is Suscripcion suscripcion && suscripcion.IdSuscripcion > 0)
            {
                return new SuscripcionDAL().BuscarPorId(suscripcion.IdSuscripcion);
            }

            if (nombreTabla == "Traduccion" && entidad is Traduccion traduccion && traduccion.IdTraduccion > 0)
            {
                return new TraduccionDAL().BuscarPorId(traduccion.IdTraduccion);
            }

            if (nombreTabla == "TokenRecuperacion" && entidad is TokenRecuperacion token && !string.IsNullOrEmpty(token.TokenHash))
            {
                return new TokenRecuperacionDAL().BuscarPorHash(token.TokenHash);
            }

            if (nombreTabla == "Bitacora" && entidad is Bitacora bitacora && bitacora.IdBitacora > 0)
            {
                return new BitacoraDAL().BuscarPorId(bitacora.IdBitacora);
            }

            if (nombreTabla == "Idioma" && entidad is Idioma idioma && !string.IsNullOrEmpty(idioma.NombreIdioma))
            {
                return new IdiomaDAL().BuscarPorNombre(idioma.NombreIdioma);
            }

            return null;
        }

        public void ActualizarDVV(string nombreTabla)
        {
            string dvv = CalcularDVV(nombreTabla);
            DigitoVerificadorDAL digitoVerificadorDAL = new DigitoVerificadorDAL();
            int cr = digitoVerificadorDAL.CalcularCount(nombreTabla);

            digitoVerificadorDAL.ActualizarDVV(nombreTabla, dvv, cr);
        }
        public void ActualizarDVHPermiso(string nombreTabla, string clave1, string clave2 = null)
        {
            string dvh = CalcularDVHClave(clave1, clave2);
            PermisoDAL permisoDAL = new PermisoDAL();
            permisoDAL.ActualizarDVH(nombreTabla, clave1, clave2, dvh);
            ActualizarDVV(nombreTabla);
        }

        #endregion

        #region Verificacion de integridad

        public bool VerificarIntegridadDVH(object entidad)
        {
            if (entidad is Psicologo || entidad is Paciente || entidad is Consulta ||
                entidad is Suscripcion || entidad is Traduccion || entidad is TokenRecuperacion || entidad is Bitacora ||
                entidad is Idioma)
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
            if (entidad is Suscripcion suscripcion) return suscripcion.DigitoVerificador;
            if (entidad is Traduccion traduccion) return traduccion.DigitoVerificador;
            if (entidad is TokenRecuperacion token) return token.DigitoVerificador;
            if (entidad is Bitacora bitacora) return bitacora.DigitoVerificador;
            if (entidad is Idioma idioma) return idioma.DigitoVerificador;
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

                if (tabla == "Suscripcion")
                {
                    SuscripcionDAL suscripcionDAL = new SuscripcionDAL();
                    List<Suscripcion> suscripciones = suscripcionDAL.ObtenerTodas();

                    bool huboInconsistenciaDeRegistro = false;

                    foreach (Suscripcion suscripcion in suscripciones)
                    {
                        if (!VerificarIntegridadDVH(suscripcion))
                        {
                            huboInconsistenciaDeRegistro = true;
                            inconsistencias.Add(new InconsistenciaDetectada(
                                "dvh_registro_inconsistente_suscripcion",
                                suscripcion.IdSuscripcion,
                                suscripcion.IdProfesional
                            ));
                        }
                    }

                    AgregarInconsistenciasDeConteo(inconsistencias, digitoVerificadorDAL, tabla, huboInconsistenciaDeRegistro);
                }

                if (tabla == "Traduccion")
                {
                    TraduccionDAL traduccionDAL = new TraduccionDAL();
                    List<Traduccion> traducciones = traduccionDAL.ObtenerTodas();

                    bool huboInconsistenciaDeRegistro = false;

                    foreach (Traduccion traduccion in traducciones)
                    {
                        if (!VerificarIntegridadDVH(traduccion))
                        {
                            huboInconsistenciaDeRegistro = true;
                            inconsistencias.Add(new InconsistenciaDetectada(
                                "dvh_registro_inconsistente_traduccion",
                                traduccion.Idioma,
                                traduccion.Clave
                            ));
                        }
                    }

                    AgregarInconsistenciasDeConteo(inconsistencias, digitoVerificadorDAL, tabla, huboInconsistenciaDeRegistro);
                }

                if (tabla == "TokenRecuperacion")
                {
                    TokenRecuperacionDAL tokenDAL = new TokenRecuperacionDAL();
                    List<TokenRecuperacion> tokens = tokenDAL.ObtenerTodos();

                    bool huboInconsistenciaDeRegistro = false;

                    foreach (TokenRecuperacion token in tokens)
                    {
                        if (!VerificarIntegridadDVH(token))
                        {
                            huboInconsistenciaDeRegistro = true;
                            inconsistencias.Add(new InconsistenciaDetectada(
                                "dvh_registro_inconsistente_token",
                                token.IdToken,
                                token.IdProfesional
                            ));
                        }
                    }

                    AgregarInconsistenciasDeConteo(inconsistencias, digitoVerificadorDAL, tabla, huboInconsistenciaDeRegistro);
                }

                if (tabla == "Bitacora")
                {
                    BitacoraDAL bitacoraDAL = new BitacoraDAL();
                    List<Bitacora> eventos = bitacoraDAL.ObtenerTodos();

                    bool huboInconsistenciaDeRegistro = false;

                    foreach (Bitacora bitacora in eventos)
                    {
                        if (!VerificarIntegridadDVH(bitacora))
                        {
                            huboInconsistenciaDeRegistro = true;
                            inconsistencias.Add(new InconsistenciaDetectada(
                                "dvh_registro_inconsistente_bitacora",
                                bitacora.IdBitacora,
                                bitacora.Usuario
                            ));
                        }
                    }

                    AgregarInconsistenciasDeConteo(inconsistencias, digitoVerificadorDAL, tabla, huboInconsistenciaDeRegistro);
                }

                if (tabla == "Idioma")
                {
                    IdiomaDAL idiomaDAL = new IdiomaDAL();
                    List<Idioma> idiomas = idiomaDAL.ObtenerTodos();

                    bool huboInconsistenciaDeRegistro = false;

                    foreach (Idioma idioma in idiomas)
                    {
                        if (!VerificarIntegridadDVH(idioma))
                        {
                            huboInconsistenciaDeRegistro = true;
                            inconsistencias.Add(new InconsistenciaDetectada(
                                "dvh_registro_inconsistente_idioma",
                                idioma.NombreIdioma,
                                idioma.CodigoIso
                            ));
                        }
                    }

                    AgregarInconsistenciasDeConteo(inconsistencias, digitoVerificadorDAL, tabla, huboInconsistenciaDeRegistro);
                }
                if (TablasFamiliaPermiso.Contains(tabla))
                {
                    PermisoDAL permisoDAL = new PermisoDAL();
                    List<PermisoDAL.FilaPermiso> filas = permisoDAL.ObtenerFilas(tabla);
                    string claveMensaje = ObtenerClaveMensajeInconsistenciaPermiso(tabla);

                    bool huboInconsistenciaDeRegistro = false;

                    foreach (PermisoDAL.FilaPermiso fila in filas)
                    {
                        string dvhCalculado = CalcularDVHClave(fila.Clave1, fila.Clave2);
                        if (dvhCalculado != fila.DigitoVerificador)
                        {
                            huboInconsistenciaDeRegistro = true;
                            inconsistencias.Add(fila.Clave2 == null
                                ? new InconsistenciaDetectada(claveMensaje, fila.Clave1)
                                : new InconsistenciaDetectada(claveMensaje, fila.Clave1, fila.Clave2));
                        }
                    }

                    AgregarInconsistenciasDeConteo(inconsistencias, digitoVerificadorDAL, tabla, huboInconsistenciaDeRegistro);
                }
            }

            return inconsistencias;
        }
        private string ObtenerClaveMensajeInconsistenciaPermiso(string tabla)
        {
            switch (tabla)
            {
                case "PermisoSimple": return "dvh_registro_inconsistente_permiso_simple";
                case "Familia": return "dvh_registro_inconsistente_familia";
                case "Perfil": return "dvh_registro_inconsistente_perfil";
                case "PermisoSimple_Familia": return "dvh_registro_inconsistente_permiso_simple_familia";
                case "Familia_Familia": return "dvh_registro_inconsistente_familia_familia";
                case "PermisoSimple_Perfil": return "dvh_registro_inconsistente_permiso_simple_perfil";
                case "Perfil_Familia": return "dvh_registro_inconsistente_perfil_familia";
                default: return "dvh_alteracion_no_asociada_tabla";
            }
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

                if (tabla == "Suscripcion")
                {
                    SuscripcionDAL suscripcionDAL = new SuscripcionDAL();
                    List<Suscripcion> suscripciones = suscripcionDAL.ObtenerTodas();

                    if (suscripciones.Count == 0)
                    {
                        ActualizarDVV(tabla);
                    }

                    foreach (Suscripcion suscripcion in suscripciones)
                    {
                        ActualizarDVH(suscripcion, tabla);
                    }
                }

                if (tabla == "Traduccion")
                {
                    TraduccionDAL traduccionDAL = new TraduccionDAL();
                    List<Traduccion> traducciones = traduccionDAL.ObtenerTodas();

                    if (traducciones.Count == 0)
                    {
                        ActualizarDVV(tabla);
                    }

                    foreach (Traduccion traduccion in traducciones)
                    {
                        ActualizarDVH(traduccion, tabla);
                    }
                }

                if (tabla == "TokenRecuperacion")
                {
                    TokenRecuperacionDAL tokenDAL = new TokenRecuperacionDAL();
                    List<TokenRecuperacion> tokens = tokenDAL.ObtenerTodos();

                    if (tokens.Count == 0)
                    {
                        ActualizarDVV(tabla);
                    }

                    foreach (TokenRecuperacion token in tokens)
                    {
                        ActualizarDVH(token, tabla);
                    }
                }

                if (tabla == "Bitacora")
                {
                    BitacoraDAL bitacoraDAL = new BitacoraDAL();
                    List<Bitacora> eventos = bitacoraDAL.ObtenerTodos();

                    if (eventos.Count == 0)
                    {
                        ActualizarDVV(tabla);
                    }

                    foreach (Bitacora bitacora in eventos)
                    {
                        ActualizarDVH(bitacora, tabla);
                    }
                }

                if (tabla == "Idioma")
                {
                    IdiomaDAL idiomaDAL = new IdiomaDAL();
                    List<Idioma> idiomas = idiomaDAL.ObtenerTodos();

                    if (idiomas.Count == 0)
                    {
                        ActualizarDVV(tabla);
                    }

                    foreach (Idioma idioma in idiomas)
                    {
                        ActualizarDVH(idioma, tabla);
                    }
                }
                if (TablasFamiliaPermiso.Contains(tabla))
                {
                    PermisoDAL permisoDAL = new PermisoDAL();
                    List<PermisoDAL.FilaPermiso> filas = permisoDAL.ObtenerFilas(tabla);

                    if (filas.Count == 0)
                    {
                        ActualizarDVV(tabla);
                    }

                    foreach (PermisoDAL.FilaPermiso fila in filas)
                    {
                        ActualizarDVHPermiso(tabla, fila.Clave1, fila.Clave2);
                    }
                }
            }

            GestorBitacora gestorBitacora = new GestorBitacora();
            gestorBitacora.RegistrarEvento(EventosBitacora.MOD_ADMINISTRACION, EventosBitacora.DESC_RECALCULO_DVH, EventosBitacora.CRIT_RECALCULO_DVH);
        }

        #endregion
    }
}