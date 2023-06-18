using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ViewModels;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : Page
    {
        private LoginViewModel _viewModel;
        public StartMenu()
        {
            InitializeComponent();
            _viewModel = LoginViewModel.GetInstance();
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
    
        private void LogInto()
        {
            string userName = ((TextBox)this.FindName("LoginTextBox")).Text;
            string password = ((PasswordBox)this.FindName("LoginPasswordBox")).Password;
            _viewModel.Login(userName, password);
            this.NavigationService.Navigate(new MainMenu());
        }
    }
}
