using System;
using System.Windows;
using BankingApp.Models.Entities;
using BankingApp.BLL.Services;

namespace BankingApp.UI
{
    public partial class TransferWindow : Window
    {
        private Account source;//Stores the source account
        private AccountService service = new AccountService();// Connect to business logic

        //constructor - runs when transfer window open
        public TransferWindow(Account acc)
        {
            InitializeComponent();
            source = acc;
            txtSource.Text = acc.AccountNumber.ToString();//Shows source account number
        }

        // This runs when user clicks Transfer button
        private void Transfer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                long destAcc = long.Parse(txtDestAcc.Text);// Get destination account number
                decimal amount = decimal.Parse(txtAmount.Text);// Get transfer amount 
                service.Transfer(source, destAcc, amount);// Call transfer logic
                MessageBox.Show("Transfer Successful!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}