using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingApp.Models.Entities
{
    public class User
    {
        public int UserId { get; set; } // User id 
        public string Username { get; set; } // Username 
        public string Password { get; set; } // Password
    }
}
