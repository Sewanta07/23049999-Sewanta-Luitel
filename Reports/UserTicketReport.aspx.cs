using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _23049999_Sewanta_Luitel
{
    public partial class Reports_UserTicketReport : Page
    {
        protected DropDownList ddlUsers;
        protected TextBox txtFromDate;
        protected TextBox txtToDate;
        protected GridView GridViewReport;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlUsers.DataSource = new DBConnection().ExecuteQuery("SELECT User_Id, User_Name || ' (ID: ' || User_Id || ')' AS User_Display FROM USER_TABLE ORDER BY User_Name");
                ddlUsers.DataTextField = "User_Display";
                ddlUsers.DataValueField = "User_Id";
                ddlUsers.DataBind();
                ddlUsers.Items.Insert(0, new ListItem("All Users", ""));
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string query = @"SELECT u.User_Name,
                                    m.Movie_Name,
                                    s.Show_Name,
                                    b.Booking_Date,
                                    t.Seat_Number,
                                    t.Ticket_Price,
                                    b.Booking_Status
                             FROM BOOKING b
                             INNER JOIN USER_TABLE u ON b.User_Id = u.User_Id
                             INNER JOIN SHOWS s ON b.Show_Id = s.Show_Id
                             INNER JOIN MOVIE m ON s.Movie_Id = m.Movie_Id
                             INNER JOIN TICKET t ON b.Booking_Id = t.Booking_Id
                             WHERE 1 = 1";

            Dictionary<string, object> p = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(ddlUsers.SelectedValue))
            {
                query += " AND b.User_Id = :User_Id";
                p.Add(":User_Id", Convert.ToInt32(ddlUsers.SelectedValue));
            }

            DateTime fromDate;
            if (DateTime.TryParse(txtFromDate.Text.Trim(), out fromDate))
            {
                query += " AND TRUNC(b.Booking_Date) >= :From_Date";
                p.Add(":From_Date", fromDate.Date);
            }

            DateTime toDate;
            if (DateTime.TryParse(txtToDate.Text.Trim(), out toDate))
            {
                query += " AND TRUNC(b.Booking_Date) <= :To_Date";
                p.Add(":To_Date", toDate.Date);
            }

            query += " ORDER BY b.Booking_Date DESC";

            DataTable dt = new DBConnection().ExecuteQuery(query, p);
            GridViewReport.DataSource = dt;
            GridViewReport.DataBind();
        }
    }
}
