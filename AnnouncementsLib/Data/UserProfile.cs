using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Announcements.Data
{
    public class UserProfile
    {
        private DatabaseManager _manager;

        private int _id;
        private string _username, _displayname;

        public int Id { get { return _id; } }
        public string Username { get { return _username; } }
        public string DisplayName { get { return _displayname; } set { _displayname = value; } }

        public UserProfile(DatabaseManager manager, string username)
        {
            _manager = manager;

            _id = -1;
            _username = username;
            _displayname = username.Split('\\')[1];
        }

        public UserProfile(DatabaseManager manager, SqlDataReader r)
        {
            _manager = manager;

            _id = (int)r["Id"];
            _username = (string)r["Username"];
            _displayname = (string)r["DisplayName"];
        }

        public void Insert()
        {
            using (SqlCommand cmd = _manager.CreateCommand("INSERT INTO Users (Username, DisplayName) VALUES (@username, @displayName)"))
            {
                cmd.Parameters.AddWithValue("@username", Username);
                cmd.Parameters.AddWithValue("@displayName", DisplayName);
                cmd.ExecuteNonQuery();
            }

            using (SqlCommand cmd = _manager.CreateCommand("SELECT ID FROM Users WHERE Username=@username"))
            {
                cmd.Parameters.AddWithValue("@username", Username);
                _id = (int)cmd.ExecuteScalar();
            }
        }

        public void Update()
        {
            using (SqlCommand cmd = _manager.CreateCommand("UPDATE Users SET Username=@username, DisplayName=@displayName WHERE Id=@id"))
            {
                cmd.Parameters.AddWithValue("@id", Id);
                cmd.Parameters.AddWithValue("@username", Username);
                cmd.Parameters.AddWithValue("@displayName", DisplayName);
                cmd.ExecuteNonQuery();
            }
        }

        public static UserProfile FromDatabase(DatabaseManager manager, int id)
        {
            using (SqlCommand cmd = manager.CreateCommand("SELECT * FROM Users WHERE Id=@id"))
            {
                cmd.Parameters.AddWithValue("@id", id);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                        return new UserProfile(manager, r);
                    else
                        return null;
                }
            }
        }

        public static UserProfile FromDatabase(DatabaseManager manager, string name)
        {
            return FromDatabase(manager, name, true);
        }

        public static UserProfile FromDatabase(DatabaseManager manager, string name, bool create)
        {
            using (SqlCommand cmd = manager.CreateCommand("SELECT * FROM Users WHERE Username=@username"))
            {
                cmd.Parameters.AddWithValue("@username", name);

                using (SqlDataReader r = cmd.ExecuteReader())
                {
                    if (r.Read())
                    {
                        return new UserProfile(manager, r);
                    }
                    else if (create)
                    {
                        r.Close();
                        UserProfile u = new UserProfile(manager, name);
                        u.Insert();
                        return u;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }
    }
}