<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="404.aspx.cs" Inherits="OpenAnnounce._404" %>
<asp:Content ID="TitleSuffix" runat="server" ContentPlaceHolderID="titlesuffix"> - Not Found</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <form id="form1" runat="server">
    <ann:Infobox runat="server" Title="Error 404 - Page Not Found">
        <p>
            The requested page could not be found. Double check that you spelled the URL right.
        </p>
    </ann:Infobox>
    </form>
</asp:Content>
