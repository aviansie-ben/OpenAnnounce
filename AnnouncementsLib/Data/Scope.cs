using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Announcements.Data
{
    public class Scope
    {
        public static readonly Scope everybodyScope = new Scope()
        {
            Id = 0,
            Name = "Everybody"
        };

        public int Id { get; private set; }
        public string Name { get; set; }

        public Scope()
        {
            Name = "";
        }

        public Scope(SqlDataReader r)
        {
            Id = (int)r["Id"];
            Name = (string)r["Name"];
        }

        public static Scope FromDatabase(int id)
        {
            if (id <= 0)
                return everybodyScope;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Scopes WHERE Id=@id", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader r = cmd.ExecuteReader())
            {
                if (r.Read())
                    return new Scope(r);
                else
                    return null;
            }
        }

        public static List<Scope> AllFromDatabase()
        {
            List<Scope> scopes = new List<Scope>();
            scopes.Add(everybodyScope);

            SqlCommand cmd = new SqlCommand("SELECT * FROM Scopes", DatabaseManager.DatabaseConnection);

            using (SqlDataReader r = cmd.ExecuteReader())
            {
                while (r.Read())
                    scopes.Add(new Scope(r));
            }

            return scopes;
        }
    }
}
