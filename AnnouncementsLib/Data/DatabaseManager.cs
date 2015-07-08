using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Web;
using System.Data;
using System.Data.Common;

namespace Announcements.Data
{
    public class DatabaseManager
    {
        public static DatabaseManager Current
        {
            get
            {
                return HttpContext.Current.Items["DatabaseManager"] as DatabaseManager;
            }

            private set
            {
                HttpContext.Current.Items["DatabaseManager"] = value;
            }
        }

        public static void OpenConnection(string connectionString, IsolationLevel isolationLevel)
        {
            if (Current != null)
                throw new InvalidOperationException("A database connection is already open!");

            Current = new DatabaseManager(connectionString, isolationLevel);
        }

        public static void CloseConnection()
        {
            if (Current != null)
            {
                Current.Close();
                Current = null;
            }
        }

        public SqlConnection Connection { get; private set; }
        public SqlTransaction Transaction { get; private set; }

        private bool closed;

        private DatabaseManager(string connectionString, IsolationLevel isolationLevel)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();

            Transaction = Connection.BeginTransaction(isolationLevel);
            closed = false;
        }

        public SqlCommand CreateCommand()
        {
            return CreateCommand("");
        }

        public SqlCommand CreateCommand(string command)
        {
            return new SqlCommand(command, this.Connection, this.Transaction);
        }

        public void Close()
        {
            if (closed)
                throw new InvalidOperationException("Database connection is already closed!");

            closed = true;

            try
            {
                Transaction.Commit();
            }
            catch (SqlException)
            {
                Transaction.Rollback();
                throw;
            }
            finally
            {
                Connection.Close();
            }
        }
    }
}