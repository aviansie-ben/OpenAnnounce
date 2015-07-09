using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Announcements.Data;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Announcements
{
    public partial class Main : System.Web.UI.MasterPage
    {
        public bool NoDatabase { get; set; }
        public IsolationLevel Isolation { get; set; }

        public User RequestingUser { get; set; }

        public Main()
        {
            NoDatabase = false;
            Isolation = IsolationLevel.RepeatableRead;
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
                    DatabaseManager.OpenConnection(ConfigurationManager.ConnectionStrings["mainDb"].ConnectionString, Isolation);
                }
                catch (Exception ex)
                {
                    throw new HttpException(503, "Failed to connect to the database", ex);
                }

                RequestingUser = new User(DatabaseManager.Current, HttpContext.Current.User);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.DataBind();
        }

        protected void Page_Error(object sender, EventArgs e)
        {
            
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            if (DatabaseManager.Current != null)
                DatabaseManager.CloseConnection();
        }

        public static void DisplaySuccessMessage(HtmlGenericControl control, string message)
        {
            control.Visible = true;
            control.Attributes["class"] = "message-base message-success";
            control.InnerHtml = message;
        }

        public static void DisplayErrorMessage(HtmlGenericControl control, string message)
        {
            control.Visible = true;
            control.Attributes["class"] = "message-base message-error";
            control.InnerHtml = message;
        }
    }
}