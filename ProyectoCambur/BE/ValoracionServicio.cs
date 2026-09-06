using System;

namespace BE
{
    public class ValoracionServicio
    {
        public int IdValoracion { get; set; }
        public int IdProfesional { get; set; }
        public string NombreProfesional { get; set; }
        public string ApellidoProfesional { get; set; }
        public PlanSuscripcion Plan { get; set; }
        public int Puntuacion { get; set; }
        public string Comentario { get; set; }
        public DateTime FechaValoracion { get; set; }
    }
}