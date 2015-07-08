using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Announcements
{
    public partial class _503 : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Response.StatusCode = 503;
            ((Main)this.Master).NoDatabase = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex != null)
            {
                this.ErrorBox.Controls.Add(new Literal()
                {
                    Text = "<h1>Error details</h1>"
                });
                this.ErrorBox.Controls.Add(new TextBox()
                {
                    Text = ex.ToString(),
                    Rows = 10,
                    Columns = 100,
                    ReadOnly = true,
                    TextMode = TextBoxMode.MultiLine,
                    Wrap = false
                });

                ex = null;
            }
        }
    }
}