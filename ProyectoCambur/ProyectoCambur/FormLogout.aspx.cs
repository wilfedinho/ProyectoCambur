using SERVICIOS;
using System;
using System.Web;
using System.Web.UI;

public partial class FormLogout : System.Web.UI.Page
{
    // =========================================================
    // PAGE LOAD — ejecuta el logout al cargar la página
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

        if (!GestorSesion.EstaAutenticado)
        {
            // No hay sesion activa: no tiene sentido mostrar la pantalla de logout
            Response.Redirect("FormLogin.aspx");
            return;
        }

        EjecutarLogout();
    }

    // =========================================================
    // LÓGICA DE CIERRE DE SESIÓN (CUS02)
    // =========================================================
    private void EjecutarLogout()
    {
        try
        {
            // ── PASO 1: Recuperar datos antes de limpiar, para bitacora ──
            int idPsicologo = GestorSesion.PsicologoActual.IdPsicologo;

            // TODO: cuando se arme Bitacora en SERVICIOS, registrar aca el evento de logout:
            //   Bitacora.Registrar(idPsicologo, "Logout", "Cierre de sesion");

            // TODO: cuando se arme DigitoVerificador en SERVICIOS, recalcular aca si corresponde.

            // ── PASO 2: Limpiar la sesion ─────────────────────────────────
            GestorSesion.Logout();

            // ── PASO 3: Mostrar estado de exito y redirigir ──────────────
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
            catch { /* Ignorar errores secundarios */ }

            pnlCerrando.Visible = false;
            pnlError.Visible = true;
            lblErrorLogout.Text = "Ocurrió un error al cerrar la sesión. " +
                                      "Por seguridad, cerrá el navegador para asegurarte " +
                                      "de que la sesión quedó terminada.";

            // TODO: registrar el error en log del sistema
        }
    }
}