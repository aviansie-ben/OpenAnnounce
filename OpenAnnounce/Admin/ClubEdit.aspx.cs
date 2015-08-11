using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenAnnounce.Data;

namespace OpenAnnounce.Admin
{
    public partial class ClubEdit : AnnouncementsPage
    {
        Club info;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (Request.Params["id"] != null)
            {
                try
                {
                    info = Club.FromDatabase(DatabaseManager.Current, Int32.Parse(Request.Params["id"]));
                    if (info == null)
                        Response.Redirect("ClubList.aspx", true);
                }
                catch (FormatException)
                {
                    Response.Redirect("ClubList.aspx", true);
                }
            }
            else
            {
                info = new Club(DatabaseManager.Current);
            }

            if (!CurrentUser.SecurityAccess["CanAccessBackend"])
            {
                Response.Redirect("403.aspx", true);
            }

            if (info.Id > 0 && info.CreatorId != CurrentUser.Profile.Id && !CurrentUser.SecurityAccess["CanViewAllClub"])
                Response.Redirect("ClubList.aspx", true);
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
                CreatedTime.Text = info.CreateTime.ToString("dd/MM/yyyy hh:mmtt");
                if (info.EditorId > 0)
                {
                    Editor.Visible = true;
                    EditorName.Text = info.EditorDisplayName;
                    EditedTime.Text = info.EditTime.ToString("dd/MM/yyyy hh:mmtt");
                }
                else
                {
                    Editor.Visible = false;
                }

                switch (info.Status)
                {
                    case Club.ClubStatus.Pending:
                        StatusLabel.Text = "Club is <strong>Pending Approval</strong>";
                        break;
                    case Club.ClubStatus.Approved:
                        StatusLabel.Text = "Club was approved by <strong>" + info.StatusUserDisplayName + "</strong> at <strong>" + info.StatusTime.ToString("dd/MM/yyyy hh:mmtt") + "</strong>.";
                        break;
                    case Club.ClubStatus.Denied:
                        StatusLabel.Text = "Club was denied by <strong>" + info.StatusUserDisplayName + "</strong> at <strong>" + info.StatusTime.ToString("dd/MM/yyyy hh:mmtt") + "</strong>. (<strong>" + info.StatusMessage + "</strong>)";
                        break;
                    case Club.ClubStatus.Deleted:
                        StatusLabel.Text = "Club was deleted by <strong>" + info.StatusUserDisplayName + "</strong> at <strong>" + info.StatusTime.ToString("dd/MM/yyyy hh:mmtt") + "</strong>.";
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
            SubmitLink.Visible = (info.Id <= 0 || info.CreatorId == CurrentUser.Profile.Id || CurrentUser.SecurityAccess["CanEditAllClub"]);
            SubmitAndApproveLink.Visible = SubmitLink.Visible && CurrentUser.SecurityAccess["CanApproveClub"] && (info.Status == Club.ClubStatus.Pending || info.Status == Club.ClubStatus.Denied || info.Status == Club.ClubStatus.Deleted);
            DeleteLink.Visible = (info.Id > 0 && SubmitLink.Visible);
            EditProfile.Visible = CurrentUser.SecurityAccess["CanEditProfiles"];

            ClubName.ReadOnly = !SubmitLink.Visible;
            ClubDescription.ReadOnly = !SubmitLink.Visible;
            Teacher.ReadOnly = !SubmitLink.Visible;
            Weekday.Enabled = SubmitLink.Visible;
            AfterSchool.Enabled = SubmitLink.Visible;
            Location.ReadOnly = !SubmitLink.Visible;

            AdminInfobox.Visible = (info.Id > 0 && (CurrentUser.SecurityAccess["CanApproveClub"] || CurrentUser.SecurityAccess["CanHardDeleteClub"]));
            ApproveDeny.Visible = CurrentUser.SecurityAccess["CanApproveClub"];
            HardDelete.Visible = CurrentUser.SecurityAccess["CanHardDeleteClub"] && SubmitLink.Visible;
        }

        private void PopulateFields()
        {
            UserProfile teacher = UserProfile.FromDatabase(DatabaseManager.Current, info.TeacherId);

            ClubName.Text = HttpUtility.HtmlDecode(info.Name);
            ClubDescription.Text = info.Description;
            Teacher.Text = (teacher == null) ? "" : teacher.Username;
            Weekday.SelectedIndex = info.Weekday;
            AfterSchool.Checked = info.AfterSchool;
            Location.Text = HttpUtility.HtmlDecode(info.Location);
        }

        private void ShowMessage(string message, string cssClass)
        {
            Message.Visible = true;
            Message.Attributes["class"] = "message-base " + cssClass;
            Message.InnerHtml = message;
        }

        private bool UpdateClub()
        {
            info.Name = HttpUtility.HtmlEncode(ClubName.Text);
            info.Description = Sanitizer.Sanitize(ClubDescription.Text, Sanitizer.StrictRules);

            UserProfile teacher = UserProfile.FromDatabase(DatabaseManager.Current, Teacher.Text, false);

            if (teacher == null)
            {
                ShowMessage("The user specified as the teacher in charge does not exist. To create a new user entry, click on the \"Create/Edit Profile\" button.", "message-error");
                return false;
            }
            info.TeacherId = teacher.Id;

            try
            {
                info.Weekday = Int32.Parse(Weekday.SelectedValue);
            }
            catch (FormatException)
            {
                ShowMessage("An invalid weekday was passed. Please contact an administrator for further assistance.", "message-error");
                return false;
            }
            info.AfterSchool = AfterSchool.Checked;
            info.Location = HttpUtility.HtmlEncode(Location.Text);
            return true;
        }

        protected void SubmitLink_Click(object sender, EventArgs e)
        {
            if (UpdateClub())
            {
                if (info.Id > 0)
                {
                    if (info.CreatorId != CurrentUser.Profile.Id && !CurrentUser.SecurityAccess["CanEditAllClub"])
                    {
                        ShowMessage("You have not been granted access to edit this club. Please contact an administrator for assistance.", "message-error");
                    }
                    else
                    {
                        Club.ClubStatus status = info.Status;

                        if (status == Club.ClubStatus.Deleted || status == Club.ClubStatus.Denied)
                        {
                            info.Status = Club.ClubStatus.Pending;
                        }
                        else if (status == Club.ClubStatus.Approved && !CurrentUser.SecurityAccess["CanApproveClub"])
                        {
                            info.Status = Club.ClubStatus.Pending;
                        }

                        info.EditorId = CurrentUser.Profile.Id;
                        info.EditTime = DateTime.Now;
                        info.Update();

                        if (status == Club.ClubStatus.Approved && !CurrentUser.SecurityAccess["CanApproveClub"])
                            Response.Redirect("ClubList.aspx?msg=edit_reapprove", true);
                        else if (status == Club.ClubStatus.Denied)
                            Response.Redirect("ClubList.aspx?msg=resubmit", true);
                        else if (status == Club.ClubStatus.Deleted)
                            Response.Redirect("ClubList.aspx?msg=undelete", true);
                        else
                            Response.Redirect("ClubList.aspx?msg=edit", true);
                    }
                }
                else
                {
                    if (!CurrentUser.SecurityAccess["CanSubmitClub"])
                    {
                        ShowMessage("You have not been granted access to submit clubs. Please contact an administrator for assistance.", "message-error");
                    }
                    else
                    {
                        info.CreatorId = CurrentUser.Profile.Id;
                        info.CreateTime = DateTime.Now;
                        info.Insert();
                        Response.Redirect("ClubList.aspx?msg=submit", true);
                    }
                }
            }
        }

        protected void SubmitAndApproveLink_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.SecurityAccess["CanApproveClub"])
            {
                ShowMessage("You don't have access to approve clubs.", "message-error");
                return;
            }

            if (UpdateClub())
            {
                if (info.Id > 0)
                {
                    if (info.CreatorId != CurrentUser.Profile.Id && !CurrentUser.SecurityAccess["CanEditAllClub"])
                    {
                        ShowMessage("You have not been granted access to edit this club. Please contact an administrator for assistance.", "message-error");
                    }
                    else
                    {
                        Club.ClubStatus status = info.Status;
                        info.Status = Club.ClubStatus.Approved;

                        info.EditorId = CurrentUser.Profile.Id;
                        info.EditTime = DateTime.Now;
                        info.StatusUserId = CurrentUser.Profile.Id;
                        info.StatusTime = DateTime.Now;
                        info.Update();

                        if (status == Club.ClubStatus.Pending || status == Club.ClubStatus.Approved)
                            Response.Redirect("ClubList.aspx?msg=edit_autoapproval", true);
                        else if (status == Club.ClubStatus.Denied)
                            Response.Redirect("ClubList.aspx?msg=resubmit_autoapproval", true);
                        else if (status == Club.ClubStatus.Deleted)
                            Response.Redirect("ClubList.aspx?msg=undelete_autoapproval", true);
                    }
                }
                else
                {
                    if (!CurrentUser.SecurityAccess["CanSubmitClub"])
                    {
                        ShowMessage("You have not been granted access to submit clubs. Please contact an administrator for assistance.", "message-error");
                    }
                    else
                    {
                        info.Status = Club.ClubStatus.Approved;
                        info.CreatorId = CurrentUser.Profile.Id;
                        info.CreateTime = DateTime.Now;
                        info.StatusUserId = CurrentUser.Profile.Id;
                        info.StatusTime = DateTime.Now;
                        info.Insert();
                        Response.Redirect("ClubList.aspx?msg=submit_autoapproval", true);
                    }
                }
            }
        }

        protected void DeleteLink_Click(object sender, EventArgs e)
        {
            info.Status = Club.ClubStatus.Deleted;
            info.StatusTime = DateTime.Now;
            info.StatusUserId = CurrentUser.Profile.Id;
            info.Update();
            Response.Redirect("ClubList.aspx?msg=delete_soft", true);
        }

        protected void HardDeleteLink_Click(object sender, EventArgs e)
        {
            if (!SubmitLink.Visible || !CurrentUser.SecurityAccess["CanHardDeleteClub"])
            {
                ShowMessage("You have not been granted access to permanently delete clubs!", "message-error");
            }
            else
            {
                if (HardDeleteCheck.Checked)
                {
                    using (SqlCommand cmd = DatabaseManager.Current.CreateCommand("DELETE FROM Clubs WHERE Id=@id"))
                    {
                        cmd.Parameters.AddWithValue("@id", info.Id);
                        cmd.ExecuteNonQuery();
                        Response.Redirect("ClubList.aspx?msg=delete_hard", true);
                    }
                }
                else
                {
                    ShowMessage("Please check the confirmation box before hard deleting the club", "message-error");
                }
            }
        }

        protected void ApproveLink_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.SecurityAccess["CanApproveClub"])
            {
                ShowMessage("You have not been granted access to approve clubs.", "message-error");
            }
            else if (info.Status == Club.ClubStatus.Approved)
            {
                ShowMessage("That club has already been approved.", "message-error");
            }
            else
            {
                info.Status = Club.ClubStatus.Approved;
                info.StatusTime = DateTime.Now;
                info.StatusUserId = CurrentUser.Profile.Id;
                info.Update();
                Response.Redirect("ClubList.aspx?msg=approve", true);
            }
        }

        protected void DenyLink_Click(object sender, EventArgs e)
        {
            if (!CurrentUser.SecurityAccess["CanApproveClub"])
            {
                ShowMessage("You have not been granted access to deny clubs.", "message-error");
            }
            else if (info.Status == Club.ClubStatus.Denied)
            {
                ShowMessage("That club has already been denied.", "message-error");
            }
            else if (DenyReason.Text == String.Empty)
            {
                ShowMessage("Please enter a reason for denying the club.", "message-error");
            }
            else
            {
                info.Status = Club.ClubStatus.Denied;
                info.StatusMessage = DenyReason.Text;
                info.StatusTime = DateTime.Now;
                info.StatusUserId = CurrentUser.Profile.Id;
                info.Update();
                Response.Redirect("ClubList.aspx?msg=deny", true);
            }
        }

        protected void EditProfile_Click(object sender, EventArgs e)
        {
            if (Teacher.Text.Length > 0)
            {
                UserProfile u = UserProfile.FromDatabase(DatabaseManager.Current, Teacher.Text, true);

                Response.Redirect("ProfileEdit.aspx?id=" + u.Id);
            }
        }
    }
}