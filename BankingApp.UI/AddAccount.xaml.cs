using BankingApp.BLL.Services;
using BankingApp.Models.Entities;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BankingApp.UI
{
    public partial class AddAccount : Window
    {
        AccountService service = new AccountService();

        public AddAccount()
        {
            InitializeComponent();
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validation

                // Account Number validation- it should be exactly 8 digits 
                if (txtAccountNumber.Text.Length != 8 || !long.TryParse(txtAccountNumber.Text, out long accNumber))
                {
                    MessageBox.Show("Account number must be exactly 8 digits");
                    return;
                }

                // Duplicacy check- if account number already exist
                if (!service.IsAccountNumberUnique(accNumber))
                {
                    MessageBox.Show("Account number already exists");
                    return;
                }

                // Balance validation- balance should be greater than 0
                if (!decimal.TryParse(txtBalance.Text, out decimal balance) || balance < 0)
                {
                    MessageBox.Show("Invalid balance");
                    return;
                }

                // Overdraft validation - overdraft should be greater than 0
                decimal overdraft = 0;
                if (!string.IsNullOrEmpty(txtOverdraft.Text))
                {
                    if (!decimal.TryParse(txtOverdraft.Text, out overdraft) || overdraft < 0)
                    {
                        MessageBox.Show("Invalid overdraft value");
                        return;
                    }
                }

                // Account type validation
                string accountType = rbSavings.IsChecked == true ? "Savings" : "Current";

                // Create account Object
                Account acc = new Account
                {
                    FirstName = txtFirstName.Text,
                    LastName = txtLastName.Text,
                    Email = txtEmail.Text,
                    Phone = txtPhone.Text,
                    AddressLine1 = txtAddress1.Text,
                    AddressLine2 = txtAddress2.Text,
                    City = txtCity.Text,
                    County = (cmbCounty.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    AccountNumber = accNumber,
                    AccountType = accountType,
                    Balance = balance,
                    OverdraftLimit = accountType == "Savings" ? 0 : overdraft
                };
                // Call BLL
                service.CreateAccount(acc);
                MessageBox.Show("Account Created Successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}