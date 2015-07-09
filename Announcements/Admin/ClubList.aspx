<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ClubList.aspx.cs" Inherits="Announcements.Admin.ClubList" %>
<asp:Content ID="TitleSuffix" runat="server" ContentPlaceHolderID="titlesuffix"> - Admin Panel</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="Message" class="message-base message-success" runat="server" visible="false"></div>
    <ann:Infobox ID="Clubs" runat="server" Title="All Clubs">
        <form runat="server">
            View Mode: <asp:DropDownList ID="ViewMode" runat="server" AutoPostBack="true" /><br />
            <asp:CheckBox ID="ViewDeleted" runat="server" AutoPostBack="true" Text="Show Deleted" /><br /><br />
            <div style="margin-bottom: 3px"><asp:LinkButton ID="DeleteButton" runat="server" CssClass="linkbutton linkbutton-small" OnClick="DeleteButton_Click">Delete</asp:LinkButton></div>
            <table id="ClubTable" class="club-table" runat="server">
                <tr><th class="check"></th><th class="name">Name</th><th class="creator">Submitted By</th><th class="weekday">Day of Week</th><th class="status">Status</th><th class="action"></th></tr>
            </table>
            Displaying page <asp:Label ID="CurrentPage" Font-Bold="true" runat="server">0</asp:Label> of <asp:Label ID="MaxPage" Font-Bold="true" runat="server">0</asp:Label><br />
            <asp:LinkButton ID="FirstPageButton" runat="server" CssClass="linkbutton linkbutton-small" OnClick="FirstPageButton_Click">&lt;&lt;</asp:LinkButton>
            <asp:LinkButton ID="PreviousPageButton" runat="server" CssClass="linkbutton linkbutton-small" OnClick="PreviousPageButton_Click">&lt;</asp:LinkButton>
            <asp:LinkButton ID="NextPageButton" runat="server" CssClass="linkbutton linkbutton-small" OnClick="NextPageButton_Click">&gt;</asp:LinkButton>
            <asp:LinkButton ID="LastPageButton" runat="server" CssClass="linkbutton linkbutton-small" OnClick="LastPageButton_Click">&gt;&gt;</asp:LinkButton><br /><br />
            <a id="NewClub" runat="server" href="ClubEdit.aspx" class="linkbutton">New...</a> <a id="Back" href="Default.aspx" class="linkbutton">Back to Panel</a>
        </form>
    </ann:Infobox>
</asp:Content>
