using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class GestorSerializacionProfesionales
    {
        private readonly GestorPsicologo gestorPsicologo;

        public GestorSerializacionProfesionales()
        {
            gestorPsicologo = new GestorPsicologo();
        }

        public ExportacionProfesionalesXml ConstruirExportacion(List<int> idsProfesionales, string exportadoPor)
        {
            if (idsProfesionales == null || idsProfesionales.Count == 0)
            {
                throw new ExcepcionTraducible("error_xml_ningun_profesional_seleccionado");
            }

            ExportacionProfesionalesXml exportacion = new ExportacionProfesionalesXml
            {
                FechaExportacion = DateTime.Now,
                ExportadoPor = exportadoPor
            };

            foreach (int idPsicologo in idsProfesionales)
            {
                Psicologo psicologo = gestorPsicologo.BuscarPorId(idPsicologo);
                if (psicologo == null) continue;

                exportacion.Profesionales.Add(new ProfesionalXml
                {
                    IdOrigen = psicologo.IdPsicologo,
                    Nombre = psicologo.Nombre,
                    Apellido = psicologo.Apellido,
                    DniEncriptado = Cifrador.GestorCifrador.EncriptarReversible(psicologo.Dni),
                    EmailEncriptado = Cifrador.GestorCifrador.EncriptarReversible(psicologo.Email),
                    Idioma = psicologo.Idioma,
                    RolPermiso = psicologo.RolPermiso,
                    Activo = psicologo.Activo,
                    FechaRegistro = psicologo.FechaRegistro
                });
            }

            if (exportacion.Profesionales.Count == 0)
            {
                throw new ExcepcionTraducible("error_xml_ningun_profesional_seleccionado");
            }

            exportacion.CantidadProfesionales = exportacion.Profesionales.Count;
            return exportacion;
        }

        public List<ProfesionalXmlVista> ValidarYDesencriptar(ExportacionProfesionalesXml exportacion)
        {
            if (exportacion == null
                || string.IsNullOrEmpty(exportacion.SistemaOrigen)
                || exportacion.SistemaOrigen != ExportacionProfesionalesXml.SistemaOrigenEsperado)
            {
                throw new ExcepcionTraducible("error_xml_origen_invalido");
            }

            if (string.IsNullOrEmpty(exportacion.VersionFormato)
                || exportacion.VersionFormato != ExportacionProfesionalesXml.VersionFormatoActual)
            {
                throw new ExcepcionTraducible("error_xml_version_no_soportada");
            }

            if (exportacion.Profesionales == null
                || exportacion.Profesionales.Count == 0
                || exportacion.Profesionales.Count != exportacion.CantidadProfesionales)
            {
                throw new ExcepcionTraducible("error_xml_formato_invalido");
            }

            List<ProfesionalXmlVista> resultado = new List<ProfesionalXmlVista>();
            foreach (ProfesionalXml profesional in exportacion.Profesionales)
            {
                string dniDesencriptado;
                string emailDesencriptado;
                try
                {
                    dniDesencriptado = Cifrador.GestorCifrador.DesencriptarReversible(profesional.DniEncriptado);
                    emailDesencriptado = Cifrador.GestorCifrador.DesencriptarReversible(profesional.EmailEncriptado);
                }
                catch (Exception)
                {
                    throw new ExcepcionTraducible("error_xml_formato_invalido");
                }

                resultado.Add(new ProfesionalXmlVista
                {
                    IdOrigen = profesional.IdOrigen,
                    Nombre = profesional.Nombre,
                    Apellido = profesional.Apellido,
                    Dni = dniDesencriptado,
                    Email = emailDesencriptado,
                    Idioma = profesional.Idioma,
                    RolPermiso = profesional.RolPermiso,
                    Activo = profesional.Activo,
                    FechaRegistro = profesional.FechaRegistro
                });
            }

            return resultado;
        }
    }
}