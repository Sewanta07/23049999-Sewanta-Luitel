<%@ Page Title="Movies" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Movies.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Movies_Movies" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Movies</h2>
        <p><asp:Button ID="btnAddMovie" runat="server" Text="Add New Movie" CssClass="btn btn-update" PostBackUrl="~/Movies/MovieForm.aspx" /></p>
        <div class="table-wrap">
            <asp:GridView ID="GridViewMovies" runat="server" AutoGenerateColumns="False" DataKeyNames="Movie_Id" CssClass="table table-bordered" OnRowDeleting="GridViewMovies_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Movie_Id" HeaderText="Movie ID" />
                    <asp:BoundField DataField="Movie_Name" HeaderText="Movie Name" />
                    <asp:BoundField DataField="Movie_Release_Date" HeaderText="Release Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                    <asp:HyperLinkField Text="Edit" ControlStyle-CssClass="table-action action-edit" DataNavigateUrlFields="Movie_Id" DataNavigateUrlFormatString="~/Movies/MovieForm.aspx?id={0}" />
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" ControlStyle-CssClass="table-action action-delete" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
