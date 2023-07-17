using BusinessLogic.ViewModels;
using Presentation.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Presentation
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
        private JobHistoryMenu _jobHistoryMenu;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public EmployeesMenu(
            EmployeesMenuViewModel viewModel,
            JobHistoryMenu jobHistoryMenu)
        {
            _jobHistoryMenu = jobHistoryMenu;
            _viewModel = viewModel;

            InitializeComponent();
            OverlayContentControl.Content = new LoadingUserControl();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OverlayContentControl.Visibility = Visibility.Visible;
            await Task.Run(() => _viewModel.InitializeData());
            OverlayContentControl.Visibility = Visibility.Hidden;


            DataContext = _viewModel;
            EmployeesTable.ItemsSource = _viewModel.Employees;
            JobsComboBox.ItemsSource = _viewModel.Jobs;


        }

        private void ReturnToPreviousPage_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void GoToJobHistoryMenu_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(_jobHistoryMenu);
        }

        private void EmployeeDeleteCancel_Clicked(object sender, RoutedEventArgs e)
        {
            DeleteEmployeeConfirmation.Visibility = Visibility.Hidden;
        }

        private void EmployeeDeleteOpen_Clicked(object sender, RoutedEventArgs e)
        {
            DeleteEmployeeConfirmation.Visibility = Visibility.Visible;
            Button senderButton = (Button)sender;
            _viewModel.EmployeeToFire = (EmployeeViewModel)senderButton.Tag;
        }

        private void EmployeeDeleteProceed_Clicked(object sender, RoutedEventArgs e)
        {
            DeleteEmployeeConfirmation.Visibility = Visibility.Hidden;
        }

        private void EmployeeCommandFailCancel_Clicked(object sender, RoutedEventArgs e)
        {
            _viewModel.IsLastCommandFailAcknowledged = true;

            NavigationService.Refresh();
        }

        /// <summary>
        /// The purpose of this event handler is to reduce the amount of clicks needed to edit a cell
        /// by immediately beginning the editing
        /// </summary>
        private void DataGridCell_Selected(object sender, RoutedEventArgs e)
        {
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                DataGrid table = (DataGrid)sender;
                table.BeginEdit(e);
            }
        }
    }
}
