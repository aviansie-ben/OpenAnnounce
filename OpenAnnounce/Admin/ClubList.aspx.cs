using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OpenAnnounce.Data;

namespace OpenAnnounce.Admin
{
    public partial class ClubList : AnnouncementsPage
    {
        public static readonly Dictionary<string, string> messages = new Dictionary<string, string>()
        {
            { "submit", "Your club has been submitted. An administrator will review it shortly." },
            { "submit_autoapproval", "Your club has been submitted and automatically approved." },
            { "resubmit", "Your club has been resubmitted for approval. An administrator will review your changes shortly. Note that repeated resubmissions could result in loss of access to this system." },
            { "resubmit_autoapproval", "Your changes have been saved and your club has been automatically approved." },
            { "edit_reapprove", "Your changes have been saved. An administrator will review these changes and reply accordingly." },
            { "edit", "Your changes have been saved." },
            { "edit_autoapproval", "Your changes have been saved and the club has been successfully approved." },
            { "undelete", "Your changes have been saved and the club has been undeleted. An administrator will review your club shortly." },
            { "undelete_autoapproval", "Your changes have been saved and the club has been undeleted and automatically approved." },
            { "delete_soft", "The club has been deleted successfully." },
            { "delete_hard", "The club has been successfully purged from the database." },
            { "approve", "The club has been successfully marked as approved and will be displayed." },
            { "deny", "The club has been sucessfully denied." }
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

        Dictionary<int, CheckBox> checkBoxes = new Dictionary<int, CheckBox>();

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!CurrentUser.SecurityAccess["CanAccessBackend"])
            {
                Response.Redirect("403.aspx", true);
            }

            NewClub.Visible = CurrentUser.SecurityAccess["CanSubmitClub"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["msg"] != null && messages.ContainsKey(Request.Params["msg"]))
            {
                Message.Visible = true;
                Message.InnerHtml = messages[Request.Params["msg"]];
            }

            PopulateModes();
            while (ClubTable.Rows.Count > 1)
            {
                ClubTable.Rows.RemoveAt(1);
            }
            Club.PopulatePageNumber(DatabaseManager.Current, CurrentUser.Profile, CurrentPage, MaxPage, ViewMode.SelectedValue, Page, 10);
            Club.PopulateClubTable(DatabaseManager.Current, CurrentUser.Profile, ViewMode.SelectedValue, ViewDeleted.Checked, ClubTable, (Page - 1) * 10, 10, checkBoxes);
        }

        private void PopulateModes()
        {
            if (ViewMode.Items.Count == 0)
            {
                if (CurrentUser.SecurityAccess["CanApproveClub"] && CurrentUser.SecurityAccess["CanViewAllClub"])
                    ViewMode.Items.Add(new ListItem("Approval Mode", "Approval"));
                if (CurrentUser.SecurityAccess["CanViewAllClub"])
                    ViewMode.Items.Add(new ListItem("View All Mode", "ViewAll"));
                ViewMode.Items.Add(new ListItem("Submission Mode", "Submission"));
                
            }
            else
            {
                if (ViewMode.SelectedValue == "ViewAll" && !CurrentUser.SecurityAccess["CanViewAllClub"])
                    ViewMode.SelectedIndex = 0;
                else if (ViewMode.SelectedValue == "Approval" && !CurrentUser.SecurityAccess["CanApproveClub"])
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
            List<Club> toDelete = new List<Club>();
            foreach (KeyValuePair<int, CheckBox> check in checkBoxes)
            {
                if (check.Value.Checked)
                    toDelete.Add(Club.FromDatabase(DatabaseManager.Current, check.Key));
            }

            if (toDelete.Count == 0)
            {
                Message.Visible = true;
                Message.Attributes["class"] = "message-base message-error";
                Message.InnerHtml = "Please select the clubs you would like to delete.";
                return;
            }

            if (!CurrentUser.SecurityAccess["CanEditAllClub"])
            {
                foreach (Club c in toDelete)
                {
                    if (c.CreatorId != CurrentUser.Profile.Id)
                    {
                        Message.Visible = true;
                        Message.Attributes["class"] = "message-base message-error";
                        Message.InnerHtml = "You cannot delete one or more of the selected clubs.";
                        return;
                    }
                }
            }

            foreach (Club c in toDelete)
            {
                c.Status = Club.ClubStatus.Deleted;
                c.StatusUserId = CurrentUser.Profile.Id;
                c.StatusTime = DateTime.Now;
                c.Update();
            }

            Message.Visible = true;
            Message.Attributes["class"] = "message-base message-success";
            Message.InnerHtml = "All selected clubs have been successfully deleted.";

            Page_Load(this, new EventArgs());
        }
    }
}