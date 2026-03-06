using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _23049999_Sewanta_Luitel
{
    public partial class Reports_MovieOccupancyReport : Page
    {
        protected DropDownList ddlMovies;
        protected GridView GridViewReport;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlMovies.DataSource = new DBConnection().ExecuteQuery("SELECT Movie_Id, Movie_Name FROM MOVIE ORDER BY Movie_Name");
                ddlMovies.DataTextField = "Movie_Name";
                ddlMovies.DataValueField = "Movie_Id";
                ddlMovies.DataBind();
                ddlMovies.Items.Insert(0, new ListItem("All Movies", ""));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string query = @"SELECT m.Movie_Name,
                                    SUM(h.Hall_Capacity) AS Total_Seats,
                                    SUM(NVL(bs.Booked_Seats, 0)) AS Booked_Seats,
                                    SUM(h.Hall_Capacity) - SUM(NVL(bs.Booked_Seats, 0)) AS Available_Seats
                             FROM SHOWS s
                             INNER JOIN MOVIE m ON s.Movie_Id = m.Movie_Id
                             INNER JOIN HALL h ON s.Hall_Id = h.Hall_Id
                             LEFT JOIN (
                                 SELECT b.Show_Id,
                                        COUNT(t.Ticket_Id) AS Booked_Seats
                                 FROM BOOKING b
                                 LEFT JOIN TICKET t ON b.Booking_Id = t.Booking_Id
                                 WHERE UPPER(NVL(b.Booking_Status, 'BOOKED')) <> 'CANCELLED'
                                 GROUP BY b.Show_Id
                             ) bs ON s.Show_Id = bs.Show_Id
                             WHERE 1 = 1";

            Dictionary<string, object> p = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(ddlMovies.SelectedValue))
            {
                query += " AND m.Movie_Id = :Movie_Id";
                p.Add(":Movie_Id", Convert.ToInt32(ddlMovies.SelectedValue));
            }

            query += " GROUP BY m.Movie_Name ORDER BY m.Movie_Name";

            DataTable dt = new DBConnection().ExecuteQuery(query, p);
            GridViewReport.DataSource = dt;
            GridViewReport.DataBind();
        }
    }
}
