using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using ViewModels;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Interaction logic for DepartmentsMenu.xaml
    /// </summary>
    public partial class DepartmentsMenu : Page
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        public delegate Point GetPosition(IInputElement element);
        int rowIndex = -1;
        private object draggedItem;
        private Point startPoint;


        private DepartmentsMenuViewModel viewModel = new();


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentsMenu()
        {
            InitializeComponent();

            DepartmentsTable.ItemsSource = viewModel.Employees;
        }
        


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
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
                    DragDrop.DoDragDrop(DepartmentsTable, draggedItem, DragDropEffects.Move);
                    draggedItem = null;
                }
            }
        }
        private void SalariesTable_Drop(object sender, DragEventArgs e)
        {
            var targetCanvas = e.Source as Canvas;
            if (targetCanvas != null)
            {
                var targetDepartment = Convert.ToDouble(targetCanvas.Tag.ToString());
                var droppedItem = e.Data.GetData(typeof(EmployeeViewModel)) as EmployeeViewModel;

                if (droppedItem != null)
                {
                    droppedItem.DepartmentId = (short?)targetDepartment;
                }
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            var targetCanvas = sender as Canvas;
            if (targetCanvas != null)
            {
                var targetDepartment = Convert.ToDouble(targetCanvas.Tag.ToString());
                
                var droppedItem = e.Data.GetData(typeof(EmployeeViewModel)) as EmployeeViewModel;

                if (droppedItem != null)
                {
                    droppedItem.DepartmentId = (short?)targetDepartment;

                    DepartmentsTable.Items.Refresh();
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
}
