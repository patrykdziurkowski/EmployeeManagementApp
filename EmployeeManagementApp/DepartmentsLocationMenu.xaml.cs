﻿using System;
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
using ViewModels;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Logika interakcji dla klasy DepartmentsLocationMenu.xaml
    /// </summary>
    public partial class DepartmentsLocationMenu : Page
    {
        public DepartmentsLocationMenu()
        {
            InitializeComponent();

            DepartmentLocationMenuViewModel viewModel = new();
            DataContext = viewModel;
            DepartmentLocationTable.ItemsSource = viewModel.DepartmentLocation;
        }

        private void ReturnToDepartmentsMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
