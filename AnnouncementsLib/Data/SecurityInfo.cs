using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Security;

namespace Announcements.Data
{
    public class SecurityInfo
    {
        internal static List<string> registeredPermissions = new List<string>();

        public static void RegisterPermission(string permission)
        {
            registeredPermissions.Add(permission);
        }

        public static void RegisterPermissions(List<string> permissions)
        {
            registeredPermissions.AddRange(permissions);
        }

        public bool this[string permission]
        {
            get
            {
                if (Permissions.ContainsKey(permission))
                    return Permissions[permission];
                else
                    return false;
            }

            set
            {
                if (Permissions.ContainsKey(permission))
                    Permissions[permission] = value;
                else
                    Permissions.Add(permission, value);
            }
        }

        public int Id { get; private set; }

        public string Domain { get; private set; }
        public string Name { get; private set; }
        public bool IsUser { get; private set; }

        public Dictionary<string, bool> Permissions { get; private set; }

        public SecurityInfo(string domain, string name, bool isUser)
        {
            Domain = domain;
            Name = name;
            IsUser = isUser;

            Permissions = new Dictionary<string, bool>();
        }

        public SecurityInfo(SqlDataReader r)
        {
            Id = (int)r["Id"];
            Domain = (string)r["Domain"];
            Name = (string)r["PrincipalName"];
            IsUser = (bool)r["CanAccessBackend"];

            Permissions = new Dictionary<string, bool>();
            foreach (string permission in registeredPermissions)
            {
                Permissions.Add(permission, (bool)r[permission]);
            }
        }

        public List<int> ListScopeIds()
        {
            SqlCommand cmd = new SqlCommand("SELECT ScopeId FROM SecurityPrincipalScopes WHERE SecurityPrincipalId=@id", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@id", Id);

            List<int> scopes = new List<int>();

            using (SqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                    scopes.Add((int)r["ScopeId"]);
            }

            return scopes;
        }

        public SecurityInfo CreateCopy(string domain, string name, bool isUser)
        {
            SecurityInfo copy = new SecurityInfo(domain, name, isUser);
            foreach (KeyValuePair<string, bool> permission in Permissions)
            {
                copy.Permissions.Add(permission.Key, permission.Value);
            }
            return copy;
        }

        public void Insert()
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO SecurityPrincipals(Domain, PrincipalName, IsUser" + InsertPopulate1() + ") VALUES (@domain, @principalName, @isUser" + InsertPopulate2() + ")", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@domain", Domain);
            cmd.Parameters.AddWithValue("@principalName", Name);
            cmd.Parameters.AddWithValue("@isUser", IsUser);
            foreach (string permission in registeredPermissions)
                cmd.Parameters.AddWithValue("@" + permission, this[permission]);

            cmd.ExecuteNonQuery();
        }

        private string InsertPopulate1()
        {
            string r = "";
            foreach (string permission in registeredPermissions)
                r += ", " + permission;
            return r;
        }

        private string InsertPopulate2()
        {
            string r = "";
            foreach (string permission in registeredPermissions)
                r += ", @" + permission;
            return r;
        }

        public void Update()
        {
            SqlCommand cmd = new SqlCommand("UPDATE SecurityPrincipals SET Domain=@domain, PrincipalName=@principalName, IsUser=@isUser" + UpdatePopulate1() + " WHERE Id=@id", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@id", Id);
            cmd.Parameters.AddWithValue("@domain", Domain);
            cmd.Parameters.AddWithValue("@principalName", Name);
            cmd.Parameters.AddWithValue("@isUser", IsUser);
            foreach (string permission in registeredPermissions)
                cmd.Parameters.AddWithValue("@" + permission, this[permission]);

            cmd.ExecuteNonQuery();
        }

        private string UpdatePopulate1()
        {
            string r = "";
            foreach (string permission in registeredPermissions)
                r += ", " + permission + "=@" + permission;
            return r;
        }

        public static SecurityInfo FromDatabase(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SecurityPrincipals WHERE Id=@id", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader r = cmd.ExecuteReader())
            {
                if (r.Read())
                    return new SecurityInfo(r);
                else
                    return null;
            }
        }

        public static SecurityInfo FromDatabase(string principalName)
        {
            return FromDatabase(principalName.Split('\\')[0], principalName.Split('\\')[1]);
        }

        public static SecurityInfo FromDatabase(string domain, string principal)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM SecurityPrincipals WHERE Domain=@domain AND PrincipalName=@principalName", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@domain", domain);
            cmd.Parameters.AddWithValue("@principalName", principal);

            using (SqlDataReader r = cmd.ExecuteReader())
            {
                if (r.Read())
                    return new SecurityInfo(r);
                else
                    return null;
            }
        }
    }

    public class CompiledSecurityInfo
    {
        public bool this[string permission]
        {
            get
            {
                if (permissions.ContainsKey(permission))
                    return permissions[permission];
                else
                    return false;
            }
        }

        public List<int> Scopes { get; private set; }
        private Dictionary<string, bool> permissions = new Dictionary<string, bool>();

        public CompiledSecurityInfo()
        {
            Scopes = new List<int>();
            Scopes.Add(0);
            foreach (string permission in SecurityInfo.registeredPermissions)
                permissions.Add(permission, false);
        }

        public void AddPermissions(SecurityInfo level)
        {
            if (level == null)
                return;

            Scopes.AddRange(level.ListScopeIds());
            foreach (string permission in SecurityInfo.registeredPermissions)
            {
                if (permissions.ContainsKey(permission))
                    permissions[permission] |= level[permission];
                else
                    permissions.Add(permission, level[permission]);
            }
        }

        public static CompiledSecurityInfo CompileAccessLevel(IPrincipal user)
        {
            if (user != null && user.Identity != null && user.Identity.Name != String.Empty)
            {
                CompiledSecurityInfo level = new CompiledSecurityInfo();

                level.AddPermissions(SecurityInfo.FromDatabase(user.Identity.Name));

                foreach (string group in Roles.GetRolesForUser())
                {
                    if (group.Contains('\\'))
                        level.AddPermissions(SecurityInfo.FromDatabase(group));
                }

                return level;
            }
            else
            {
                return new CompiledSecurityInfo();
            }
        }
    }
}