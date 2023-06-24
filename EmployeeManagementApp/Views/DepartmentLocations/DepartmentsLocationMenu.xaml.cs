using EmployeeManagementApp.Views;
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
        private DepartmentLocationsMenuViewModel _viewModel;
        public DepartmentsLocationMenu(DepartmentLocationsMenuViewModel departmentLocationMenuViewModel)
        {
            _viewModel = departmentLocationMenuViewModel;

            InitializeComponent();
            OverlayContentControl.Content = new LoadingUserControl();
        }

        private void ReturnToDepartmentsMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OverlayContentControl.Visibility = Visibility.Visible;
            await Task.Run(() => _viewModel.InitializeData());
            OverlayContentControl.Visibility = Visibility.Hidden;

            DataContext = _viewModel;
            DepartmentLocationTable.ItemsSource = _viewModel.DepartmentLocation;
        }
    }
}
