using GUI;
using System;
public partial class AyudaRapida : System.Web.UI.UserControl
{
    public string ClaveAyuda { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        PaginaBase paginaBase = Page as PaginaBase;

        if (paginaBase == null || string.IsNullOrWhiteSpace(ClaveAyuda))
        {
            Visible = false;
            return;
        }

        string claveTitulo = "ayuda_" + ClaveAyuda + "_titulo";
        string claveTexto = "ayuda_" + ClaveAyuda + "_texto";
        string titulo = paginaBase.Traducir(claveTitulo);
        string texto = paginaBase.Traducir(claveTexto);
        if (titulo == claveTitulo || texto == claveTexto)
        {
            Visible = false;
            return;
        }

        lblBotonAyudaRapida.Text = paginaBase.Traducir("btn_ayuda_rapida");
        lblTituloAyuda.Text = titulo;
        lblTextoAyuda.Text = texto;
    }
}