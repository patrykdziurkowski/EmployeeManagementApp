using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ViewModels;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {
        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public MainMenu()
        {
            InitializeComponent();

            LoginViewModel loginViewModel = LoginViewModel.GetInstance();
            DataContext = loginViewModel;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartMenu());
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EmployeesMenu());
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DepartmentsMenu());
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SalariesMenu());
        }
    }
}
