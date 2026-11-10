using System.Windows;
using BankingApp.BLL.Services;

namespace BankingApp.UI
{
    public partial class ViewTransactions : Window
    {
        AccountService service = new AccountService();// Connect to business logic

        // Constructor, runs when transaction window opens
        public ViewTransactions()
        {
            InitializeComponent();
            LoadTransactions();//Load all transaction into grid
        }

        // Fills DataGrid with transaction data
        private void LoadTransactions() 
        {
            dgTransactions.ItemsSource = service.GetTransactions();
        }
    }
}