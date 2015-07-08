using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Announcements
{
    public class AnnouncementsPage : Page
    {
        public Main MainMaster { get { return this.Master as Main; } }
        public User CurrentUser { get { return MainMaster.RequestingUser; } }
    }
}