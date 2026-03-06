using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Shows_Shows : Page
    {
        protected GridView GridViewShows;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadShows();
            }
        }

        private void LoadShows()
        {
            const string query = "SELECT s.Show_Id, m.Movie_Name, h.Hall_Number, s.Show_Name, s.Show_Date, s.Show_Time FROM SHOWS s LEFT JOIN MOVIE m ON s.Movie_Id = m.Movie_Id LEFT JOIN HALL h ON s.Hall_Id = h.Hall_Id ORDER BY s.Show_Id";
            GridViewShows.DataSource = new DBConnection().ExecuteQuery(query);
            GridViewShows.DataBind();
        }

        protected void GridViewShows_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int showId = Convert.ToInt32(GridViewShows.DataKeys[e.RowIndex].Value);

            using (OracleConnection con = new DBConnection().GetConnection())
            using (OracleTransaction tran = con.BeginTransaction())
            {
                try
                {
                    // Remove child records first to satisfy FK constraints.
                    ExecuteDelete(con, tran,
                        "DELETE FROM TICKET WHERE Booking_Id IN (SELECT Booking_Id FROM BOOKING WHERE Show_Id = :Show_Id)",
                        showId);

                    ExecuteDelete(con, tran,
                        "DELETE FROM BOOKING WHERE Show_Id = :Show_Id",
                        showId);

                    ExecuteDelete(con, tran,
                        "DELETE FROM SHOWS WHERE Show_Id = :Show_Id",
                        showId);

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

            LoadShows();
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
