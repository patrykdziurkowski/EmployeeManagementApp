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
    /// Interaction logic for EmployeesMenu.xaml
    /// </summary>
    public partial class EmployeesMenu : Page
    {
        public EmployeesMenu()
        {
            InitializeComponent();

            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee() { Id = 1, Name = "Jan", Surname = "Kowlski", Email = "mail@gmail.com", PhoneNr = "999999999",  Salary = "1000", Commision = "5", Department = "55", RoleId = Employee.getRoleId("ST_MAN"), HireDate = new DateTime(1983,6,23), Manager = "110"});

            EmployeesTable.ItemsSource = employees;
        }

        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainMenu());
        }

        private void Button_Add_Clicked(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNr { get; set; }
        public string Salary { get; set; }
        public string Department { get; set; }
        public int RoleId { get; set; }
        public string Commision { get; set; }
        public DateTime HireDate { get; set; }
        public string Manager { get; set; }

        public static int getRoleId(string JobName)
        { 
            switch(JobName)
            {
                case "AD_PRES":
                    return 0;
                case "AD_VP":
                    return 1;
                case "AD_ASST":
                    return 2;
                case "AC_MGR":
                    return 3;
                case "AC_ACCOUNT":
                    return 4;
                case "SA_MAN":
                    return 5;
                case "SA_REP":
                    return 6;
                case "ST_MAN":
                    return 7;
                case "ST_CLERK":
                    return 8;
                case "IT_PROG":
                    return 9;
                case "MK_MAN":
                    return 10;
                case "MK_REP":
                    return 11;
                default: return -1;
            }
        }
    }
}
