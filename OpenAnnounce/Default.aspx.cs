using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OpenAnnounce.Data;

namespace OpenAnnounce
{
    public partial class Default : AnnouncementsPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string scopes = "";
            foreach (int scope in CurrentUser.SecurityAccess.Scopes)
            {
                scopes += scope + ",";
            }

            scopes = scopes.Remove(scopes.Length - 1);

            using (SqlCommand cmd = DatabaseManager.Current.CreateCommand("SELECT * FROM Announcements WHERE StartDate<=@today AND EndDate>=@today AND Status=1 AND (Scope IS NULL OR Scope IN (" + scopes + ")) ORDER BY IMPORTANCE DESC, StartDate DESC"))
            {
                cmd.Parameters.AddWithValue("@today", DateTime.Today);
                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    while (r.Read() && r.HasRows)
                    {
                        Announcements.Controls.Add(new Control.AnnouncementInfobox(new Announcement(DatabaseManager.Current, r)));
                    }
                }
            }
        }
    }
}