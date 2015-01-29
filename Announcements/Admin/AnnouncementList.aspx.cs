﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Announcements.Data;

namespace Announcements.Admin
{
    public partial class AnnouncementList : System.Web.UI.Page
    {
        public static readonly Dictionary<string, string> messages = new Dictionary<string, string>()
        {
            { "submit", "Your announcement has been submitted. An administrator will review it shortly." },
            { "submit_autoapproval", "Your announcement has been submitted and automatically approved." },
            { "resubmit", "Your announcement has been resubmitted for approval. An administrator will review your changes shortly. Note that repeated resubmissions could result in loss of access to this system." },
            { "resubmit_autoapproval", "Your changes have been saved and your announcement has been automatically approved." },
            { "edit_reapprove", "Your changes have been saved. An administrator will review these changes and reply accordingly." },
            { "edit", "Your changes have been saved." },
            { "edit_autoapproval", "Your changes have been saved and the announcement has been successfully approved." },
            { "undelete", "Your changes have been saved and the announcement has been undeleted. An administrator will review your announcement shortly." },
            { "undelete_autoapproval", "Your changes have been saved and the announcement has been undeleted and automatically approved." },
            { "delete_soft", "The announcement has been deleted successfully." },
            { "delete_hard", "The announcement has been successfully purged from the database." },
            { "approve", "The announcement has been successfully marked as approved and will be displayed." },
            { "deny", "The announcement has been sucessfully denied." }
        };

        public new int Page
        {
            get
            {
                return (ViewState["Page"] == null) ? 1 : (int)ViewState["Page"];
            }

            set
            {
                ViewState["Page"] = value;
            }
        }

        User userInfo;
        Dictionary<int, CheckBox> checkBoxes = new Dictionary<int, CheckBox>();

        protected void Page_Init(object sender, EventArgs e)
        {
            userInfo = new User(User);
            if (!userInfo.SecurityAccess["CanAccessBackend"])
            {
                Response.Redirect("403.aspx", true);
            }

            NewAnnouncement.Visible = userInfo.SecurityAccess["CanSubmitAnnouncement"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["msg"] != null && messages.ContainsKey(Request.Params["msg"]))
            {
                Message.Visible = true;
                Message.Attributes["class"] = "message-base message-success";
                Message.InnerHtml = messages[Request.Params["msg"]];
            }

            PopulateModes();
            while (AnnouncementTable.Rows.Count > 1)
            {
                AnnouncementTable.Rows.RemoveAt(1);
            }
            Announcement.PopulatePageNumber(userInfo.Profile, CurrentPage, MaxPage, ViewMode.SelectedValue, ViewExpired.Checked, Page, 10);
            Announcement.PopulateAnnouncementTable(userInfo.Profile, ViewMode.SelectedValue, ViewExpired.Checked, ViewDeleted.Checked, AnnouncementTable, (Page - 1) * 10, 10, checkBoxes);
        }

        private void PopulateModes()
        {
            if (ViewMode.Items.Count == 0)
            {
                if (userInfo.SecurityAccess["CanApproveAnnouncement"] && userInfo.SecurityAccess["CanViewAllAnnouncement"])
                    ViewMode.Items.Add(new ListItem("Approval Mode", "Approval"));
                if (userInfo.SecurityAccess["CanViewAllAnnouncement"])
                    ViewMode.Items.Add(new ListItem("View All Mode", "ViewAll"));
                ViewMode.Items.Add(new ListItem("Submission Mode", "Submission"));
                
            }
            else
            {
                if (ViewMode.SelectedValue == "ViewAll" && !userInfo.SecurityAccess["CanViewAllAnnouncement"])
                    ViewMode.SelectedIndex = 0;
                else if (ViewMode.SelectedValue == "Approval" && !userInfo.SecurityAccess["CanApproveAnnouncement"])
                    ViewMode.SelectedIndex = 0;
            }
        }

        protected void FirstPageButton_Click(object sender, EventArgs e)
        {
            Page = 1;
            Page_Load(null, new EventArgs());
        }

        protected void PreviousPageButton_Click(object sender, EventArgs e)
        {
            if (Page != 1)
                Page--;
            Page_Load(this, new EventArgs());
        }

        protected void NextPageButton_Click(object sender, EventArgs e)
        {
            if (Page != Int32.Parse(MaxPage.Text))
                Page++;
            Page_Load(this, new EventArgs());
        }

        protected void LastPageButton_Click(object sender, EventArgs e)
        {
            if (Page != Int32.Parse(MaxPage.Text))
                Page = Int32.Parse(MaxPage.Text);
            Page_Load(this, new EventArgs());
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            List<Announcement> toDelete = new List<Announcement>();
            foreach (KeyValuePair<int, CheckBox> check in checkBoxes)
            {
                if (check.Value.Checked)
                    toDelete.Add(Announcement.FromDatabase(check.Key));
            }

            if (toDelete.Count == 0)
            {
                Message.Visible = true;
                Message.Attributes["class"] = "message-base message-error";
                Message.InnerHtml = "Please select the announcements you would like to delete.";
                return;
            }

            if (!userInfo.SecurityAccess["CanEditAllAnnouncement"])
            {
                foreach (Announcement a in toDelete)
                {
                    if (a.CreatorId != userInfo.Profile.Id)
                    {
                        Message.Visible = true;
                        Message.Attributes["class"] = "message-base message-error";
                        Message.InnerHtml = "You cannot delete one or more of the selected announcements.";
                        return;
                    }
                }
            }

            foreach (Announcement a in toDelete)
            {
                a.Status = Announcement.AnnouncementStatus.Deleted;
                a.StatusUserId = userInfo.Profile.Id;
                a.StatusTime = DateTime.Now;
                a.Update();
            }

            Message.Visible = true;
            Message.Attributes["class"] = "message-base message-success";
            Message.InnerHtml = "All selected announcements have been successfully deleted.";

            Page_Load(this, new EventArgs());
        }
    }
}