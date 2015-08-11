<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="401.aspx.cs" Inherits="OpenAnnounce._401" %>
<asp:Content ID="TitleSuffix" runat="server" ContentPlaceHolderID="titlesuffix"> - Access Denied</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <form id="form1" runat="server">
    <ann:Infobox runat="server" Title="Access Denied">
        <p>
            In order to access the Centennial Daily Bulletin, you must be logged into your computer with a valid Calgary Board of Education account.
            If you continue to experience this, please contact Tech Support.
        </p>
    </ann:Infobox>
    </form>
</asp:Content>
