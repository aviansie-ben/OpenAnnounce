using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenAnnounce.Control
{
    public class AnnouncementInfobox : Infobox
    {
        public AnnouncementInfobox(Data.Announcement a)
        {
            base.Text = a.Body;
            base.Title = "<span class=\"infobox-title-date\">" + a.StartDate.ToString("ddd MMM d, yyyy") + "</span>" + a.Title;
        }
    }
}