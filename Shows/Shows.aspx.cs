using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Shows_Shows : Page
    {
        protected GridView GridViewShows;
        protected Label lblMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadShows();
            }
        }

        private void LoadShows()
        {
            try
            {
                const string query = "SELECT s.Show_Id, m.Movie_Name, h.Hall_Number, s.Show_Name, s.Show_Date, s.Show_Time FROM SHOWS s LEFT JOIN MOVIE m ON s.Movie_Id = m.Movie_Id LEFT JOIN HALL h ON s.Hall_Id = h.Hall_Id ORDER BY s.Show_Id";
                GridViewShows.DataSource = new DBConnection().ExecuteQuery(query);
                GridViewShows.DataBind();
            }
            catch
            {
                SetMessage("Unable to load shows.", true);
            }
        }

        protected void GridViewShows_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int showId = Convert.ToInt32(GridViewShows.DataKeys[e.RowIndex].Value);

                using (OracleConnection con = new DBConnection().GetConnection())
                {
                    using (OracleCommand checkCmd = con.CreateCommand())
                    {
                        checkCmd.BindByName = true;
                        checkCmd.CommandText = "SELECT COUNT(*) FROM BOOKING WHERE Show_Id = :Show_Id";
                        checkCmd.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = showId;
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
                        cmd.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = showId;
                        cmd.ExecuteNonQuery();
                    }
                }

                SetMessage("Show deleted successfully.", false);
                LoadShows();
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
