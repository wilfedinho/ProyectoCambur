namespace BLL
{
    public class ComandoDarBajaPaciente : IComandoAccion
    {
        public void Ejecutar(int idEntidad) { new GestorPaciente().Baja(idEntidad); }
    }

    public class ComandoReactivarPaciente : IComandoAccion
    {
        public void Ejecutar(int idEntidad) { new GestorPaciente().Activar(idEntidad); }
    }
}