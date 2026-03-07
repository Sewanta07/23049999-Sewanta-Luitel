<%@ Page Title="Theater Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TheaterForm.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Theaters_TheaterForm" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="form-page">
        <section class="form-card">
            <h2>Theater Form</h2>
            <asp:Label ID="lblMessage" runat="server" Visible="false" />
            <div class="form-grid">
                <div class="form-group"><label for="txtTheaterName">Theater Name</label><asp:TextBox ID="txtTheaterName" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="txtTheaterCity">Theater City</label><asp:TextBox ID="txtTheaterCity" runat="server" CssClass="form-control" /></div>
            </div>
            <div class="form-actions">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-save" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-delete" OnClick="btnDelete_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Cancel" CssClass="btn btn-cancel" PostBackUrl="~/Theaters/Theaters.aspx" />
            </div>
        </section>
    </main>
</asp:Content>
