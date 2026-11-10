using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingApp.DAL.Data;

namespace BankingApp.BLL.Services
{
    public class UserService
    {
        private UserRepository userRepo = new UserRepository();

        public bool Login(string username, string password)
        {
            // validation of Username & Password
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;

            return userRepo.ValidateUser(username, password);
        }
    }
}
