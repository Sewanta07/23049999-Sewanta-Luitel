<%@ Page Title="Theater Movie Report" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TheaterMovieReport.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Reports_TheaterMovieReport" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Theater Movie Report</h2>
        <div class="form-group"><label for="ddlTheaters">Theater</label><asp:DropDownList ID="ddlTheaters" runat="server" CssClass="form-control" /></div>
        <p><asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-update" OnClick="btnSearch_Click" /></p>
        <div class="table-wrap">
            <asp:GridView ID="GridViewReport" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered">
                <Columns>
                    <asp:BoundField DataField="Theater_Name" HeaderText="Theater" />
                    <asp:BoundField DataField="Hall_Number" HeaderText="Hall" />
                    <asp:BoundField DataField="Movie_Name" HeaderText="Movie" />
                    <asp:BoundField DataField="Show_Name" HeaderText="Show" />
                    <asp:BoundField DataField="Show_Date" HeaderText="Show Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                    <asp:BoundField DataField="Show_Time" HeaderText="Show Time" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
