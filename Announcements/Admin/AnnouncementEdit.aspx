<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="AnnouncementEdit.aspx.cs" Inherits="Announcements.Admin.AnnouncementEdit" %>
<asp:Content ID="TitleSuffix" runat="server" ContentPlaceHolderID="titlesuffix"> - Edit Announcement</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="content" runat="server">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager" runat="server" />
        <div id="Message" class="message-base" runat="server" visible="false"></div>
        <ann:Infobox ID="MainInfobox" runat="server" Title="Announcement">
            <asp:TextBox ID="AnnouncementTitle" runat="server" MaxLength="32" Width="256px" Font-Size="1.2em" style="margin-bottom: 10px;" />
            <cke:CKEditorControl ID="AnnouncementBody" runat="server" BasePath="../Scripts/CKEditor" Width="100%" Height="256px"
                Toolbar="Source
                Cut|Copy|Paste|PasteText|PasteFromWord|-|Undo|Redo
                Find|Replace|-|SelectAll
                Bold|Italic|Strike|-|RemoveFormat
                NumberedList|BulletedList|-|Blockquote
                Link|Unlink
                HorizontalRule|SpecialChar
                /
                Format
                Maximize|ShowBlocks
                About" FormatTags="h1;h2;h3;pre" ContentsCss="../Style/Infobox.css" RemoveDialogTabs="link:advanced"></cke:CKEditorControl>
            <br />Display from 
            <asp:TextBox ID="StartDate" runat="server" /><ajax:CalendarExtender ID="StartDateCalendar" runat="server" TargetControlID="StartDate" Format="dd/MM/yyyy" /> to 
            <asp:TextBox ID="EndDate" runat="server" /><ajax:CalendarExtender ID="EndDateCalendar" runat="server" TargetControlID="EndDate" Format="dd/MM/yyyy" /> for 
            <asp:DropDownList ID="Scope" runat="server" /><br />
            <span id="ImportanceContainer" runat="server">Importance:
                <asp:DropDownList ID="Importance" runat="server">
                    <asp:ListItem Value="10" Text="10" />
                    <asp:ListItem Value="9" Text="9" />
                    <asp:ListItem Value="8" Text="8" />
                    <asp:ListItem Value="7" Text="7" />
                    <asp:ListItem Value="6" Text="6" />
                    <asp:ListItem Value="5" Text="5" />
                    <asp:ListItem Value="4" Text="4" />
                    <asp:ListItem Value="3" Text="3" />
                    <asp:ListItem Value="2" Text="2" />
                    <asp:ListItem Value="1" Text="1" />
                    <asp:ListItem Value="0" Text="0" />
                </asp:DropDownList>
                <br /></span><br />
            <asp:LinkButton Id="SubmitLink" CssClass="linkbutton" runat="server" OnClick="SubmitLink_Click">Submit</asp:LinkButton>
            <asp:LinkButton Id="SubmitAndApproveLink" CssClass="linkbutton" runat="server" OnClick="SubmitAndApproveLink_Click">Submit + Approve</asp:LinkButton>
            <asp:LinkButton ID="DeleteLink" CssClass="linkbutton" runat="server" OnClick="DeleteLink_Click">Delete</asp:LinkButton>
            <a href="AnnouncementList.aspx" class="linkbutton">Cancel</a>
        </ann:Infobox>
        <ann:Infobox ID="AdminInfobox" runat="server" Title="Administrative Actions">
            <div id="ApproveDeny" runat="server">
                <h1>Approve/Deny</h1>
                <asp:LinkButton ID="ApproveLink" CssClass="linkbutton" runat="server" OnClick="ApproveLink_Click">Approve</asp:LinkButton><br />
                <asp:TextBox ID="DenyReason" runat="server" /><asp:LinkButton ID="DenyLink" CssClass="linkbutton" runat="server" OnClick="DenyLink_Click">Deny</asp:LinkButton>
            </div>
            <div id="HardDelete" runat="server">
                <h1>Hard Delete</h1>
                <strong>WARNING:</strong> Hard deleting an announcement will prevent it from being undeleted. Please be absolutely sure that you wish to delete this announcement
                before proceeding.<br /><br />
                <asp:CheckBox ID="HardDeleteCheck" runat="server" Text="I'm sure that I want to hard delete this announcement" /><br />
                <asp:LinkButton ID="HardDeleteLink" CssClass="linkbutton" runat="server" OnClick="HardDeleteLink_Click">HARD DELETE</asp:LinkButton>
            </div>
        </ann:Infobox>
        <ann:Infobox ID="StatusInfobox" runat="server" Title="Status Information">
            Submitted by <asp:Label ID="CreatorName" runat="server" Font-Bold="true">Creator</asp:Label> at <asp:Label ID="CreatedTime" runat="server" Font-Bold="true">Time</asp:Label>.<br />
            <span id="Editor" runat="server">Last edited by <asp:Label ID="EditorName" runat="server" Font-Bold="true">Editor</asp:Label> at <asp:Label ID="EditedTime" runat="server" Font-Bold="true">Time</asp:Label>.<br /></span>
            <asp:Label ID="StatusLabel" runat="server">Announcement is <strong>Status</strong></asp:Label>
        </ann:Infobox>
    </form>
</asp:Content>
