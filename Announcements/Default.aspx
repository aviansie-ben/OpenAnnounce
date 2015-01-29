<%@ Page Title="Centennial Daily Bulletin - Home" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Announcements.Default" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="content">
    <ann:Infobox ID="SubmitInfobox" runat="server" SmallText="true">
        If you would like to submit an announcement for inclusion on this page, please send a copy of the proposed announcement to <a href="mailto:centennialstudentvoice@cbe.ab.ca">Centennial Student Voice</a>. In order to be eligible for inclusion,
        announcements must be submitted by <strong>1:30pm</strong> the day before they should be shown.
    </ann:Infobox>
    <br />
    <asp:Panel ID="Announcements" runat="server"></asp:Panel>
    <ann:CafeteriaWeeklyMenuInfobox ID="CafeteriaMenu" runat="server" Title="Cafeteria Menu" />
    <ann:ClubListInfobox ID="ClubList" runat="server" Title="Clubs meeting today" DescriptionText="The following clubs will be meeting today at lunch. Click on the name for a short description of the club. If you need further information, please contact the responsible teacher." />
</asp:Content>
