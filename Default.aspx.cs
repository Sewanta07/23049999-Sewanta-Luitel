using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Oracle.ManagedDataAccess.Client;

namespace _23049999_Sewanta_Luitel
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadDashboardSummary();
                LoadUsers();
                LoadMovies();
                LoadTheaterDropdown();
                LoadTheaters();
                LoadMoviesDropdown();
                LoadHallsDropdown();
                LoadShows();
                LoadBookingUsersDropdown();
                LoadBookingShowsDropdown();
                LoadTickets();
            }
        }

        protected void LoadDashboardSummary()
        {
            lblTotalUsers.Text = GetCount("SELECT COUNT(*) AS TOTAL_COUNT FROM USER_TABLE").ToString();
            lblTotalMovies.Text = GetCount("SELECT COUNT(*) AS TOTAL_COUNT FROM MOVIE").ToString();
            lblTotalShows.Text = GetCount("SELECT COUNT(*) AS TOTAL_COUNT FROM SHOWS").ToString();
            lblTotalTicketsSold.Text = GetCount("SELECT COUNT(*) AS TOTAL_COUNT FROM TICKET").ToString();
        }

        private int GetCount(string query)
        {
            DataTable result = new DBConnection().ExecuteQuery(query);

            if (result.Rows.Count == 0)
            {
                return 0;
            }

            return Convert.ToInt32(result.Rows[0]["TOTAL_COUNT"]);
        }

        protected void LoadUsers()
        {
            const string query = "SELECT User_Id, User_Name, User_Email, User_Address FROM USER_TABLE ORDER BY User_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                DataTable usersTable = new DataTable();
                adapter.Fill(usersTable);
                GridViewUsers.DataSource = usersTable;
                GridViewUsers.DataBind();
            }
        }

        protected void InsertUser()
        {
            const string query = "INSERT INTO USER_TABLE (User_Id, User_Name, User_Email, User_Address) VALUES ((SELECT NVL(MAX(User_Id), 0) + 1 FROM USER_TABLE), :User_Name, :User_Email, :User_Address)";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":User_Name", OracleDbType.Varchar2).Value = txtName.Text.Trim();
                command.Parameters.Add(":User_Email", OracleDbType.Varchar2).Value = txtEmail.Text.Trim();
                command.Parameters.Add(":User_Address", OracleDbType.Varchar2).Value = txtAddress.Text.Trim();
                command.ExecuteNonQuery();
            }
        }

        protected void UpdateUser()
        {
            if (GridViewUsers.SelectedDataKey == null)
            {
                return;
            }

            const string query = "UPDATE USER_TABLE SET User_Name = :User_Name, User_Email = :User_Email, User_Address = :User_Address WHERE User_Id = :User_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":User_Name", OracleDbType.Varchar2).Value = txtName.Text.Trim();
                command.Parameters.Add(":User_Email", OracleDbType.Varchar2).Value = txtEmail.Text.Trim();
                command.Parameters.Add(":User_Address", OracleDbType.Varchar2).Value = txtAddress.Text.Trim();
                command.Parameters.Add(":User_Id", OracleDbType.Int32).Value = Convert.ToInt32(GridViewUsers.SelectedDataKey.Value);
                command.ExecuteNonQuery();
            }
        }

        protected void DeleteUser()
        {
            if (GridViewUsers.SelectedDataKey == null)
            {
                return;
            }

            const string query = "DELETE FROM USER_TABLE WHERE User_Id = :User_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":User_Id", OracleDbType.Int32).Value = Convert.ToInt32(GridViewUsers.SelectedDataKey.Value);
                command.ExecuteNonQuery();
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            InsertUser();
            LoadUsers();
            ClearInputs();
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateUser();
            LoadUsers();
            ClearInputs();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteUser();
            LoadUsers();
            ClearInputs();
        }

        protected void GridViewUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridViewUsers.SelectedRow == null)
            {
                return;
            }

            txtName.Text = GridViewUsers.SelectedRow.Cells[2].Text;
            txtEmail.Text = GridViewUsers.SelectedRow.Cells[3].Text;
            txtAddress.Text = GridViewUsers.SelectedRow.Cells[4].Text;
        }

        protected void LoadMovies()
        {
            const string query = "SELECT Movie_Id, Movie_Name, Movie_Release_Date FROM MOVIE ORDER BY Movie_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                DataTable moviesTable = new DataTable();
                adapter.Fill(moviesTable);
                GridViewMovies.DataSource = moviesTable;
                GridViewMovies.DataBind();
            }
        }

        protected void InsertMovie()
        {
            DateTime releaseDate;
            if (!DateTime.TryParse(txtReleaseDate.Text.Trim(), out releaseDate))
            {
                throw new InvalidOperationException("Please enter a valid movie release date.");
            }

            const string query = "INSERT INTO MOVIE (Movie_Id, Movie_Name, Movie_Release_Date) VALUES ((SELECT NVL(MAX(Movie_Id), 0) + 1 FROM MOVIE), :Movie_Name, :Movie_Release_Date)";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Movie_Name", OracleDbType.Varchar2).Value = txtMovieName.Text.Trim();
                command.Parameters.Add(":Movie_Release_Date", OracleDbType.Date).Value = releaseDate;
                command.ExecuteNonQuery();
            }
        }

        protected void UpdateMovie()
        {
            if (GridViewMovies.SelectedDataKey == null)
            {
                return;
            }

            DateTime releaseDate;
            if (!DateTime.TryParse(txtReleaseDate.Text.Trim(), out releaseDate))
            {
                throw new InvalidOperationException("Please enter a valid movie release date.");
            }

            const string query = "UPDATE MOVIE SET Movie_Name = :Movie_Name, Movie_Release_Date = :Movie_Release_Date WHERE Movie_Id = :Movie_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Movie_Name", OracleDbType.Varchar2).Value = txtMovieName.Text.Trim();
                command.Parameters.Add(":Movie_Release_Date", OracleDbType.Date).Value = releaseDate;
                command.Parameters.Add(":Movie_Id", OracleDbType.Int32).Value = Convert.ToInt32(GridViewMovies.SelectedDataKey.Value);
                command.ExecuteNonQuery();
            }
        }

        protected void DeleteMovie()
        {
            if (GridViewMovies.SelectedDataKey == null)
            {
                return;
            }

            const string query = "DELETE FROM MOVIE WHERE Movie_Id = :Movie_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Movie_Id", OracleDbType.Int32).Value = Convert.ToInt32(GridViewMovies.SelectedDataKey.Value);
                command.ExecuteNonQuery();
            }
        }

        protected void btnAddMovie_Click(object sender, EventArgs e)
        {
            InsertMovie();
            LoadMovies();
            ClearMovieInputs();
        }

        protected void btnUpdateMovie_Click(object sender, EventArgs e)
        {
            UpdateMovie();
            LoadMovies();
            ClearMovieInputs();
        }

        protected void btnDeleteMovie_Click(object sender, EventArgs e)
        {
            DeleteMovie();
            LoadMovies();
            ClearMovieInputs();
        }

        protected void GridViewMovies_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridViewMovies.SelectedRow == null)
            {
                return;
            }

            txtMovieName.Text = GridViewMovies.SelectedRow.Cells[2].Text;
            txtReleaseDate.Text = GridViewMovies.SelectedRow.Cells[3].Text;
        }

        protected void LoadTheaterDropdown()
        {
            const string query = "SELECT Theater_Id, Theater_Name || ' (ID: ' || Theater_Id || ')' AS Theater_Display FROM THEATER ORDER BY Theater_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                DataTable theatersTable = new DataTable();
                adapter.Fill(theatersTable);

                ddlTheaterId.DataSource = theatersTable;
                ddlTheaterId.DataTextField = "Theater_Display";
                ddlTheaterId.DataValueField = "Theater_Id";
                ddlTheaterId.DataBind();
            }
        }

        protected void LoadTheaters()
        {
            const string query = "SELECT t.Theater_Id, t.Theater_Name, t.Theater_City, h.Hall_Id, h.Hall_Number, h.Hall_Capacity FROM THEATER t LEFT JOIN HALL h ON t.Theater_Id = h.Theater_Id ORDER BY t.Theater_Id, h.Hall_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                DataTable theaterHallTable = new DataTable();
                adapter.Fill(theaterHallTable);
                GridViewTheaters.DataSource = theaterHallTable;
                GridViewTheaters.DataBind();
            }
        }

        protected void InsertTheater()
        {
            const string query = "INSERT INTO THEATER (Theater_Id, Theater_Name, Theater_City) VALUES ((SELECT NVL(MAX(Theater_Id), 0) + 1 FROM THEATER), :Theater_Name, :Theater_City)";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Theater_Name", OracleDbType.Varchar2).Value = txtTheaterName.Text.Trim();
                command.Parameters.Add(":Theater_City", OracleDbType.Varchar2).Value = txtTheaterCity.Text.Trim();
                command.ExecuteNonQuery();
            }
        }

        protected void InsertHall()
        {
            int hallCapacity;
            if (!int.TryParse(txtHallCapacity.Text.Trim(), out hallCapacity))
            {
                throw new InvalidOperationException("Please enter a valid hall capacity.");
            }

            const string query = "INSERT INTO HALL (Hall_Id, Theater_Id, Hall_Number, Hall_Capacity) VALUES ((SELECT NVL(MAX(Hall_Id), 0) + 1 FROM HALL), :Theater_Id, :Hall_Number, :Hall_Capacity)";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlTheaterId.SelectedValue);
                command.Parameters.Add(":Hall_Number", OracleDbType.Varchar2).Value = txtHallNumber.Text.Trim();
                command.Parameters.Add(":Hall_Capacity", OracleDbType.Int32).Value = hallCapacity;
                command.ExecuteNonQuery();
            }
        }

        protected void UpdateHall()
        {
            if (GridViewTheaters.SelectedDataKey == null)
            {
                return;
            }

            object hallIdValue = GridViewTheaters.SelectedDataKey.Values["Hall_Id"];
            if (hallIdValue == null || hallIdValue == DBNull.Value)
            {
                return;
            }

            int hallCapacity;
            if (!int.TryParse(txtHallCapacity.Text.Trim(), out hallCapacity))
            {
                throw new InvalidOperationException("Please enter a valid hall capacity.");
            }

            const string query = "UPDATE HALL SET Theater_Id = :Theater_Id, Hall_Number = :Hall_Number, Hall_Capacity = :Hall_Capacity WHERE Hall_Id = :Hall_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Theater_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlTheaterId.SelectedValue);
                command.Parameters.Add(":Hall_Number", OracleDbType.Varchar2).Value = txtHallNumber.Text.Trim();
                command.Parameters.Add(":Hall_Capacity", OracleDbType.Int32).Value = hallCapacity;
                command.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = Convert.ToInt32(hallIdValue);
                command.ExecuteNonQuery();
            }
        }

        protected void DeleteHall()
        {
            if (GridViewTheaters.SelectedDataKey == null)
            {
                return;
            }

            object hallIdValue = GridViewTheaters.SelectedDataKey.Values["Hall_Id"];
            if (hallIdValue == null || hallIdValue == DBNull.Value)
            {
                return;
            }

            const string query = "DELETE FROM HALL WHERE Hall_Id = :Hall_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = Convert.ToInt32(hallIdValue);
                command.ExecuteNonQuery();
            }
        }

        protected void btnAddTheater_Click(object sender, EventArgs e)
        {
            InsertTheater();
            LoadTheaterDropdown();
            LoadTheaters();
            ClearTheaterInputs();
        }

        protected void btnAddHall_Click(object sender, EventArgs e)
        {
            InsertHall();
            LoadTheaters();
            ClearHallInputs();
        }

        protected void btnUpdateHall_Click(object sender, EventArgs e)
        {
            UpdateHall();
            LoadTheaters();
            ClearHallInputs();
        }

        protected void btnDeleteHall_Click(object sender, EventArgs e)
        {
            DeleteHall();
            LoadTheaters();
            ClearHallInputs();
        }

        protected void GridViewTheaters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridViewTheaters.SelectedRow == null)
            {
                return;
            }

            txtTheaterName.Text = GridViewTheaters.SelectedRow.Cells[2].Text;
            txtTheaterCity.Text = GridViewTheaters.SelectedRow.Cells[3].Text;

            string selectedTheaterId = GridViewTheaters.SelectedRow.Cells[1].Text;
            ListItem theaterItem = ddlTheaterId.Items.FindByValue(selectedTheaterId);
            if (theaterItem != null)
            {
                ddlTheaterId.ClearSelection();
                theaterItem.Selected = true;
            }

            txtHallNumber.Text = GridViewTheaters.SelectedRow.Cells[5].Text == "&nbsp;" ? string.Empty : GridViewTheaters.SelectedRow.Cells[5].Text;
            txtHallCapacity.Text = GridViewTheaters.SelectedRow.Cells[6].Text == "&nbsp;" ? string.Empty : GridViewTheaters.SelectedRow.Cells[6].Text;
        }

        protected void LoadMoviesDropdown()
        {
            const string query = "SELECT Movie_Id, Movie_Name || ' (ID: ' || Movie_Id || ')' AS Movie_Display FROM MOVIE ORDER BY Movie_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                DataTable movieTable = new DataTable();
                adapter.Fill(movieTable);

                ddlMovies.DataSource = movieTable;
                ddlMovies.DataTextField = "Movie_Display";
                ddlMovies.DataValueField = "Movie_Id";
                ddlMovies.DataBind();
            }
        }

        protected void LoadHallsDropdown()
        {
            const string query = "SELECT Hall_Id, Hall_Number || ' (ID: ' || Hall_Id || ')' AS Hall_Display FROM HALL ORDER BY Hall_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                DataTable hallTable = new DataTable();
                adapter.Fill(hallTable);

                ddlHalls.DataSource = hallTable;
                ddlHalls.DataTextField = "Hall_Display";
                ddlHalls.DataValueField = "Hall_Id";
                ddlHalls.DataBind();
            }
        }

        protected void LoadShows()
        {
            const string query = "SELECT s.Show_Id, s.Movie_Id, m.Movie_Name, s.Hall_Id, h.Hall_Number, s.Show_Name, s.Show_Date, s.Show_Time FROM SHOWS s LEFT JOIN MOVIE m ON s.Movie_Id = m.Movie_Id LEFT JOIN HALL h ON s.Hall_Id = h.Hall_Id ORDER BY s.Show_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                DataTable showTable = new DataTable();
                adapter.Fill(showTable);
                GridViewShows.DataSource = showTable;
                GridViewShows.DataBind();
            }
        }

        protected void InsertShow()
        {
            DateTime showDate;
            if (!DateTime.TryParse(txtShowDate.Text.Trim(), out showDate))
            {
                throw new InvalidOperationException("Please enter a valid show date.");
            }

            const string query = "INSERT INTO SHOWS (Show_Id, Movie_Id, Hall_Id, Show_Name, Show_Date, Show_Time) VALUES ((SELECT NVL(MAX(Show_Id), 0) + 1 FROM SHOWS), :Movie_Id, :Hall_Id, :Show_Name, :Show_Date, :Show_Time)";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Movie_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlMovies.SelectedValue);
                command.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlHalls.SelectedValue);
                command.Parameters.Add(":Show_Name", OracleDbType.Varchar2).Value = txtShowName.Text.Trim();
                command.Parameters.Add(":Show_Date", OracleDbType.Date).Value = showDate;
                command.Parameters.Add(":Show_Time", OracleDbType.Varchar2).Value = txtShowTime.Text.Trim();
                command.ExecuteNonQuery();
            }
        }

        protected void UpdateShow()
        {
            if (GridViewShows.SelectedDataKey == null)
            {
                return;
            }

            DateTime showDate;
            if (!DateTime.TryParse(txtShowDate.Text.Trim(), out showDate))
            {
                throw new InvalidOperationException("Please enter a valid show date.");
            }

            const string query = "UPDATE SHOWS SET Movie_Id = :Movie_Id, Hall_Id = :Hall_Id, Show_Name = :Show_Name, Show_Date = :Show_Date, Show_Time = :Show_Time WHERE Show_Id = :Show_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Movie_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlMovies.SelectedValue);
                command.Parameters.Add(":Hall_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlHalls.SelectedValue);
                command.Parameters.Add(":Show_Name", OracleDbType.Varchar2).Value = txtShowName.Text.Trim();
                command.Parameters.Add(":Show_Date", OracleDbType.Date).Value = showDate;
                command.Parameters.Add(":Show_Time", OracleDbType.Varchar2).Value = txtShowTime.Text.Trim();
                command.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = Convert.ToInt32(GridViewShows.SelectedDataKey.Value);
                command.ExecuteNonQuery();
            }
        }

        protected void DeleteShow()
        {
            if (GridViewShows.SelectedDataKey == null)
            {
                return;
            }

            const string query = "DELETE FROM SHOWS WHERE Show_Id = :Show_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = Convert.ToInt32(GridViewShows.SelectedDataKey.Value);
                command.ExecuteNonQuery();
            }
        }

        protected void btnAddShow_Click(object sender, EventArgs e)
        {
            InsertShow();
            LoadShows();
            ClearShowInputs();
        }

        protected void btnUpdateShow_Click(object sender, EventArgs e)
        {
            UpdateShow();
            LoadShows();
            ClearShowInputs();
        }

        protected void btnDeleteShow_Click(object sender, EventArgs e)
        {
            DeleteShow();
            LoadShows();
            ClearShowInputs();
        }

        protected void GridViewShows_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridViewShows.SelectedRow == null)
            {
                return;
            }

            string selectedMovieId = GridViewShows.SelectedRow.Cells[2].Text;
            ListItem movieItem = ddlMovies.Items.FindByValue(selectedMovieId);
            if (movieItem != null)
            {
                ddlMovies.ClearSelection();
                movieItem.Selected = true;
            }

            string selectedHallId = GridViewShows.SelectedRow.Cells[4].Text;
            ListItem hallItem = ddlHalls.Items.FindByValue(selectedHallId);
            if (hallItem != null)
            {
                ddlHalls.ClearSelection();
                hallItem.Selected = true;
            }

            txtShowName.Text = GridViewShows.SelectedRow.Cells[6].Text;
            txtShowDate.Text = GridViewShows.SelectedRow.Cells[7].Text;
            txtShowTime.Text = GridViewShows.SelectedRow.Cells[8].Text;
        }

        protected void LoadBookingUsersDropdown()
        {
            const string query = "SELECT User_Id, User_Name || ' (ID: ' || User_Id || ')' AS User_Display FROM USER_TABLE ORDER BY User_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                DataTable usersTable = new DataTable();
                adapter.Fill(usersTable);

                ddlBookingUsers.DataSource = usersTable;
                ddlBookingUsers.DataTextField = "User_Display";
                ddlBookingUsers.DataValueField = "User_Id";
                ddlBookingUsers.DataBind();
            }
        }

        protected void LoadBookingShowsDropdown()
        {
            const string query = "SELECT Show_Id, Show_Name || ' (ID: ' || Show_Id || ')' AS Show_Display FROM SHOWS ORDER BY Show_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                DataTable showsTable = new DataTable();
                adapter.Fill(showsTable);

                ddlBookingShows.DataSource = showsTable;
                ddlBookingShows.DataTextField = "Show_Display";
                ddlBookingShows.DataValueField = "Show_Id";
                ddlBookingShows.DataBind();
            }
        }

        protected void LoadTickets()
        {
            const string query = "SELECT b.Booking_Id, t.Ticket_Id, b.User_Id, u.User_Name, b.Show_Id, s.Show_Name, b.Booking_Date, b.Booking_Status, t.Seat_Number, t.Ticket_Price FROM BOOKING b INNER JOIN TICKET t ON b.Booking_Id = t.Booking_Id LEFT JOIN USER_TABLE u ON b.User_Id = u.User_Id LEFT JOIN SHOWS s ON b.Show_Id = s.Show_Id ORDER BY b.Booking_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            using (OracleDataAdapter adapter = new OracleDataAdapter(command))
            {
                DataTable ticketsTable = new DataTable();
                adapter.Fill(ticketsTable);
                GridViewTickets.DataSource = ticketsTable;
                GridViewTickets.DataBind();
            }
        }

        protected void InsertBookingAndTicket()
        {
            decimal ticketPrice;
            if (!decimal.TryParse(txtTicketPrice.Text.Trim(), out ticketPrice))
            {
                throw new InvalidOperationException("Please enter a valid ticket price.");
            }

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    int bookingId;
                    int ticketId;

                    using (OracleCommand bookingIdCommand = new OracleCommand("SELECT NVL(MAX(Booking_Id), 0) + 1 FROM BOOKING", connection))
                    {
                        bookingIdCommand.Transaction = transaction;
                        bookingId = Convert.ToInt32(bookingIdCommand.ExecuteScalar());
                    }

                    using (OracleCommand ticketIdCommand = new OracleCommand("SELECT NVL(MAX(Ticket_Id), 0) + 1 FROM TICKET", connection))
                    {
                        ticketIdCommand.Transaction = transaction;
                        ticketId = Convert.ToInt32(ticketIdCommand.ExecuteScalar());
                    }

                    const string bookingQuery = "INSERT INTO BOOKING (Booking_Id, User_Id, Show_Id, Booking_Date, Booking_Status) VALUES (:Booking_Id, :User_Id, :Show_Id, :Booking_Date, :Booking_Status)";
                    using (OracleCommand bookingCommand = new OracleCommand(bookingQuery, connection))
                    {
                        bookingCommand.Transaction = transaction;
                        bookingCommand.Parameters.Add(":Booking_Id", OracleDbType.Int32).Value = bookingId;
                        bookingCommand.Parameters.Add(":User_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlBookingUsers.SelectedValue);
                        bookingCommand.Parameters.Add(":Show_Id", OracleDbType.Int32).Value = Convert.ToInt32(ddlBookingShows.SelectedValue);
                        bookingCommand.Parameters.Add(":Booking_Date", OracleDbType.Date).Value = DateTime.Now;
                        bookingCommand.Parameters.Add(":Booking_Status", OracleDbType.Varchar2).Value = "Booked";
                        bookingCommand.ExecuteNonQuery();
                    }

                    const string ticketQuery = "INSERT INTO TICKET (Ticket_Id, Booking_Id, Ticket_Price, Seat_Number) VALUES (:Ticket_Id, :Booking_Id, :Ticket_Price, :Seat_Number)";
                    using (OracleCommand ticketCommand = new OracleCommand(ticketQuery, connection))
                    {
                        ticketCommand.Transaction = transaction;
                        ticketCommand.Parameters.Add(":Ticket_Id", OracleDbType.Int32).Value = ticketId;
                        ticketCommand.Parameters.Add(":Booking_Id", OracleDbType.Int32).Value = bookingId;
                        ticketCommand.Parameters.Add(":Ticket_Price", OracleDbType.Decimal).Value = ticketPrice;
                        ticketCommand.Parameters.Add(":Seat_Number", OracleDbType.Varchar2).Value = txtSeatNumber.Text.Trim();
                        ticketCommand.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        protected void CancelTicketBooking()
        {
            if (GridViewTickets.SelectedDataKey == null)
            {
                return;
            }

            object bookingIdValue = GridViewTickets.SelectedDataKey.Values["Booking_Id"];
            if (bookingIdValue == null || bookingIdValue == DBNull.Value)
            {
                return;
            }

            const string query = "UPDATE BOOKING SET Booking_Status = :Booking_Status WHERE Booking_Id = :Booking_Id";

            using (OracleConnection connection = new DBConnection().GetConnection())
            using (OracleCommand command = new OracleCommand(query, connection))
            {
                command.Parameters.Add(":Booking_Status", OracleDbType.Varchar2).Value = "Cancelled";
                command.Parameters.Add(":Booking_Id", OracleDbType.Int32).Value = Convert.ToInt32(bookingIdValue);
                command.ExecuteNonQuery();
            }
        }

        protected void btnBookTicket_Click(object sender, EventArgs e)
        {
            InsertBookingAndTicket();
            LoadTickets();
            ClearTicketInputs();
        }

        protected void btnCancelTicket_Click(object sender, EventArgs e)
        {
            CancelTicketBooking();
            LoadTickets();
            ClearTicketInputs();
        }

        protected void GridViewTickets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridViewTickets.SelectedRow == null)
            {
                return;
            }

            string selectedUserId = GridViewTickets.SelectedRow.Cells[3].Text;
            ListItem userItem = ddlBookingUsers.Items.FindByValue(selectedUserId);
            if (userItem != null)
            {
                ddlBookingUsers.ClearSelection();
                userItem.Selected = true;
            }

            string selectedShowId = GridViewTickets.SelectedRow.Cells[5].Text;
            ListItem showItem = ddlBookingShows.Items.FindByValue(selectedShowId);
            if (showItem != null)
            {
                ddlBookingShows.ClearSelection();
                showItem.Selected = true;
            }

            txtSeatNumber.Text = GridViewTickets.SelectedRow.Cells[9].Text;
            txtTicketPrice.Text = GridViewTickets.SelectedRow.Cells[10].Text;
        }

        private void ClearInputs()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtAddress.Text = string.Empty;
            GridViewUsers.SelectedIndex = -1;
        }

        private void ClearMovieInputs()
        {
            txtMovieName.Text = string.Empty;
            txtReleaseDate.Text = string.Empty;
            GridViewMovies.SelectedIndex = -1;
        }

        private void ClearTheaterInputs()
        {
            txtTheaterName.Text = string.Empty;
            txtTheaterCity.Text = string.Empty;
            GridViewTheaters.SelectedIndex = -1;
        }

        private void ClearHallInputs()
        {
            txtHallNumber.Text = string.Empty;
            txtHallCapacity.Text = string.Empty;
            GridViewTheaters.SelectedIndex = -1;
        }

        private void ClearShowInputs()
        {
            txtShowName.Text = string.Empty;
            txtShowDate.Text = string.Empty;
            txtShowTime.Text = string.Empty;
            GridViewShows.SelectedIndex = -1;
        }

        private void ClearTicketInputs()
        {
            txtSeatNumber.Text = string.Empty;
            txtTicketPrice.Text = string.Empty;
            GridViewTickets.SelectedIndex = -1;
        }
    }
}