using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Announcements.Data;

namespace Announcements.Admin
{
    public partial class AnnouncementEdit : System.Web.UI.Page
    {
        User userInfo;
        Announcement info;

        protected void Page_Init(object sender, EventArgs e)
        {
            userInfo = new User(User);

            if (Request.Params["id"] != null)
            {
                try
                {
                    info = Announcement.FromDatabase(Int32.Parse(Request.Params["id"]));
                    if (info == null)
                        Response.Redirect("AnnouncementList.aspx", true);
                }
                catch (FormatException)
                {
                    Response.Redirect("AnnouncementList.aspx", true);
                }
            }
            else
            {
                info = new Announcement();
            }

            if (!userInfo.SecurityAccess["CanAccessBackend"])
            {
                Response.Redirect("403.aspx", true);
            }

            if (info.Id > 0 && info.CreatorId != userInfo.Profile.Id && !userInfo.SecurityAccess["CanViewAllAnnouncement"])
                Response.Redirect("AnnouncementList.aspx", true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod != "POST")
                PopulateFields();

            CheckEditPermissions();
            PopulateInfo();
        }

        private void PopulateInfo()
        {
            if (info.Id > 0)
            {
                CreatorName.Text = info.CreatorDisplayName;
                CreatedTime.Text = info.CreateTime.ToString("dd/MM/yyyy hh:mmtt", CultureInfo.InvariantCulture);
                if (info.EditorId > 0)
                {
                    Editor.Visible = true;
                    EditorName.Text = info.EditorDisplayName;
                    EditedTime.Text = info.EditTime.ToString("dd/MM/yyyy hh:mmtt", CultureInfo.InvariantCulture);
                }
                else
                {
                    Editor.Visible = false;
                }

                switch (info.Status)
                {
                    case Announcement.AnnouncementStatus.Pending:
                        StatusLabel.Text = "Announcement is <strong>Pending Approval</strong>";
                        break;
                    case Announcement.AnnouncementStatus.Approved:
                        StatusLabel.Text = "Announcement was approved by <strong>" + info.StatusUserDisplayName + "</strong> at <strong>" + info.StatusTime.ToString("dd/MM/yyyy hh:mmtt", CultureInfo.InvariantCulture) + "</strong>.";
                        break;
                    case Announcement.AnnouncementStatus.Denied:
                        StatusLabel.Text = "Announcement was denied by <strong>" + info.StatusUserDisplayName + "</strong> at <strong>" + info.StatusTime.ToString("dd/MM/yyyy hh:mmtt", CultureInfo.InvariantCulture) + "</strong>. (<strong>" + info.StatusMessage + "</strong>)";
                        break;
                    case Announcement.AnnouncementStatus.Deleted:
                        StatusLabel.Text = "Announcement was deleted by <strong>" + info.StatusUserDisplayName + "</strong> at <strong>" + info.StatusTime.ToString("dd/MM/yyyy hh:mmtt", CultureInfo.InvariantCulture) + "</strong>.";
                        break;
                }
            }
            else
            {
                StatusInfobox.Visible = false;
            }
        }

        private void CheckEditPermissions()
        {
            SubmitLink.Visible = (info.Id <= 0 || info.CreatorId == userInfo.Profile.Id || userInfo.SecurityAccess["CanEditAllAnnouncement"]);
            SubmitAndApproveLink.Visible = SubmitLink.Visible && userInfo.SecurityAccess["CanApproveAnnouncement"] && (info.Status == Announcement.AnnouncementStatus.Pending || info.Status == Announcement.AnnouncementStatus.Denied || info.Status == Announcement.AnnouncementStatus.Deleted);
            DeleteLink.Visible = (info.Id > 0 && SubmitLink.Visible);

            ImportanceContainer.Visible = userInfo.SecurityAccess["CanAdvancedEditAnnouncement"];

            AnnouncementTitle.ReadOnly = !SubmitLink.Visible;
            AnnouncementBody.ReadOnly = !SubmitLink.Visible;
            StartDate.ReadOnly = !SubmitLink.Visible;
            StartDateCalendar.Enabled = SubmitLink.Visible;
            EndDate.ReadOnly = !SubmitLink.Visible;
            EndDateCalendar.Enabled = SubmitLink.Visible;
            Scope.Enabled = SubmitLink.Visible;
            Importance.Enabled = SubmitLink.Visible;

            AdminInfobox.Visible = (info.Id > 0 && (userInfo.SecurityAccess["CanApproveAnnouncement"] || userInfo.SecurityAccess["CanHardDeleteAnnouncement"]));
            ApproveDeny.Visible = userInfo.SecurityAccess["CanApproveAnnouncement"];
            HardDelete.Visible = userInfo.SecurityAccess["CanHardDeleteAnnouncement"] && SubmitLink.Visible;
        }

        private void PopulateFields()
        {
            AnnouncementTitle.Text = info.Title;
            AnnouncementBody.Text = info.Body;
            StartDate.Text = info.StartDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            EndDate.Text = info.EndDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            PopulateScope();
            Importance.SelectedIndex = Importance.Items.Count - 1 - info.Importance;
        }

        private void PopulateScope()
        {
            foreach (Scope s in Data.Scope.AllFromDatabase())
            {
                Scope.Items.Add(new ListItem(s.Name, s.Id.ToString()));
                if (info.ScopeId == s.Id)
                    Scope.SelectedIndex = Scope.Items.Count - 1;
            }
        }

        private void ShowMessage(string message, string cssClass)
        {
            Message.Visible = true;
            Message.Attributes["class"] = "message-base " + cssClass;
            Message.InnerHtml = message;
        }

        private bool UpdateAnnouncement()
        {
            info.Title = Sanitizer.Sanitize(AnnouncementTitle.Text, Sanitizer.NoHtml);
            info.Body = Sanitizer.Sanitize(AnnouncementBody.Text, Sanitizer.StrictRules);
            try
            {
                info.StartDate = DateTime.ParseExact(StartDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                ShowMessage(StartDate.Text + " is not a valid start date. Make sure that the date is in the format DD/MM/YYYY.", "message-error");
                return false;
            }
            try
            {
                info.EndDate = DateTime.ParseExact(EndDate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            catch (FormatException)
            {
                ShowMessage(EndDate.Text + " is not a valid end date. Make sure that the date is in the format DD/MM/YYYY.", "message-error");
                return false;
            }
            try
            {
                info.ScopeId = Int32.Parse(Scope.SelectedValue);
            }
            catch (FormatException)
            {
                ShowMessage("An invalid scope was passed. Please contact an administrator for further assistance.", "message-error");
                return false;
            }
            try
            {
                info.Importance = Int32.Parse(Importance.SelectedValue);
            }
            catch (FormatException)
            {
                ShowMessage("An invalid importance was passed. Please contact an administrator for further assistance.", "message-error");
                return false;
            }
            return true;
        }

        protected void SubmitLink_Click(object sender, EventArgs e)
        {
            if (UpdateAnnouncement())
            {
                if (info.Id > 0)
                {
                    if (info.CreatorId != userInfo.Profile.Id && !userInfo.SecurityAccess["CanEditAllAnnouncement"])
                    {
                        ShowMessage("You have not been granted access to edit this announcement. Please contact an administrator for assistance.", "message-error");
                    }
                    else
                    {
                        Announcement.AnnouncementStatus status = info.Status;

                        if (status == Announcement.AnnouncementStatus.Deleted || status == Announcement.AnnouncementStatus.Denied)
                        {
                            info.Status = Announcement.AnnouncementStatus.Pending;
                        }
                        else if (status == Announcement.AnnouncementStatus.Approved && !userInfo.SecurityAccess["CanApproveAnnouncement"])
                        {
                            info.Status = Announcement.AnnouncementStatus.Pending;
                        }

                        info.EditorId = userInfo.Profile.Id;
                        info.EditTime = DateTime.Now;
                        info.Update();

                        if (status == Announcement.AnnouncementStatus.Approved && !userInfo.SecurityAccess["CanApproveAnnouncement"])
                            Response.Redirect("AnnouncementList.aspx?msg=edit_reapprove", true);
                        else if (status == Announcement.AnnouncementStatus.Denied)
                            Response.Redirect("AnnouncementList.aspx?msg=resubmit", true);
                        else if (status == Announcement.AnnouncementStatus.Deleted)
                            Response.Redirect("AnnouncementList.aspx?msg=undelete", true);
                        else
                            Response.Redirect("AnnouncementList.aspx?msg=edit", true);
                    }
                }
                else
                {
                    if (!userInfo.SecurityAccess["CanSubmitAnnouncement"])
                    {
                        ShowMessage("You have not been granted access to submit announcements. Please contact an administrator for assistance.", "message-error");
                    }
                    else
                    {
                        info.CreatorId = userInfo.Profile.Id;
                        info.CreateTime = DateTime.Now;
                        info.Insert();
                        Response.Redirect("AnnouncementList.aspx?msg=submit", true);
                    }
                }
            }
        }

        protected void SubmitAndApproveLink_Click(object sender, EventArgs e)
        {
            if (!userInfo.SecurityAccess["CanApproveAnnouncement"])
            {
                ShowMessage("You don't have access to approve announcements.", "message-error");
                return;
            }
            if (UpdateAnnouncement())
            {
                if (info.Id > 0)
                {
                    if (info.CreatorId != userInfo.Profile.Id && !userInfo.SecurityAccess["CanEditAllAnnouncement"])
                    {
                        ShowMessage("You have not been granted access to edit this announcement. Please contact an administrator for assistance.", "message-error");
                    }
                    else
                    {
                        Announcement.AnnouncementStatus status = info.Status;
                        info.Status = Announcement.AnnouncementStatus.Approved;

                        info.EditorId = userInfo.Profile.Id;
                        info.EditTime = DateTime.Now;
                        info.StatusUserId = userInfo.Profile.Id;
                        info.StatusTime = DateTime.Now;
                        info.Update();

                        if (status == Announcement.AnnouncementStatus.Pending || status == Announcement.AnnouncementStatus.Approved)
                            Response.Redirect("AnnouncementList.aspx?msg=edit_autoapproval", true);
                        else if (status == Announcement.AnnouncementStatus.Denied)
                            Response.Redirect("AnnouncementList.aspx?msg=resubmit_autoapproval", true);
                        else if (status == Announcement.AnnouncementStatus.Deleted)
                            Response.Redirect("AnnouncementList.aspx?msg=undelete_autoapproval", true);
                    }
                }
                else
                {
                    if (!userInfo.SecurityAccess["CanSubmitAnnouncement"])
                    {
                        ShowMessage("You have not been granted access to submit announcements. Please contact an administrator for assistance.", "message-error");
                    }
                    else
                    {
                        info.Status = Announcement.AnnouncementStatus.Approved;
                        info.CreatorId = userInfo.Profile.Id;
                        info.CreateTime = DateTime.Now;
                        info.StatusUserId = userInfo.Profile.Id;
                        info.StatusTime = DateTime.Now;
                        info.Insert();
                        Response.Redirect("AnnouncementList.aspx?msg=submit_autoapproval", true);
                    }
                }
            }
        }

        protected void DeleteLink_Click(object sender, EventArgs e)
        {
            info.Status = Announcement.AnnouncementStatus.Deleted;
            info.StatusTime = DateTime.Now;
            info.StatusUserId = userInfo.Profile.Id;
            info.Update();
            Response.Redirect("AnnouncementList.aspx?msg=delete_soft", true);
        }

        protected void HardDeleteLink_Click(object sender, EventArgs e)
        {
            if (!SubmitLink.Visible || !userInfo.SecurityAccess["CanHardDeleteAnnouncement"])
            {
                ShowMessage("You have not been granted access to permanently delete announcements!", "message-error");
            }
            else
            {
                if (HardDeleteCheck.Checked)
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Announcements WHERE Id=@id", DatabaseManager.DatabaseConnection);
                    cmd.Parameters.AddWithValue("@id", info.Id);
                    cmd.ExecuteNonQuery();
                    Response.Redirect("AnnouncementList.aspx?msg=delete_hard", true);
                }
                else
                {
                    ShowMessage("Please check the confirmation box before hard deleting the announcement", "message-error");
                }
            }
        }

        protected void ApproveLink_Click(object sender, EventArgs e)
        {
            if (!userInfo.SecurityAccess["CanApproveAnnouncement"])
            {
                ShowMessage("You have not been granted access to approve announcements.", "message-error");
            }
            else if (info.Status == Announcement.AnnouncementStatus.Approved)
            {
                ShowMessage("That announcement has already been approved.", "message-error");
            }
            else
            {
                info.Status = Announcement.AnnouncementStatus.Approved;
                info.StatusTime = DateTime.Now;
                info.StatusUserId = userInfo.Profile.Id;
                info.Update();
                Response.Redirect("AnnouncementList.aspx?msg=approve", true);
            }
        }

        protected void DenyLink_Click(object sender, EventArgs e)
        {
            if (!userInfo.SecurityAccess["CanApproveAnnouncement"])
            {
                ShowMessage("You have not been granted access to deny announcements.", "message-error");
            }
            else if (info.Status == Announcement.AnnouncementStatus.Denied)
            {
                ShowMessage("That announcement has already been denied.", "message-error");
            }
            else if (DenyReason.Text == String.Empty)
            {
                ShowMessage("Please enter a reason for denying the announcement.", "message-error");
            }
            else
            {
                info.Status = Announcement.AnnouncementStatus.Denied;
                info.StatusMessage = DenyReason.Text;
                info.StatusTime = DateTime.Now;
                info.StatusUserId = userInfo.Profile.Id;
                info.Update();
                Response.Redirect("AnnouncementList.aspx?msg=deny", true);
            }
        }
    }
}