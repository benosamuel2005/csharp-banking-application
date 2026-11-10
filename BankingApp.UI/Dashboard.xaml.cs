using BankingApp.BLL.Services;
using BankingApp.Models.Entities;
using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;


namespace BankingApp.UI
{
    public partial class Dashboard : Window
    {
        AccountService service = new AccountService();//Connects UI with business logic
        public Dashboard() // Constructor runs when dashboard opens
        {
            InitializeComponent();
            LoadAccounts();// Load all accounts into grid
        }

        private void LoadAccounts()// This method loads data into DataGrid
        {
            dgAccounts.ItemsSource = service.GetAccounts();
        }


        // Logout button - go back to login screen
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow login = new MainWindow();
            login.Show();
            this.Close();
        }

        // Exit button - close full application
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        //Add account window
        private void NewAccount_Click(object sender, RoutedEventArgs e)
        {
            AddAccount window = new AddAccount();
            window.ShowDialog();

            LoadAccounts(); // refresh grid
        }


        //Edit Account window
        private void EditAccount_Click(object sender, RoutedEventArgs e)
        {
            // Check if user selected any account
            if (dgAccounts.SelectedItem == null)
            {
                MessageBox.Show("Select an account first!");
                return;
            }
            Account selected = (Account)dgAccounts.SelectedItem;// Get selected accoun
            Account fullAccount = service.GetAccountDetails(selected.AccountId);// Get full details from database
            EditAccount window = new EditAccount(fullAccount);// Open edit window
            window.ShowDialog();

            LoadAccounts();
        }


        //Deposite & transactions
        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            // Check selection
            if (dgAccounts.SelectedItem == null)
            {
                MessageBox.Show("Select account");
                return;
            }
            // Check selection
            TransactionWindow window = new TransactionWindow((Account)dgAccounts.SelectedItem);
            window.ShowDialog();

            LoadAccounts();
        }

        //withdraw - uses same window as deposit
        private void Withdraw_Click(object sender, RoutedEventArgs e)
        {
            Deposit_Click(sender, e); // reuse same logic
        }

        // Transfer money between accounts
        private void Transfer_Click(object sender, RoutedEventArgs e)
        {  
            if (dgAccounts.SelectedItem == null)
            {
                MessageBox.Show("Select account first");
                return;
            }
            TransferWindow window = new TransferWindow((Account)dgAccounts.SelectedItem);
            window.ShowDialog();

            dgAccounts.ItemsSource = null;
            LoadAccounts();
        }

        // Open Transactions history window
        private void ViewTransactions_Click(object sender, RoutedEventArgs e)
        {
            ViewTransactions window = new ViewTransactions();
            window.ShowDialog();
        }

        //Export selected account to XML file
        private void ExportXML_Click(object sender, RoutedEventArgs e)
        {
            if (dgAccounts.SelectedItem == null) // Check if account selected
            {
                MessageBox.Show("Select an account first!");
                return;
            }

            Account selected = (Account)dgAccounts.SelectedItem;
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(Account));// Convert object to XML format
                string filePath = $"Account_{selected.AccountNumber}.xml";// File name Ex:Account_12345678.xml
                using (FileStream fs = new FileStream(filePath, FileMode.Create))// Create file and write data
                {
                    serializer.Serialize(fs, selected);
                }

                MessageBox.Show("Account exported to XML successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}