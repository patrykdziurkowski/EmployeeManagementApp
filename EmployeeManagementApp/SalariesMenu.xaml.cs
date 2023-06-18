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
    /// Interaction logic for SalariesMenu.xaml class
    /// </summary>
    public partial class SalariesMenu : Page
    {
        public SalariesMenu()
        {
            InitializeComponent();
            List<SalariesTableObject> tableSalariesData = new List<SalariesTableObject>();

            //Add objects
            tableSalariesData.Add(new SalariesTableObject() { Id = 1, Name = "Jan", Surname = "Jowalski", Role = "AC_MGR", Salary = "5555" });

            SalariesTable.ItemsSource = tableSalariesData;
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }
    }

    public class SalariesTableObject 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Salary { get; set; }
    }
}
