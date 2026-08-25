using SERVICIOS;
using System;
using GUI;
public partial class FormAccesoDenegado : PaginaBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblTitulo.Text = Traducir("acceso_denegado_titulo");
        lblTexto.Text = Traducir("acceso_denegado_texto");
        lblCountdownTexto.Text = Traducir("acceso_denegado_countdown");
        if (GestorSesion.EstaAutenticado)
        {
            string emailUsuario = GestorSesion.PsicologoActual.Email;

            new GestorBitacora().RegistrarEvento(
                emailUsuario,
                EventosBitacora.MOD_AUTENTICACION,
                EventosBitacora.DESC_CIERRE_SESION,
                EventosBitacora.CRIT_CIERRE_SESION);

            GestorSesion.Logout();
        }
    }
}