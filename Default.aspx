<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_23049999_Sewanta_Luitel._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Welcome</h2>
        <p>This project has been refactored into modular pages.</p>
        <p>
            <asp:HyperLink ID="lnkDashboard" runat="server" NavigateUrl="~/Pages/Home.aspx" CssClass="btn btn-primary">Go To Dashboard</asp:HyperLink>
        </p>
    </main>
</asp:Content>
