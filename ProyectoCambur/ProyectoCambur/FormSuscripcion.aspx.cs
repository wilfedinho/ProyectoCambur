using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormSuscripcion : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarSuscripcionDemo();
            CargarUsoDemo();
            MarcarPlanActual(2);
        }
    }
    private void CargarProfesionalDemo()
    {
        lblNombreProfesional.Text = "Lucía Martínez";
        lblIniciales.Text = "LM";
    }
    private void CargarSuscripcionDemo()
    {
        lblPlanNombre.Text = "Plan Profesional";
        lblPlanBadge.Text = "✓ Activa";
        lblPlanBadge.CssClass = "badge-estado activo";
        lblFechaInicio.Text = "01/03/2026";
        lblProxVencimiento.Text = "01/06/2026";
        lblMedioPago.Text = "**** **** **** 4521";
        lblPrecio.Text = "$9.990 / mes";
    }
    private void CargarUsoDemo()
    {
        lblUsoConsultas.Text = "14";
        lblUsoResumenes.Text = "3";
        lblUsoDerivaciones.Text = "1";
        lblUsoPerfiles.Text = "2";
    }
    private void MarcarPlanActual(int idPlan)
    {
        btnSelBasico.Text = "Seleccionar";
        btnSelBasico.CssClass = "btn-plan-sel";
        btnSelBasico.Enabled = true;

        btnSelProfesional.Text = "Seleccionar";
        btnSelProfesional.CssClass = "btn-plan-sel";
        btnSelProfesional.Enabled = true;

        btnSelPremium.Text = "Seleccionar";
        btnSelPremium.CssClass = "btn-plan-sel";
        btnSelPremium.Enabled = true;
        switch (idPlan)
        {
            case 1:
                btnSelBasico.Text = "Plan actual";
                btnSelBasico.CssClass = "btn-plan-actual";
                btnSelBasico.Enabled = false;
                break;
            case 2:
                btnSelProfesional.Text = "Plan actual";
                btnSelProfesional.CssClass = "btn-plan-actual";
                btnSelProfesional.Enabled = false;
                break;
            case 3:
                btnSelPremium.Text = "Plan actual";
                btnSelPremium.CssClass = "btn-plan-actual";
                btnSelPremium.Enabled = false;
                break;
        }
    }
    protected void btnActualizarPago_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        pnlActualizarPago.Visible = true;
        pnlModalCancelacion.Visible = false;
    }

    protected void btnCancelarPago_Click(object sender, EventArgs e)
    {
        pnlActualizarPago.Visible = false;
        LimpiarFormPago();
    }
    protected void btnGuardarPago_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        string tarjeta = txtNuevaTarjeta.Text.Trim();
        string vence = txtNuevoVence.Text.Trim();
        string ultimos4 = tarjeta.Replace(" ", "");
        if (ultimos4.Length >= 4)
            ultimos4 = "**** **** **** " + ultimos4.Substring(ultimos4.Length - 4);

        lblMedioPago.Text = ultimos4;
        pnlActualizarPago.Visible = false;
        LimpiarFormPago();
        MostrarExito("Medio de pago actualizado correctamente.");
    }
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        lblPlanBadge.Text = "Cancelada";
        lblPlanBadge.CssClass = "badge-estado inactivo";
        lblMensajeCancelacion.Text =
            "Tu suscripción fue cancelada. Permanecerá activa hasta el " +
            lblProxVencimiento.Text + ". Podés reactivarla en cualquier momento.";

        pnlModalCancelacion.Visible = true;
    }
    protected void btnCerrarModal_Click(object sender, EventArgs e)
    {
        pnlModalCancelacion.Visible = false;
    }
    protected void btnCambiarPlan_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;
        System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
        int nuevoPlan = Convert.ToInt32(btn.CommandArgument);
        string[] nombres = { "", "Básico", "Profesional", "Premium" };
        string[] precios = { "", "$4.990 / mes", "$9.990 / mes", "$14.990 / mes" };

        lblPlanNombre.Text = "Plan " + nombres[nuevoPlan];
        lblPrecio.Text = precios[nuevoPlan];
        MarcarPlanActual(nuevoPlan);

        MostrarExito("Plan actualizado a " + nombres[nuevoPlan] + " correctamente. El cambio se aplicará en el próximo período de facturación.");
    }
    private void LimpiarFormPago()
    {
        txtNuevaTarjeta.Text = string.Empty;
        txtNuevoVence.Text = string.Empty;
        txtNuevoCVV.Text = string.Empty;
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
