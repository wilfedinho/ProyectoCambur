using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Psicologo
    {
        public int IdPsicologo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Contrasena { get; set; }
        public string Idioma { get; set; }
        public string RolPermiso { get; set; }
        public bool Activo { get; set; }
        public bool IsHabilitado { get; set; }
        public bool IsBloqueado { get; set; }
        public int Intentos { get; set; }
        public DateTime HoraUltimaSesion { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DigitoVerificador { get; set; }

        public Psicologo()
        {
        }

        public Psicologo(int nIdPsicologo, string nNombre, string nApellido, string nDni, string nEmail, string nContrasena, string nIdioma, string nRolPermiso, bool nActivo, bool nIsHabilitado, bool nIsBloqueado, int nIntentos, DateTime nHoraUltimaSesion, DateTime nFechaRegistro, string nDigitoVerificador = null)
        {
            IdPsicologo = nIdPsicologo;
            Nombre = nNombre;
            Apellido = nApellido;
            Dni = nDni;
            Email = nEmail;
            Contrasena = nContrasena;
            Idioma = nIdioma;
            RolPermiso = nRolPermiso;
            Activo = nActivo;
            IsHabilitado = nIsHabilitado;
            IsBloqueado = nIsBloqueado;
            Intentos = nIntentos;
            HoraUltimaSesion = nHoraUltimaSesion;
            FechaRegistro = nFechaRegistro;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}