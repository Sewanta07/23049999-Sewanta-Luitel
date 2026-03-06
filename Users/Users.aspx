<%@ Page Title="Users" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Users_Users" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Users</h2>
        <p><asp:Button ID="btnAddUser" runat="server" Text="Add New User" CssClass="btn btn-update" PostBackUrl="~/Users/UserForm.aspx" /></p>
        <div class="table-wrap">
            <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="User_Id" CssClass="table table-bordered" OnRowDeleting="GridViewUsers_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="User_Id" HeaderText="User ID" />
                    <asp:BoundField DataField="User_Name" HeaderText="Name" />
                    <asp:BoundField DataField="User_Email" HeaderText="Email" />
                    <asp:BoundField DataField="User_Address" HeaderText="Address" />
                    <asp:HyperLinkField Text="Edit" ControlStyle-CssClass="table-action action-edit" DataNavigateUrlFields="User_Id" DataNavigateUrlFormatString="~/Users/UserForm.aspx?id={0}" />
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" ControlStyle-CssClass="table-action action-delete" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
