using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;

public partial class FormCambiarClave : System.Web.UI.Page
{
    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
        }
    }

    // =========================================================
    // PROFESIONAL (demo)
    // TODO: reemplazar por Session["Profesional"]
    // =========================================================
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }

    // =========================================================
    // EVENTO: CONFIRMAR CAMBIO DE CLAVE (CUS04)
    // =========================================================
    protected void btnConfirmar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        string claveActual = txtClaveActual.Text;
        string claveNueva = txtClaveNueva.Text;
        string claveConfirmacion = txtClaveConfirmacion.Text;

        // ── PASO 5: Verificar clave actual con SHA-256 ─────────
        // TODO: reemplazar por:
        //   int idProfesional = (int)Session["IdProfesional"];
        //   BE.Profesional prof = BLL.ProfesionalBLL.ObtenerPorId(idProfesional);
        //   string hashActualIngresado = ComputarSHA256(claveActual);
        //   if (hashActualIngresado != prof.PasswordHash)
        //   { MostrarError("La contraseña actual ingresada es incorrecta."); return; }

        string hashClaveActualIngresada = ComputarSHA256(claveActual);
        string hashClaveActualAlmacenada = ObtenerHashActualDemo(); // Demo

        if (hashClaveActualIngresada != hashClaveActualAlmacenada)
        {
            MostrarError("La contraseña actual ingresada es incorrecta. Verificá e intentá nuevamente.");
            LimpiarCampos();
            return;
        }

        // ── PASO 6: Validar política de seguridad ─────────────
        string errorPolitica = ValidarPoliticaContrasena(claveNueva);
        if (!string.IsNullOrEmpty(errorPolitica))
        {
            MostrarError(errorPolitica);
            txtClaveNueva.Text = string.Empty;
            txtClaveConfirmacion.Text = string.Empty;
            return;
        }

        // ── PASO 7: Verificar que sea distinta a la actual ─────
        string hashClaveNueva = ComputarSHA256(claveNueva);
        if (hashClaveNueva == hashClaveActualAlmacenada)
        {
            MostrarError("La nueva contraseña no puede ser igual a la contraseña actual. Elegí una contraseña diferente.");
            txtClaveNueva.Text = string.Empty;
            txtClaveConfirmacion.Text = string.Empty;
            return;
        }

        // ── PASO 8: Verificar coincidencia con confirmación ────
        string hashConfirmacion = ComputarSHA256(claveConfirmacion);
        if (hashClaveNueva != hashConfirmacion)
        {
            MostrarError("La nueva contraseña y su confirmación no coinciden. Verificá e intentá nuevamente.");
            txtClaveNueva.Text = string.Empty;
            txtClaveConfirmacion.Text = string.Empty;
            return;
        }

        // ── PASO 9: Persistir el nuevo hash ───────────────────
        // TODO: reemplazar por:
        //   bool ok = BLL.ProfesionalBLL.ActualizarPassword(idProfesional, hashClaveNueva);
        //   if (!ok) { MostrarError("No fue posible guardar la nueva contraseña. Intentá nuevamente."); return; }
        //   BLL.DigitoVerificadorBLL.RecalcularPorProfesional(idProfesional);
        //   BLL.BitacoraBLL.Registrar(idProfesional, "Seguridad", "Cambio de contraseña", criticidad: 2);

        // DEMO: simular guardado exitoso
        GuardarHashDemo(hashClaveNueva);

        LimpiarCampos();
        MostrarExito("Contraseña actualizada correctamente. El nuevo hash SHA-256 fue almacenado de forma segura.");
    }

    // =========================================================
    // SHA-256 — Capa de Servicios
    // TODO: mover a Servicios.Hash.SHA256(input) en producción
    // =========================================================
    private string ComputarSHA256(string input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bytes)
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }
    }

    // =========================================================
    // VALIDACIÓN DE POLÍTICA (server-side — espejo del JS)
    // =========================================================
    private string ValidarPoliticaContrasena(string clave)
    {
        if (string.IsNullOrEmpty(clave))
            return "La nueva contraseña es obligatoria.";

        if (clave.Length < 7)
            return "La contraseña debe tener al menos 7 caracteres.";

        bool tieneMayuscula = false;
        bool tieneMinuscula = false;
        bool tieneEspecial = false;

        foreach (char c in clave)
        {
            if (char.IsUpper(c)) tieneMayuscula = true;
            if (char.IsLower(c)) tieneMinuscula = true;
            if (!char.IsLetterOrDigit(c)) tieneEspecial = true;
        }

        if (!tieneMayuscula)
            return "La contraseña debe contener al menos una letra mayúscula.";
        if (!tieneMinuscula)
            return "La contraseña debe contener al menos una letra minúscula.";
        if (!tieneEspecial)
            return "La contraseña debe contener al menos un carácter especial (ej: @, !, #, $).";

        return string.Empty; // OK
    }

    // =========================================================
    // DEMO: hash de la contraseña actual almacenada
    // La contraseña demo es "Demo2026@"
    // TODO: reemplazar por prof.PasswordHash desde la BD
    // =========================================================
    private string ObtenerHashActualDemo()
    {
        return ComputarSHA256("Demo2026@");
    }

    private void GuardarHashDemo(string nuevoHash)
    {
        // En demo no persiste nada.
        // TODO: BLL.ProfesionalBLL.ActualizarPassword(idProfesional, nuevoHash);
    }

    // =========================================================
    // HELPERS
    // =========================================================
    private void LimpiarCampos()
    {
        txtClaveActual.Text = string.Empty;
        txtClaveNueva.Text = string.Empty;
        txtClaveConfirmacion.Text = string.Empty;
    }

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
}
