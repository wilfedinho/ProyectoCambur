using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Text;

namespace SERVICIOS
{
    public class DigitoVerificador
    {
        
        private static readonly List<string> TablasControladas = new List<string> { "Profesional" };

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
            if (entidad is Psicologo psicologo)
            {
                return CalcularDVH(psicologo) == psicologo.DigitoVerificador;
            }

            return false;
        }

        public bool VerificarIntegridadDVV(string nombreTabla)
        {
            DigitoVerificadorDAL digitoVerificadorDAL = new DigitoVerificadorDAL();
            return CalcularDVV(nombreTabla) == digitoVerificadorDAL.ObtenerDVV(nombreTabla);
        }

        
        public List<string> VerificarIntegridadTodasLasTablas()
        {
            List<string> inconsistencias = new List<string>();
            DigitoVerificadorDAL digitoVerificadorDAL = new DigitoVerificadorDAL();

            foreach (string tabla in TablasControladas)
            {
                if (tabla == "Profesional")
                {
                    PsicologoDAL psicologoDAL = new PsicologoDAL();
                    List<Psicologo> psicologos = psicologoDAL.ObtenerTodos();

                    foreach (Psicologo psicologo in psicologos)
                    {
                        if (!VerificarIntegridadDVH(psicologo))
                        {
                            inconsistencias.Add(
                                "Tabla Profesional: el registro de \"" + psicologo.Nombre + " " + psicologo.Apellido +
                                "\" (" + psicologo.Email + ") fue modificado por fuera del sistema.");
                        }
                    }

                    int cantidadReal = digitoVerificadorDAL.CalcularCount(tabla);
                    int cantidadRegistrada = digitoVerificadorDAL.ObtenerCR(tabla);

                    if (cantidadReal < cantidadRegistrada)
                    {
                        int faltantes = cantidadRegistrada - cantidadReal;
                        inconsistencias.Add(
                            "Tabla Profesional: falta" + (faltantes == 1 ? "" : "n") + " " + faltantes +
                            " registro" + (faltantes == 1 ? "" : "s") +
                            " respecto de lo esperado (posible eliminación directa en la base de datos).");
                    }
                    else if (cantidadReal > cantidadRegistrada)
                    {
                        int sobrantes = cantidadReal - cantidadRegistrada;
                        inconsistencias.Add(
                            "Tabla Profesional: hay " + sobrantes + " registro" + (sobrantes == 1 ? "" : "s") +
                            " de más respecto de lo esperado (posible inserción directa en la base de datos).");
                    }
                    else if (!VerificarIntegridadDVV(tabla))
                    {
                        
                        inconsistencias.Add("Tabla Profesional: se detectó una alteración que no pudo asociarse a un registro puntual.");
                    }
                }

                
            }

            return inconsistencias;
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
            }
        }

        #endregion
    }
}