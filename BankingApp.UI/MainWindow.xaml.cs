using System.Windows;
using BankingApp.BLL.Services;

namespace BankingApp.UI
{
    public partial class MainWindow : Window
    {
        UserService userService = new UserService();// Connect to business logic for login
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)//Runs when user clicks Login button
        {
            // Get username and password from input fields
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            bool isValid = userService.Login(username, password); // Check if login is valid

            if (isValid)
            {
                MessageBox.Show("Login Successful!");
                Dashboard dashboard = new Dashboard();// Open dashboard window
                dashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password!");
            }
        }
    }
}
