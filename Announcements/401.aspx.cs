using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Announcements
{
    public partial class _401 : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Response.StatusCode = 401;
            ((Main)this.Master).NoDatabase = true;
        }
    }
}