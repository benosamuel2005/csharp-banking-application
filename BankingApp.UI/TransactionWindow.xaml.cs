using System;
using System.Windows;
using BankingApp.Models.Entities;
using BankingApp.BLL.Services;

namespace BankingApp.UI
{
    public partial class TransactionWindow : Window
    {
        private Account account;// Store selected account
        private AccountService service = new AccountService();// Connect to business logic

        public TransactionWindow(Account acc)
        {
            InitializeComponent();
            account = acc;

            txtAccount.Text = acc.AccountNumber.ToString();// Show account number in textbox
        }

        //Runs when Deposit button is clicked
        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal amount = decimal.Parse(txtAmount.Text);// Convert input amount to decimal

                service.Deposit(account.AccountId, amount);// Call deposit logic

                MessageBox.Show("Deposit successful");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        // This runs when Withdraw button is clicked
        private void Withdraw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                decimal amount = decimal.Parse(txtAmount.Text);// Convert input amount
                service.Withdraw(account, amount); // Call withdraw logic
                MessageBox.Show("Withdraw successful");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}