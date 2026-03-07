using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Users_UserForm : Page
    {
        protected TextBox txtName;
        protected TextBox txtEmail;
        protected TextBox txtAddress;
        protected Label lblMessage;

        private int? UserId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : (int?)null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && UserId.HasValue)
            {
                try
                {
                    Dictionary<string, object> p = new Dictionary<string, object> { { ":User_Id", UserId.Value } };
                    DataTable dt = new DBConnection().ExecuteQuery("SELECT User_Name, User_Email, User_Address FROM USER_TABLE WHERE User_Id = :User_Id", p);
                    if (dt.Rows.Count > 0)
                    {
                        txtName.Text = dt.Rows[0]["User_Name"].ToString();
                        txtEmail.Text = dt.Rows[0]["User_Email"].ToString();
                        txtAddress.Text = dt.Rows[0]["User_Address"].ToString();
                    }
                }
                catch
                {
                    SetMessage("Unable to load user details.", true);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                SetMessage("Name, email, and address are required.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;

                    if (UserId.HasValue)
                    {
                        cmd.CommandText = "UPDATE USER_TABLE SET User_Name=:User_Name, User_Email=:User_Email, User_Address=:User_Address WHERE User_Id=:User_Id";
                        cmd.Parameters.Add(":User_Id", OracleDbType.Int32).Value = UserId.Value;
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO USER_TABLE (User_Id, User_Name, User_Email, User_Address) VALUES ((SELECT NVL(MAX(User_Id),0)+1 FROM USER_TABLE), :User_Name, :User_Email, :User_Address)";
                    }

                    cmd.Parameters.Add(":User_Name", OracleDbType.Varchar2).Value = txtName.Text.Trim();
                    cmd.Parameters.Add(":User_Email", OracleDbType.Varchar2).Value = txtEmail.Text.Trim();
                    cmd.Parameters.Add(":User_Address", OracleDbType.Varchar2).Value = txtAddress.Text.Trim();
                    cmd.ExecuteNonQuery();
                }

                Response.Redirect("~/Users/Users.aspx");
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1)
                {
                    SetMessage("Unable to save user: email already exists.", true);
                }
                else
                {
                    SetMessage("Unable to save user due to a database error.", true);
                }
            }
            catch
            {
                SetMessage("Unable to save user.", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!UserId.HasValue)
            {
                SetMessage("Open an existing user to delete.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                {
                    using (OracleCommand checkCmd = con.CreateCommand())
                    {
                        checkCmd.BindByName = true;
                        checkCmd.CommandText = "SELECT COUNT(*) FROM BOOKING WHERE User_Id = :User_Id";
                        checkCmd.Parameters.Add(":User_Id", OracleDbType.Int32).Value = UserId.Value;
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
                        cmd.CommandText = "DELETE FROM USER_TABLE WHERE User_Id=:User_Id";
                        cmd.Parameters.Add(":User_Id", OracleDbType.Int32).Value = UserId.Value;
                        cmd.ExecuteNonQuery();
                    }
                }

                Response.Redirect("~/Users/Users.aspx");
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
