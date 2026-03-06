<%@ Page Title="Show Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ShowForm.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Shows_ShowForm" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="form-page">
        <section class="form-card">
            <h2>Show Form</h2>
            <div class="form-grid">
                <div class="form-group"><label for="ddlMovies">Movie</label><asp:DropDownList ID="ddlMovies" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="ddlHalls">Hall</label><asp:DropDownList ID="ddlHalls" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="txtShowName">Show Name</label><asp:TextBox ID="txtShowName" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="txtShowDate">Show Date</label><asp:TextBox ID="txtShowDate" runat="server" CssClass="form-control" /></div>
                <div class="form-group field-full"><label for="txtShowTime">Show Time</label><asp:TextBox ID="txtShowTime" runat="server" CssClass="form-control" /></div>
            </div>
            <div class="form-actions">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-save" OnClick="btnSave_Click" />
                <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-delete" OnClick="btnDelete_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Cancel" CssClass="btn btn-cancel" PostBackUrl="~/Shows/Shows.aspx" />
            </div>
        </section>
    </main>
</asp:Content>
