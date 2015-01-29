<%@ Page Title="Centennial Daily Bulletin - Internal Server Error" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="500.aspx.cs" Inherits="Announcements._500" %>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <form id="form1" runat="server">
    <ann:Infobox runat="server" Title="Error 500 - Internal Server Error" ID="ErrorBox">
        <p>
            A fatal error occurred while processing your request. Try your request again in a few minutes
            and contact Tech Support if the problem persists.
        </p>
    </ann:Infobox>
    </form>
</asp:Content>
