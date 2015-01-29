using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Announcements.Data
{
    public class UserProfile
    {
        private int _id;
        private string _username, _displayname;

        public int Id { get { return _id; } }
        public string Username { get { return _username; } }
        public string DisplayName { get { return _displayname; } set { _displayname = value; } }

        public UserProfile(string username)
        {
            _id = -1;
            _username = username;
            _displayname = username.Split('\\')[1];
        }

        public UserProfile(SqlDataReader r)
        {
            _id = (int)r["Id"];
            _username = (string)r["Username"];
            _displayname = (string)r["DisplayName"];
        }

        public void Insert()
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Users (Username, DisplayName) VALUES (@username, @displayName)", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@username", Username);
            cmd.Parameters.AddWithValue("@displayName", DisplayName);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("SELECT ID FROM Users WHERE Username=@username", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@username", Username);
            _id = (int)cmd.ExecuteScalar();
        }

        public void Update()
        {
            SqlCommand cmd = new SqlCommand("UPDATE Users SET Username=@username, DisplayName=@displayName WHERE Id=@id", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@id", Id);
            cmd.Parameters.AddWithValue("@username", Username);
            cmd.Parameters.AddWithValue("@displayName", DisplayName);
            cmd.ExecuteNonQuery();
        }

        public static UserProfile FromDatabase(int id)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Id=@id", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@id", id);

            using (SqlDataReader r = cmd.ExecuteReader())
            {
                if (r.Read())
                    return new UserProfile(r);
                else
                    return null;
            }
        }

        public static UserProfile FromDatabase(string name)
        {
            return FromDatabase(name, true);
        }

        public static UserProfile FromDatabase(string name, bool create)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Users WHERE Username=@username", DatabaseManager.DatabaseConnection);
            cmd.Parameters.AddWithValue("@username", name);

            using (SqlDataReader r = cmd.ExecuteReader())
            {
                if (r.Read())
                    return new UserProfile(r);
                else
                {
                    r.Close();
                    UserProfile u = new UserProfile(name);
                    u.Insert();
                    return u;
                }
            }
        }
    }
}