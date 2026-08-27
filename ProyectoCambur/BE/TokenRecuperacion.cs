using System;

namespace BE
{
    public class TokenRecuperacion
    {
        public int IdToken { get; set; }
        public int IdProfesional { get; set; }
        public string TokenHash { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool Usado { get; set; }

        public TokenRecuperacion()
        {
        }

        public TokenRecuperacion(int nIdToken, int nIdProfesional, string nTokenHash, DateTime nFechaGeneracion, DateTime nFechaExpiracion, bool nUsado)
        {
            IdToken = nIdToken;
            IdProfesional = nIdProfesional;
            TokenHash = nTokenHash;
            FechaGeneracion = nFechaGeneracion;
            FechaExpiracion = nFechaExpiracion;
            Usado = nUsado;
        }
    }
}