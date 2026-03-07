<%@ Page Title="User Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserForm.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Users_UserForm" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="form-page">
        <section class="form-card">
            <h2>User Form</h2>
            <asp:Label ID="lblMessage" runat="server" Visible="false" />
            <div class="form-grid">
                <div class="form-group"><label for="txtName">Name</label><asp:TextBox ID="txtName" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="txtEmail">Email</label><asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" /></div>
                <div class="form-group field-full"><label for="txtAddress">Address</label><asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" /></div>
            </div>
            <div class="form-actions">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-save" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-delete" OnClick="btnDelete_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Cancel" CssClass="btn btn-cancel" PostBackUrl="~/Users/Users.aspx" />
            </div>
        </section>
    </main>
</asp:Content>
