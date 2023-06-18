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
    /// Interaction logic for DepartmentsMenu.xaml
    /// </summary>
    public partial class DepartmentsMenu : Page
    {
        public DepartmentsMenu()
        {
            InitializeComponent();

            List<DepartmentsTableObject> tableDepartmentsData = new List<DepartmentsTableObject>();

            //Add objects
            tableDepartmentsData.Add(new DepartmentsTableObject() { Id = 1, Name = "Jan", Surname = "Jowalski", Role = "AC_MGR", Department = DepartmentsTableObject.getDepartmentName(10) });

            SalariesTable.ItemsSource = tableDepartmentsData;
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }
    }

    public class DepartmentsTableObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Role { get; set; }
        public string Department { get; set; }

        public static string getDepartmentName(int DepartmentId)
        {
            switch (DepartmentId)
            {
                case 10:
                    return "Administration";
                case 20:
                    return "Marketing";
                case 50:
                    return "Shipping";
                case 60:
                    return "IT";
                case 80:
                    return "Sales";
                case 90:
                    return "Executive";
                case 110:
                    return "Accounting";
                case 190:
                    return "Concracting";
                default: return "None";
            }
        }

    }
}
