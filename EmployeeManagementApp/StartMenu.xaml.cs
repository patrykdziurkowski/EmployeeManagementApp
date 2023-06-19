using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ViewModels;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : Page
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private LoginViewModel _viewModel;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public StartMenu()
        {
            InitializeComponent();
            _viewModel = LoginViewModel.GetInstance();

            LoginTextBox.Focus();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void LogInto()
        {
            string userName = ((TextBox)this.FindName("LoginTextBox")).Text;
            string password = ((PasswordBox)this.FindName("LoginPasswordBox")).Password;
            bool isLoggedIn = _viewModel.TryLogIn(userName, password);

            if (isLoggedIn == false)
            {
                LoginFailedLabel.Content = "Login failed!";
                LoginPasswordBox.Clear();
                LoginTextBox.Clear();
            }
            else
            {
                LoginFailedLabel.Content = string.Empty;
                this.NavigationService.Navigate(new MainMenu());
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            //Exit from application
            System.Windows.Application.Current.Shutdown();                  
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            LogInto();
        }

        private void LoginPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                LogInto();
            }
        }

        private void LoginTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            
        }
    }
}
