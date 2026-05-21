using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormLogout : System.Web.UI.Page
{
    // =========================================================
    // PAGE LOAD — ejecuta el logout al cargar la página
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        // Validar que haya sesión activa antes de proceder
        // TODO: en producción verificar Session["IdProfesional"] != null
        //bool sesionActiva = (Session["IdProfesional"] != null);

        //if (!sesionActiva)
        //{
        //    // No hay sesión: redirigir directo al login
        //    Response.Redirect("FormLogin.aspx");
        //    return;
        //}

        //EjecutarLogout();
    }

    // =========================================================
    // LÓGICA DE CIERRE DE SESIÓN (CUS02)
    // =========================================================
    private void EjecutarLogout()
    {
        try
        {
            // ── PASO 1: Recuperar datos antes de limpiar ───────
            // TODO: obtener idProfesional de Session para registrar en bitácora
            int idProfesional = Session["IdProfesional"] != null
                ? (int)Session["IdProfesional"]
                : 0;

            // ── PASO 2: Registrar evento en bitácora ───────────
            // TODO: reemplazar por:
            //   BLL.BitacoraBLL.Registrar(idProfesional, "Logout", "Cierre de sesión", criticidad: 3);

            // ── PASO 3: Recalcular dígitos verificadores ───────
            // TODO: reemplazar por:
            //   BLL.DigitoVerificadorBLL.RecalcularPorProfesional(idProfesional);

            // ── PASO 4: Limpiar datos de sesión ────────────────
            Session.Clear();
            Session.Abandon();

            // Limpiar cookie de sesión ASP.NET
            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                var cookie = new HttpCookie("ASP.NET_SessionId", "");
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            // ── PASO 5: Mostrar estado de éxito y redirigir ────
            pnlCerrando.Visible = false;
            pnlExito.Visible = true;

            // Redirigir al login después de 1.5 segundos
            // (meta-refresh client-side para dar tiempo al spinner)
            Response.Write("<meta http-equiv='refresh' content='1;url=FormLogin.aspx?logout=ok'/>");
        }
        catch (Exception ex)
        {
            // ── FLUJO ALTERNATIVO 2.1: Error al limpiar sesión ─
            // En caso de fallo, forzar cierre parcial y redirigir

            try
            {
                // Intento forzado de cierre
                Session.Abandon();
            }
            catch { /* Ignorar errores secundarios */ }

            pnlCerrando.Visible = false;
            pnlError.Visible = true;
            lblErrorLogout.Text = "Ocurrió un error al cerrar la sesión. " +
                                      "Por seguridad, cerrá el navegador para asegurarte " +
                                      "de que la sesión quedó terminada.";

            // TODO: registrar el error en log del sistema
            // Logger.Error("Error en logout: " + ex.Message);
        }
    }
}
