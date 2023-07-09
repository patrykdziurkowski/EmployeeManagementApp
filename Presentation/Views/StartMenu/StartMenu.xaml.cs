using Presentation.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using BusinessLogic.ViewModels;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for StartMenu.xaml
    /// </summary>
    public partial class StartMenu : Page
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private StartMenuViewModel _viewModel;
        private MainMenu _mainMenu;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public StartMenu(StartMenuViewModel viewModel,
            MainMenu mainMenu)
        {
            _viewModel = viewModel;
            _mainMenu = mainMenu;

            InitializeComponent();
            OverlayContentControl.Content = new LoadingUserControl();
            OverlayContentControl.Visibility = Visibility.Hidden;
            LoginTextBox.Focus();
            errorMessagesList.ItemsSource = _viewModel.LoginErrorMessages;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private async Task LogInto()
        {
            OverlayContentControl.Visibility = Visibility.Visible;
            _viewModel.UserCredentials.Password = LoginPasswordBox.Password;
            _viewModel.UserCredentials.UserName = LoginTextBox.Text;

            bool isLoggedIn = await Task.Run(() =>
            {
                if (_viewModel.LoginCommand.CanExecute(null))
                {
                    _viewModel.LoginCommand.Execute(null);
                    return _viewModel.IsLoginSuccessful;
                }

                return false;
            });
            OverlayContentControl.Visibility = Visibility.Hidden;

            if (isLoggedIn)
            {
                this.NavigationService.Navigate(_mainMenu);
            }
            
        }

        private void ExitApplication_Clicked(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();                  
        }
        private void LoginButton_Clicked(object sender, RoutedEventArgs e)
        {
            LogInto();
        }

        private void LoginPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                LogInto();
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            _viewModel.UserCredentials.Clear();
            LoginTextBox.Text = string.Empty;
            LoginPasswordBox.Password = string.Empty;
        }
    }
}
