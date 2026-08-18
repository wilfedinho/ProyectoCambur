using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class Paciente
    {
        public int IdPaciente { get; set; }
        public int IdPsicologo { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string DNI { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Ocupacion { get; set; }
        public string EstadoCivil { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Sexo { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string DigitoVerificador { get; set; }

        public Paciente()
        {
        }

        public Paciente(int nIdPaciente, int nIdProfesional, string nNombre, string nApellido, string nDNI,DateTime nFechaNacimiento, string nOcupacion, string nEstadoCivil, string nEmail, string nTelefono, string nSexo, bool nActivo, DateTime nFechaRegistro, string nDigitoVerificador = null)
        {
            IdPaciente = nIdPaciente;
            IdPsicologo = nIdProfesional;
            Nombre = nNombre;
            Apellido = nApellido;
            DNI = nDNI;
            FechaNacimiento = nFechaNacimiento;
            Ocupacion = nOcupacion;
            EstadoCivil = nEstadoCivil;
            Email = nEmail;
            Telefono = nTelefono;
            Sexo = nSexo;
            Activo = nActivo;
            FechaRegistro = nFechaRegistro;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}