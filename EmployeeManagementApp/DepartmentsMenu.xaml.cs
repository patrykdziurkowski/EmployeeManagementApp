using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Interaction logic for DepartmentsMenu.xaml
    /// </summary>
    public partial class DepartmentsMenu : Page
    {
        public delegate Point GetPosition(IInputElement element);
        int rowIndex = -1;

        private List<DepartmentsTableObject> tableDepartmentsData = new List<DepartmentsTableObject>();

        public DepartmentsMenu()
        {
            InitializeComponent();

            tableDepartmentsData.Add(new DepartmentsTableObject() { Id = 1, Name = "Jan", Surname = "Jowalski", Role = "AC_MGR", Department = DepartmentsTableObject.getDepartmentName(10) });
            tableDepartmentsData.Add(new DepartmentsTableObject() { Id = 2, Name = "Adam", Surname = "Nowa", Role = "IT_MGR", Department = DepartmentsTableObject.getDepartmentName(60) });
            tableDepartmentsData.Add(new DepartmentsTableObject() { Id = 3, Name = "Magdam", Surname = "Nowak", Role = "ACC", Department = DepartmentsTableObject.getDepartmentName(90) });

            SalariesTable.ItemsSource = tableDepartmentsData;
        }
        private object draggedItem;
        private Point startPoint;

        private void SalariesTable_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(null);
            draggedItem = FindDataGridRow(e.OriginalSource as DependencyObject)?.Item;
        }

        private void SalariesTable_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && draggedItem != null)
            {
                Point position = e.GetPosition(null);
                if (Math.Abs(position.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    DragDrop.DoDragDrop(SalariesTable, draggedItem, DragDropEffects.Move);
                    draggedItem = null;
                }
            }
        }
        private void SalariesTable_Drop(object sender, DragEventArgs e)
        {
            var targetCanvas = e.Source as Canvas;
            if (targetCanvas != null)
            {
                var targetDepartment = targetCanvas.Tag.ToString();
                var droppedItem = e.Data.GetData(typeof(DepartmentsTableObject)) as DepartmentsTableObject;

                if (droppedItem != null)
                {
                    droppedItem.Department = targetDepartment;
                }
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            var targetCanvas = sender as Canvas;
            if (targetCanvas != null)
            {
                var targetDepartment = targetCanvas.Tag.ToString();
                var droppedItem = e.Data.GetData(typeof(DepartmentsTableObject)) as DepartmentsTableObject;

                if (droppedItem != null)
                {
                    droppedItem.Department = targetDepartment;

                    SalariesTable.Items.Refresh();
                }
            }
        }

        private static T FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                if (obj is T parent)
                    return parent;
                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }

        private static DataGridRow FindDataGridRow(DependencyObject obj)
        {
            return FindVisualParent<DataGridRow>(obj);
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
