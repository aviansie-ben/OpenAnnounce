using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Announcements.Control
{
    [ToolboxData("<{0}:AnnouncementTable runat=server />")]
    public class AnnouncementTable : WebControl
    {
        [Category("Appearance")]
        [DefaultValue(true)]
        public bool CheckBoxColumn { get; set; }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool TitleColumn { get; set; }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool SubmitterColumn { get; set; }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool RunningDatesColumn { get; set; }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool ScopeColumn { get; set; }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool StatusColumn { get; set; }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool ButtonColumn { get; set; }

        [Category("Appearance")]
        [DefaultValue(true)]
        public bool EditButton { get; set; }
    }
}
