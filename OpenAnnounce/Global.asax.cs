using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace OpenAnnounce
{
    public class Global : System.Web.HttpApplication
    {
        public Global()
        {
            this.EndRequest += Global_EndRequest;
        }

        private void Global_EndRequest(object sender, EventArgs e)
        {
            if (Response.StatusCode == 401 && !Request.IsAuthenticated)
            {
                Response.ClearContent();
                Server.Execute("~/401.aspx");
            }
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Data.SecurityInfo.RegisterPermissions(new List<string>() { "CanAccessBackend", "CanEditNavbar", "CanSetPermissions", "CanEditProfiles" });
            Data.SecurityInfo.RegisterPermissions(new List<string>() { "CanSubmitAnnouncement", "CanViewAllAnnouncement", "CanEditAllAnnouncement", "CanAdvancedEditAnnouncement", "CanApproveAnnouncement", "CanHardDeleteAnnouncement" });
            Data.SecurityInfo.RegisterPermissions(new List<string>() { "CanSubmitClub", "CanViewAllClub", "CanEditAllClub", "CanApproveClub", "CanHardDeleteClub" });
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            HttpException ex = Server.GetLastError() as HttpException;

            if (ex != null && ex.GetHttpCode() == 500)
            {
                // Display the error page and clear the error
                Response.ClearContent();
                Server.Execute("~/500.aspx");
                Server.ClearError();

                // Reset the database connection
                Data.DatabaseManager.CloseConnection();
            }
            else if (ex != null && ex.GetHttpCode() == 503)
            {
                // Display the error page and clear the error
                Response.ClearContent();
                Server.Execute("~/503.aspx");
                Server.ClearError();

                // Reset the database connection
                Data.DatabaseManager.CloseConnection();
            }
            else if (ex != null && ex.GetHttpCode() == 404)
            {
                // Display the error page and clear the error
                Response.ClearContent();
                Server.Execute("~/404.aspx");
                Server.ClearError();

                // Reset the database connection
                Data.DatabaseManager.CloseConnection();
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            
        }
    }
}