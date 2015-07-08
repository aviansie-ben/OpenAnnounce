using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Announcements.Data;

namespace Announcements.Admin
{
    public partial class ProfileEdit : AnnouncementsPage
    {
        UserProfile editProfile;

        private int id;

        protected void Page_Init(object sender, EventArgs e)
        {
            if (!CurrentUser.SecurityAccess["CanAccessBackend"])
            {
                Response.Redirect("403.aspx", true);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod != "POST")
                ViewState["id"] = (Request.Params["id"] != null) ? Int32.Parse(Request.Params["id"]) : CurrentUser.Profile.Id;

            if (ViewState["id"] == null)
                Response.Redirect("Default.aspx", true);
            else
                id = (int)ViewState["id"];

            if (!CurrentUser.SecurityAccess["CanEditProfiles"] && id != CurrentUser.Profile.Id)
                Response.Redirect("Default.aspx", true);

            editProfile = UserProfile.FromDatabase(DatabaseManager.Current, id);
            if (editProfile == null)
                Response.Redirect("Default.aspx", true);

            ProfileBox.Title = "Edit Profile: " + editProfile.Username;
            Username.Text = editProfile.Username;
            if (Request.HttpMethod != "POST")
                DisplayName.Text = editProfile.DisplayName;
        }

        private void ShowMessage(string message, string cssClass)
        {
            Message.Visible = true;
            Message.Attributes["class"] = "message-base " + cssClass;
            Message.InnerHtml = message;
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            editProfile.DisplayName = DisplayName.Text;
            editProfile.Update();
            ShowMessage((editProfile.Id == CurrentUser.Profile.Id) ? "Your profile has been successfully updated." : "That user's profile has been successfully updated.", "message-success");
        }
    }
}