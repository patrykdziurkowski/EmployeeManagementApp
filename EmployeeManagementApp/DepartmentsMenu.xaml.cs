﻿using System;
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
        private int _rowIndex = -1;
        private object _draggedItem;
        private Point _startPoint;

        public delegate Point GetPosition(IInputElement element);

        private DepartmentsMenuViewModel _viewModel = new();


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentsMenu()
        {
            InitializeComponent();

            DepartmentsTable.ItemsSource = _viewModel.Employees;
        }



        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        //Occurs when the left mouse button is pressed while the mouse pointer is over this element.
        private void SalariesTable_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
            _draggedItem = FindDataGridRow((DependencyObject)e.OriginalSource)?.Item;
        }

        //Occurs when the mouse pointer moves while the mouse pointer is over this element.
        private void SalariesTable_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            bool leftClickIsPressed = e.LeftButton == MouseButtonState.Pressed;

            if ((leftClickIsPressed) && (_draggedItem is not null))
            {
                Point currentPosition = e.GetPosition(null);
                if (Math.Abs(currentPosition.X - _startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(currentPosition.Y - _startPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    DragDrop.DoDragDrop(DepartmentsTable, _draggedItem, DragDropEffects.Move);
                    _draggedItem = null;
                }
            }
        }

        private void SalariesTable_Drop(object sender, DragEventArgs e)
        {
            Canvas targetCanvas = (Canvas)e.Source;
            if (targetCanvas != null)
            {
                double targetDepartmentId = Convert.ToDouble(targetCanvas.Tag.ToString());
                EmployeeViewModel droppedEmployee = (EmployeeViewModel)e.Data.GetData(typeof(EmployeeViewModel));

                if (droppedEmployee != null)
                {
                    droppedEmployee.DepartmentId = (short?)targetDepartmentId;
                }
            }
        }

        private void Canvas_Drop(object sender, DragEventArgs e)
        {
            var targetCanvas = sender as Canvas;
            if (targetCanvas != null)
            {
                double targetDepartmentId = Convert.ToDouble(targetCanvas.Tag.ToString());

                EmployeeViewModel droppedEmployee = (EmployeeViewModel)e.Data.GetData(typeof(EmployeeViewModel));

                if (droppedEmployee != null)
                {
                    droppedEmployee.DepartmentId = (short?)targetDepartmentId;

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
