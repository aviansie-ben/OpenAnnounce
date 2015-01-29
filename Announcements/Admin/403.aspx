<%@ Page Title="Centennial Daily Bulletin - Access Denied" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="403.aspx.cs" Inherits="Announcements.Admin._403" %>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <form id="form1" runat="server">
    <ann:Infobox runat="server" Title="Access Denied">
        <p>The account that you are using to view this page has not been granted the ability to use the Centennial Daily Bulletin administration system. If you have a secondary account with access to this system, please use that account to log into the computer before accessing this page.</p>
        <p>If you believe that you should have access to this page and are sure that you are logged into the correct account, please contact <a href="mailto:centennialstudentvoice@cbe.ab.ca">Centennial Student Voice</a> for further assistance.</p>
        <p>If you&#39;re <strong>not</strong> supposed to have access to this page, then the security system has done its job. Please return to what you were doing before and feel very bad about what you attempted to do.</p>
        <a href="/Default.aspx" style="width: 5.5em" class="linkbutton linkbutton-right">Return Home</a>
    </ann:Infobox>
    </form>
</asp:Content>
