<%@ Page Title="Reports" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Reports</h2>
        <p class="page-intro">Generate operational reports from one place and review ticketing, scheduling, and occupancy data.</p>

        <div class="row">
            <section class="col-md-4">
                <article class="card">
                    <div class="card-body">
                        <h4 class="card-title">User Ticket Report</h4>
                        <p>View ticket bookings by user with optional date filtering.</p>
                        <p><a class="btn btn-update" href="~/Reports/UserTicketReport.aspx" runat="server">Open Report</a></p>
                    </div>
                </article>
            </section>

            <section class="col-md-4">
                <article class="card">
                    <div class="card-body">
                        <h4 class="card-title">Theater Movie Report</h4>
                        <p>Analyze shows by theater, hall, movie, date, and time.</p>
                        <p><a class="btn btn-update" href="~/Reports/TheaterMovieReport.aspx" runat="server">Open Report</a></p>
                    </div>
                </article>
            </section>

            <section class="col-md-4">
                <article class="card">
                    <div class="card-body">
                        <h4 class="card-title">Movie Occupancy Report</h4>
                        <p>Track total, booked, and available seats for each movie.</p>
                        <p><a class="btn btn-update" href="~/Reports/MovieOccupancyReport.aspx" runat="server">Open Report</a></p>
                    </div>
                </article>
            </section>
        </div>
    </main>
</asp:Content>
