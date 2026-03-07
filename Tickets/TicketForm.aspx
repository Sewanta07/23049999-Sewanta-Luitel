<%@ Page Title="Ticket Form" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TicketForm.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Tickets_TicketForm" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="form-page">
        <section class="form-card">
            <h2>Book Ticket</h2>
            <asp:Label ID="lblMessage" runat="server" Visible="false" />
            <div class="form-grid">
                <div class="form-group"><label for="ddlUsers">User</label><asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="ddlShows">Show</label><asp:DropDownList ID="ddlShows" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="txtSeatNumber">Seat Number</label><asp:TextBox ID="txtSeatNumber" runat="server" CssClass="form-control" /></div>
                <div class="form-group"><label for="txtTicketPrice">Ticket Price</label><asp:TextBox ID="txtTicketPrice" runat="server" CssClass="form-control" /></div>
            </div>
            <div class="form-actions">
                <asp:Button ID="btnBook" runat="server" Text="Save" CssClass="btn btn-save" OnClick="btnBook_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Cancel" CssClass="btn btn-cancel" PostBackUrl="~/Tickets/Tickets.aspx" />
            </div>
        </section>
    </main>
</asp:Content>
