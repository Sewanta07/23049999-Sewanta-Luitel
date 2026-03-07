<%@ Page Title="Halls" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Halls.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Halls_Halls" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Halls</h2>
        <asp:Label ID="lblMessage" runat="server" Visible="false" />
        <p>
            <asp:Button ID="btnAddHall" runat="server" Text="Add New Hall" CssClass="btn btn-update" PostBackUrl="~/Halls/HallForm.aspx" />
        </p>
        <div class="table-wrap">
            <asp:GridView ID="GridViewHalls" runat="server" AutoGenerateColumns="False" DataKeyNames="Hall_Id" CssClass="table table-bordered" OnRowDeleting="GridViewHalls_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Hall_Id" HeaderText="Hall ID" />
                    <asp:BoundField DataField="Theater_Name" HeaderText="Theater" />
                    <asp:BoundField DataField="Hall_Number" HeaderText="Hall Number" />
                    <asp:BoundField DataField="Hall_Capacity" HeaderText="Capacity" />
                    <asp:HyperLinkField Text="Edit" ControlStyle-CssClass="table-action action-edit" DataNavigateUrlFields="Hall_Id" DataNavigateUrlFormatString="~/Halls/HallForm.aspx?id={0}" />
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" ControlStyle-CssClass="table-action action-delete" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
