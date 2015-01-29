<%@ Page Title="Centennial Daily Bulletin - Home" Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="License.aspx.cs" Inherits="Announcements.License" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="content">
    <ann:Infobox runat="server" Title="Announcement System Licensing">
        <img style="display: inline-block; float: right; margin: 5px" src="Images/agpl.png" alt="GNU Affero GPL version 3" />
        The Centennial announcements system is released under the terms of the <a href="https://www.gnu.org/licenses/agpl-3.0.html">GNU Affero General Public License version 3</a>, which is an open source license.
        You are free to view the <a href="https://www.github.com/bendude56/Centennial-Announcements"> source code</a> for this application on GitHub and modify it to your heart's content. <em>However, any modifications
        you make to this system must also be released under the same license.</em> This means that you <strong>must</strong> provide source code for <em>your modified version</em> to anybody who accesses any modified
        version of this system.
        <p />
        This program is distributed in the hope that it will be useful, but <strong>WITHOUT ANY WARRANTY</strong>; without even the implied warranty of <strong>MERCHANTABILITY</strong> or <strong>FITNESS FOR A PARTICULAR
        PURPOSE</strong>. See the GNU General Public License for more details.
        <p />
        This license is in use to protect the rights of end users to use and modify this software. If you notice an ongoing violation of this license, please report the violation to Benjamin Thomas
        &lt;<a href="mailto:bendude56@gmail.com">bendude56@gmail.com</a>&gt; as soon as possible so that it can be investigated.
    </ann:Infobox>
    <ann:Infobox runat="server" Title="Third-Party Libraries">
        In order to run and provide extra functionality, this system makes use of several third-party libraries which are released under their own respective licenses:
        <ul>
            <li><a href="http://www.ckeditor.com/">CKEditor</a> - Licensed under the terms of the <a href="https://www.gnu.org/licenses/gpl.html">GNU General Public License v3</a>.</li>
            <li><a href="https://ajaxcontroltoolkit.codeplex.com/">Ajax Control Toolkit</a> - Licensed under the terms of the <a href="https://ajaxcontroltoolkit.codeplex.com/license">New BSD License</a>.</li>
        </ul>
    </ann:Infobox>
</asp:Content>
