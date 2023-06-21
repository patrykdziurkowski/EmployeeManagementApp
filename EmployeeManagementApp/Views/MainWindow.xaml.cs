using System.Windows;
using System.Windows.Navigation;
using ViewModels;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : NavigationWindow
    {
        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public MainWindow(StartMenu startMenu)
        {
            this.Navigate(startMenu);

            InitializeComponent();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}
