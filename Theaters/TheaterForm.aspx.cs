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
        protected TextBox txtHallNumber;
        protected TextBox txtHallCapacity;

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
                Dictionary<string, object> p = new Dictionary<string, object> { { ":Theater_Id", TheaterId.Value } };
                DataTable dt = new DBConnection().ExecuteQuery("SELECT Theater_Name, Theater_City FROM THEATER WHERE Theater_Id = :Theater_Id", p);
                if (dt.Rows.Count > 0)
                {
                    txtTheaterName.Text = dt.Rows[0]["Theater_Name"].ToString();
                    txtTheaterCity.Text = dt.Rows[0]["Theater_City"].ToString();
                }

                DataTable hallDt = new DBConnection().ExecuteQuery("SELECT Hall_Number, Hall_Capacity FROM HALL WHERE Theater_Id = :Theater_Id ORDER BY Hall_Id", p);
                if (hallDt.Rows.Count > 0)
                {
                    txtHallNumber.Text = hallDt.Rows[0]["Hall_Number"].ToString();
                    txtHallCapacity.Text = hallDt.Rows[0]["Hall_Capacity"].ToString();
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int hallCapacity = 0;
            bool hasHallNumber = !string.IsNullOrWhiteSpace(txtHallNumber.Text);
            bool hasHallCapacity = !string.IsNullOrWhiteSpace(txtHallCapacity.Text);

            if (hasHallCapacity && !int.TryParse(txtHallCapacity.Text.Trim(), out hallCapacity))
            {
                throw new InvalidOperationException("Please enter a valid hall capacity.");
            }

            using (OracleConnection con = new DBConnection().GetConnection())
            using (OracleTransaction tran = con.BeginTransaction())
            {
                try
                {
                    int theaterId;

                    if (TheaterId.HasValue)
                    {
                        theaterId = TheaterId.Value;
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = "UPDATE THEATER SET Theater_Name=:Theater_Name, Theater_City=:Theater_City WHERE Theater_Id=:Theater_Id";
                            cmd.Parameters.Add(":Theater_Name", OracleDbType.Varchar2).Value = txtTheaterName.Text.Trim();
                            cmd.Parameters.Add(":Theater_City", OracleDbType.Varchar2).Value = txtTheaterCity.Text.Trim();
                            cmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = theaterId;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = "SELECT NVL(MAX(Theater_Id),0)+1 FROM THEATER";
                            theaterId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = "INSERT INTO THEATER (Theater_Id, Theater_Name, Theater_City) VALUES (:Theater_Id, :Theater_Name, :Theater_City)";
                            cmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = theaterId;
                            cmd.Parameters.Add(":Theater_Name", OracleDbType.Varchar2).Value = txtTheaterName.Text.Trim();
                            cmd.Parameters.Add(":Theater_City", OracleDbType.Varchar2).Value = txtTheaterCity.Text.Trim();
                            cmd.ExecuteNonQuery();
                        }
                    }

                    if (hasHallNumber || hasHallCapacity)
                    {
                        int? hallId = null;
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            cmd.Transaction = tran;
                            cmd.CommandText = "SELECT MIN(Hall_Id) FROM HALL WHERE Theater_Id = :Theater_Id";
                            cmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = theaterId;
                            object result = cmd.ExecuteScalar();
                            if (result != DBNull.Value && result != null)
                            {
                                hallId = Convert.ToInt32(result);
                            }
                        }

                        if (hallId.HasValue)
                        {
                            using (OracleCommand cmd = con.CreateCommand())
                            {
                                cmd.Transaction = tran;
                                cmd.CommandText = "UPDATE HALL SET Hall_Number=:Hall_Number, Hall_Capacity=:Hall_Capacity WHERE Hall_Id=:Hall_Id";
                                cmd.Parameters.Add(":Hall_Number", OracleDbType.Varchar2).Value = txtHallNumber.Text.Trim();
                                cmd.Parameters.Add(":Hall_Capacity", OracleDbType.Int32).Value = hasHallCapacity ? hallCapacity : 0;
                                cmd.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = hallId.Value;
                                cmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            int newHallId;
                            using (OracleCommand cmd = con.CreateCommand())
                            {
                                cmd.Transaction = tran;
                                cmd.CommandText = "SELECT NVL(MAX(Hall_Id),0)+1 FROM HALL";
                                newHallId = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            using (OracleCommand cmd = con.CreateCommand())
                            {
                                cmd.Transaction = tran;
                                cmd.CommandText = "INSERT INTO HALL (Hall_Id, Theater_Id, Hall_Number, Hall_Capacity) VALUES (:Hall_Id, :Theater_Id, :Hall_Number, :Hall_Capacity)";
                                cmd.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = newHallId;
                                cmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = theaterId;
                                cmd.Parameters.Add(":Hall_Number", OracleDbType.Varchar2).Value = txtHallNumber.Text.Trim();
                                cmd.Parameters.Add(":Hall_Capacity", OracleDbType.Int32).Value = hasHallCapacity ? hallCapacity : 0;
                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

            Response.Redirect("~/Theaters/Theaters.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!TheaterId.HasValue)
            {
                return;
            }

            using (OracleConnection con = new DBConnection().GetConnection())
            using (OracleTransaction tran = con.BeginTransaction())
            {
                try
                {
                    // Remove child records first to satisfy FK constraints.
                    ExecuteDelete(con, tran,
                        "DELETE FROM TICKET WHERE Booking_Id IN (SELECT b.Booking_Id FROM BOOKING b INNER JOIN SHOWS s ON b.Show_Id = s.Show_Id INNER JOIN HALL h ON s.Hall_Id = h.Hall_Id WHERE h.Theater_Id = :Theater_Id)",
                        TheaterId.Value);

                    ExecuteDelete(con, tran,
                        "DELETE FROM BOOKING WHERE Show_Id IN (SELECT s.Show_Id FROM SHOWS s INNER JOIN HALL h ON s.Hall_Id = h.Hall_Id WHERE h.Theater_Id = :Theater_Id)",
                        TheaterId.Value);

                    ExecuteDelete(con, tran,
                        "DELETE FROM SHOWS WHERE Hall_Id IN (SELECT Hall_Id FROM HALL WHERE Theater_Id = :Theater_Id)",
                        TheaterId.Value);

                    // Support environments where show table name is SHOW instead of SHOWS.
                    if (TableExists(con, tran, "SHOW"))
                    {
                        ExecuteDelete(con, tran,
                            "DELETE FROM TICKET WHERE Booking_Id IN (SELECT b.Booking_Id FROM BOOKING b INNER JOIN \"SHOW\" s ON b.Show_Id = s.Show_Id INNER JOIN HALL h ON s.Hall_Id = h.Hall_Id WHERE h.Theater_Id = :Theater_Id)",
                            TheaterId.Value);

                        ExecuteDelete(con, tran,
                            "DELETE FROM BOOKING WHERE Show_Id IN (SELECT s.Show_Id FROM \"SHOW\" s INNER JOIN HALL h ON s.Hall_Id = h.Hall_Id WHERE h.Theater_Id = :Theater_Id)",
                            TheaterId.Value);

                        ExecuteDelete(con, tran,
                            "DELETE FROM \"SHOW\" WHERE Hall_Id IN (SELECT Hall_Id FROM HALL WHERE Theater_Id = :Theater_Id)",
                            TheaterId.Value);
                    }

                    ExecuteDelete(con, tran,
                        "DELETE FROM HALL WHERE Theater_Id = :Theater_Id",
                        TheaterId.Value);

                    ExecuteDelete(con, tran,
                        "DELETE FROM THEATER WHERE Theater_Id = :Theater_Id",
                        TheaterId.Value);

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

            Response.Redirect("~/Theaters/Theaters.aspx");
        }

        private static void ExecuteDelete(OracleConnection con, OracleTransaction tran, string sql, int theaterId)
        {
            using (OracleCommand cmd = con.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.BindByName = true;
                cmd.CommandText = sql;
                cmd.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = theaterId;
                cmd.ExecuteNonQuery();
            }
        }

        private static bool TableExists(OracleConnection con, OracleTransaction tran, string tableName)
        {
            using (OracleCommand cmd = con.CreateCommand())
            {
                cmd.Transaction = tran;
                cmd.BindByName = true;
                cmd.CommandText = "SELECT COUNT(*) FROM USER_TABLES WHERE TABLE_NAME = :Table_Name";
                cmd.Parameters.Add(":Table_Name", OracleDbType.Varchar2).Value = tableName.ToUpperInvariant();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }
}
