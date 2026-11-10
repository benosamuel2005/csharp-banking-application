using BankingApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace BankingApp.DAL.Data
{
    public class AccountRepository
    {
        //This is used to connect with database
        private DbConnection db = new DbConnection();


        //This method gets all accounts with customer details
        public List<Account> GetAllAccounts()
        {
            List<Account> accounts = new List<Account>();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                //Join Accounts and Customers tables to get full details
                string query = @"
                    SELECT a.*, 
                           c.FirstName, c.LastName, c.Email, c.Phone,
                           c.AddressLine1, c.AddressLine2, c.City, c.County
                    FROM Accounts a
                    JOIN Customers c ON a.CustomerId = c.CustomerId";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                // Loop through each record and store in list
                while (reader.Read())
                {
                    accounts.Add(new Account
                    {
                        AccountId = (int)reader["AccountId"],
                        CustomerId = (int)reader["CustomerId"],
                        AccountNumber = (long)reader["AccountNumber"],
                        SortCode = (int)reader["SortCode"],
                        AccountType = reader["AccountType"].ToString(),
                        Balance = (decimal)reader["Balance"],
                        OverdraftLimit = (decimal)reader["OverdraftLimit"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        AddressLine1 = reader["AddressLine1"].ToString(),
                        AddressLine2 = reader["AddressLine2"].ToString(),
                        City = reader["City"].ToString(),
                        County = reader["County"].ToString()
                    });
                }
            }
            // Return full list of accounts
            return accounts;
        }


        //Add New Account
        //This method adds a new account in Accounts table
        public void AddAccount(Account account)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                //Insert account details into database 
                string query = @"INSERT INTO Accounts 
                        (CustomerId, AccountNumber, AccountType, Balance, OverdraftLimit) 
                        VALUES (@CustomerId, @AccountNumber, @AccountType, @Balance, @OverdraftLimit)";

                SqlCommand cmd = new SqlCommand(query, conn);
                //Passing values safely using parameters
                cmd.Parameters.AddWithValue("@CustomerId", account.CustomerId);
                cmd.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
                cmd.Parameters.AddWithValue("@AccountType", account.AccountType);
                cmd.Parameters.AddWithValue("@Balance", account.Balance);
                cmd.Parameters.AddWithValue("@OverdraftLimit", account.OverdraftLimit);

                cmd.ExecuteNonQuery();
            }
        }

        //This method adds customer details and returns new CustomerId
        public int AddCustomer(Account account)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                // Insert customer and get generated CustomerId
                string query = @"INSERT INTO Customers 
                        (FirstName, LastName, Email, Phone, AddressLine1, AddressLine2, City, County)
                        OUTPUT INSERTED.CustomerId
                        VALUES (@FirstName, @LastName, @Email, @Phone, @Address1, @Address2, @City, @County)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@FirstName", account.FirstName);
                cmd.Parameters.AddWithValue("@LastName", account.LastName);
                cmd.Parameters.AddWithValue("@Email", account.Email);
                cmd.Parameters.AddWithValue("@Phone", account.Phone);
                cmd.Parameters.AddWithValue("@Address1", account.AddressLine1);
                cmd.Parameters.AddWithValue("@Address2", account.AddressLine2);
                cmd.Parameters.AddWithValue("@City", account.City);
                cmd.Parameters.AddWithValue("@County", account.County);
                // Return new CustomerId
                return (int)cmd.ExecuteScalar();
            }
        }
        //Prevent duplicacy of Acc. no
        public bool AccountExists(long accountNumber)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM Accounts WHERE AccountNumber=@Acc";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Acc", accountNumber);

                int count = (int)cmd.ExecuteScalar();
                //If count > 0, account exists
                return count > 0;
            }
        }

        //Fetching Account Details + customer details using AccountId
        public Account GetAccountWithCustomer(int accountId)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"
                    SELECT a.*, c.FirstName, c.LastName, c.Email, c.Phone,
                           c.AddressLine1, c.AddressLine2, c.City, c.County
                    FROM Accounts a
                    JOIN Customers c ON a.CustomerId = c.CustomerId
                    WHERE a.AccountId = @Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", accountId);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Account
                    {
                        AccountId = (int)reader["AccountId"],
                        CustomerId = (int)reader["CustomerId"],
                        AccountNumber = (long)reader["AccountNumber"],
                        SortCode = (int)reader["SortCode"],
                        AccountType = reader["AccountType"].ToString(),
                        Balance = (decimal)reader["Balance"],
                        OverdraftLimit = (decimal)reader["OverdraftLimit"],
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        AddressLine1 = reader["AddressLine1"].ToString(),
                        AddressLine2 = reader["AddressLine2"].ToString(),
                        City = reader["City"].ToString(),
                        County = reader["County"].ToString()
                    };
                }

                return null;
            }
        }
        //Updates overdraft value of account
        public void UpdateAccount(Account account)
            {
                using (SqlConnection conn = db.GetConnection())
                {
                    conn.Open();

                    string query = @"UPDATE Accounts 
                             SET OverdraftLimit=@OverdraftLimit
                             WHERE AccountId=@AccountId";

                    SqlCommand cmd = new SqlCommand(query, conn);

                    cmd.Parameters.AddWithValue("@OverdraftLimit", account.OverdraftLimit);
                    cmd.Parameters.AddWithValue("@AccountId", account.AccountId);

                    cmd.ExecuteNonQuery();
                }
            }

        //// This adds money to account balance (Deposit)
        public void Deposit(int accountId, decimal amount)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "UPDATE Accounts SET Balance = Balance + @Amount WHERE AccountId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Id", accountId);

                cmd.ExecuteNonQuery();
            }
        }

        //// This removes money from account balance (Withdraw) 
        public void Withdraw(int accountId, decimal amount)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "UPDATE Accounts SET Balance = Balance - @Amount WHERE AccountId=@Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Id", accountId);

                cmd.ExecuteNonQuery();
            }
        }

        // This saves every transaction (audit system)
        public void AddTransaction(int accountId, string type, decimal amount)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = @"INSERT INTO Transactions 
                        (AccountId, TransactionType, Amount, ReferenceNumber)
                        VALUES (@AccountId, @Type, @Amount, @Ref)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@AccountId", accountId);
                cmd.Parameters.AddWithValue("@Type", type);
                cmd.Parameters.AddWithValue("@Amount", amount);
                cmd.Parameters.AddWithValue("@Ref", Guid.NewGuid().ToString());// Unique reference number for each transaction

                cmd.ExecuteNonQuery();
            }
        }

        // This finds account using account number (used in transfer)
        public Account GetAccountByNumber(long accountNumber)
        {
            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();

                string query = "SELECT * FROM Accounts WHERE AccountNumber=@Acc";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Acc", accountNumber);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return new Account
                    {
                        AccountId = (int)reader["AccountId"],
                        AccountNumber = (long)reader["AccountNumber"],
                        AccountType = reader["AccountType"].ToString(),
                        Balance = (decimal)reader["Balance"],
                        OverdraftLimit = (decimal)reader["OverdraftLimit"]
                    };
                }

                return null;
            }
        }


        // This returns all transactions for viewing 
        public List<Transaction> GetTransactions()
        {
            List<Transaction> list = new List<Transaction>();

            using (SqlConnection conn = db.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM Transactions ORDER BY TransactionDate DESC"; // Get latest transactions first
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    list.Add(new Transaction
                    {
                        TransactionId = (int)reader["TransactionId"],
                        AccountId = (int)reader["AccountId"],
                        TransactionType = reader["TransactionType"].ToString(),
                        Amount = (decimal)reader["Amount"],
                        ReferenceNumber = reader["ReferenceNumber"].ToString(),
                        TransactionDate = (DateTime)reader["TransactionDate"]
                    });
                }
            }

            return list;
        }
    }
}