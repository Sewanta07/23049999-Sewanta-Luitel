using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;

namespace _23049999_Sewanta_Luitel
{
    public partial class Report : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReportUsers();
                LoadMoviesForOccupancy();
                LoadHallsForReport();
            }
        }

        protected void LoadHallsForReport()
        {
            const string query = "SELECT Hall_Id, Hall_Number || ' (ID: ' || Hall_Id || ')' AS Hall_Display FROM HALL ORDER BY Hall_Number";

            DataTable hallTable = new DBConnection().ExecuteQuery(query);

            ddlHallReport.DataSource = hallTable;
            ddlHallReport.DataTextField = "Hall_Display";
            ddlHallReport.DataValueField = "Hall_Id";
            ddlHallReport.DataBind();
        }

        protected void LoadMoviesForOccupancy()
        {
            const string query = "SELECT Movie_Id, Movie_Name FROM MOVIE ORDER BY Movie_Name";

            DataTable moviesTable = new DBConnection().ExecuteQuery(query);

            ddlMovieOccupancy.DataSource = moviesTable;
            ddlMovieOccupancy.DataTextField = "Movie_Name";
            ddlMovieOccupancy.DataValueField = "Movie_Id";
            ddlMovieOccupancy.DataBind();
        }

        protected void LoadReportUsers()
        {
            const string query = "SELECT User_Id, User_Name || ' (ID: ' || User_Id || ')' AS User_Display FROM USER_TABLE ORDER BY User_Id";

            DataTable usersTable = new DBConnection().ExecuteQuery(query);

            ddlReportUsers.DataSource = usersTable;
            ddlReportUsers.DataTextField = "User_Display";
            ddlReportUsers.DataValueField = "User_Id";
            ddlReportUsers.DataBind();
        }

        protected void LoadReport()
        {
            if (ddlReportUsers.Items.Count == 0)
            {
                GridViewReport.DataSource = null;
                GridViewReport.DataBind();
                return;
            }

            const string query = "SELECT u.User_Name, m.Movie_Name, s.Show_Date, t.Seat_Number, t.Ticket_Price FROM USER_TABLE u INNER JOIN BOOKING b ON u.User_Id = b.User_Id INNER JOIN TICKET t ON b.Booking_Id = t.Booking_Id INNER JOIN SHOWS s ON b.Show_Id = s.Show_Id INNER JOIN MOVIE m ON s.Movie_Id = m.Movie_Id WHERE b.User_Id = :User_Id AND s.Show_Date >= ADD_MONTHS(SYSDATE, -6) ORDER BY s.Show_Date DESC";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":User_Id", Convert.ToInt32(ddlReportUsers.SelectedValue) }
            };

            DataTable reportTable = new DBConnection().ExecuteQuery(query, parameters);
            GridViewReport.DataSource = reportTable;
            GridViewReport.DataBind();
        }

        protected void btnSearchReport_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        protected void LoadTopHallOccupancyReport()
        {
            if (ddlMovieOccupancy.Items.Count == 0)
            {
                GridViewOccupancy.DataSource = null;
                GridViewOccupancy.DataBind();
                return;
            }

            const string query = @"SELECT Movie_Name, Hall_Number, Occupancy_Percentage
                                   FROM (
                                       SELECT m.Movie_Name,
                                              h.Hall_Number,
                                              ROUND((COUNT(t.Ticket_Id) / NULLIF(h.Hall_Capacity, 0)) * 100, 2) AS Occupancy_Percentage
                                       FROM MOVIE m
                                       INNER JOIN SHOWS s ON m.Movie_Id = s.Movie_Id
                                       INNER JOIN HALL h ON s.Hall_Id = h.Hall_Id
                                       INNER JOIN BOOKING b ON s.Show_Id = b.Show_Id
                                       INNER JOIN TICKET t ON b.Booking_Id = t.Booking_Id
                                       WHERE m.Movie_Id = :Movie_Id
                                         AND b.Booking_Status = 'PAID'
                                       GROUP BY m.Movie_Name, h.Hall_Number, h.Hall_Capacity
                                       ORDER BY Occupancy_Percentage DESC
                                   )
                                   WHERE ROWNUM <= 3";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":Movie_Id", Convert.ToInt32(ddlMovieOccupancy.SelectedValue) }
            };

            DataTable occupancyTable = new DBConnection().ExecuteQuery(query, parameters);
            GridViewOccupancy.DataSource = occupancyTable;
            GridViewOccupancy.DataBind();
        }

        protected void btnSearchOccupancy_Click(object sender, EventArgs e)
        {
            LoadTopHallOccupancyReport();
        }

        protected void LoadHallMovieReport()
        {
            if (ddlHallReport.Items.Count == 0)
            {
                GridViewHallReport.DataSource = null;
                GridViewHallReport.DataBind();
                return;
            }

            const string query = @"SELECT t.Theater_Name,
                                          h.Hall_Number,
                                          m.Movie_Name,
                                          s.Show_Date,
                                          s.Show_Time
                                   FROM HALL h
                                   INNER JOIN THEATER t ON h.Theater_Id = t.Theater_Id
                                   INNER JOIN SHOWS s ON h.Hall_Id = s.Hall_Id
                                   INNER JOIN MOVIE m ON s.Movie_Id = m.Movie_Id
                                   WHERE h.Hall_Id = :Hall_Id
                                   ORDER BY s.Show_Date, s.Show_Time";

            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { ":Hall_Id", Convert.ToInt32(ddlHallReport.SelectedValue) }
            };

            DataTable hallReportTable = new DBConnection().ExecuteQuery(query, parameters);
            GridViewHallReport.DataSource = hallReportTable;
            GridViewHallReport.DataBind();
        }

        protected void btnSearchHallReport_Click(object sender, EventArgs e)
        {
            LoadHallMovieReport();
        }
    }
}
