using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Users_Users : Page
    {
        protected GridView GridViewUsers;
        protected Label lblMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            try
            {
                GridViewUsers.DataSource = new DBConnection().ExecuteQuery("SELECT User_Id, User_Name, User_Email, User_Address FROM USER_TABLE ORDER BY User_Id");
                GridViewUsers.DataBind();
            }
            catch
            {
                SetMessage("Unable to load users.", true);
            }
        }

        protected void GridViewUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int userId = Convert.ToInt32(GridViewUsers.DataKeys[e.RowIndex].Value);

                using (OracleConnection con = new DBConnection().GetConnection())
                {
                    using (OracleCommand checkCmd = con.CreateCommand())
                    {
                        checkCmd.BindByName = true;
                        checkCmd.CommandText = "SELECT COUNT(*) FROM BOOKING WHERE User_Id = :User_Id";
                        checkCmd.Parameters.Add(":User_Id", OracleDbType.Int32).Value = userId;
                        int dependentCount = Convert.ToInt32(checkCmd.ExecuteScalar());
                        if (dependentCount > 0)
                        {
                            SetMessage("Cannot delete user because related bookings exist.", true);
                            return;
                        }
                    }

                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        cmd.BindByName = true;
                        cmd.CommandText = "DELETE FROM USER_TABLE WHERE User_Id = :User_Id";
                        cmd.Parameters.Add(":User_Id", OracleDbType.Int32).Value = userId;
                        cmd.ExecuteNonQuery();
                    }
                }

                SetMessage("User deleted successfully.", false);
                LoadUsers();
            }
            catch
            {
                SetMessage("Unable to delete user.", true);
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
