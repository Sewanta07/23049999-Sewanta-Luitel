<%@ Page Title="Movie Occupancy Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovieOccupancyReport.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Reports_MovieOccupancyReport" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Movie Occupancy Report</h2>
        <div class="form-group"><label for="ddlMovies">Movie</label><asp:DropDownList ID="ddlMovies" runat="server" CssClass="form-control" /></div>
        <p><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-update" OnClick="btnSearch_Click" /></p>
        <div class="table-wrap">
            <asp:GridView ID="GridViewReport" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                <Columns>
                    <asp:BoundField DataField="Movie_Name" HeaderText="Movie" />
                    <asp:BoundField DataField="Total_Seats" HeaderText="Total Seats" />
                    <asp:BoundField DataField="Booked_Seats" HeaderText="Booked Seats" />
                    <asp:BoundField DataField="Available_Seats" HeaderText="Available Seats" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
