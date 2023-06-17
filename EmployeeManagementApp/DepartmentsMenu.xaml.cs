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
    /// Logika interakcji dla klasy DepartmentsMenu.xaml
    /// </summary>
    public partial class DepartmentsMenu : Page
    {
        public DepartmentsMenu()
        {
            InitializeComponent();
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }
    }
}
