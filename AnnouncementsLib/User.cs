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

        public User(IPrincipal userPrincipal)
        {
            Profile = UserProfile.FromDatabase(userPrincipal.Identity.Name);
            SecurityAccess = CompiledSecurityInfo.CompileAccessLevel(userPrincipal);
        }
    }
}
