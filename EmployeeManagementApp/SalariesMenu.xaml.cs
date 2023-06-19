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
            SalariesTable.ItemsSource = viewModel.Salaries;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }
    }
}
