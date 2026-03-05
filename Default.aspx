<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="_23049999_Sewanta_Luitel._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <h2>Cinema Dashboard</h2>
        <div class="row" style="margin-bottom: 16px;">
            <div class="col-md-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Total Users</h5>
                        <asp:Label ID="lblTotalUsers" runat="server" CssClass="h4" Text="0" />
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Total Movies</h5>
                        <asp:Label ID="lblTotalMovies" runat="server" CssClass="h4" Text="0" />
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Total Shows</h5>
                        <asp:Label ID="lblTotalShows" runat="server" CssClass="h4" Text="0" />
                    </div>
                </div>
            </div>
            <div class="col-md-3">
                <div class="card">
                    <div class="card-body">
                        <h5 class="card-title">Total Tickets Sold</h5>
                        <asp:Label ID="lblTotalTicketsSold" runat="server" CssClass="h4" Text="0" />
                    </div>
                </div>
            </div>
        </div>

        <h2>User Management</h2>

        <div class="form-group">
            <label for="txtName">Name</label>
            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtEmail">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtAddress">Address</label>
            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
        </div>

        <div style="margin-top: 12px; margin-bottom: 12px;">
            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" />
            <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-warning" OnClick="btnUpdate_Click" />
            <asp:Button ID="btnDelete" runat="server" Text="Delete" CssClass="btn btn-danger" OnClick="btnDelete_Click" />
        </div>

        <asp:GridView ID="GridViewUsers" runat="server" AutoGenerateColumns="False" DataKeyNames="User_Id" CssClass="table table-bordered" OnSelectedIndexChanged="GridViewUsers_SelectedIndexChanged">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="User_Id" HeaderText="User ID" ReadOnly="True" />
                <asp:BoundField DataField="User_Name" HeaderText="Name" />
                <asp:BoundField DataField="User_Email" HeaderText="Email" />
                <asp:BoundField DataField="User_Address" HeaderText="Address" />
            </Columns>
        </asp:GridView>

        <h2 style="margin-top: 24px;">Movie Management</h2>

        <div class="form-group">
            <label for="txtMovieName">Movie Name</label>
            <asp:TextBox ID="txtMovieName" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtReleaseDate">Release Date</label>
            <asp:TextBox ID="txtReleaseDate" runat="server" CssClass="form-control" />
        </div>

        <div style="margin-top: 12px; margin-bottom: 12px;">
            <asp:Button ID="btnAddMovie" runat="server" Text="Add Movie" CssClass="btn btn-primary" OnClick="btnAddMovie_Click" />
            <asp:Button ID="btnUpdateMovie" runat="server" Text="Update Movie" CssClass="btn btn-warning" OnClick="btnUpdateMovie_Click" />
            <asp:Button ID="btnDeleteMovie" runat="server" Text="Delete Movie" CssClass="btn btn-danger" OnClick="btnDeleteMovie_Click" />
        </div>

        <asp:GridView ID="GridViewMovies" runat="server" AutoGenerateColumns="False" DataKeyNames="Movie_Id" CssClass="table table-bordered" OnSelectedIndexChanged="GridViewMovies_SelectedIndexChanged">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Movie_Id" HeaderText="Movie ID" ReadOnly="True" />
                <asp:BoundField DataField="Movie_Name" HeaderText="Movie Name" />
                <asp:BoundField DataField="Movie_Release_Date" HeaderText="Release Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
            </Columns>
        </asp:GridView>

        <h2 style="margin-top: 24px;">Theater and Hall Management</h2>

        <h4>Add Theater</h4>
        <div class="form-group">
            <label for="txtTheaterName">Theater Name</label>
            <asp:TextBox ID="txtTheaterName" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtTheaterCity">Theater City</label>
            <asp:TextBox ID="txtTheaterCity" runat="server" CssClass="form-control" />
        </div>

        <div style="margin-top: 12px; margin-bottom: 12px;">
            <asp:Button ID="btnAddTheater" runat="server" Text="Add Theater" CssClass="btn btn-primary" OnClick="btnAddTheater_Click" />
        </div>

        <h4>Add / Update Hall</h4>
        <div class="form-group">
            <label for="ddlTheaterId">Select Theater</label>
            <asp:DropDownList ID="ddlTheaterId" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtHallNumber">Hall Number</label>
            <asp:TextBox ID="txtHallNumber" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtHallCapacity">Hall Capacity</label>
            <asp:TextBox ID="txtHallCapacity" runat="server" CssClass="form-control" />
        </div>

        <div style="margin-top: 12px; margin-bottom: 12px;">
            <asp:Button ID="btnAddHall" runat="server" Text="Add Hall" CssClass="btn btn-primary" OnClick="btnAddHall_Click" />
            <asp:Button ID="btnUpdateHall" runat="server" Text="Update Hall" CssClass="btn btn-warning" OnClick="btnUpdateHall_Click" />
            <asp:Button ID="btnDeleteHall" runat="server" Text="Delete Hall" CssClass="btn btn-danger" OnClick="btnDeleteHall_Click" />
        </div>

        <asp:GridView ID="GridViewTheaters" runat="server" AutoGenerateColumns="False" DataKeyNames="Hall_Id,Theater_Id" CssClass="table table-bordered" OnSelectedIndexChanged="GridViewTheaters_SelectedIndexChanged">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Theater_Id" HeaderText="Theater ID" ReadOnly="True" />
                <asp:BoundField DataField="Theater_Name" HeaderText="Theater Name" />
                <asp:BoundField DataField="Theater_City" HeaderText="Theater City" />
                <asp:BoundField DataField="Hall_Id" HeaderText="Hall ID" ReadOnly="True" />
                <asp:BoundField DataField="Hall_Number" HeaderText="Hall Number" />
                <asp:BoundField DataField="Hall_Capacity" HeaderText="Hall Capacity" />
            </Columns>
        </asp:GridView>

        <h2 style="margin-top: 24px;">Show Management</h2>

        <div class="form-group">
            <label for="ddlMovies">Select Movie</label>
            <asp:DropDownList ID="ddlMovies" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="ddlHalls">Select Hall</label>
            <asp:DropDownList ID="ddlHalls" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtShowName">Show Name</label>
            <asp:TextBox ID="txtShowName" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtShowDate">Show Date</label>
            <asp:TextBox ID="txtShowDate" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtShowTime">Show Time</label>
            <asp:TextBox ID="txtShowTime" runat="server" CssClass="form-control" />
        </div>

        <div style="margin-top: 12px; margin-bottom: 12px;">
            <asp:Button ID="btnAddShow" runat="server" Text="Add Show" CssClass="btn btn-primary" OnClick="btnAddShow_Click" />
            <asp:Button ID="btnUpdateShow" runat="server" Text="Update Show" CssClass="btn btn-warning" OnClick="btnUpdateShow_Click" />
            <asp:Button ID="btnDeleteShow" runat="server" Text="Delete Show" CssClass="btn btn-danger" OnClick="btnDeleteShow_Click" />
        </div>

        <asp:GridView ID="GridViewShows" runat="server" AutoGenerateColumns="False" DataKeyNames="Show_Id" CssClass="table table-bordered" OnSelectedIndexChanged="GridViewShows_SelectedIndexChanged">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Show_Id" HeaderText="Show ID" ReadOnly="True" />
                <asp:BoundField DataField="Movie_Id" HeaderText="Movie ID" />
                <asp:BoundField DataField="Movie_Name" HeaderText="Movie Name" />
                <asp:BoundField DataField="Hall_Id" HeaderText="Hall ID" />
                <asp:BoundField DataField="Hall_Number" HeaderText="Hall Number" />
                <asp:BoundField DataField="Show_Name" HeaderText="Show Name" />
                <asp:BoundField DataField="Show_Date" HeaderText="Show Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                <asp:BoundField DataField="Show_Time" HeaderText="Show Time" />
            </Columns>
        </asp:GridView>

        <h2 style="margin-top: 24px;">Ticket Booking Management</h2>

        <div class="form-group">
            <label for="ddlBookingUsers">Select User</label>
            <asp:DropDownList ID="ddlBookingUsers" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="ddlBookingShows">Select Show</label>
            <asp:DropDownList ID="ddlBookingShows" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtSeatNumber">Seat Number</label>
            <asp:TextBox ID="txtSeatNumber" runat="server" CssClass="form-control" />
        </div>

        <div class="form-group">
            <label for="txtTicketPrice">Ticket Price</label>
            <asp:TextBox ID="txtTicketPrice" runat="server" CssClass="form-control" />
        </div>

        <div style="margin-top: 12px; margin-bottom: 12px;">
            <asp:Button ID="btnBookTicket" runat="server" Text="Book Ticket" CssClass="btn btn-primary" OnClick="btnBookTicket_Click" />
            <asp:Button ID="btnCancelTicket" runat="server" Text="Cancel Ticket" CssClass="btn btn-danger" OnClick="btnCancelTicket_Click" />
        </div>

        <asp:GridView ID="GridViewTickets" runat="server" AutoGenerateColumns="False" DataKeyNames="Booking_Id,Ticket_Id" CssClass="table table-bordered" OnSelectedIndexChanged="GridViewTickets_SelectedIndexChanged">
            <Columns>
                <asp:CommandField ShowSelectButton="True" />
                <asp:BoundField DataField="Booking_Id" HeaderText="Booking ID" ReadOnly="True" />
                <asp:BoundField DataField="Ticket_Id" HeaderText="Ticket ID" ReadOnly="True" />
                <asp:BoundField DataField="User_Id" HeaderText="User ID" />
                <asp:BoundField DataField="User_Name" HeaderText="User Name" />
                <asp:BoundField DataField="Show_Id" HeaderText="Show ID" />
                <asp:BoundField DataField="Show_Name" HeaderText="Show Name" />
                <asp:BoundField DataField="Booking_Date" HeaderText="Booking Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                <asp:BoundField DataField="Booking_Status" HeaderText="Status" />
                <asp:BoundField DataField="Seat_Number" HeaderText="Seat Number" />
                <asp:BoundField DataField="Ticket_Price" HeaderText="Ticket Price" />
            </Columns>
        </asp:GridView>
    </main>

</asp:Content>
