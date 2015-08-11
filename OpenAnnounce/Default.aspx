<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="OpenAnnounce.Default" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="content">
    <ann:Infobox ID="SubmitInfobox" runat="server" SmallText="true" Visible="<%#!String.IsNullOrEmpty(OpenAnnounce.Config.FrontPageNotice)%>">
        <%#OpenAnnounce.Config.FrontPageNotice%>
    </ann:Infobox>
    <asp:Panel ID="Announcements" runat="server"></asp:Panel>
    <ann:ClubListInfobox ID="ClubList" runat="server" Title="Clubs meeting today" DescriptionText="The following clubs will be meeting today at lunch. Click on the name for a short description of the club. If you need further information, please contact the responsible teacher." />
</asp:Content>
