using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;

namespace Announcements.Data
{
    public static class DatabaseManager
    {
        public static SqlConnection DatabaseConnection
        {
            get
            {
                return HttpContext.Current.Items["DatabaseConnection"] as SqlConnection;
            }

            private set
            {
                HttpContext.Current.Items["DatabaseConnection"] = value;
            }
        }

        public static void OpenConnection(string connectionString)
        {
            if (DatabaseConnection != null)
                CloseConnection();
            DatabaseConnection = new SqlConnection(connectionString);
            DatabaseConnection.Open();
        }

        public static void CloseConnection()
        {
            if (DatabaseConnection != null)
            {
                DatabaseConnection.Close();
                DatabaseConnection = null;
            }
        }
    }
}