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
    /// Logika interakcji dla klasy DepartmentsLocationMenu.xaml
    /// </summary>
    public partial class DepartmentsLocationMenu : Page
    {
        private DepartmentLocationMenuViewModel _viewModel;
        public DepartmentsLocationMenu(DepartmentLocationMenuViewModel departmentLocationMenuViewModel)
        {
            _viewModel = departmentLocationMenuViewModel;
            InitializeComponent();
        }

        private void ReturnToDepartmentsMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.InitializeData();
            DataContext = _viewModel;
            DepartmentLocationTable.ItemsSource = _viewModel.DepartmentLocation;
        }
    }
}
