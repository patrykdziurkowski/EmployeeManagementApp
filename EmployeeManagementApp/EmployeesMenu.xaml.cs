using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ViewModels;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Interaction logic for EmployeesMenu.xaml
    /// </summary>
    public partial class EmployeesMenu : Page
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private EmployeesMenuViewModel _viewModel;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public EmployeesMenu()
        {
            InitializeComponent();

            List<string> jobs = new()
            {
                "AD_PRES",
                "AD_VP",
                "AD_ASST",
                "AC_MGR",
                "AC_ACCOUNT",
                "SA_MAN",
                "SA_REP",
                "ST_MAN",
                "ST_CLERK",
                "IT_PROG",
                "MK_MAN",
                "MK_REP"
            };

            _viewModel = new EmployeesMenuViewModel();
            EmployeesTable.ItemsSource = _viewModel.Employees;
            DataGridComboBoxColumn comboBox = (DataGridComboBoxColumn)this.FindName("JobsComboBox");
            comboBox.ItemsSource = jobs;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }

        private void Click_AddEmployee(object sender, RoutedEventArgs e)
        {
            _viewModel.AddEmployee();
            _viewModel =  new EmployeesMenuViewModel();
            EmployeesTable.ItemsSource = _viewModel.Employees;
        }

        private void Button_Add_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)e.Source;
            if (clickedButton is not null)
            {
                EmployeeViewModel employee = (EmployeeViewModel)EmployeesTable.SelectedItem;
                _viewModel.RemoveEmployee((int)employee.EmployeeId);
            }
            
        }

        private void Click_Navigate_JobHistory(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new JobHistoryMenu());
        }
    }
}
