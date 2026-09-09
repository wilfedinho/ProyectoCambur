using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace BE
{
    [Serializable]
    [XmlRoot("ExportacionProfesionalesCambur")]
    public class ExportacionProfesionalesXml
    {
        public const string SistemaOrigenEsperado = "Cambur";
        public const string VersionFormatoActual = "1.0";

        public string SistemaOrigen { get; set; }
        public string VersionFormato { get; set; }
        public DateTime FechaExportacion { get; set; }
        public string ExportadoPor { get; set; }
        public int CantidadProfesionales { get; set; }

        [XmlArray("Profesionales")]
        [XmlArrayItem("Profesional")]
        public List<ProfesionalXml> Profesionales { get; set; }

        public ExportacionProfesionalesXml()
        {
            SistemaOrigen = SistemaOrigenEsperado;
            VersionFormato = VersionFormatoActual;
            Profesionales = new List<ProfesionalXml>();
        }
    }
}