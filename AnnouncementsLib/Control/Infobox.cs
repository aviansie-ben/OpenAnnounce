using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Announcements.Control
{
    [DefaultProperty("Text")]
    [ToolboxData("<{0}:Infobox runat=server></{0}:Infobox>")]
    [ParseChildren(false)]
    public class Infobox : WebControl
    {
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Title
        {
            get
            {
                String s = (String)ViewState["Title"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Title"] = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return ((s == null) ? String.Empty : s);
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue(false)]
        [Localizable(true)]
        public bool SmallText
        {
            get
            {
                object b = ViewState["SmallText"];
                return ((b == null) ? false : (bool)b);
            }

            set
            {
                ViewState["SmallText"] = value;
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write("<div class=\"infobox-base\">");
            if (Title != String.Empty)
                output.Write("<div class=\"infobox-title\">" + Title + "</div>");
            output.Write("<div class=\"infobox-body" + ((SmallText) ? " infobox-small-text" : "") + "\">");
            output.Write(Text);
            base.Render(output);
            output.Write("</div></div>");
        }
    }
}
