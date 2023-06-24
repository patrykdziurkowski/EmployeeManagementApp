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
    /// Interaction logic for JobHistoryMenu.xaml
    /// </summary>
    public partial class JobHistoryMenu : Page
    {
        private JobHistoryMenuViewModel _viewModel;
        public JobHistoryMenu(JobHistoryMenuViewModel jobHistoryMenuViewModel)
        {
            _viewModel = jobHistoryMenuViewModel;

            InitializeComponent();
            OverlayContentControl.Content = new LoadingUserControl();
        }

        private void ReturnToEmployeeMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OverlayContentControl.Visibility = Visibility.Visible;
            await Task.Run(() => _viewModel.InitializeData());
            OverlayContentControl.Visibility = Visibility.Hidden;

            DataContext = _viewModel;
            JobHistoryTable.ItemsSource = _viewModel.JobHistory;
        }
    }
}
