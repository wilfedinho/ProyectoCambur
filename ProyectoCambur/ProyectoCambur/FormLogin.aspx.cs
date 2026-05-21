using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormLogin : System.Web.UI.Page
{
    // =========================================================
    // CONSTANTES
    // =========================================================
    private const int MAX_INTENTOS = 3;
    private const int MINUTOS_BLOQUEO = 10;

    // Claves de Session para el contador de intentos (demo)
    // TODO: en producción estos valores viven en la BD (columnas del profesional)
    private const string SK_INTENTOS = "LoginIntentosFallidos";
    private const string SK_BLOQUEADO_HTA = "LoginBloqueadoHasta";

    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            // Pre-relleno demo
            txtEmail.Text = "lucia@consultorio.com";
            // Contraseña no se pre-rellena por seguridad

            // Si viene de un logout con mensaje de éxito
            if (Request.QueryString["logout"] == "ok")
            {
                MostrarExito("Sesión cerrada correctamente. ¡Hasta la próxima!");
            }

            // Si el registro fue exitoso
            if (Request.QueryString["registro"] == "ok")
            {
                MostrarExito("Cuenta creada correctamente. Podés iniciar sesión.");
            }
        }
    }

    // =========================================================
    // EVENTO: INICIAR SESIÓN (CUS01)
    // =========================================================
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlBloqueado.Visible = false;
        pnlIntentos.Visible = false;

        if (!Page.IsValid) return;

        string email = txtEmail.Text.Trim().ToLower();
        string password = txtPassword.Text;

        // ── PASO 1: Verificar si la cuenta está bloqueada ──────
        if (CuentaBloqueada(email))
        {
            MostrarPanelBloqueado(
                "Tu cuenta fue bloqueada por exceso de intentos fallidos. " +
                "Podés intentarlo nuevamente en " + MINUTOS_BLOQUEO + " minutos " +
                "o contactar al administrador para desbloqueo inmediato.");
            return;
        }

        // ── PASO 2: Verificar dígitos verificadores ────────────
        // TODO: reemplazar por Servicios.DigitoVerificador.VerificarSistema()
        bool integridadOk = VerificarIntegridadDemo();
        if (!integridadOk)
        {
            MostrarError("El sistema no puede operar por una inconsistencia en los datos. " +
                         "Contactá al administrador.");
            return;
        }

        // ── PASO 3: Validar credenciales ───────────────────────
        // TODO: reemplazar por:
        //   string hashIngresado = Servicios.Hash.SHA256(password);
        //   BE.Profesional prof   = BLL.ProfesionalBLL.ObtenerPorEmail(email);
        //   if (prof == null || prof.PasswordHash != hashIngresado) → credenciales inválidas
        //   if (prof.Bloqueado) → cuenta bloqueada
        //   if (!prof.Activo)   → cuenta desactivada

        bool credencialesOk = ValidarCredencialesDemo(email, password);

        if (!credencialesOk)
        {
            int intentos = IncrementarIntentos(email);

            if (intentos >= MAX_INTENTOS)
            {
                BloquearCuenta(email);
                MostrarPanelBloqueado(
                    "Cuenta bloqueada automáticamente por " + MAX_INTENTOS +
                    " intentos fallidos consecutivos. Intentá nuevamente en " +
                    MINUTOS_BLOQUEO + " minutos o contactá al administrador.");
            }
            else
            {
                int restantes = MAX_INTENTOS - intentos;
                pnlIntentos.Visible = true;
                lblIntentos.Text = "⚠ Credenciales incorrectas. " +
                                       "Te queda" + (restantes == 1 ? "" : "n") +
                                       " " + restantes + " intento" +
                                       (restantes == 1 ? "" : "s") + " antes del bloqueo.";
                MostrarError("El correo o la contraseña ingresados son incorrectos.");
            }
            return;
        }

        // ── PASO 4: Verificar estado del usuario ───────────────
        // TODO: validar prof.Activo y prof.Bloqueado desde la BD
        bool usuarioActivo = VerificarUsuarioActivoDemo(email);
        if (!usuarioActivo)
        {
            MostrarError("Tu cuenta se encuentra desactivada. Contactá al administrador.");
            return;
        }

        // ── PASO 5: Inicializar sesión ─────────────────────────
        // TODO: reemplazar por:
        //   Session["IdProfesional"] = prof.Id;
        //   Session["Profesional"]   = prof.NombreCompleto;
        //   BLL.BitacoraBLL.Registrar(prof.Id, "Login", "Inicio de sesión", criticidad: 3);

        InicializarSesionDemo(email);
        ResetearIntentos(email);

        // Redirigir al dashboard
        Response.Redirect("FormDashboard.aspx");
    }

    // =========================================================
    // LÓGICA DE BLOQUEO (demo con Session)
    // TODO: en producción mover a columnas de la tabla Profesional en BD
    // =========================================================
    private bool CuentaBloqueada(string email)
    {
        var bloqueadoHasta = Session[SK_BLOQUEADO_HTA + "_" + email];
        if (bloqueadoHasta == null) return false;

        DateTime hasta = (DateTime)bloqueadoHasta;
        if (DateTime.Now < hasta) return true;

        // Bloqueo expirado → limpiar
        Session.Remove(SK_BLOQUEADO_HTA + "_" + email);
        Session.Remove(SK_INTENTOS + "_" + email);
        return false;
    }

    private int IncrementarIntentos(string email)
    {
        string key = SK_INTENTOS + "_" + email;
        int intentos = Session[key] != null ? (int)Session[key] : 0;
        intentos++;
        Session[key] = intentos;
        return intentos;
    }

    private void BloquearCuenta(string email)
    {
        Session[SK_BLOQUEADO_HTA + "_" + email] = DateTime.Now.AddMinutes(MINUTOS_BLOQUEO);
    }

    private void ResetearIntentos(string email)
    {
        Session.Remove(SK_INTENTOS + "_" + email);
        Session.Remove(SK_BLOQUEADO_HTA + "_" + email);
    }

    // =========================================================
    // VALIDACIONES DEMO
    // TODO: reemplazar por llamadas a BLL real
    // =========================================================
    private bool VerificarIntegridadDemo()
    {
        // Siempre OK en demo
        return true;
    }

    private bool ValidarCredencialesDemo(string email, string password)
    {
        // Credenciales demo válidas
        return email == "lucia@consultorio.com" && password == "Demo2026@";
    }

    private bool VerificarUsuarioActivoDemo(string email)
    {
        return true;
    }

    private void InicializarSesionDemo(string email)
    {
        Session["IdProfesional"] = 1;
        Session["Profesional"] = "Lucía Martínez";
        Session["Email"] = email;
        Session["InicioSesion"] = DateTime.Now;
    }

    // =========================================================
    // HELPERS DE MENSAJES
    // =========================================================
    private void MostrarError(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }

    private void MostrarExito(string msg)
    {
        lblMensaje.Text = msg;
        lblMensaje.CssClass = "server-success";
        lblMensaje.Visible = true;
    }

    private void MostrarPanelBloqueado(string msg)
    {
        pnlBloqueado.Visible = true;
        lblMensajeBloqueado.Text = msg;
        btnLogin.Enabled = false;
    }
}
