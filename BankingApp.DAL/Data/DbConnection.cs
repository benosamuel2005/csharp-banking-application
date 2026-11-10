using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;//Connect with SQL Server

namespace BankingApp.DAL.Data
{
    public class DbConnection
    {
        // Connection string
        // Connect to the database
        private string connectionString =
            @"Server=localhost;Database=BankingDB;Trusted_Connection=True;TrustServerCertificate=True";
        //Returns a new SQL connection
        public SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);// Create and return connection
        }
    }
}