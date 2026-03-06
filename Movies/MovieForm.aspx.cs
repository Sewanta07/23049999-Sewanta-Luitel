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
                Dictionary<string, object> p = new Dictionary<string, object> { { ":Movie_Id", MovieId.Value } };
                DataTable dt = new DBConnection().ExecuteQuery("SELECT Movie_Name, Movie_Release_Date FROM MOVIE WHERE Movie_Id = :Movie_Id", p);
                if (dt.Rows.Count > 0)
                {
                    txtMovieName.Text = dt.Rows[0]["Movie_Name"].ToString();
                    txtReleaseDate.Text = Convert.ToDateTime(dt.Rows[0]["Movie_Release_Date"]).ToString("yyyy-MM-dd");
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            DateTime releaseDate;
            if (!DateTime.TryParse(txtReleaseDate.Text.Trim(), out releaseDate))
            {
                throw new InvalidOperationException("Please enter a valid release date.");
            }

            using (OracleConnection con = new DBConnection().GetConnection())
            using (OracleCommand cmd = con.CreateCommand())
            {
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!MovieId.HasValue)
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
                        "DELETE FROM TICKET WHERE Booking_Id IN (SELECT b.Booking_Id FROM BOOKING b INNER JOIN SHOWS s ON b.Show_Id = s.Show_Id WHERE s.Movie_Id = :Movie_Id)",
                        MovieId.Value);

                    ExecuteDelete(con, tran,
                        "DELETE FROM BOOKING WHERE Show_Id IN (SELECT Show_Id FROM SHOWS WHERE Movie_Id = :Movie_Id)",
                        MovieId.Value);

                    ExecuteDelete(con, tran,
                        "DELETE FROM SHOWS WHERE Movie_Id = :Movie_Id",
                        MovieId.Value);

                    ExecuteDelete(con, tran,
                        "DELETE FROM MOVIE WHERE Movie_Id = :Movie_Id",
                        MovieId.Value);

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

            Response.Redirect("~/Movies/Movies.aspx");
        }

        private static void ExecuteDelete(OracleConnection con, OracleTransaction tran, string sql, int movieId)
        {
            using (OracleCommand cmd = con.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.BindByName = true;
                cmd.CommandText = sql;
                cmd.Parameters.Add(":Movie_Id", OracleDbType.Int32).Value = movieId;
                cmd.ExecuteNonQuery();
            }
        }
    }
}
