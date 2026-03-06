using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class Tickets_TicketForm : Page
    {
        protected DropDownList ddlUsers;
        protected DropDownList ddlShows;
        protected TextBox txtSeatNumber;
        protected TextBox txtTicketPrice;
        protected Button btnBook;

        private int? TicketId
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
                ddlUsers.DataSource = new DBConnection().ExecuteQuery("SELECT User_Id, User_Name FROM USER_TABLE ORDER BY User_Id");
                ddlUsers.DataTextField = "User_Name";
                ddlUsers.DataValueField = "User_Id";
                ddlUsers.DataBind();

                ddlShows.DataSource = new DBConnection().ExecuteQuery("SELECT Show_Id, Show_Name FROM SHOWS ORDER BY Show_Id");
                ddlShows.DataTextField = "Show_Name";
                ddlShows.DataValueField = "Show_Id";
                ddlShows.DataBind();

                if (TicketId.HasValue)
                {
                    Dictionary<string, object> p = new Dictionary<string, object> { { ":Ticket_Id", TicketId.Value } };
                    DataTable dt = new DBConnection().ExecuteQuery("SELECT t.Ticket_Id, t.Booking_Id, b.User_Id, b.Show_Id, t.Seat_Number, t.Ticket_Price FROM TICKET t INNER JOIN BOOKING b ON t.Booking_Id = b.Booking_Id WHERE t.Ticket_Id = :Ticket_Id", p);

                    if (dt.Rows.Count > 0)
                    {
                        string userId = dt.Rows[0]["User_Id"].ToString();
                        string showId = dt.Rows[0]["Show_Id"].ToString();

                        if (ddlUsers.Items.FindByValue(userId) != null)
                        {
                            ddlUsers.SelectedValue = userId;
                        }

                        if (ddlShows.Items.FindByValue(showId) != null)
                        {
                            ddlShows.SelectedValue = showId;
                        }

                        txtSeatNumber.Text = dt.Rows[0]["Seat_Number"].ToString();
                        txtTicketPrice.Text = dt.Rows[0]["Ticket_Price"].ToString();
                        btnBook.Text = "Update";
                    }
                }
            }
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            decimal price;
            if (!decimal.TryParse(txtTicketPrice.Text.Trim(), out price))
            {
                throw new InvalidOperationException("Please enter a valid ticket price.");
            }

            using (OracleConnection con = new DBConnection().GetConnection())
            using (OracleTransaction tran = con.BeginTransaction())
            {
                try
                {
                    if (TicketId.HasValue)
                    {
                        int bookingId;
                        using (OracleCommand cmd = new OracleCommand("SELECT Booking_Id FROM TICKET WHERE Ticket_Id = :Ticket_Id", con))
                        {
                            cmd.Transaction = tran;
                            cmd.Parameters.Add(":Ticket_Id", OracleDbType.Int32).Value = TicketId.Value;
                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                throw new InvalidOperationException("Ticket not found for update.");
                            }
                            bookingId = Convert.ToInt32(result);
                        }

                        using (OracleCommand cmd = new OracleCommand("UPDATE BOOKING SET User_Id = :User_Id, Show_Id = :Show_Id WHERE Booking_Id = :Booking_Id", con))
                        {
                            cmd.Transaction = tran;
                            cmd.Parameters.Add(":User_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlUsers.SelectedValue);
                            cmd.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlShows.SelectedValue);
                            cmd.Parameters.Add(":Booking_Id", OracleDbType.Int32).Value = bookingId;
                            cmd.ExecuteNonQuery();
                        }

                        using (OracleCommand cmd = new OracleCommand("UPDATE TICKET SET Ticket_Price = :Ticket_Price, Seat_Number = :Seat_Number WHERE Ticket_Id = :Ticket_Id", con))
                        {
                            cmd.Transaction = tran;
                            cmd.Parameters.Add(":Ticket_Price", OracleDbType.Decimal).Value = price;
                            cmd.Parameters.Add(":Seat_Number", OracleDbType.Varchar2).Value = txtSeatNumber.Text.Trim();
                            cmd.Parameters.Add(":Ticket_Id", OracleDbType.Int32).Value = TicketId.Value;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        int bookingId;
                        int ticketId;

                        using (OracleCommand cmd = new OracleCommand("SELECT NVL(MAX(Booking_Id),0)+1 FROM BOOKING", con))
                        {
                            cmd.Transaction = tran;
                            bookingId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        using (OracleCommand cmd = new OracleCommand("SELECT NVL(MAX(Ticket_Id),0)+1 FROM TICKET", con))
                        {
                            cmd.Transaction = tran;
                            ticketId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        using (OracleCommand cmd = new OracleCommand("INSERT INTO BOOKING (Booking_Id, User_Id, Show_Id, Booking_Date, Booking_Status) VALUES (:Booking_Id, :User_Id, :Show_Id, :Booking_Date, :Booking_Status)", con))
                        {
                            cmd.Transaction = tran;
                            cmd.Parameters.Add(":Booking_Id", OracleDbType.Int32).Value = bookingId;
                            cmd.Parameters.Add(":User_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlUsers.SelectedValue);
                            cmd.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlShows.SelectedValue);
                            cmd.Parameters.Add(":Booking_Date", OracleDbType.Date).Value = DateTime.Now;
                            cmd.Parameters.Add(":Booking_Status", OracleDbType.Varchar2).Value = "Booked";
                            cmd.ExecuteNonQuery();
                        }

                        using (OracleCommand cmd = new OracleCommand("INSERT INTO TICKET (Ticket_Id, Booking_Id, Ticket_Price, Seat_Number) VALUES (:Ticket_Id, :Booking_Id, :Ticket_Price, :Seat_Number)", con))
                        {
                            cmd.Transaction = tran;
                            cmd.Parameters.Add(":Ticket_Id", OracleDbType.Int32).Value = ticketId;
                            cmd.Parameters.Add(":Booking_Id", OracleDbType.Int32).Value = bookingId;
                            cmd.Parameters.Add(":Ticket_Price", OracleDbType.Decimal).Value = price;
                            cmd.Parameters.Add(":Seat_Number", OracleDbType.Varchar2).Value = txtSeatNumber.Text.Trim();
                            cmd.ExecuteNonQuery();
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

            Response.Redirect("~/Tickets/Tickets.aspx");
        }
    }
}
