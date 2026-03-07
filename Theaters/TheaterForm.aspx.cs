using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Theaters_TheaterForm : Page
    {
        protected TextBox txtTheaterName;
        protected TextBox txtTheaterCity;
        protected Label lblMessage;

        private int? TheaterId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : (int?)null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && TheaterId.HasValue)
            {
                try
                {
                    Dictionary<string, object> p = new Dictionary<string, object> { { ":Theater_Id", TheaterId.Value } };
                    DataTable dt = new DBConnection().ExecuteQuery("SELECT Theater_Name, Theater_City FROM THEATER WHERE Theater_Id = :Theater_Id", p);
                    if (dt.Rows.Count > 0)
                    {
                        txtTheaterName.Text = dt.Rows[0]["Theater_Name"].ToString();
                        txtTheaterCity.Text = dt.Rows[0]["Theater_City"].ToString();
                    }
                }
                catch
                {
                    SetMessage("Unable to load theater details.", true);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTheaterName.Text) || string.IsNullOrWhiteSpace(txtTheaterCity.Text))
            {
                SetMessage("Theater name and city are required.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;

                    if (TheaterId.HasValue)
                    {
                        cmd.CommandText = "UPDATE THEATER SET Theater_Name=:Theater_Name, Theater_City=:Theater_City WHERE Theater_Id=:Theater_Id";
                        cmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = TheaterId.Value;
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO THEATER (Theater_Id, Theater_Name, Theater_City) VALUES ((SELECT NVL(MAX(Theater_Id),0)+1 FROM THEATER), :Theater_Name, :Theater_City)";
                    }

                    cmd.Parameters.Add(":Theater_Name", OracleDbType.Varchar2).Value = txtTheaterName.Text.Trim();
                    cmd.Parameters.Add(":Theater_City", OracleDbType.Varchar2).Value = txtTheaterCity.Text.Trim();
                    cmd.ExecuteNonQuery();
                }

                Response.Redirect("~/Theaters/Theaters.aspx");
            }
            catch
            {
                SetMessage("Unable to save theater.", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!TheaterId.HasValue)
            {
                SetMessage("Open an existing theater to delete.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                {
                    using (OracleCommand checkCmd = con.CreateCommand())
                    {
                        checkCmd.BindByName = true;
                        checkCmd.CommandText = "SELECT COUNT(*) FROM HALL WHERE Theater_Id = :Theater_Id";
                        checkCmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = TheaterId.Value;
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
                        cmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = TheaterId.Value;
                        cmd.ExecuteNonQuery();
                    }
                }

                Response.Redirect("~/Theaters/Theaters.aspx");
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
