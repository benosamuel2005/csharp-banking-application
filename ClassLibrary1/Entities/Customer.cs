using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.Entities
{
    public class Customer
    {
        public int CustomerId { get; set; }// Customer ID
        public string FirstName { get; set; }//Customers firstname
        public string LastName { get; set; }//Customers lastname
        public string Email { get; set; }//Customers email
        public string Phone { get; set; }//Customers phone no.
        public string AddressLine1 { get; set; }//Customers address
        public string AddressLine2 { get; set; }
        public string City { get; set; }// City of Customers
        public string County { get; set; } // County 
    }
}