using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Halls_HallForm : Page
    {
        protected DropDownList ddlTheaters;
        protected TextBox txtHallNumber;
        protected TextBox txtHallCapacity;
        protected Label lblMessage;

        private int? HallId
        {
            get
            {
                int id;
                return int.TryParse(Request.QueryString["id"], out id) ? id : (int?)null;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTheaters();
                LoadHall();
            }
        }

        private void LoadTheaters()
        {
            try
            {
                ddlTheaters.DataSource = new DBConnection().ExecuteQuery("SELECT Theater_Id, Theater_Name FROM THEATER ORDER BY Theater_Name");
                ddlTheaters.DataTextField = "Theater_Name";
                ddlTheaters.DataValueField = "Theater_Id";
                ddlTheaters.DataBind();
            }
            catch
            {
                SetMessage("Unable to load theaters.", true);
            }
        }

        private void LoadHall()
        {
            if (!HallId.HasValue)
            {
                return;
            }

            try
            {
                Dictionary<string, object> p = new Dictionary<string, object> { { ":Hall_Id", HallId.Value } };
                DataTable dt = new DBConnection().ExecuteQuery("SELECT Theater_Id, Hall_Number, Hall_Capacity FROM HALL WHERE Hall_Id = :Hall_Id", p);
                if (dt.Rows.Count > 0)
                {
                    string theaterId = dt.Rows[0]["Theater_Id"].ToString();
                    if (ddlTheaters.Items.FindByValue(theaterId) != null)
                    {
                        ddlTheaters.SelectedValue = theaterId;
                    }

                    txtHallNumber.Text = dt.Rows[0]["Hall_Number"].ToString();
                    txtHallCapacity.Text = dt.Rows[0]["Hall_Capacity"].ToString();
                }
            }
            catch
            {
                SetMessage("Unable to load hall details.", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ddlTheaters.SelectedValue) ||
                string.IsNullOrWhiteSpace(txtHallNumber.Text) ||
                string.IsNullOrWhiteSpace(txtHallCapacity.Text))
            {
                SetMessage("Theater, hall number, and hall capacity are required.", true);
                return;
            }

            int hallCapacity;
            if (!int.TryParse(txtHallCapacity.Text.Trim(), out hallCapacity) || hallCapacity <= 0)
            {
                SetMessage("Hall capacity must be a positive number.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;

                    if (HallId.HasValue)
                    {
                        cmd.CommandText = "UPDATE HALL SET Theater_Id = :Theater_Id, Hall_Number = :Hall_Number, Hall_Capacity = :Hall_Capacity WHERE Hall_Id = :Hall_Id";
                        cmd.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = HallId.Value;
                    }
                    else
                    {
                        cmd.CommandText = "INSERT INTO HALL (Hall_Id, Theater_Id, Hall_Number, Hall_Capacity) VALUES ((SELECT NVL(MAX(Hall_Id),0)+1 FROM HALL), :Theater_Id, :Hall_Number, :Hall_Capacity)";
                    }

                    cmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlTheaters.SelectedValue);
                    cmd.Parameters.Add(":Hall_Number", OracleDbType.Varchar2).Value = txtHallNumber.Text.Trim();
                    cmd.Parameters.Add(":Hall_Capacity", OracleDbType.Int32).Value = hallCapacity;
                    cmd.ExecuteNonQuery();
                }

                Response.Redirect("~/Halls/Halls.aspx");
            }
            catch
            {
                SetMessage("Unable to save hall.", true);
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!HallId.HasValue)
            {
                SetMessage("Open an existing hall to delete.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                {
                    using (OracleCommand checkCmd = con.CreateCommand())
                    {
                        checkCmd.BindByName = true;
                        checkCmd.CommandText = "SELECT COUNT(*) FROM SHOWS WHERE Hall_Id = :Hall_Id";
                        checkCmd.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = HallId.Value;
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
                        cmd.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = HallId.Value;
                        cmd.ExecuteNonQuery();
                    }
                }

                Response.Redirect("~/Halls/Halls.aspx");
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
