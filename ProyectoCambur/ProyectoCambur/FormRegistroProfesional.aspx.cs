using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class FormRegistroProfesional : System.Web.UI.Page
{
    private const string DEMO_NOMBRE = "Lucía";
    private const string DEMO_APELLIDO = "Martínez";
    private const string DEMO_DNI = "32145678";
    private const string DEMO_EMAIL = "lucia@consultorio.com";
    private const string DEMO_TARJETA = "4111 1111 1111 1111";
    private const string DEMO_TITULAR = "LUCIA MARTINEZ";
    private const string DEMO_VENCE = "12/28";
    private const string DEMO_PLAN = "2"; 
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarDatosDemo();
        }
    }
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
    }
    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        if (!Page.IsValid) return;
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
        int idPlan = 2;

        if (!int.TryParse(hfPlanSeleccionado.Value, out idPlan))
            idPlan = 2;
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
        bool registroExitoso = SimularRegistroDemo(nombre, apellido, dni, email, idPlan);

        if (registroExitoso)
        {
            MostrarExito("Cuenta creada correctamente. Podés iniciar sesión.");
            LimpiarFormulario();
        }
        else
        {
            MostrarError("No fue posible completar el registro. Verificá los datos ingresados.");
        }
    }
    private bool SimularRegistroDemo(string nombre, string apellido, string dni, string email, int idPlan)
    {
        if (email.ToLower() == "repetido@test.com")
            return false;
        return true;
    }
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

