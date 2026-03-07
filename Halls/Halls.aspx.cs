using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Halls_Halls : Page
    {
        protected GridView GridViewHalls;
        protected Label lblMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadHalls();
            }
        }

        private void LoadHalls()
        {
            try
            {
                const string query = "SELECT h.Hall_Id, t.Theater_Name, h.Hall_Number, h.Hall_Capacity FROM HALL h INNER JOIN THEATER t ON h.Theater_Id = t.Theater_Id ORDER BY t.Theater_Name, h.Hall_Number";
                GridViewHalls.DataSource = new DBConnection().ExecuteQuery(query);
                GridViewHalls.DataBind();
            }
            catch
            {
                SetMessage("Unable to load halls right now.", true);
            }
        }

        protected void GridViewHalls_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int hallId = Convert.ToInt32(GridViewHalls.DataKeys[e.RowIndex].Value);

                using (OracleConnection con = new DBConnection().GetConnection())
                {
                    using (OracleCommand checkCmd = con.CreateCommand())
                    {
                        checkCmd.BindByName = true;
                        checkCmd.CommandText = "SELECT COUNT(*) FROM SHOWS WHERE Hall_Id = :Hall_Id";
                        checkCmd.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = hallId;
                        int dependentCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (dependentCount > 0)
                        {
                            SetMessage("Cannot delete hall because it has related shows.", true);
                            return;
                        }
                    }

                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        cmd.BindByName = true;
                        cmd.CommandText = "DELETE FROM HALL WHERE Hall_Id = :Hall_Id";
                        cmd.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = hallId;
                        cmd.ExecuteNonQuery();
                    }
                }

                SetMessage("Hall deleted successfully.", false);
                LoadHalls();
            }
            catch
            {
                SetMessage("Unable to delete hall.", true);
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
