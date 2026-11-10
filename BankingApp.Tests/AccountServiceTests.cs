using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankingApp.BLL.Services;
using BankingApp.Models.Entities;

namespace BankingApp.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        AccountService service = new AccountService();

        [TestMethod]
        public void CanWithdraw_ShouldReturnFalse_WhenAmountTooHigh()
        {
            Account acc = new Account
            {
                Balance = 100,
                AccountType = "Savings",
                OverdraftLimit = 0
            };

            bool result = service.CanWithdraw(acc, 500);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void CanWithdraw_ShouldReturnTrue_WhenValid()
        {
            Account acc = new Account
            {
                Balance = 1000,
                AccountType = "Savings",
                OverdraftLimit = 0
            };

            bool result = service.CanWithdraw(acc, 200);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Deposit_ShouldThrow_WhenInvalidAmount()
        {
            Assert.ThrowsException<System.Exception>(() =>
                service.Deposit(1, -100));
        }

        [TestMethod]
        public void Transfer_ShouldThrow_WhenInvalidAmount()
        {
            Account acc = new Account
            {
                AccountId = 1,
                Balance = 1000,
                AccountType = "Current",
                OverdraftLimit = 0
            };

            Assert.ThrowsException<System.Exception>(() =>
                service.Transfer(acc, 12345678, -50));
        }
    }
}