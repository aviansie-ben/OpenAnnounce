<%@ Page Title="Centennial Daily Bulletin - Edit Club" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="ClubEdit.aspx.cs" Inherits="Announcements.Admin.ClubEdit" ValidateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <form id="aspnetForm" runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" />
        <div id="Message" class="message-base" runat="server" visible="false"></div>
        <ann:Infobox ID="MainInfobox" runat="server" Title="Club">
            <asp:TextBox ID="ClubName" runat="server" MaxLength="32" Width="256px" Font-Size="1.2em" /><br />
            <cke:CKEditorControl ID="ClubDescription" runat="server" BasePath="../Scripts/CKEditor" Width="100%" Height="256px"></cke:CKEditorControl>
            <br />
            Teacher in charge (Username in form CBEORG\&lt;Username&gt;): <asp:TextBox ID="Teacher" runat="server" />
            <asp:LinkButton ID="EditProfile" runat="server" CssClass="linkbutton" OnClientClick="aspnetForm.target='_blank';" OnClick="EditProfile_Click" Text="Create/Edit Profile" /><br />
            Day of week:
            <asp:DropDownList ID="Weekday" runat="server">
                <asp:ListItem Text="Sunday" Value="0" />
                <asp:ListItem Text="Monday" Value="1" />
                <asp:ListItem Text="Tuesday" Value="2" />
                <asp:ListItem Text="Wednesday" Value="3" />
                <asp:ListItem Text="Thursday" Value="4" />
                <asp:ListItem Text="Friday" Value="5" />
                <asp:ListItem Text="Saturday" Value="6" />
            </asp:DropDownList>
            <asp:CheckBox ID="AfterSchool" runat="server" Text="After school" /><br />
            Location: <asp:TextBox ID="Location" runat="server" /><br />
            <br /><br />
            <asp:LinkButton Id="SubmitLink" CssClass="linkbutton" runat="server" OnClientClick="aspnetForm.target='_self';" OnClick="SubmitLink_Click">Submit</asp:LinkButton>
            <asp:LinkButton Id="SubmitAndApproveLink" CssClass="linkbutton" runat="server" OnClientClick="aspnetForm.target='_self';" OnClick="SubmitAndApproveLink_Click">Submit + Approve</asp:LinkButton>
            <asp:LinkButton ID="DeleteLink" CssClass="linkbutton" runat="server" OnClientClick="aspnetForm.target='_self';" OnClick="DeleteLink_Click">Delete</asp:LinkButton>
            <a href="ClubList.aspx" class="linkbutton">Cancel</a>
        </ann:Infobox>
        <ann:Infobox ID="AdminInfobox" runat="server" Title="Administrative Actions">
            <div id="ApproveDeny" runat="server">
                <h2>Approve/Deny</h2>
                <asp:LinkButton ID="ApproveLink" CssClass="linkbutton" runat="server" OnClientClick="aspnetForm.target='_self';" OnClick="ApproveLink_Click">Approve</asp:LinkButton><br />
                <asp:TextBox ID="DenyReason" runat="server" /><asp:LinkButton ID="DenyLink" OnClientClick="aspnetForm.target='_self';" CssClass="linkbutton" runat="server" OnClick="DenyLink_Click">Deny</asp:LinkButton>
            </div>
            <div id="HardDelete" runat="server">
                <h2>Hard Delete</h2>
                <strong>WARNING:</strong> Hard deleting a club will prevent it from being undeleted. Please be absolutely sure that you wish to delete this club
                before proceeding.<br /><br />
                <asp:CheckBox ID="HardDeleteCheck" runat="server" Text="I'm sure that I want to hard delete this club" /><br />
                <asp:LinkButton ID="HardDeleteLink" CssClass="linkbutton" runat="server" OnClientClick="aspnetForm.target='_self';" OnClick="HardDeleteLink_Click">HARD DELETE</asp:LinkButton>
            </div>
        </ann:Infobox>
        <ann:Infobox ID="StatusInfobox" runat="server" Title="Status Information">
            Submitted by <asp:Label ID="CreatorName" runat="server" Font-Bold="true">Creator</asp:Label> at <asp:Label ID="CreatedTime" runat="server" Font-Bold="true">Time</asp:Label>.<br />
            <span id="Editor" runat="server">Last edited by <asp:Label ID="EditorName" runat="server" Font-Bold="true">Editor</asp:Label> at <asp:Label ID="EditedTime" runat="server" Font-Bold="true">Time</asp:Label>.<br /></span>
            <asp:Label ID="StatusLabel" runat="server">Club is <strong>Status</strong></asp:Label>
        </ann:Infobox>
    </form>
</asp:Content>
