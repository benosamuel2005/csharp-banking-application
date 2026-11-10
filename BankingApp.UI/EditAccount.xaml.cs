using BankingApp.BLL.Services;
using BankingApp.Models.Entities;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BankingApp.UI
{
    public partial class EditAccount : Window
    {
        private Account account;
        private AccountService service = new AccountService();

        public EditAccount(Account acc)
        {
            InitializeComponent();
            account = acc;//save account data

            //Fill all fields with existing data (pre-population)
            txtAccountNumber.Text = acc.AccountNumber.ToString();
            txtAccountType.Text = acc.AccountType;
            txtBalance.Text = acc.Balance.ToString();
            txtSortCode.Text = acc.SortCode.ToString();
            txtFirstName.Text = acc.FirstName;
            txtLastName.Text = acc.LastName;
            txtEmail.Text = acc.Email;
            txtPhone.Text = acc.Phone;
            txtAddress1.Text = acc.AddressLine1;
            txtAddress2.Text = acc.AddressLine2;
            txtCity.Text = acc.City;

            // Set County from dropdown
            foreach (ComboBoxItem item in cmbCounty.Items)
            {
                if (item.Content.ToString() == acc.County)
                {
                    cmbCounty.SelectedItem = item;
                    break;
                }
            }
            txtOverdraft.Text = acc.OverdraftLimit.ToString(); // Overdraft value - editable
        }

        //This runs after user clicks "Update Account"
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!decimal.TryParse(txtOverdraft.Text, out decimal overdraft) || overdraft < 0)// Validate overdraft value
                {
                    MessageBox.Show("Invalid overdraft");
                    return;
                }
                account.OverdraftLimit = overdraft;// Update only allowed field (overdraft)
                service.UpdateAccount(account);// Send updated data to BLL → DAL → DB
                MessageBox.Show("Account Updated Successfully!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}