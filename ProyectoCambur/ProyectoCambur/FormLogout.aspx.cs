using SERVICIOS;
using System;
using System.Web;
using System.Web.UI;

public partial class FormLogout : GUI.PaginaBase
{
   
    protected void Page_Load(object sender, EventArgs e)
    {
      
        AplicarTraducciones();

        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        EjecutarLogout();
    }

    private void AplicarTraducciones()
    {
        lblTituloCerrando.Text = Traducir("logout_cerrando_titulo");
        lblSubtituloCerrando.Text = Traducir("logout_cerrando_sub");
        lblTituloError.Text = Traducir("logout_error_titulo");
        lnkIrLogin.Text = Traducir("logout_ir_login");
        lblTituloExito.Text = Traducir("logout_exito_titulo");
        lblSubtituloExito.Text = Traducir("logout_exito_sub");
    }


    private void EjecutarLogout()
    {
        try
        {
            int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;

     

            GestorSesion.Logout();

            pnlCerrando.Visible = false;
            pnlExito.Visible = true;

            Response.Write("<meta http-equiv='refresh' content='1;url=FormLogin.aspx?logout=ok'/>");
        }
        catch (Exception)
        {
            try
            {
                HttpContext.Current.Session.Abandon();
            }
            catch {  }

            pnlCerrando.Visible = false;
            pnlError.Visible = true;
            lblErrorLogout.Text = Traducir("logout_error_texto");
        }
    }
}