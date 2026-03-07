<%@ Page Title="Theater Movie Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TheaterMovieReport.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Reports_TheaterMovieReport" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Theater Movie Report</h2>
        <p class="page-intro">Review scheduled shows by theater with hall, movie, date, and time details.</p>
        <asp:Label ID="lblMessage" runat="server" Visible="false" />

        <article class="card report-filter-card">
            <div class="card-body">
                <h4 class="section-title">Filters</h4>

                <div class="form-group">
                    <label for="ddlTheaters">Theater</label>
                    <asp:DropDownList ID="ddlTheaters" runat="server" CssClass="form-control" />
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
                            <asp:BoundField DataField="Theater_Name" HeaderText="Theater" />
                            <asp:BoundField DataField="Hall_Number" HeaderText="Hall" />
                            <asp:BoundField DataField="Movie_Name" HeaderText="Movie" />
                            <asp:BoundField DataField="Show_Name" HeaderText="Show" />
                            <asp:BoundField DataField="Show_Date" HeaderText="Show Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                            <asp:BoundField DataField="Show_Time" HeaderText="Show Time" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </article>
    </main>
</asp:Content>
