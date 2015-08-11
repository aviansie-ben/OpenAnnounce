<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ClubList.aspx.cs" Inherits="OpenAnnounce.ClubList" %>
<asp:Content ID="TitleSuffix" runat="server" ContentPlaceHolderID="titlesuffix"> - Club Information</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="content">
    <ann:ClubListInfobox ID="ClubListInfobox" runat="server" Title="Full Club List" DescriptionText="" ShowAll="true" />
</asp:Content>
