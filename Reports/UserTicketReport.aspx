<%@ Page Title="User Ticket Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserTicketReport.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Reports_UserTicketReport" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>User Ticket Report</h2>

        <div class="form-group">
            <label for="ddlUsers">User</label>
            <asp:DropDownList ID="ddlUsers" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtFromDate">From Date</label>
            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date" />
        </div>

        <div class="form-group">
            <label for="txtToDate">To Date</label>
            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date" />
        </div>

        <p><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-update" OnClick="btnSearch_Click" /></p>

        <div class="table-wrap">
            <asp:GridView ID="GridViewReport" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                <Columns>
                    <asp:BoundField DataField="User_Name" HeaderText="User" />
                    <asp:BoundField DataField="Movie_Name" HeaderText="Movie" />
                    <asp:BoundField DataField="Show_Name" HeaderText="Show" />
                    <asp:BoundField DataField="Booking_Date" HeaderText="Booking Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                    <asp:BoundField DataField="Seat_Number" HeaderText="Seat" />
                    <asp:BoundField DataField="Ticket_Price" HeaderText="Price" />
                    <asp:BoundField DataField="Booking_Status" HeaderText="Status" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
