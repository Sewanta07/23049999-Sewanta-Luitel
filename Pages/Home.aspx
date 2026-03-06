<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Pages_Home" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Cinema Dashboard</h2>
        <section class="stats-grid" aria-label="Dashboard Statistics">
            <article class="stat-card users">
                <div class="stat-icon" aria-hidden="true">
                    <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M12 12C14.7614 12 17 9.76142 17 7C17 4.23858 14.7614 2 12 2C9.23858 2 7 4.23858 7 7C7 9.76142 9.23858 12 12 12Z" stroke="currentColor" stroke-width="1.8"/>
                        <path d="M3 21C3 17.6863 6.13401 15 10 15H14C17.866 15 21 17.6863 21 21" stroke="currentColor" stroke-width="1.8" stroke-linecap="round"/>
                    </svg>
                </div>
                <div class="stat-content">
                    <asp:Label ID="lblTotalUsers" runat="server" CssClass="stat-value" Text="0" />
                    <p class="stat-label">Total Users</p>
                </div>
            </article>

            <article class="stat-card movies">
                <div class="stat-icon" aria-hidden="true">
                    <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <rect x="3" y="4" width="18" height="16" rx="2" stroke="currentColor" stroke-width="1.8"/>
                        <path d="M8 4V20M16 4V20M3 9H21M3 15H21" stroke="currentColor" stroke-width="1.8"/>
                    </svg>
                </div>
                <div class="stat-content">
                    <asp:Label ID="lblTotalMovies" runat="server" CssClass="stat-value" Text="0" />
                    <p class="stat-label">Total Movies</p>
                </div>
            </article>

            <article class="stat-card theaters">
                <div class="stat-icon" aria-hidden="true">
                    <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M3 21V7L12 3L21 7V21" stroke="currentColor" stroke-width="1.8" stroke-linejoin="round"/>
                        <path d="M9 21V13H15V21" stroke="currentColor" stroke-width="1.8"/>
                    </svg>
                </div>
                <div class="stat-content">
                    <asp:Label ID="lblTotalTheaters" runat="server" CssClass="stat-value" Text="0" />
                    <p class="stat-label">Total Theaters</p>
                </div>
            </article>

            <article class="stat-card shows">
                <div class="stat-icon" aria-hidden="true">
                    <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <rect x="3" y="5" width="18" height="14" rx="2" stroke="currentColor" stroke-width="1.8"/>
                        <path d="M8 2V8M16 2V8M3 11H21" stroke="currentColor" stroke-width="1.8" stroke-linecap="round"/>
                    </svg>
                </div>
                <div class="stat-content">
                    <asp:Label ID="lblTotalShows" runat="server" CssClass="stat-value" Text="0" />
                    <p class="stat-label">Total Shows</p>
                </div>
            </article>

            <article class="stat-card tickets">
                <div class="stat-icon" aria-hidden="true">
                    <svg viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M4 8C5.65685 8 7 6.65685 7 5H17C17 6.65685 18.3431 8 20 8V11C18.3431 11 17 12.3431 17 14C17 15.6569 18.3431 17 20 17V20C18.3431 20 17 18.6569 17 17H7C7 18.6569 5.65685 20 4 20V17C5.65685 17 7 15.6569 7 14C7 12.3431 5.65685 11 4 11V8Z" stroke="currentColor" stroke-width="1.8" stroke-linejoin="round"/>
                    </svg>
                </div>
                <div class="stat-content">
                    <asp:Label ID="lblTotalTicketsBooked" runat="server" CssClass="stat-value" Text="0" />
                    <p class="stat-label">Total Tickets Booked</p>
                </div>
            </article>
        </section>

        <div class="dashboard-section-spacing">
            <h4>Recent Bookings</h4>
        </div>
        <div class="table-wrap">
            <asp:GridView ID="GridViewRecentBookings" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                <Columns>
                    <asp:BoundField DataField="Booking_Id" HeaderText="Booking ID" />
                    <asp:BoundField DataField="User_Name" HeaderText="User" />
                    <asp:BoundField DataField="Movie_Name" HeaderText="Movie" />
                    <asp:BoundField DataField="Show_Name" HeaderText="Show" />
                    <asp:BoundField DataField="Booking_Date" HeaderText="Booking Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                    <asp:BoundField DataField="Booking_Status" HeaderText="Status" />
                    <asp:BoundField DataField="Seat_Number" HeaderText="Seat" />
                    <asp:BoundField DataField="Ticket_Price" HeaderText="Price" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
