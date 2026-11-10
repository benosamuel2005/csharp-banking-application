using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BankingApp.Models.Entities
{
    public class Transaction
    {
        public int TransactionId { get; set; } // Transaction Id 
        public int AccountId { get; set; } //Account Id 
        public string TransactionType { get; set; } // Deposit, Withdraw, Transfer
        public decimal Amount { get; set; }// Amount of an account
        public string ReferenceNumber { get; set; } // Unique Reference Number
        public DateTime TransactionDate { get; set; }// date & time of transactions
    }
}
