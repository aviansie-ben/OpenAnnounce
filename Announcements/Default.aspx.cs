using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Announcements.Data;

namespace Announcements
{
    public partial class Default : System.Web.UI.Page
    {
        private User userInfo;

        protected void Page_Load(object sender, EventArgs e)
        {
            userInfo = new User(User);

            string scopes = "";
            foreach (int scope in userInfo.SecurityAccess.Scopes)
            {
                scopes += scope + ",";
            }

            scopes = scopes.Remove(scopes.Length - 1);

            SqlCommand cmd = new SqlCommand("SELECT * FROM Announcements WHERE StartDate<=@today AND EndDate>=@today AND Status=1 AND (Scope IS NULL OR Scope IN (" + scopes + ")) ORDER BY IMPORTANCE DESC, StartDate DESC", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@today", DateTime.Today);
            using (SqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read() && r.HasRows)
                {
                    Announcements.Controls.Add(new Control.AnnouncementInfobox(new Announcement(r)));
                }
            }
        }
    }
}