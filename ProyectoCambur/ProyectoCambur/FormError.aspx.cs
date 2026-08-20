using System;

public partial class FormError : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        string codigo = Request.QueryString["codigo"];

        switch (codigo)
        {
            case "inconsistencia_bd":
                lblMensaje.Text = "Inconsistencia en la base de datos. Por favor, comuníquese con el webmaster del sistema.";
                break;

            case "no_disponible":
            default:
                lblMensaje.Text = "La página solicitada no está disponible en este momento.";
                break;
        }
    }
}