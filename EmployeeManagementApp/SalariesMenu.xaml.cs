using System;
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
        //  Constructors
        ////////////////////////////////////////////
        public SalariesMenu()
        {
            InitializeComponent();

            SalariesMenuViewModel viewModel = new();
            DataContext = viewModel;
            SalariesTable.ItemsSource = viewModel.Salaries;
            
            ComboSalary.Text = FormatStat(viewModel.SumOfSalaries);
            AvgSalary.Text = FormatStat(viewModel.AverageSalary);
            HighestSalary.Text = FormatStat(viewModel.MaxSalary);
            LowestSalary.Text = FormatStat(viewModel.MinSalary);
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }
        public static string FormatStat(double myNumber)
        {
            return string.Format("{0:0.00}$", myNumber);
        }
    }
}
