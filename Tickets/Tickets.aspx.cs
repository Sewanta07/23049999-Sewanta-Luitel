using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Tickets_Tickets : Page
    {
        protected GridView GridViewTickets;
        protected Label lblMessage;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadTickets();
            }
        }

        private void LoadTickets()
        {
            try
            {
                const string query = "SELECT b.Booking_Id, t.Ticket_Id, u.User_Name, s.Show_Name, b.Booking_Date, b.Booking_Status, t.Seat_Number, t.Ticket_Price FROM BOOKING b INNER JOIN TICKET t ON b.Booking_Id = t.Booking_Id LEFT JOIN USER_TABLE u ON b.User_Id = u.User_Id LEFT JOIN SHOWS s ON b.Show_Id = s.Show_Id ORDER BY b.Booking_Id";
                GridViewTickets.DataSource = new DBConnection().ExecuteQuery(query);
                GridViewTickets.DataBind();
            }
            catch
            {
                SetMessage("Unable to load tickets.", true);
            }
        }

        protected void GridViewTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnCancelBooking_Click(object sender, EventArgs e)
        {
            if (GridViewTickets.SelectedDataKey == null)
            {
                SetMessage("Please select a ticket row first.", true);
                return;
            }

            try
            {
                using (OracleConnection con = new DBConnection().GetConnection())
                using (OracleCommand cmd = con.CreateCommand())
                {
                    cmd.BindByName = true;
                    cmd.CommandText = "UPDATE BOOKING SET Booking_Status='Cancelled' WHERE Booking_Id=:Booking_Id";
                    cmd.Parameters.Add(":Booking_Id", OracleDbType.Int32).Value = Convert.ToInt32(GridViewTickets.SelectedDataKey.Values["Booking_Id"]);
                    cmd.ExecuteNonQuery();
                }

                SetMessage("Booking cancelled successfully.", false);
                LoadTickets();
            }
            catch
            {
                SetMessage("Unable to cancel booking.", true);
            }
        }

        protected void GridViewTickets_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int ticketId = Convert.ToInt32(GridViewTickets.DataKeys[e.RowIndex].Values["Ticket_Id"]);
                int bookingId = Convert.ToInt32(GridViewTickets.DataKeys[e.RowIndex].Values["Booking_Id"]);

                using (OracleConnection con = new DBConnection().GetConnection())
                using (OracleTransaction tran = con.BeginTransaction())
                {
                    try
                    {
                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            cmd.BindByName = true;
                            cmd.Transaction = tran;
                            cmd.CommandText = "DELETE FROM TICKET WHERE Ticket_Id = :Ticket_Id";
                            cmd.Parameters.Add(":Ticket_Id", OracleDbType.Int32).Value = ticketId;
                            cmd.ExecuteNonQuery();
                        }

                        using (OracleCommand cmd = con.CreateCommand())
                        {
                            cmd.BindByName = true;
                            cmd.Transaction = tran;
                            cmd.CommandText = "DELETE FROM BOOKING WHERE Booking_Id = :Booking_Id";
                            cmd.Parameters.Add(":Booking_Id", OracleDbType.Int32).Value = bookingId;
                            cmd.ExecuteNonQuery();
                        }

                        tran.Commit();
                    }
                    catch
                    {
                        tran.Rollback();
                        throw;
                    }
                }

                SetMessage("Ticket deleted successfully.", false);
                LoadTickets();
            }
            catch
            {
                SetMessage("Unable to delete ticket.", true);
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
