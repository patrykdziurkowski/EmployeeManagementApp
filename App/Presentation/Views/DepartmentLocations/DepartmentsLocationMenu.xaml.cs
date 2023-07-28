using Presentation.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BusinessLogic.ViewModels;

namespace Presentation
{
    /// <summary>
    /// Logika interakcji dla klasy DepartmentsLocationMenu.xaml
    /// </summary>
    public partial class DepartmentsLocationMenu : Page
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private DepartmentLocationsMenuViewModel _viewModel;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentsLocationMenu(DepartmentLocationsMenuViewModel departmentLocationMenuViewModel)
        {
            _viewModel = departmentLocationMenuViewModel;

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
            await Task.Run(() =>
            {
                if (_viewModel.LoadDepartmentLocationsCommand.CanExecute(null))
                {
                    _viewModel.LoadDepartmentLocationsCommand.Execute(null);
                }
            });
            OverlayContentControl.Visibility = Visibility.Hidden;

            DataContext = _viewModel;
            DepartmentLocationTable.ItemsSource = _viewModel.DepartmentLocations;
        }
    }
}
