using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormSuscripcion : System.Web.UI.Page
{
    // =========================================================
    // PAGE LOAD
    // =========================================================
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarProfesionalDemo();
            CargarSuscripcionDemo();
            CargarUsoDemo();
            MarcarPlanActual(2); // demo: plan Profesional activo
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
    // SUSCRIPCIÓN ACTIVA (demo)
    // TODO: reemplazar por BLL.SuscripcionBLL.ObtenerPorProfesional(id)
    // =========================================================
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

    // =========================================================
    // USO DEL PERÍODO (demo)
    // TODO: reemplazar por BLL.DashboardBLL.ObtenerKPIs(idProf, "MES")
    // =========================================================
    private void CargarUsoDemo()
    {
        lblUsoConsultas.Text = "14";
        lblUsoResumenes.Text = "3";
        lblUsoDerivaciones.Text = "1";
        lblUsoPerfiles.Text = "2";
    }

    // =========================================================
    // MARCAR PLAN ACTIVO EN LA COMPARATIVA
    // =========================================================
    private void MarcarPlanActual(int idPlan)
    {
        // Resetear todos
        btnSelBasico.Text = "Seleccionar";
        btnSelBasico.CssClass = "btn-plan-sel";
        btnSelBasico.Enabled = true;

        btnSelProfesional.Text = "Seleccionar";
        btnSelProfesional.CssClass = "btn-plan-sel";
        btnSelProfesional.Enabled = true;

        btnSelPremium.Text = "Seleccionar";
        btnSelPremium.CssClass = "btn-plan-sel";
        btnSelPremium.Enabled = true;

        // Marcar el activo
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

    // =========================================================
    // EVENTO: MOSTRAR FORM ACTUALIZAR PAGO
    // =========================================================
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

    // =========================================================
    // EVENTO: GUARDAR NUEVO MEDIO DE PAGO
    // =========================================================
    protected void btnGuardarPago_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        string tarjeta = txtNuevaTarjeta.Text.Trim();
        string vence = txtNuevoVence.Text.Trim();

        // TODO: reemplazar por:
        //   int idProfesional = (int)Session["IdProfesional"];
        //   bool ok = BLL.SuscripcionBLL.ActualizarMedioPago(idProfesional, tarjeta, vence, txtNuevoCVV.Text);
        //   if (!ok) { MostrarError("El banco rechazó los datos de la tarjeta."); return; }

        // DEMO: actualizar el label del medio de pago
        string ultimos4 = tarjeta.Replace(" ", "");
        if (ultimos4.Length >= 4)
            ultimos4 = "**** **** **** " + ultimos4.Substring(ultimos4.Length - 4);

        lblMedioPago.Text = ultimos4;
        pnlActualizarPago.Visible = false;
        LimpiarFormPago();
        MostrarExito("Medio de pago actualizado correctamente.");
    }

    // =========================================================
    // EVENTO: CANCELAR SUSCRIPCIÓN
    // =========================================================
    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        // TODO: reemplazar por:
        //   int idProfesional = (int)Session["IdProfesional"];
        //   bool ok = BLL.SuscripcionBLL.Cancelar(idProfesional);
        //   if (!ok) { MostrarError("No fue posible cancelar la suscripción."); return; }

        lblPlanBadge.Text = "Cancelada";
        lblPlanBadge.CssClass = "badge-estado inactivo";

        // Mostrar modal de confirmación
        lblMensajeCancelacion.Text =
            "Tu suscripción fue cancelada. Permanecerá activa hasta el " +
            lblProxVencimiento.Text + ". Podés reactivarla en cualquier momento.";

        pnlModalCancelacion.Visible = true;
    }

    // =========================================================
    // EVENTO: CERRAR MODAL CANCELACIÓN
    // =========================================================
    protected void btnCerrarModal_Click(object sender, EventArgs e)
    {
        pnlModalCancelacion.Visible = false;
    }

    // =========================================================
    // EVENTO: CAMBIAR DE PLAN
    // =========================================================
    protected void btnCambiarPlan_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
        int nuevoPlan = Convert.ToInt32(btn.CommandArgument);

        // TODO: reemplazar por:
        //   int idProfesional = (int)Session["IdProfesional"];
        //   bool ok = BLL.SuscripcionBLL.CambiarPlan(idProfesional, nuevoPlan);
        //   if (!ok) { MostrarError("No fue posible cambiar el plan."); return; }

        string[] nombres = { "", "Básico", "Profesional", "Premium" };
        string[] precios = { "", "$4.990 / mes", "$9.990 / mes", "$14.990 / mes" };

        lblPlanNombre.Text = "Plan " + nombres[nuevoPlan];
        lblPrecio.Text = precios[nuevoPlan];
        MarcarPlanActual(nuevoPlan);

        MostrarExito("Plan actualizado a " + nombres[nuevoPlan] + " correctamente. El cambio se aplicará en el próximo período de facturación.");
    }

    // =========================================================
    // HELPERS
    // =========================================================
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
