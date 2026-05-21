using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class FormRegistroProfesional : System.Web.UI.Page
{
    // =========================================================
    // DATOS DEMO — hardcodeados para funcionamiento demostrativo
    // Reemplazar por llamadas a BLL cuando el backend esté listo
    // =========================================================
    private const string DEMO_NOMBRE = "Lucía";
    private const string DEMO_APELLIDO = "Martínez";
    private const string DEMO_DNI = "32145678";
    private const string DEMO_EMAIL = "lucia@consultorio.com";
    private const string DEMO_TARJETA = "4111 1111 1111 1111";
    private const string DEMO_TITULAR = "LUCIA MARTINEZ";
    private const string DEMO_VENCE = "12/28";
    private const string DEMO_PLAN = "2"; // 1=Básico 2=Profesional 3=Premium

    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarDatosDemo();
        }
    }

    // =========================================================
    // CARGA DATOS DEMO EN LOS CONTROLES
    // Cuando tengas BLL: reemplazá el contenido de este método
    // por la llamada real (ej: CargarPlanesDisponibles(), etc.)
    // =========================================================
    private void CargarDatosDemo()
    {
        txtNombre.Text = DEMO_NOMBRE;
        txtApellido.Text = DEMO_APELLIDO;
        txtDNI.Text = DEMO_DNI;
        txtEmail.Text = DEMO_EMAIL;
        txtNumeroTarjeta.Text = DEMO_TARJETA;
        txtTitular.Text = DEMO_TITULAR;
        txtVencimiento.Text = DEMO_VENCE;
        hfPlanSeleccionado.Value = DEMO_PLAN;

        // Las contraseñas no se pre-rellenan por seguridad
        // (TextMode="Password" no permite setear .Text desde server)
    }

    // =========================================================
    // EVENTO BOTÓN REGISTRAR
    // =========================================================
    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        // Ocultar mensaje previo
        lblMensaje.Visible = false;

        // ── Validación básica server-side ──────────────────────
        if (!Page.IsValid) return;

        // Obtener valores de los controles
        string nombre = txtNombre.Text.Trim();
        string apellido = txtApellido.Text.Trim();
        string dni = txtDNI.Text.Trim();
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text;
        string confirmPw = txtConfirmPassword.Text;
        string tarjeta = txtNumeroTarjeta.Text.Trim();
        string titular = txtTitular.Text.Trim();
        string vence = txtVencimiento.Text.Trim();
        string cvv = txtCVV.Text;
        int idPlan = 2; // default Profesional

        if (!int.TryParse(hfPlanSeleccionado.Value, out idPlan))
            idPlan = 2;

        // ── Validación de contraseña ───────────────────────────
        if (password != confirmPw)
        {
            MostrarError("Las contraseñas no coinciden.");
            return;
        }

        if (!ValidarFortalezaPassword(password))
        {
            MostrarError("La contraseña debe tener al menos 7 caracteres, una mayúscula y un carácter especial.");
            return;
        }

        // ── DEMO: simular registro exitoso ─────────────────────
        // TODO: reemplazar este bloque por:
        //   BE.Profesional prof = new BE.Profesional();
        //   prof.Nombre   = nombre;
        //   prof.Apellido = apellido;
        //   prof.DNI      = dni;
        //   prof.Email    = email;
        //   prof.PasswordHash = Servicios.Hash.SHA256(password);
        //   prof.IdPlan   = idPlan;
        //   bool ok = BLL.ProfesionalBLL.Registrar(prof, tarjeta, titular, vence, cvv);
        //   if (ok) Response.Redirect("FormLogin.aspx?registro=ok");
        //   else    MostrarError("No fue posible completar el registro. Verificá tus datos.");

        bool registroExitoso = SimularRegistroDemo(nombre, apellido, dni, email, idPlan);

        if (registroExitoso)
        {
            // En producción haría Response.Redirect("FormLogin.aspx?registro=ok")
            // En demo mostramos mensaje de éxito inline
            MostrarExito("Cuenta creada correctamente. Podés iniciar sesión.");
            LimpiarFormulario();
        }
        else
        {
            MostrarError("No fue posible completar el registro. Verificá los datos ingresados.");
        }
    }

    // =========================================================
    // SIMULACIÓN DE REGISTRO (DEMO)
    // Devuelve true siempre que los datos básicos sean válidos
    // =========================================================
    private bool SimularRegistroDemo(string nombre, string apellido, string dni, string email, int idPlan)
    {
        // Simular que el email ya existe (para probar el flujo de error)
        if (email.ToLower() == "repetido@test.com")
            return false;

        // Cualquier otro caso → éxito
        return true;
    }

    // =========================================================
    // VALIDACIÓN DE FORTALEZA DE CONTRASEÑA (server-side)
    // Espejo de la validación client-side en JS
    // =========================================================
    private bool ValidarFortalezaPassword(string password)
    {
        if (password.Length < 7) return false;

        bool tieneMayuscula = false;
        bool tieneEspecial = false;

        foreach (char c in password)
        {
            if (char.IsUpper(c)) tieneMayuscula = true;
            if (!char.IsLetterOrDigit(c)) tieneEspecial = true;
        }

        return tieneMayuscula && tieneEspecial;
    }

    // =========================================================
    // HELPERS DE MENSAJES
    // =========================================================
    private void MostrarError(string mensaje)
    {
        lblMensaje.Text = mensaje;
        lblMensaje.CssClass = "server-error";
        lblMensaje.Visible = true;
    }

    private void MostrarExito(string mensaje)
    {
        lblMensaje.Text = mensaje;
        lblMensaje.CssClass = "server-success";
        lblMensaje.Visible = true;
    }

    // =========================================================
    // LIMPIAR FORMULARIO DESPUÉS DEL REGISTRO DEMO
    // =========================================================
    private void LimpiarFormulario()
    {
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtDNI.Text = string.Empty;
        txtEmail.Text = string.Empty;
        txtNumeroTarjeta.Text = string.Empty;
        txtTitular.Text = string.Empty;
        txtVencimiento.Text = string.Empty;
        hfPlanSeleccionado.Value = "2";
    }
}

