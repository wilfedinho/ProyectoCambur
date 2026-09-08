namespace BLL
{
    public class ComandoDarBajaProfesional : IComandoAccion
    {
        public void Ejecutar(int idEntidad) { new GestorPsicologo().Baja(idEntidad); }
    }

    public class ComandoReactivarProfesional : IComandoAccion
    {
        public void Ejecutar(int idEntidad) { new GestorPsicologo().Activar(idEntidad); }
    }

    public class ComandoDeshabilitarProfesional : IComandoAccion
    {
        public void Ejecutar(int idEntidad) { new GestorPsicologo().Deshabilitar(idEntidad); }
    }

    public class ComandoHabilitarProfesional : IComandoAccion
    {
        public void Ejecutar(int idEntidad) { new GestorPsicologo().Habilitar(idEntidad); }
    }

    public class ComandoDesbloquearProfesional : IComandoAccion
    {
        public void Ejecutar(int idEntidad) { new GestorPsicologo().Desbloquear(idEntidad); }
    }
}