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
        public int IdIdioma { get; set; }
        public string RolPermiso { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DigitoVerificador { get; set; }

        public Psicologo()
        {
        }

        public Psicologo(int nIdPsicologo, string nNombre, string nApellido, string nDni, string nEmail, string nContrasenia, int nIdIdioma, string nRolPermiso, bool nActivo, DateTime nFechaRegistro, string nDigitoVerificador = null)
        {
            IdPsicologo = nIdPsicologo;
            Nombre = nNombre;
            Apellido = nApellido;
            Dni = nDni;
            Email = nEmail;
            Contrasena = nContrasenia;
            IdIdioma = nIdIdioma;
            RolPermiso = nRolPermiso;
            Activo = nActivo;
            FechaRegistro = nFechaRegistro;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}