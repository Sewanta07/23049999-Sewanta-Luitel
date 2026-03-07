<%@ Page Title="Movie Occupancy Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MovieOccupancyReport.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Reports_MovieOccupancyReport" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Movie Occupancy Report</h2>
        <p class="page-intro">Analyze occupancy by movie with total, booked, and available seat counts.</p>
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <article class="card report-filter-card">
            <div class="card-body">
                <h4 class="section-title">Filters</h4>

                <div class="form-group">
                    <label for="ddlMovies">Movie</label>
                    <asp:DropDownList ID="ddlMovies" runat="server" CssClass="form-control" />
                </div>

                <div class="report-actions">
                    <asp:Button ID="btnSearch" runat="server" Text="Generate Report" CssClass="btn btn-update" OnClick="btnSearch_Click" />
                    <a class="btn btn-secondary" href="~/Reports/Reports.aspx" runat="server">Back to Reports</a>
                </div>
            </div>
        </article>

        <article class="card report-results-card">
            <div class="card-body">
                <h4 class="section-title">Report Results</h4>
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
            </div>
        </article>
    </main>
</asp:Content>
