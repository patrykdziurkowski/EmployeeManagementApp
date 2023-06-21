using Models;
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
        //  Fields and properties
        ////////////////////////////////////////////
        private UserCredentials _userCredentials;
        private EmployeesMenu _employeesMenu;
        private DepartmentsMenu _departmentsMenu;
        private SalariesMenu _salariesMenu;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public MainMenu(UserCredentials userCredentials,
            EmployeesMenu employeesMenu,
            DepartmentsMenu departmentsMenu,
            SalariesMenu salariesMenu)
        {
            _userCredentials = userCredentials;
            _departmentsMenu = departmentsMenu;
            _salariesMenu = salariesMenu;
            _employeesMenu = employeesMenu;

            InitializeComponent();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(_employeesMenu);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(_departmentsMenu);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(_salariesMenu);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = _userCredentials;
        }
    }
}
