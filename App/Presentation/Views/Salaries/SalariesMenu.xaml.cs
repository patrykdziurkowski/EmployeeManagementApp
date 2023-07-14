using BusinessLogic.ViewModels;
using Presentation.Views;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Presentation
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
            await Task.Run(() => _viewModel.InitializeData());
            OverlayContentControl.Visibility = Visibility.Hidden;

            DataContext = _viewModel;
            SalariesTable.ItemsSource = _viewModel.Salaries.OrderByDescending(employee => employee.Salary);
        }
    }
}
