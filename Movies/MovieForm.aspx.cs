using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Movies_MovieForm : Page
    {
        protected TextBox txtMovieName;
        protected TextBox txtReleaseDate;
        protected Label lblMessage;

        private int? MovieId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : (int?)null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && MovieId.HasValue)
            {
                try
                {
                    Dictionary<string, object> p = new Dictionary<string, object> { { ":Movie_Id", MovieId.Value } };
                    DataTable dt = new DBConnection().ExecuteQuery("SELECT Movie_Name, Movie_Release_Date FROM MOVIE WHERE Movie_Id = :Movie_Id", p);
                    if (dt.Rows.Count > 0)
                    {
                        txtMovieName.Text = dt.Rows[0]["Movie_Name"].ToString();
                        txtReleaseDate.Text = Convert.ToDateTime(dt.Rows[0]["Movie_Release_Date"]).ToString("yyyy-MM-dd");
                    }
                }
                catch
                {
                    SetMessage("Unable to load movie details.", true);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMovieName.Text) || string.IsNullOrWhiteSpace(txtReleaseDate.Text))
            {
                SetMessage("Movie name and release date are required.", true);
                return;
            }

            DateTime releaseDate;
            if (!DateTime.TryParse(txtReleaseDate.Text.Trim(), out releaseDate))
            {
                SetMessage("Please enter a valid release date.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;

                    if (MovieId.HasValue)
                    {
                        cmd.CommandText = "UPDATE MOVIE SET Movie_Name=:Movie_Name, Movie_Release_Date=:Movie_Release_Date WHERE Movie_Id=:Movie_Id";
                        cmd.Parameters.Add(":Movie_Id", OracleDbType.Int32).Value = MovieId.Value;
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO MOVIE (Movie_Id, Movie_Name, Movie_Release_Date) VALUES ((SELECT NVL(MAX(Movie_Id),0)+1 FROM MOVIE), :Movie_Name, :Movie_Release_Date)";
                    }

                    cmd.Parameters.Add(":Movie_Name", OracleDbType.Varchar2).Value = txtMovieName.Text.Trim();
                    cmd.Parameters.Add(":Movie_Release_Date", OracleDbType.Date).Value = releaseDate;
                    cmd.ExecuteNonQuery();
                }

                Response.Redirect("~/Movies/Movies.aspx");
            }
            catch
            {
                SetMessage("Unable to save movie.", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!MovieId.HasValue)
            {
                SetMessage("Open an existing movie to delete.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                {
                    using (OracleCommand checkCmd = con.CreateCommand())
                    {
                        checkCmd.BindByName = true;
                        checkCmd.CommandText = "SELECT COUNT(*) FROM SHOWS WHERE Movie_Id = :Movie_Id";
                        checkCmd.Parameters.Add(":Movie_Id", OracleDbType.Int32).Value = MovieId.Value;
                        int dependentCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (dependentCount > 0)
                        {
                            SetMessage("Cannot delete movie because related shows exist.", true);
                            return;
                        }
                    }

                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        cmd.BindByName = true;
                        cmd.CommandText = "DELETE FROM MOVIE WHERE Movie_Id = :Movie_Id";
                        cmd.Parameters.Add(":Movie_Id", OracleDbType.Int32).Value = MovieId.Value;
                        cmd.ExecuteNonQuery();
                    }
                }

                Response.Redirect("~/Movies/Movies.aspx");
            }
            catch
            {
                SetMessage("Unable to delete movie.", true);
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
