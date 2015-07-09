<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ClubInfo.aspx.cs" Inherits="Announcements.ClubInfo" %>
<asp:Content ID="TitleSuffix" runat="server" ContentPlaceHolderID="titlesuffix"> - Club Information</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <ann:ClubInfobox ID="ClubBox" runat="server" />
</asp:Content>
