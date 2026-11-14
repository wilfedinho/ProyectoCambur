<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AyudaRapida.ascx.cs" Inherits="AyudaRapida" %>

<asp:LinkButton ID="btnAbrirAyudaRapida" runat="server" CssClass="btn-ayuda-rapida" ClientIDMode="Static"
    CausesValidation="false" OnClientClick="return mostrarAyudaRapida();">
    <span>❓</span> <asp:Label ID="lblBotonAyudaRapida" runat="server" Text="" />
</asp:LinkButton>

<div id="overlayAyudaRapida" class="overlay-ayuda-rapida" style="display:none;">
    <div class="ayuda-rapida-card">
        <div class="ayuda-rapida-header">
            <span class="ayuda-rapida-icono">💡</span>
            <h3 class="ayuda-rapida-titulo"><asp:Label ID="lblTituloAyuda" runat="server" Text="" /></h3>
            <button type="button" class="ayuda-rapida-cerrar" onclick="cerrarAyudaRapida();">✕</button>
        </div>
        <div class="ayuda-rapida-cuerpo">
            <asp:Label ID="lblTextoAyuda" runat="server" Text="" />
        </div>
    </div>
</div>

<script type="text/javascript">
    function mostrarAyudaRapida() {
        var overlay = document.getElementById('overlayAyudaRapida');
        if (overlay) overlay.style.display = 'flex';
        return false;
    }

    function cerrarAyudaRapida() {
        var overlay = document.getElementById('overlayAyudaRapida');
        if (overlay) overlay.style.display = 'none';
    }

    document.addEventListener('click', function (evento) {
        var overlay = document.getElementById('overlayAyudaRapida');
        if (overlay && evento.target === overlay) {
            overlay.style.display = 'none';
        }
    });
    document.addEventListener('keydown', function (evento) {
        if (evento.key === 'Escape') cerrarAyudaRapida();
    });
</script>
