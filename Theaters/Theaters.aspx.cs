using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Theaters_Theaters : Page
    {
        protected GridView GridViewTheaters;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTheaters();
            }
        }

        private void LoadTheaters()
        {
            const string query = "SELECT t.Theater_Id, t.Theater_Name, t.Theater_City, h.Hall_Id, h.Hall_Number, h.Hall_Capacity FROM THEATER t LEFT JOIN HALL h ON t.Theater_Id = h.Theater_Id ORDER BY t.Theater_Id, h.Hall_Id";
            GridViewTheaters.DataSource = new DBConnection().ExecuteQuery(query);
            GridViewTheaters.DataBind();
        }

        protected void GridViewTheaters_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int theaterId = Convert.ToInt32(GridViewTheaters.DataKeys[e.RowIndex].Value);

            using (OracleConnection con = new DBConnection().GetConnection())
            using (OracleTransaction tran = con.BeginTransaction())
            {
                try
                {
                    // Remove child records first to satisfy FK constraints.
                    ExecuteDelete(con, tran,
                        "DELETE FROM TICKET WHERE Booking_Id IN (SELECT b.Booking_Id FROM BOOKING b INNER JOIN SHOWS s ON b.Show_Id = s.Show_Id INNER JOIN HALL h ON s.Hall_Id = h.Hall_Id WHERE h.Theater_Id = :Theater_Id)",
                        theaterId);

                    ExecuteDelete(con, tran,
                        "DELETE FROM BOOKING WHERE Show_Id IN (SELECT s.Show_Id FROM SHOWS s INNER JOIN HALL h ON s.Hall_Id = h.Hall_Id WHERE h.Theater_Id = :Theater_Id)",
                        theaterId);

                    ExecuteDelete(con, tran,
                        "DELETE FROM SHOWS WHERE Hall_Id IN (SELECT Hall_Id FROM HALL WHERE Theater_Id = :Theater_Id)",
                        theaterId);

                    // Support environments where show table name is SHOW instead of SHOWS.
                    if (TableExists(con, tran, "SHOW"))
                    {
                        ExecuteDelete(con, tran,
                            "DELETE FROM TICKET WHERE Booking_Id IN (SELECT b.Booking_Id FROM BOOKING b INNER JOIN \"SHOW\" s ON b.Show_Id = s.Show_Id INNER JOIN HALL h ON s.Hall_Id = h.Hall_Id WHERE h.Theater_Id = :Theater_Id)",
                            theaterId);

                        ExecuteDelete(con, tran,
                            "DELETE FROM BOOKING WHERE Show_Id IN (SELECT s.Show_Id FROM \"SHOW\" s INNER JOIN HALL h ON s.Hall_Id = h.Hall_Id WHERE h.Theater_Id = :Theater_Id)",
                            theaterId);

                        ExecuteDelete(con, tran,
                            "DELETE FROM \"SHOW\" WHERE Hall_Id IN (SELECT Hall_Id FROM HALL WHERE Theater_Id = :Theater_Id)",
                            theaterId);
                    }

                    ExecuteDelete(con, tran,
                        "DELETE FROM HALL WHERE Theater_Id = :Theater_Id",
                        theaterId);

                    ExecuteDelete(con, tran,
                        "DELETE FROM THEATER WHERE Theater_Id = :Theater_Id",
                        theaterId);

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }

            LoadTheaters();
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
