<%@ Page Title="Tickets" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Tickets.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Tickets_Tickets" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Tickets</h2>
        <p>
            <asp:Button ID="btnBook" runat="server" Text="Book Ticket" CssClass="btn btn-update" PostBackUrl="~/Tickets/TicketForm.aspx" />
            <asp:Button ID="btnCancelBooking" runat="server" Text="Cancel Selected Booking" CssClass="btn btn-delete" OnClick="btnCancelBooking_Click" />
        </p>
        <div class="table-wrap">
            <asp:GridView ID="GridViewTickets" runat="server" AutoGenerateColumns="False" DataKeyNames="Ticket_Id,Booking_Id" CssClass="table table-bordered" OnSelectedIndexChanged="GridViewTickets_SelectedIndexChanged" OnRowDeleting="GridViewTickets_RowDeleting">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" SelectText="Select" ControlStyle-CssClass="table-action action-select" />
                    <asp:BoundField DataField="Booking_Id" HeaderText="Booking ID" />
                    <asp:BoundField DataField="Ticket_Id" HeaderText="Ticket ID" />
                    <asp:BoundField DataField="User_Name" HeaderText="User" />
                    <asp:BoundField DataField="Show_Name" HeaderText="Show" />
                    <asp:BoundField DataField="Booking_Date" HeaderText="Booking Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                    <asp:BoundField DataField="Booking_Status" HeaderText="Status" />
                    <asp:BoundField DataField="Seat_Number" HeaderText="Seat" />
                    <asp:BoundField DataField="Ticket_Price" HeaderText="Price" />
                    <asp:HyperLinkField Text="Edit" ControlStyle-CssClass="table-action action-edit" DataNavigateUrlFields="Ticket_Id" DataNavigateUrlFormatString="~/Tickets/TicketForm.aspx?id={0}" />
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" ControlStyle-CssClass="table-action action-delete" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
