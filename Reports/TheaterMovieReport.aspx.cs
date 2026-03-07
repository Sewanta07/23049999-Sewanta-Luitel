using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace _23049999_Sewanta_Luitel
{
    public partial class Reports_TheaterMovieReport : Page
    {
        protected DropDownList ddlTheaters;
        protected GridView GridViewReport;
        protected Label lblMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    ddlTheaters.DataSource = new DBConnection().ExecuteQuery("SELECT Theater_Id, Theater_Name FROM THEATER ORDER BY Theater_Name");
                    ddlTheaters.DataTextField = "Theater_Name";
                    ddlTheaters.DataValueField = "Theater_Id";
                    ddlTheaters.DataBind();
                    ddlTheaters.Items.Insert(0, new ListItem("All Theaters", ""));
                }
                catch
                {
                    SetMessage("Unable to load theaters for report.", true);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string query = @"SELECT t.Theater_Name,
                                    h.Hall_Number,
                                    m.Movie_Name,
                                    s.Show_Name,
                                    s.Show_Date,
                                    s.Show_Time
                             FROM THEATER t
                             INNER JOIN HALL h ON t.Theater_Id = h.Theater_Id
                             INNER JOIN SHOWS s ON h.Hall_Id = s.Hall_Id
                             INNER JOIN MOVIE m ON s.Movie_Id = m.Movie_Id
                             WHERE 1 = 1";

            Dictionary<string, object> p = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(ddlTheaters.SelectedValue))
            {
                query += " AND t.Theater_Id = :Theater_Id";
                p.Add(":Theater_Id", Convert.ToInt32(ddlTheaters.SelectedValue));
            }

            query += " ORDER BY t.Theater_Name, h.Hall_Number, s.Show_Date, s.Show_Time";

            try
            {
                DataTable dt = new DBConnection().ExecuteQuery(query, p);
                GridViewReport.DataSource = dt;
                GridViewReport.DataBind();
            }
            catch
            {
                SetMessage("Unable to load report data.", true);
            }
        }

        private void SetMessage(string message, bool isError)
        {
            lblMessage.Text = message;
            lblMessage.Visible = true;
            lblMessage.CssClass = isError ? "alert alert-error" : "alert alert-success";
        }
    }
}
