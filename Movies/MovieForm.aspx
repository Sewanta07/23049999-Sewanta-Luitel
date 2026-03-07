<%@ Page Title="Movie Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovieForm.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Movies_MovieForm" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="form-page">
        <section class="form-card">
            <h2>Movie Form</h2>
            <asp:Label ID="lblMessage" runat="server" Visible="false" />
            <div class="form-grid">
                <div class="form-group"><label for="txtMovieName">Movie Name</label><asp:TextBox ID="txtMovieName" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="txtReleaseDate">Release Date</label><asp:TextBox ID="txtReleaseDate" runat="server" CssClass="form-control" TextMode="Date" /></div>
            </div>
            <div class="form-actions">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-save" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-delete" OnClick="btnDelete_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Cancel" CssClass="btn btn-cancel" PostBackUrl="~/Movies/Movies.aspx" />
            </div>
        </section>
    </main>
</asp:Content>
