using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using Announcements.Data;

namespace Announcements
{
    public class User
    {
        public UserProfile Profile { get; private set; }
        public CompiledSecurityInfo SecurityAccess { get; private set; }

        public User(DatabaseManager manager, IPrincipal userPrincipal)
        {
            Profile = UserProfile.FromDatabase(manager, userPrincipal.Identity.Name);
            SecurityAccess = CompiledSecurityInfo.CompileAccessLevel(manager, userPrincipal);
        }
    }
}
