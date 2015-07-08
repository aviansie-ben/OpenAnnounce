using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using Announcements.Data;

namespace Announcements.Control
{
    [DefaultProperty("Description")]
    [ToolboxData("<{0}:ClubListInfobox runat=server></{0}:ClubListInfobox>")]
    [ParseChildren(false)]
    public class ClubListInfobox : Infobox
    {
        public readonly string[] dayNames = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };

        public string DescriptionText { get; set; }
        public bool ShowAll { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            Text = (DescriptionText == null) ? String.Empty : (DescriptionText + "<br />");

            using (SqlCommand cmd = DatabaseManager.Current.CreateCommand("SELECT * FROM Clubs WHERE Weekday=@weekday AND status=1 ORDER BY Name ASC"))
            {
                if (ShowAll)
                {
                    for (int i = 0; i < 7; i++)
                    {
                        if (cmd.Parameters.Contains("@weekday"))
                            cmd.Parameters.RemoveAt("@weekday");
                        cmd.Parameters.AddWithValue("@weekday", i);
                        using (SqlDataReader r = cmd.ExecuteReader())
                        {
                            if (r.HasRows)
                            {
                                Text += "<h2>" + dayNames[i] + "</h2><ul>";
                                while (r.Read())
                                {
                                    Club c = new Club(DatabaseManager.Current, r);
                                    Text += "<li><a href=\"ClubInfo.aspx?id=" + c.Id + "\">" + c.Name + " - " + c.Location + ((c.AfterSchool) ? " (After school)" : "") + "</a></li>";
                                }
                                Text += "</ul>";
                            }
                        }
                    }
                }
                else
                {
                    cmd.Parameters.AddWithValue("@weekday", DateTime.Today.DayOfWeek);
                    Text += "<ul>";
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                Club c = new Club(DatabaseManager.Current, r);
                                Text += "<li><a href=\"ClubInfo.aspx?id=" + c.Id + "\">" + c.Name + " - " + c.Location + ((c.AfterSchool) ? " (After school)" : "") + "</a></li>";
                            }
                        }
                        else
                        {
                            Text += "<li><em>No clubs are running today</em></li>";
                        }
                    }
                    Text += "</ul><a href=\"ClubList.aspx\">See all clubs</a>";
                }
            }
        }
    }
}