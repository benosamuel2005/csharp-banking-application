using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.Entities
{
    public class Account
    {
        public int AccountId { get; set; }// Unique ID of account - primary key
        public int CustomerId { get; set; }// Link to customer - foreign key
        public string FirstName { get; set; }// First name of customer
        public string LastName { get; set; }// Lastname of customer
        public long AccountNumber { get; set; }// Acc. no (8 digit)
        public int SortCode { get; set; }// Link to customer - foreign key
        public string AccountType { get; set; } // type of account - Savings / Current
        public decimal Balance { get; set; } // Current Balance
        public decimal OverdraftLimit { get; set; } // Overdraft Limit
        public string Email { get; set; }// Customers Email
        public string Phone { get; set; }//Customers phone no.
        public string AddressLine1 { get; set; }//Customers Address
        public string AddressLine2 { get; set; }
        public string City { get; set; } //City of Customers
        public string County { get; set; }// County
    }
}
