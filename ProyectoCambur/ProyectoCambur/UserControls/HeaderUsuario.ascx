<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HeaderUsuario.ascx.cs" Inherits="HeaderUsuario" %>

<div class="header-user">
    <div class="header-user-clickable" onclick="ucHeaderUsuario_Toggle(event)">
        <div class="user-avatar">
            <asp:Label ID="lblIniciales" runat="server" Text="" />
        </div>
        <div class="user-info">
            <asp:Label ID="lblNombreProfesional" runat="server" CssClass="user-name" Text="" />
            <asp:Label ID="lblRolActual" runat="server" CssClass="user-role" Text="" />
        </div>
    </div>

    <div id="ucHeaderUsuarioDropdown" class="user-menu-dropdown" style="display:none;">
        <a href="FormCambiarIdioma.aspx" class="user-menu-item">
            🌐 <asp:Label ID="lblMenuCambiarIdioma" runat="server" Text="" />
        </a>
        <a href="FormCambiarClave.aspx" class="user-menu-item">
            🔑 <asp:Label ID="lblMenuCambiarClave" runat="server" Text="" />
        </a>
        <a href="FormLogout.aspx" class="user-menu-item user-menu-item-logout">
            🚪 <asp:Label ID="lblMenuCerrarSesion" runat="server" Text="" />
        </a>
    </div>
</div>

<script type="text/javascript">
    function ucHeaderUsuario_Toggle(e) {
        e.stopPropagation();
        var dd = document.getElementById('ucHeaderUsuarioDropdown');
        if (!dd) return;
        dd.style.display = (dd.style.display === 'none' || !dd.style.display) ? 'block' : 'none';
    }
    document.addEventListener('click', function () {
        var dd = document.getElementById('ucHeaderUsuarioDropdown');
        if (dd) dd.style.display = 'none';
    });
</script>
