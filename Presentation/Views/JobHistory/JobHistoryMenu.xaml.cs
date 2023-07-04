using Presentation.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BusinessLogic.ViewModels;

namespace Presentation
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
