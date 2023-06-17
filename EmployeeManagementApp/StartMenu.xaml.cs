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

namespace EmployeeManagementApp
{
    /// <summary>
    /// Logika interakcji dla klasy StartMenu.xaml
    /// </summary>
    public partial class StartMenu : Page
    {
        public StartMenu()
        {
            InitializeComponent();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();                  //Exit from application
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new MainMenu());
        }
    }
}
