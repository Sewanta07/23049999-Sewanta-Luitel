using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Shows_ShowForm : Page
    {
        protected DropDownList ddlMovies;
        protected DropDownList ddlHalls;
        protected TextBox txtShowName;
        protected TextBox txtShowDate;
        protected TextBox txtShowTime;

        private int? ShowId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : (int?)null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlMovies.DataSource = new DBConnection().ExecuteQuery("SELECT Movie_Id, Movie_Name FROM MOVIE ORDER BY Movie_Id");
                ddlMovies.DataTextField = "Movie_Name";
                ddlMovies.DataValueField = "Movie_Id";
                ddlMovies.DataBind();

                ddlHalls.DataSource = new DBConnection().ExecuteQuery("SELECT Hall_Id, Hall_Number FROM HALL ORDER BY Hall_Id");
                ddlHalls.DataTextField = "Hall_Number";
                ddlHalls.DataValueField = "Hall_Id";
                ddlHalls.DataBind();

                if (ShowId.HasValue)
                {
                    Dictionary<string, object> p = new Dictionary<string, object> { { ":Show_Id", ShowId.Value } };
                    DataTable dt = new DBConnection().ExecuteQuery("SELECT Movie_Id, Hall_Id, Show_Name, Show_Date, Show_Time FROM SHOWS WHERE Show_Id=:Show_Id", p);
                    if (dt.Rows.Count > 0)
                    {
                        ddlMovies.SelectedValue = dt.Rows[0]["Movie_Id"].ToString();
                        ddlHalls.SelectedValue = dt.Rows[0]["Hall_Id"].ToString();
                        txtShowName.Text = dt.Rows[0]["Show_Name"].ToString();
                        txtShowDate.Text = Convert.ToDateTime(dt.Rows[0]["Show_Date"]).ToString("yyyy-MM-dd");
                        txtShowTime.Text = dt.Rows[0]["Show_Time"].ToString();
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime showDate;
            if (!DateTime.TryParse(txtShowDate.Text.Trim(), out showDate))
            {
                throw new InvalidOperationException("Please enter a valid show date.");
            }

            using (OracleConnection con = new DBConnection().GetConnection())
            using (OracleCommand cmd = con.CreateCommand())
            {
                if (ShowId.HasValue)
                {
                    cmd.CommandText = "UPDATE SHOWS SET Movie_Id=:Movie_Id, Hall_Id=:Hall_Id, Show_Name=:Show_Name, Show_Date=:Show_Date, Show_Time=:Show_Time WHERE Show_Id=:Show_Id";
                    cmd.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = ShowId.Value;
                }
                else
                {
                    cmd.CommandText = "INSERT INTO SHOWS (Show_Id, Movie_Id, Hall_Id, Show_Name, Show_Date, Show_Time) VALUES ((SELECT NVL(MAX(Show_Id),0)+1 FROM SHOWS), :Movie_Id, :Hall_Id, :Show_Name, :Show_Date, :Show_Time)";
                }

                cmd.Parameters.Add(":Movie_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlMovies.SelectedValue);
                cmd.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlHalls.SelectedValue);
                cmd.Parameters.Add(":Show_Name", OracleDbType.Varchar2).Value = txtShowName.Text.Trim();
                cmd.Parameters.Add(":Show_Date", OracleDbType.Date).Value = showDate;
                cmd.Parameters.Add(":Show_Time", OracleDbType.Varchar2).Value = txtShowTime.Text.Trim();
                cmd.ExecuteNonQuery();
            }

            Response.Redirect("~/Shows/Shows.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!ShowId.HasValue)
            {
                return;
            }

            using (OracleConnection con = new DBConnection().GetConnection())
            using (OracleTransaction tran = con.BeginTransaction())
            {
                try
                {
                    // Remove child records first to satisfy FK constraints.
                    ExecuteDelete(con, tran,
                        "DELETE FROM TICKET WHERE Booking_Id IN (SELECT Booking_Id FROM BOOKING WHERE Show_Id = :Show_Id)",
                        ShowId.Value);

                    ExecuteDelete(con, tran,
                        "DELETE FROM BOOKING WHERE Show_Id = :Show_Id",
                        ShowId.Value);

                    ExecuteDelete(con, tran,
                        "DELETE FROM SHOWS WHERE Show_Id = :Show_Id",
                        ShowId.Value);

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

            Response.Redirect("~/Shows/Shows.aspx");
        }

        private static void ExecuteDelete(OracleConnection con, OracleTransaction tran, string sql, int showId)
        {
            using (OracleCommand cmd = con.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.BindByName = true;
                cmd.CommandText = sql;
                cmd.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = showId;
                cmd.ExecuteNonQuery();
            }
        }

    }
}
