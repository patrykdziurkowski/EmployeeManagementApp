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
    /// Interaction logic for EmployeesMenu.xaml
    /// </summary>
    public partial class EmployeesMenu : Page
    {
        public EmployeesMenu()
        {
            InitializeComponent();

            EmployeesMenuViewModel viewModel = new();
            EmployeesTable.ItemsSource = viewModel.Employees;
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }
    }
}
