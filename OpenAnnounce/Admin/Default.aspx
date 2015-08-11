<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OpenAnnounce.Admin.Default" %>
<asp:Content ID="TitleSuffix" runat="server" ContentPlaceHolderID="titlesuffix"> - Admin Panel</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="content">
    <div id="Message" class="message-base" runat="server" visible="false"></div>
    <ann:Infobox ID="PersonalProfile" runat="server" Title="Personal Profile">
        <table>
            <tr><td>Username:</td><td><em><asp:Label ID="Username" runat="server" /></em></td></tr>
            <tr><td>Display Name:</td><td><em><asp:Label ID="DisplayName" runat="server" /></em></td></tr>
        </table>
        <a href="ProfileEdit.aspx" style="width: 5.0em;" class="linkbutton linkbutton-right">Edit Profile</a>
    </ann:Infobox>
    <ann:Infobox ID="Announcements" runat="server" Title="Announcements">
        <table id="AnnouncementTable" class="announcement-table" runat="server">
            <tr><th class="title">Title</th><th class="creator">Submitted By</th><th class="runtime">Running Dates</th><th class="scope">Scope</th><th class="status">Status</th><th class="action"></th></tr>
        </table><br />
        <a href="AnnouncementList.aspx" class="linkbutton">See all...</a> <a id="NewAnnouncement" runat="server" href="AnnouncementEdit.aspx" class="linkbutton">New...</a>
    </ann:Infobox>
    <ann:Infobox ID="Clubs" runat="server" Title="Clubs">
        <table id="ClubTable" class="club-table" runat="server">
            <tr><th class="name">Name</th><th class="creator">Submitted By</th><th class="weekday">Day of Week</th><th class="status">Status</th><th class="action"></th></tr>
        </table><br />
        <a href="ClubList.aspx" class="linkbutton">See all...</a> <a id="NewClub" runat="server" href="ClubEdit.aspx" class="linkbutton">New...</a>
    </ann:Infobox>
    <form runat="server">
        <ann:Infobox ID="NavbarEdit" runat="server" Title="Navbar">
            <table id="NavbarTable" class="navbar-table" runat="server">
                <tr><th class="text">Text</th><th class="url">URL</th><th class="scope">Scope</th><th class="action"></th></tr>
            </table><br />
            <asp:LinkButton ID="NavbarAddButton" runat="server" CssClass="linkbutton" OnClick="NavbarAddButton_Click">Add...</asp:LinkButton>
        </ann:Infobox>
    </form>
</asp:Content>