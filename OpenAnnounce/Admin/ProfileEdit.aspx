<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ProfileEdit.aspx.cs" Inherits="OpenAnnounce.Admin.ProfileEdit" %>
<asp:Content ID="TitleSuffix" runat="server" ContentPlaceHolderID="titlesuffix"> - Edit Profile</asp:Content>
<asp:Content ID="Content1" runat="server" contentplaceholderid="content">
    <div id="Message" class="message-base" runat="server" visible="false"></div>
    <form runat="server">
        <ann:Infobox ID="ProfileBox" runat="server" Title="Edit Profile: &amp;lt;Username&amp;gt;">
            <table>
                <tr><td>Username:</td><td><em><asp:Label ID="Username" runat="server" /></em></td></tr>
                <tr><td>Display Name:</td><td><em><asp:TextBox ID="DisplayName" runat="server" Width="200px" MaxLength="32" /></em></td></tr>
            </table><br />
            <asp:LinkButton ID="SaveButton" runat="server" CssClass="linkbutton" OnClick="SaveButton_Click">Save</asp:LinkButton>
            <a href="Default.aspx" class="linkbutton">Cancel</a>
        </ann:Infobox>
    </form>
</asp:Content>