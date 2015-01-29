using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Announcements.Data;
using System.Data;

namespace Announcements
{
    public partial class Main : System.Web.UI.MasterPage
    {
        public bool NoDatabase { get; set; }

        public Main()
        {
            NoDatabase = false;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();

            if (NoDatabase)
            {
                this.Controls.Remove(HeadNavbar);
            }
            else
            {
                try
                {
                    if (DatabaseManager.DatabaseConnection != null && (DatabaseManager.DatabaseConnection.State == ConnectionState.Broken || DatabaseManager.DatabaseConnection.State == ConnectionState.Closed))
                        DatabaseManager.CloseConnection();
                    if (DatabaseManager.DatabaseConnection == null)
                        DatabaseManager.OpenConnection(ConfigurationManager.ConnectionStrings["mainDb"].ConnectionString);
                }
                catch (Exception ex)
                {
                    Response.ClearContent();
                    Context.Items["503Exception"] = ex;
                    Server.Execute("~/503.aspx");
                    Response.Output.WriteLine("<!-- Error is as follows:\n" + ex.ToString() + " -->");
                    Response.End();
                }
            }
        }
    }
}