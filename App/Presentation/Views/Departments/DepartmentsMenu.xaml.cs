﻿using BusinessLogic.ViewModels;
using Presentation.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for DepartmentsMenu.xaml
    /// </summary>
    public partial class DepartmentsMenu : Page
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private object? _draggedItem;
        private Point _startingPoint;

        public delegate Point GetPosition(IInputElement element);

        private DepartmentsMenuViewModel _viewModel;
        private DepartmentsLocationMenu _departmentsLocationMenu;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentsMenu(
            DepartmentsMenuViewModel departmentsMenuViewModel,
            DepartmentsLocationMenu departmentsLocationMenu)
        {
            _viewModel = departmentsMenuViewModel;
            _departmentsLocationMenu = departmentsLocationMenu;

            InitializeComponent();
            OverlayContentControl.Content = new LoadingUserControl();
            DataContext = _viewModel;
        }



        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        //Occurs when the left mouse button is pressed while the mouse pointer is over this element.
        private void SalariesTable_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startingPoint = e.GetPosition(null);
            _draggedItem = FindVisualParent<DataGridRow>((DependencyObject)e.OriginalSource)?.Item;
        }

        //Occurs when the mouse pointer moves while the mouse pointer is over this element.
        private void SalariesTable_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            bool leftClickIsPressed = e.LeftButton == MouseButtonState.Pressed;

            if ((leftClickIsPressed) && (_draggedItem is not null))
            {
                Point currentPosition = e.GetPosition(null);
                if (Math.Abs(currentPosition.X - _startingPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(currentPosition.Y - _startingPoint.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    DragDrop.DoDragDrop(DepartmentsTable, _draggedItem, DragDropEffects.Move);
                    _draggedItem = null;
                }
            }
        }

        private void EmployeeIntoDepartment_Dropped(object sender, DragEventArgs e)
        {
            Grid targetDepartment = (Grid)sender;
            if (targetDepartment != null)
            {
                short targetDepartmentId = (short)targetDepartment.Tag;

                EmployeeDto droppedEmployee = (EmployeeDto)e.Data.GetData(typeof(EmployeeDto));

                if (droppedEmployee != null)
                {
                    EmployeeDto employeeToUpdate = _viewModel.Employees
                        .First(employee => employee.EmployeeId == droppedEmployee.EmployeeId);
                    employeeToUpdate.DepartmentId = targetDepartmentId;

                    DepartmentsTable.ItemsSource = _viewModel.Employees.OrderBy(employee => employee.DepartmentId);
                }
            }
        }

        private static T? FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                if (obj is T parent)
                    return parent;
                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }


        private void ReturnToPreviousPage_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void GoToDepartmentsLocationMenu_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(_departmentsLocationMenu);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OverlayContentControl.Visibility = Visibility.Visible;
            await Task.Run(() =>
            {
                if (_viewModel.LoadDepartmentsCommand.CanExecute(null))
                {
                    _viewModel.LoadDepartmentsCommand.Execute(null);
                }
            });
            OverlayContentControl.Visibility = Visibility.Hidden;

            DepartmentsTable.ItemsSource = _viewModel.Employees.OrderBy(employee => employee.DepartmentId);
            DepartmentsList.ItemsSource = _viewModel.Departments;
        }

        /// <summary>
        /// The purpose of handling this event this way is to re-raise the scroll event to stop
        /// the DataGrid from interrupting scrolling which should be done by the ScrollViewer instead
        /// </summary>
        private void DataGrid_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Handled)
            {
                return;
            }

            e.Handled = true;
            MouseWheelEventArgs eventArgs = new(
                e.MouseDevice,
                e.Timestamp,
                e.Delta)
            {
                RoutedEvent = MouseWheelEvent,
                Source = sender
            };

            UIElement parent = (UIElement)((Control)sender).Parent;
            parent?.RaiseEvent(eventArgs);
        }

        private void CommandFailCancel_Clicked(object sender, RoutedEventArgs e)
        {
            _viewModel.IsLastCommandFailAcknowledged = true;

            NavigationService.Refresh();
        }
    }
}
