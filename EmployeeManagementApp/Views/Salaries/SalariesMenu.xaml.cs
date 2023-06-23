using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ViewModels;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Interaction logic for SalariesMenu.xaml class
    /// </summary>
    public partial class SalariesMenu : Page
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private SalariesMenuViewModel _viewModel;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public SalariesMenu(
            SalariesMenuViewModel salariesMenuViewModel)
        {
            _viewModel = salariesMenuViewModel;

            InitializeComponent();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.InitializeData();

            DataContext = _viewModel;
            SalariesTable.ItemsSource = _viewModel.Salaries;
        }
    }
}
