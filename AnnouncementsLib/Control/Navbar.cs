using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Announcements.Data;

namespace Announcements.Control
{
    [ToolboxData("<{0}:Navbar runat=server></{0}:Navbar>")]
    public class Navbar : WebControl
    {
        protected override void RenderContents(HtmlTextWriter output)
        {
            output.Write("<div class=\"navbar\">");
            if (DesignMode)
            {
                output.Write("<a href=\"#\">Links</a><a href=\"#\">Go</a><a href=\"#\">Here</a>");
            }
            else
            {
                CompiledSecurityInfo level = CompiledSecurityInfo.CompileAccessLevel(DatabaseManager.Current, Page.User);
                string appPath = Page.Request.ApplicationPath;

                if (appPath == "/")
                    appPath = "";

                output.Write("<a href=\"" + appPath + "/Default.aspx\">Home</a>");
                if (level["CanAccessBackend"])
                    output.Write("<a href=\"" + appPath + "/Admin/Default.aspx\">Admin Panel</a>");

                string scopes = "";
                foreach (int scope in level.Scopes)
                {
                    scopes += scope + ",";
                }

                scopes = scopes.Remove(scopes.Length - 1);

                using (SqlCommand cmd = DatabaseManager.Current.CreateCommand("SELECT * FROM NavbarLinks WHERE Scope IS NULL OR Scope IN (" + scopes + ")"))
                {
                    using (SqlDataReader r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            NavbarLink link = new NavbarLink(r);
                            output.Write("<a href=\"" + link.Url + "\" target=\"_blank\">" + link.Text + "</a>");
                        }
                    }
                }
            }
            
            output.Write("</div>");
        }

        public class NavbarLink
        {
            public int Id { get; private set; }
            public string Text { get; set; }
            public string Url { get; set; }
            public int Scope { get; set; }

            public NavbarLink(SqlDataReader r)
            {
                Id = (int)r["Id"];
                Text = (string)r["Text"];
                Url = (string)r["URL"];
                Scope = (r["Scope"] is DBNull) ? 0 : (int)r["Scope"];
            }
        }
    }
}
