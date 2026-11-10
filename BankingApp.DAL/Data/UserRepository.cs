using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;//connect with SQL Server
using BankingApp.Models.Entities;

namespace BankingApp.DAL.Data
{
    public class UserRepository
    {
        private DbConnection db = new DbConnection(); // Create database connection object

        public bool ValidateUser(string username, string password) //This method checks if username and password are correct
        {
            using (SqlConnection conn = db.GetConnection()) //Get connection from DbConnection class
            {
                conn.Open();
                // Query checks if user exists in Users table
                string query = "SELECT COUNT(*) FROM Users WHERE Username=@Username AND Password=@Password";

                SqlCommand cmd = new SqlCommand(query, conn);
                // Pass values safely 
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                int count = (int)cmd.ExecuteScalar(); // Pass values safely (prevents SQL injection)

                return count > 0;
            }
        }
    }
}