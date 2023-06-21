using Models;
using Models.Repositories;
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
        private StartMenuViewModel _startMenuViewModel;
        private MainMenu _mainMenu;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public StartMenu(StartMenuViewModel startMenuViewModel,
            MainMenu mainMenu)
        {
            InitializeComponent();
            _startMenuViewModel = startMenuViewModel;
            _mainMenu = mainMenu;

            LoginTextBox.Focus();
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void LogInto()
        {
            string userName = ((TextBox)this.FindName("LoginTextBox")).Text;
            string password = ((PasswordBox)this.FindName("LoginPasswordBox")).Password;
            bool isLoggedIn = _startMenuViewModel.LogIn(userName, password);

            if (isLoggedIn == false)
            {
                LoginFailedLabel.Content = "Login failed!";
                LoginPasswordBox.Clear();
                LoginTextBox.Clear();
            }
            else
            {
                LoginFailedLabel.Content = string.Empty;
                this.NavigationService.Navigate(_mainMenu);
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
           
        }
    }
}
