using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Movies_Movies : Page
    {
        protected GridView GridViewMovies;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadMovies();
            }
        }

        private void LoadMovies()
        {
            GridViewMovies.DataSource = new DBConnection().ExecuteQuery("SELECT Movie_Id, Movie_Name, Movie_Release_Date FROM MOVIE ORDER BY Movie_Id");
            GridViewMovies.DataBind();
        }

        protected void GridViewMovies_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int movieId = Convert.ToInt32(GridViewMovies.DataKeys[e.RowIndex].Value);

            using (OracleConnection con = new DBConnection().GetConnection())
            using (OracleTransaction tran = con.BeginTransaction())
            {
                try
                {
                    // Remove child records first to satisfy FK constraints.
                    ExecuteDelete(con, tran,
                        "DELETE FROM TICKET WHERE Booking_Id IN (SELECT b.Booking_Id FROM BOOKING b INNER JOIN SHOWS s ON b.Show_Id = s.Show_Id WHERE s.Movie_Id = :Movie_Id)",
                        movieId);

                    ExecuteDelete(con, tran,
                        "DELETE FROM BOOKING WHERE Show_Id IN (SELECT Show_Id FROM SHOWS WHERE Movie_Id = :Movie_Id)",
                        movieId);

                    ExecuteDelete(con, tran,
                        "DELETE FROM SHOWS WHERE Movie_Id = :Movie_Id",
                        movieId);

                    ExecuteDelete(con, tran,
                        "DELETE FROM MOVIE WHERE Movie_Id = :Movie_Id",
                        movieId);

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

            LoadMovies();
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
