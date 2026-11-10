using BankingApp.DAL.Data;
using BankingApp.Models.Entities;
using System;
using System.Collections.Generic;



namespace BankingApp.BLL.Services
{
    public class AccountService
    {

        private AccountRepository repo = new AccountRepository(); // Create object of repository


        //Gets all accounts from database
        public List<Account> GetAccounts()
        {
            return repo.GetAllAccounts();
        }

        //Chscks Account Duplication
        public bool IsAccountNumberUnique(long accountNumber)
        // If account exists,return false & If account exists,return false
        {
            return !repo.AccountExists(accountNumber);
        }



        //Money withdraw logic
        public bool CanWithdraw(Account account, decimal amount)
        {
            if (amount <= 0) //Amount must be greater than 0
                return false;

            if (account.AccountType == "Savings")// In SAvings Account only balance can be used
            {
                return amount <= account.Balance;
            }
            else //If Current account,it can used Overdraft also
            {
                return amount <= (account.Balance + account.OverdraftLimit);
            }
        }


        //Create New Account (Customer & Account)
        public void CreateAccount(Account account)
        {
            if (account.Balance < 0)
                throw new Exception("Balance cannot be negative");

            if (account.AccountType == "Savings") // Saving account can not have overdraft
                account.OverdraftLimit = 0;

            int customerId = repo.AddCustomer(account); // Save Customer Details
            account.CustomerId = customerId; //Assign customer ID
            repo.AddAccount(account); //Save Account Details
        }



        //Fetching Account + Customer Details 
        public Account GetAccountDetails(int accountId)
        {
            return repo.GetAccountWithCustomer(accountId);
        }



        //Updatation of Account
        public void UpdateAccount(Account account)
        // This updates account (only allowed fields like overdraft)
        {
            if (account.OverdraftLimit < 0) // Overdraft shouldn't be negetive
                throw new Exception("Invalid overdraft value");

            repo.UpdateAccount(account);
        }


        //Deposit Amount -Add money
        public void Deposit(int accountId, decimal amount)
        {
            if (amount <= 0)//Checks amount is valid or not
                throw new Exception("Invalid amount");

            repo.Deposit(accountId, amount);//Update the balance
            repo.AddTransaction(accountId, "Deposit", amount);// Save the transaction
        }


        //Withdraw Amount - Removes money
        public void Withdraw(Account account, decimal amount)
        {
            if (!CanWithdraw(account, amount))// check withdraw is possible or not
                throw new Exception("Insufficient balance");

            repo.Withdraw(account.AccountId, amount);//Deduct Balance
            repo.AddTransaction(account.AccountId, "Withdraw", amount);//Save transaction
        }


        //Transfer Amount 
        public void Transfer(Account source, long destAccountNumber, decimal amount)
        {
            if (amount <= 0)//Amount must be valid
                throw new Exception("Invalid amount");

            if (!CanWithdraw(source, amount))//Checks if enough balance
                throw new Exception("Insufficient funds");

            Account destination = repo.GetAccountByNumber(destAccountNumber);// Find the destination account

            // Savings account restriction- can only transfer internally
            if (source.AccountType == "Savings" && destination == null)
                throw new Exception("Savings account can only transfer internally");

            // Deduct amount from source
            repo.Withdraw(source.AccountId, amount);
            repo.AddTransaction(source.AccountId, "Transfer-Out", amount);

            // Credit amount in destination (only if internal)
            if (destination != null)
            {
                repo.Deposit(destination.AccountId, amount);
                repo.AddTransaction(destination.AccountId, "Transfer-In", amount);
            }

        }
        //Get Transactions History
        public List<Transaction> GetTransactions()
            {
                return repo.GetTransactions();
            }
    }
}