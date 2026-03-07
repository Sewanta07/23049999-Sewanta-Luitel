<%@ Page Title="Hall Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HallForm.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Halls_HallForm" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="form-page">
        <section class="form-card">
            <h2>Hall Form</h2>
            <asp:Label ID="lblMessage" runat="server" Visible="false" />
            <div class="form-grid">
                <div class="form-group"><label for="ddlTheaters">Theater</label><asp:DropDownList ID="ddlTheaters" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="txtHallNumber">Hall Number</label><asp:TextBox ID="txtHallNumber" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="txtHallCapacity">Hall Capacity</label><asp:TextBox ID="txtHallCapacity" runat="server" CssClass="form-control" /></div>
            </div>
            <div class="form-actions">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-save" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-delete" OnClick="btnDelete_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Cancel" CssClass="btn btn-cancel" PostBackUrl="~/Halls/Halls.aspx" />
            </div>
        </section>
    </main>
</asp:Content>
