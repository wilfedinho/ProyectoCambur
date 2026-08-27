<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FormMaestroConsulta.aspx.cs" Inherits="FormMaestroConsulta" %>
<%@ Register Src="~/UserControls/HeaderUsuario.ascx" TagPrefix="uc" TagName="HeaderUsuario" %>
<%@ Register Src="~/UserControls/SidebarNavegacion.ascx" TagPrefix="uc" TagName="SidebarNavegacion" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>
    <title>Cambur — Consultas</title>
    <link href="EstilosPaginas/Shared.css"             rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/HeaderUsuario.css"      rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/SidebarNavegacion.css"  rel="stylesheet" type="text/css"/>
    <link href="EstilosPaginas/FormMaestroConsulta.css" rel="stylesheet" type="text/css"/>
</head>
<body>
    <form id="form1" runat="server">

        <aside class="sidebar">
            <div class="sidebar-logo">
                <div class="logotype">CAM<span>BUR</span></div>
                <asp:Label ID="lblTaglineSidebar" runat="server" CssClass="tagline" Text="" />
            </div>
            <uc:SidebarNavegacion ID="ucSidebarNavegacion" runat="server" PaginaActual="consultas" />
            <div class="sidebar-footer">
                <a href="FormLogout.aspx" class="nav-item nav-logout"><span>🚪</span> <asp:Label ID="lblMenuCerrarSesionSidebar" runat="server" Text="" /></a>
            </div>
        </aside>

        <div class="main-wrap">

            <header class="top-header">
                <div class="header-title">
                    <asp:Label ID="lblHeaderSeccion" runat="server" CssClass="header-section" Text="" />
                    <span class="header-sep">/</span>
                    <asp:Label ID="lblHeaderPagina" runat="server" CssClass="header-page" Text="" />
                </div>
                <uc:HeaderUsuario ID="ucHeaderUsuario" runat="server" />
            </header>

            <div class="page-content">

                <asp:Label ID="lblMensaje" runat="server" Visible="false" />

                <div class="content-card">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblFormTitulo" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblSubtituloForm" runat="server" Text="" /></p>
                    </div>

                    <asp:Label ID="lblSeccionVinculo" runat="server" CssClass="section-sep" Text="" />

                    <div class="grid-3">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaPsicologo" runat="server" AssociatedControlID="ddlPsicologo" Text="" />
                            <asp:DropDownList ID="ddlPsicologo" runat="server" ClientIDMode="Static"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlPsicologo_SelectedIndexChanged" />
                            <asp:RequiredFieldValidator ID="rfvPsicologo" runat="server"
                                ControlToValidate="ddlPsicologo" InitialValue=""
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaPaciente" runat="server" AssociatedControlID="ddlPaciente" Text="" />
                            <asp:DropDownList ID="ddlPaciente" runat="server" ClientIDMode="Static" Enabled="false" />
                            <asp:RequiredFieldValidator ID="rfvPaciente" runat="server"
                                ControlToValidate="ddlPaciente" InitialValue=""
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                            <asp:Label ID="lblHintPaciente" runat="server" CssClass="hint-text" Text="" />
                        </div>
                    </div>

                    <asp:Label ID="lblSeccionDatos" runat="server" CssClass="section-sep" Text="" />

                    <div class="grid-3">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaFecha" runat="server" AssociatedControlID="txtFechaConsulta" Text="" />
                            <asp:TextBox ID="txtFechaConsulta" runat="server" TextMode="Date" ClientIDMode="Static" />
                            <asp:RequiredFieldValidator ID="rfvFecha" runat="server"
                                ControlToValidate="txtFechaConsulta" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                        </div>

                        <div class="field">
                            <asp:Label ID="lblEtiquetaTiempo" runat="server" AssociatedControlID="txtTiempoConsulta" Text="" />
                            <asp:TextBox ID="txtTiempoConsulta" runat="server" TextMode="Number" ClientIDMode="Static" placeholder="60" />
                            <asp:RequiredFieldValidator ID="rfvTiempo" runat="server"
                                ControlToValidate="txtTiempoConsulta" ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                            <asp:CompareValidator ID="cvTiempo" runat="server"
                                ControlToValidate="txtTiempoConsulta"
                                Operator="GreaterThan" ValueToCompare="0" Type="Integer"
                                ErrorMessage="."
                                CssClass="field-error" Display="Dynamic" ValidationGroup="vgConsulta" />
                        </div>
                    </div>

                    <asp:Label ID="lblSeccionContenidoClinico" runat="server" CssClass="section-sep" Text="" />
                    <p class="aviso-encriptado">🔒 <asp:Label ID="lblAvisoEncriptado" runat="server" Text="" /></p>

                    <div class="grid-2">
                        <div class="field">
                            <asp:Label ID="lblEtiquetaObjetivos" runat="server" AssociatedControlID="txtObjetivos" Text="" />
                            <asp:TextBox ID="txtObjetivos" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                        </div>
                        <div class="field">
                            <asp:Label ID="lblEtiquetaObservaciones" runat="server" AssociatedControlID="txtObservaciones" Text="" />
                            <asp:TextBox ID="txtObservaciones" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                        </div>
                        <div class="field">
                            <asp:Label ID="lblEtiquetaHipotesis" runat="server" AssociatedControlID="txtHipotesis" Text="" />
                            <asp:TextBox ID="txtHipotesis" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                        </div>
                        <div class="field">
                            <asp:Label ID="lblEtiquetaIntervenciones" runat="server" AssociatedControlID="txtIntervenciones" Text="" />
                            <asp:TextBox ID="txtIntervenciones" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                        </div>
                        <div class="field">
                            <asp:Label ID="lblEtiquetaEvolucion" runat="server" AssociatedControlID="txtEvolucionObservada" Text="" />
                            <asp:TextBox ID="txtEvolucionObservada" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                        </div>
                        <div class="field">
                            <asp:Label ID="lblEtiquetaDiagnostico" runat="server" AssociatedControlID="txtDiagnostico" Text="" />
                            <asp:TextBox ID="txtDiagnostico" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                        </div>
                        <div class="field">
                            <asp:Label ID="lblEtiquetaTratamiento" runat="server" AssociatedControlID="txtTratamiento" Text="" />
                            <asp:TextBox ID="txtTratamiento" runat="server" TextMode="MultiLine" Rows="3" ClientIDMode="Static" />
                        </div>
                    </div>

                    <div class="form-actions">
                        <asp:LinkButton ID="btnCancelarEdicion" runat="server"
                            CssClass="btn-secondary" Text=""
                            OnClick="btnCancelarEdicion_Click" CausesValidation="false" Visible="false" />
                        <asp:Button ID="btnGuardar" runat="server"
                            Text=""
                            CssClass="btn-primary"
                            ValidationGroup="vgConsulta"
                            OnClick="btnGuardar_Click" />
                    </div>
                </div>

                <asp:HiddenField ID="hdnIdConsulta" runat="server" Value="0" />

                <div class="content-card mt-24">
                    <div class="card-header">
                        <h2 class="card-title"><asp:Label ID="lblTituloListado" runat="server" Text="" /></h2>
                        <p class="card-subtitle"><asp:Label ID="lblSubtituloListado" runat="server" Text="" /></p>
                    </div>

                    <div class="table-wrap">
                        <asp:GridView ID="gvConsultas" runat="server"
                            CssClass="data-table"
                            AutoGenerateColumns="false"
                            GridLines="None"
                            AllowPaging="True"
                            PageSize="50"
                            OnRowCommand="gvConsultas_RowCommand"
                            OnRowDataBound="gvConsultas_RowDataBound"
                            OnPageIndexChanging="gvConsultas_PageIndexChanging">

                            <PagerStyle CssClass="table-pager" />
                            <PagerSettings Mode="NumericFirstLast" PageButtonCount="10" />

                            <EmptyDataRowStyle CssClass="empty-row" />
                            <HeaderStyle      CssClass="table-header" />
                            <RowStyle         CssClass="table-row" />
                            <AlternatingRowStyle CssClass="table-row table-row-alt" />

                            <Columns>
                                <asp:BoundField DataField="NombrePaciente"  HeaderText="Paciente"   HeaderStyle-CssClass="th-left" />
                                <asp:BoundField DataField="NombrePsicologo" HeaderText="Psicólogo"  HeaderStyle-CssClass="th-left" />
                                <asp:BoundField DataField="FechaConsulta"  HeaderText="Fecha"       HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro"
                                    DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="TiempoConsulta" HeaderText="Duración"    HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-centro" />

                                <asp:TemplateField HeaderText="Acciones" HeaderStyle-CssClass="th-centro" ItemStyle-CssClass="td-acciones">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbModificar" runat="server"
                                            CommandName="Modificar"
                                            CommandArgument='<%# Eval("IdConsulta") %>'
                                            CssClass="tbl-btn tbl-btn-mod" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>

            </div>
        </div>

    </form>
</body>
</html>
