<%@ Page Title="Ticket Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Report" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <h2>Tickets Purchased (Last 6 Months)</h2>

        <div class="form-group">
            <label for="ddlReportUsers">Select User</label>
            <asp:DropDownList ID="ddlReportUsers" runat="server" CssClass="form-control" />
        </div>

        <div style="margin-top: 12px; margin-bottom: 12px;">
            <asp:Button ID="btnSearchReport" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearchReport_Click" />
        </div>

        <asp:GridView ID="GridViewReport" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="User_Name" HeaderText="User Name" />
                <asp:BoundField DataField="Movie_Name" HeaderText="Movie Name" />
                <asp:BoundField DataField="Show_Date" HeaderText="Show Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                <asp:BoundField DataField="Seat_Number" HeaderText="Seat Number" />
                <asp:BoundField DataField="Ticket_Price" HeaderText="Ticket Price" />
            </Columns>
        </asp:GridView>

        <h2 style="margin-top: 24px;">Top 3 Hall Occupancy by Movie</h2>

        <div class="form-group">
            <label for="ddlMovieOccupancy">Select Movie</label>
            <asp:DropDownList ID="ddlMovieOccupancy" runat="server" CssClass="form-control" />
        </div>

        <div style="margin-top: 12px; margin-bottom: 12px;">
            <asp:Button ID="btnSearchOccupancy" runat="server" Text="Search Occupancy" CssClass="btn btn-primary" OnClick="btnSearchOccupancy_Click" />
        </div>

        <asp:GridView ID="GridViewOccupancy" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="Movie_Name" HeaderText="Movie Name" />
                <asp:BoundField DataField="Hall_Number" HeaderText="Hall Number" />
                <asp:BoundField DataField="Occupancy_Percentage" HeaderText="Occupancy %" />
            </Columns>
        </asp:GridView>

        <h2 style="margin-top: 24px;">Movies Running in Selected Hall</h2>

        <div class="form-group">
            <label for="ddlHallReport">Select Hall</label>
            <asp:DropDownList ID="ddlHallReport" runat="server" CssClass="form-control" />
        </div>

        <div style="margin-top: 12px; margin-bottom: 12px;">
            <asp:Button ID="btnSearchHallReport" runat="server" Text="Search Hall Movies" CssClass="btn btn-primary" OnClick="btnSearchHallReport_Click" />
        </div>

        <asp:GridView ID="GridViewHallReport" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
            <Columns>
                <asp:BoundField DataField="Theater_Name" HeaderText="Theater Name" />
                <asp:BoundField DataField="Hall_Number" HeaderText="Hall Number" />
                <asp:BoundField DataField="Movie_Name" HeaderText="Movie Name" />
                <asp:BoundField DataField="Show_Date" HeaderText="Show Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                <asp:BoundField DataField="Show_Time" HeaderText="Show Time" />
            </Columns>
        </asp:GridView>
    </main>
</asp:Content>
