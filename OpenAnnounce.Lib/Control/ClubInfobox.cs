using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OpenAnnounce.Control
{
    public class ClubInfobox : Infobox
    {
        public Data.Club Club
        {
            set
            {
                base.Title = "Club Info - " + value.Name;
                base.Text = value.GenerateHtmlInfo();
            }
        }

        public ClubInfobox()
        {
            base.Text = "No club has been populated...";
            base.Title = "Club Info - ";
        }
    }
}