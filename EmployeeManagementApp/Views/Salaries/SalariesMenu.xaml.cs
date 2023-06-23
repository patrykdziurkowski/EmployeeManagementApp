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
        public static string FormatStat(double myNumber)
        {
            NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return string.Format(nfi,"${0:0.00}", myNumber);
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.InitializeData();

            DataContext = _viewModel;
            SalariesTable.ItemsSource = _viewModel.Salaries;

            ComboSalary.Text = FormatStat(_viewModel.SumOfSalaries);
            AvgSalary.Text = FormatStat(_viewModel.AverageSalary);
            HighestSalary.Text = FormatStat(_viewModel.MaxSalary);
            LowestSalary.Text = FormatStat(_viewModel.MinSalary);
        }
    }
}
