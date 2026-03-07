using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Theaters_Theaters : Page
    {
        protected GridView GridViewTheaters;
        protected Label lblMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTheaters();
            }
        }

        private void LoadTheaters()
        {
            try
            {
                const string query = "SELECT Theater_Id, Theater_Name, Theater_City FROM THEATER ORDER BY Theater_Id";
                GridViewTheaters.DataSource = new DBConnection().ExecuteQuery(query);
                GridViewTheaters.DataBind();
            }
            catch
            {
                SetMessage("Unable to load theaters.", true);
            }
        }

        protected void GridViewTheaters_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int theaterId = Convert.ToInt32(GridViewTheaters.DataKeys[e.RowIndex].Value);

                using (OracleConnection con = new DBConnection().GetConnection())
                {
                    using (OracleCommand checkCmd = con.CreateCommand())
                    {
                        checkCmd.BindByName = true;
                        checkCmd.CommandText = "SELECT COUNT(*) FROM HALL WHERE Theater_Id = :Theater_Id";
                        checkCmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = theaterId;
                        int dependentCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (dependentCount > 0)
                        {
                            SetMessage("Cannot delete theater because related halls exist. Delete halls first.", true);
                            return;
                        }
                    }

                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        cmd.BindByName = true;
                        cmd.CommandText = "DELETE FROM THEATER WHERE Theater_Id = :Theater_Id";
                        cmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = theaterId;
                        cmd.ExecuteNonQuery();
                    }
                }

                SetMessage("Theater deleted successfully.", false);
                LoadTheaters();
            }
            catch
            {
                SetMessage("Unable to delete theater.", true);
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
