using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Announcements.Data;

namespace Announcements
{
    public partial class ClubInfo : AnnouncementsPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["id"] == null)
            {
                Response.Redirect("Default.aspx", true);
            }
            else
            {
                int id;
                try
                {
                    id = Int32.Parse(Request.Params["id"]);
                }
                catch (FormatException)
                {
                    Response.Redirect("Default.aspx", true);
                    return;
                }

                using (SqlCommand cmd = DatabaseManager.Current.CreateCommand("SELECT * FROM Clubs WHERE Id=@id AND Status=1"))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    Club c = null;
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        if (r.HasRows)
                        {
                            r.Read();
                            c = new Club(DatabaseManager.Current, r);
                        }
                    }
                    if (c != null)
                    {
                        ClubBox.Club = c;
                    }
                    else
                    {
                        Response.Redirect("Default.aspx", true);
                    }
                }
            }
        }
    }
}