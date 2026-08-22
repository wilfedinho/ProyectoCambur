using BE;
using SERVICIOS;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FormGestionIdiomas : GUI.PaginaBase
{
    private string IdiomaSeleccionado
    {
        get { return ViewState["IdiomaSeleccionado"] as string; }
        set { ViewState["IdiomaSeleccionado"] = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!GestorSesion.EstaAutenticado)
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        Psicologo psicologoActual = GestorSesion.PsicologoActual;

        if (psicologoActual.RolPermiso != "Web Master")
        {
            Response.Redirect("FormLogin.aspx");
            return;
        }

        if (!IsPostBack)
        {
            lblNombreProfesional.Text = psicologoActual.Nombre + " " + psicologoActual.Apellido;
            lblIniciales.Text = ObtenerIniciales(psicologoActual.Nombre, psicologoActual.Apellido);

            CargarIdiomas();
        }
    }
    protected void btnAltaIdioma_Click(object sender, EventArgs e)
    {
        lblMensaje.Visible = false;

        if (!Page.IsValid) return;

        Idioma nuevoIdioma = new Idioma(
            txtNombreIdioma.Text.Trim(),
            txtCodigoIso.Text.Trim().ToLower(),
            true,
            false
        );

        GestorIdioma gestorIdioma = new GestorIdioma();

        try
        {
            gestorIdioma.Alta(nuevoIdioma);
            MostrarExito("Idioma \"" + nuevoIdioma.NombreIdioma + "\" generado correctamente. Revisá las traducciones pendientes antes de habilitarlo para los profesionales.");

            txtNombreIdioma.Text = string.Empty;
            txtCodigoIso.Text = string.Empty;
        }
        catch (Exception ex)
        {
            MostrarError(ex.Message);
        }

        CargarIdiomas();
    }


    protected void gvIdiomas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string nombreIdioma = e.CommandArgument.ToString();
        GestorIdioma gestorIdioma = new GestorIdioma();

        switch (e.CommandName)
        {
            case "Activar":
                gestorIdioma.Activar(nombreIdioma);
                MostrarExito("Idioma \"" + nombreIdioma + "\" activado.");
                CargarIdiomas();
                return;

            case "Desactivar":
                try
                {
                    gestorIdioma.Desactivar(nombreIdioma);
                    MostrarExito("Idioma \"" + nombreIdioma + "\" desactivado.");
                }
                catch (InvalidOperationException ex)
                {
                
                    MostrarError(ex.Message);
                }
                CargarIdiomas();
                return;

            case "VerPendientes":
                IdiomaSeleccionado = nombreIdioma;
                lblIdiomaSeleccionado.Text = nombreIdioma;
                pnlTraducciones.Visible = true;
                CargarTraducciones(nombreIdioma);
                return;
        }
    }

  
    protected void gvTraducciones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "GuardarTraduccion") return;

        int idTraduccion = Convert.ToInt32(e.CommandArgument);

        GridViewRow fila = ((Control)e.CommandSource).NamingContainer as GridViewRow;
        TextBox txtTexto = (TextBox)fila.FindControl("txtTexto");

        GestorIdioma gestorIdioma = new GestorIdioma();

        try
        {
            gestorIdioma.ModificarTraduccion(idTraduccion, txtTexto.Text);
            MostrarExito("Traducción actualizada.");
        }
        catch (ArgumentException ex)
        {
            MostrarError(ex.Message);
        }

        pnlTraducciones.Visible = true;
        lblIdiomaSeleccionado.Text = IdiomaSeleccionado;
        CargarTraducciones(IdiomaSeleccionado);
    }

   
    private void CargarIdiomas()
    {
        GestorIdioma gestorIdioma = new GestorIdioma();
        List<Idioma> idiomas = gestorIdioma.ObtenerTodos();

        gvIdiomas.DataSource = idiomas;
        gvIdiomas.DataBind();
    }

    private void CargarTraducciones(string nombreIdioma)
    {
        if (string.IsNullOrEmpty(nombreIdioma)) return;

        GestorIdioma gestorIdioma = new GestorIdioma();
        List<Traduccion> traducciones = gestorIdioma.ObtenerTraduccionesDe(nombreIdioma);

        gvTraducciones.DataSource = traducciones;
        gvTraducciones.DataBind();
    }

   
    private string ObtenerIniciales(string nombre, string apellido)
    {
        string inicialNombre = string.IsNullOrEmpty(nombre) ? "" : nombre.Substring(0, 1);
        string inicialApellido = string.IsNullOrEmpty(apellido) ? "" : apellido.Substring(0, 1);
        return (inicialNombre + inicialApellido).ToUpper();
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