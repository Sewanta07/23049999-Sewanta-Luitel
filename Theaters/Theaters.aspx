<%@ Page Title="Theaters" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Theaters.aspx.cs" Inherits="_23049999_Sewanta_Luitel.Theaters_Theaters" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main class="app-shell">
        <h2>Theaters and Halls</h2>
        <p><asp:Button ID="btnAddTheater" runat="server" Text="Add New Theater" CssClass="btn btn-update" PostBackUrl="~/Theaters/TheaterForm.aspx" /></p>
        <div class="table-wrap">
            <asp:GridView ID="GridViewTheaters" runat="server" AutoGenerateColumns="False" DataKeyNames="Theater_Id" CssClass="table table-bordered" OnRowDeleting="GridViewTheaters_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="Theater_Id" HeaderText="Theater ID" />
                    <asp:BoundField DataField="Theater_Name" HeaderText="Theater Name" />
                    <asp:BoundField DataField="Theater_City" HeaderText="City" />
                    <asp:BoundField DataField="Hall_Id" HeaderText="Hall ID" />
                    <asp:BoundField DataField="Hall_Number" HeaderText="Hall Number" />
                    <asp:BoundField DataField="Hall_Capacity" HeaderText="Hall Capacity" />
                    <asp:HyperLinkField Text="Edit Theater" ControlStyle-CssClass="table-action action-edit" DataNavigateUrlFields="Theater_Id" DataNavigateUrlFormatString="~/Theaters/TheaterForm.aspx?id={0}" />
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Delete" ControlStyle-CssClass="table-action action-delete" />
                </Columns>
            </asp:GridView>
        </div>
    </main>
</asp:Content>
