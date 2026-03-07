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
        protected Label lblMessage;

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
                try
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
                            string showTime = dt.Rows[0]["Show_Time"].ToString();
                            TimeSpan parsedShowTime;
                            if (TimeSpan.TryParse(showTime, out parsedShowTime))
                            {
                                txtShowTime.Text = parsedShowTime.ToString("hh\\:mm");
                            }
                            else
                            {
                                DateTime parsedShowTimeDate;
                                if (DateTime.TryParse(showTime, out parsedShowTimeDate))
                                {
                                    txtShowTime.Text = parsedShowTimeDate.ToString("HH:mm");
                                }
                                else
                                {
                                    txtShowTime.Text = showTime;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    SetMessage("Unable to load show details.", true);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtShowName.Text) ||
                string.IsNullOrWhiteSpace(txtShowDate.Text) ||
                string.IsNullOrWhiteSpace(txtShowTime.Text) ||
                string.IsNullOrWhiteSpace(ddlMovies.SelectedValue) ||
                string.IsNullOrWhiteSpace(ddlHalls.SelectedValue))
            {
                SetMessage("Movie, hall, show name, show date, and show time are required.", true);
                return;
            }

            DateTime showDate;
            if (!DateTime.TryParse(txtShowDate.Text.Trim(), out showDate))
            {
                SetMessage("Please enter a valid show date.", true);
                return;
            }

            TimeSpan showTime;
            if (!TimeSpan.TryParse(txtShowTime.Text.Trim(), out showTime))
            {
                SetMessage("Please enter a valid show time.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;

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
                    cmd.Parameters.Add(":Show_Time", OracleDbType.Varchar2).Value = showTime.ToString("hh\\:mm");
                    cmd.ExecuteNonQuery();
                }

                Response.Redirect("~/Shows/Shows.aspx");
            }
            catch
            {
                SetMessage("Unable to save show.", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!ShowId.HasValue)
            {
                SetMessage("Open an existing show to delete.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                {
                    using (OracleCommand checkCmd = con.CreateCommand())
                    {
                        checkCmd.BindByName = true;
                        checkCmd.CommandText = "SELECT COUNT(*) FROM BOOKING WHERE Show_Id = :Show_Id";
                        checkCmd.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = ShowId.Value;
                        int dependentCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (dependentCount > 0)
                        {
                            SetMessage("Cannot delete show because related bookings exist.", true);
                            return;
                        }
                    }

                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        cmd.BindByName = true;
                        cmd.CommandText = "DELETE FROM SHOWS WHERE Show_Id = :Show_Id";
                        cmd.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = ShowId.Value;
                        cmd.ExecuteNonQuery();
                    }
                }

                Response.Redirect("~/Shows/Shows.aspx");
            }
            catch
            {
                SetMessage("Unable to delete show.", true);
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
