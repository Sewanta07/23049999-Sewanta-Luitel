<%@ Page Title="Shows" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Shows.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Shows_Shows" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Shows</h2>
        <asp:Label ID="lblMessage" runat="server" Visible="false" />
        <p><asp:Button ID="btnAddShow" runat="server" Text="Add New Show" CssClass="btn btn-update" PostBackUrl="~/Shows/ShowForm.aspx" /></p>
        <div class="table-wrap">
            <asp:GridView ID="GridViewShows" runat="server" AutoGenerateColumns="False" DataKeyNames="Show_Id" CssClass="table table-bordered" OnRowDeleting="GridViewShows_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Show_Id" HeaderText="Show ID" />
                    <asp:BoundField DataField="Movie_Name" HeaderText="Movie" />
                    <asp:BoundField DataField="Hall_Number" HeaderText="Hall" />
                    <asp:BoundField DataField="Show_Name" HeaderText="Show Name" />
                    <asp:BoundField DataField="Show_Date" HeaderText="Show Date" DataFormatString="{0:yyyy-MM-dd}" HtmlEncode="False" />
                    <asp:BoundField DataField="Show_Time" HeaderText="Show Time" />
                    <asp:HyperLinkField Text="Edit" ControlStyle-CssClass="table-action action-edit" DataNavigateUrlFields="Show_Id" DataNavigateUrlFormatString="~/Shows/ShowForm.aspx?id={0}" />
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" ControlStyle-CssClass="table-action action-delete" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
