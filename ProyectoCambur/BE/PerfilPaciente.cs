using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PerfilPaciente
    {
        public int IdPerfil { get; set; }
        public int IdPaciente { get; set; }
        public int IdProfesional { get; set; }
        public int IdModelo { get; set; }
        public string Resultado { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string DigitoVerificador { get; set; }

        public PerfilPaciente()
        {
        }

        public PerfilPaciente(int nIdPerfil, int nIdPaciente, int nIdProfesional, int nIdModelo, string nResultado, DateTime nFechaGeneracion, string nDigitoVerificador = null)
        {
            IdPerfil = nIdPerfil;
            IdPaciente = nIdPaciente;
            IdProfesional = nIdProfesional;
            IdModelo = nIdModelo;
            Resultado = nResultado;
            FechaGeneracion = nFechaGeneracion;
            DigitoVerificador = nDigitoVerificador;
        }
    }
}