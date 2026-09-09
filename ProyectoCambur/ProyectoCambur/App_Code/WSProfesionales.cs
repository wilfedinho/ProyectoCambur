using BE;
using BLL;
using SERVICIOS;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using System.Xml.Serialization;

[WebService(Namespace = "http://cambur.tfi/serializacionProfesionales/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class WSProfesionales : System.Web.Services.WebService
{
    public WSProfesionales()
    {
    }

    [WebMethod(Description = "Serializa a XML los profesionales indicados por Id. El DNI y el email quedan encriptados dentro del archivo (AES).")]
    public void SerializarProfesionales(string ruta, List<int> idsProfesionales, string exportadoPor)
    {
        GestorSerializacionProfesionales gestor = new GestorSerializacionProfesionales();
        ExportacionProfesionalesXml exportacion = gestor.ConstruirExportacion(idsProfesionales, exportadoPor);

        using (FileStream fs = new FileStream(ruta, FileMode.Create))
        {
            XmlSerializer serializador = new XmlSerializer(typeof(ExportacionProfesionalesXml));
            serializador.Serialize(fs, exportacion);
        }
    }

    [WebMethod(Description = "Deserializa un XML de profesionales exportado por Cambur: valida que la metadata de origen/versión sea válida y desencripta DNI/email para su visualización.")]
    public List<ProfesionalXmlVista> DeserializarProfesionales(string ruta)
    {
        ExportacionProfesionalesXml exportacion;

        using (FileStream fs = new FileStream(ruta, FileMode.Open, FileAccess.Read))
        {
            XmlSerializer serializador = new XmlSerializer(typeof(ExportacionProfesionalesXml));
            try
            {
                exportacion = (ExportacionProfesionalesXml)serializador.Deserialize(fs);
            }
            catch (System.InvalidOperationException)
            {
                throw new ExcepcionTraducible("error_xml_formato_invalido");
            }
        }

        GestorSerializacionProfesionales gestor = new GestorSerializacionProfesionales();
        return gestor.ValidarYDesencriptar(exportacion);
    }
}