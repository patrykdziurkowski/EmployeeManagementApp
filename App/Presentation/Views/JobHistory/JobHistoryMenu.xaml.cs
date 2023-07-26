using Presentation.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BusinessLogic.ViewModels;
using System.Linq;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for JobHistoryMenu.xaml
    /// </summary>
    public partial class JobHistoryMenu : Page
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private JobHistoryMenuViewModel _viewModel;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobHistoryMenu(JobHistoryMenuViewModel jobHistoryMenuViewModel)
        {
            _viewModel = jobHistoryMenuViewModel;

            InitializeComponent();
            OverlayContentControl.Content = new LoadingUserControl();
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void ReturnToPreviousPage_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OverlayContentControl.Visibility = Visibility.Visible;
            await Task.Run(() => _viewModel.InitializeDataAsync());
            OverlayContentControl.Visibility = Visibility.Hidden;

            DataContext = _viewModel;
            JobHistoryTable.ItemsSource = _viewModel.JobHistory.OrderBy(x => x.EndDate);
        }
    }
}
