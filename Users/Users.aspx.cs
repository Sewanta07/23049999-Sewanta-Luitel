using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Users_Users : Page
    {
        protected GridView GridViewUsers;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadUsers();
            }
        }

        private void LoadUsers()
        {
            GridViewUsers.DataSource = new DBConnection().ExecuteQuery("SELECT User_Id, User_Name, User_Email, User_Address FROM USER_TABLE ORDER BY User_Id");
            GridViewUsers.DataBind();
        }

        protected void GridViewUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int userId = Convert.ToInt32(GridViewUsers.DataKeys[e.RowIndex].Value);

            using (OracleConnection con = new DBConnection().GetConnection())
            using (OracleCommand cmd = con.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM USER_TABLE WHERE User_Id = :User_Id";
                cmd.Parameters.Add(":User_Id", OracleDbType.Int32).Value = userId;
                cmd.ExecuteNonQuery();
            }

            LoadUsers();
        }
    }
}
