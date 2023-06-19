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
    /// Interaction logic for JobHistoryMenu.xaml
    /// </summary>
    public partial class JobHistoryMenu : Page
    {
        public JobHistoryMenu()
        {
            InitializeComponent();

            JobHistoryMenuViewModel viewModel = new();
            DataContext = viewModel;
            JobHistoryTable.ItemsSource = viewModel.JobHistory;
        }

        private void ReturnToEmployeeMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
