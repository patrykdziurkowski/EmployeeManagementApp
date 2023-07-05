﻿using BusinessLogic.ViewModels;
using Presentation.Views;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for EmployeesMenu.xaml
    /// </summary>
    public partial class EmployeesMenu : Page
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private EmployeesMenuViewModel _viewModel;
        private JobHistoryMenu _jobHistoryMenu;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public EmployeesMenu(
            EmployeesMenuViewModel viewModel,
            JobHistoryMenu jobHistoryMenu)
        {
            _jobHistoryMenu = jobHistoryMenu;
            _viewModel = viewModel;

            InitializeComponent();
            OverlayContentControl.Content = new LoadingUserControl();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        private void ReturnToMainMenu(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void Click_Navigate_JobHistory(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(_jobHistoryMenu);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            OverlayContentControl.Visibility = Visibility.Visible;
            await Task.Run(() => _viewModel.InitializeData());
            OverlayContentControl.Visibility = Visibility.Hidden;


            DataContext = _viewModel;
            EmployeesTable.ItemsSource = _viewModel.Employees;
            JobsComboBox.ItemsSource = _viewModel.Jobs;
        }
    }
}
