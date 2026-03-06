using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _23049999_Sewanta_Luitel
{
    public partial class Pages_Home : Page
    {
        protected Label lblTotalUsers;
        protected Label lblTotalMovies;
        protected Label lblTotalTheaters;
        protected Label lblTotalShows;
        protected Label lblTotalTicketsBooked;
        protected GridView GridViewRecentBookings;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblTotalUsers.Text = GetCount("SELECT COUNT(*) TOTAL_COUNT FROM USER_TABLE").ToString();
                lblTotalMovies.Text = GetCount("SELECT COUNT(*) TOTAL_COUNT FROM MOVIE").ToString();
                lblTotalTheaters.Text = GetCount("SELECT COUNT(*) TOTAL_COUNT FROM THEATER").ToString();
                lblTotalShows.Text = GetCount("SELECT COUNT(*) TOTAL_COUNT FROM SHOWS").ToString();
                lblTotalTicketsBooked.Text = GetCount("SELECT COUNT(*) TOTAL_COUNT FROM TICKET").ToString();

                LoadRecentBookings();
            }
        }

        private int GetCount(string query)
        {
            DataTable result = new DBConnection().ExecuteQuery(query);
            return result.Rows.Count == 0 ? 0 : Convert.ToInt32(result.Rows[0]["TOTAL_COUNT"]);
        }

        private void LoadRecentBookings()
        {
            const string query = @"SELECT b.Booking_Id,
                                          u.User_Name,
                                          m.Movie_Name,
                                          s.Show_Name,
                                          b.Booking_Date,
                                          b.Booking_Status,
                                          t.Seat_Number,
                                          t.Ticket_Price
                                   FROM BOOKING b
                                   LEFT JOIN USER_TABLE u ON b.User_Id = u.User_Id
                                   LEFT JOIN SHOWS s ON b.Show_Id = s.Show_Id
                                   LEFT JOIN MOVIE m ON s.Movie_Id = m.Movie_Id
                                   LEFT JOIN TICKET t ON b.Booking_Id = t.Booking_Id
                                   ORDER BY b.Booking_Date DESC
                                   FETCH FIRST 10 ROWS ONLY";

            GridViewRecentBookings.DataSource = new DBConnection().ExecuteQuery(query);
            GridViewRecentBookings.DataBind();
        }
    }
}
