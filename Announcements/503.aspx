<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="503.aspx.cs" Inherits="Announcements._503" %>
<asp:Content ID="TitleSuffix" runat="server" ContentPlaceHolderID="titlesuffix"> - Service Unavailable</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <form id="form1" runat="server">
    <ann:Infobox runat="server" Title="Error 503 - Service Unavailable" ID="ErrorBox">
        <p>
            The Centennial Daily Bulletin is currently offline due to technical difficulties. Please try again
            in a few minutes, or if the problem persists after several attempts please contact Tech Support for
            further assistance.
        </p>
    </ann:Infobox>
    </form>
</asp:Content>
