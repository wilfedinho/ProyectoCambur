<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HeaderUsuario.ascx.cs" Inherits="HeaderUsuario" %>

<div class="header-user">
    <div class="user-avatar">
        <asp:Label ID="lblIniciales" runat="server" Text="" />
    </div>
    <div class="user-info">
        <asp:Label ID="lblNombreProfesional" runat="server" CssClass="user-name" Text="" />
        <asp:Label ID="lblRolActual" runat="server" CssClass="user-role" Text="" />
    </div>
</div>
