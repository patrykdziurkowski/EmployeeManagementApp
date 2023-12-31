﻿using BusinessLogic;
using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();

            Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
        }
        
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddTransient<IEmployeeValidatorFactory, EmployeeValidatorFactory>();
            services.AddTransient<IDateProvider, DateProvider>();

            services.AddSingleton<UserCredentials>();
            services.AddSingleton<ConnectionStringProvider>();
            services.AddSingleton<ISqlDataAccess, OracleSqlDataAccess>();
            services.AddSingleton<IConnectionFactory, ConnectionFactory>();
            services.AddSingleton<IDapperAdapter, DapperAdapter>();

            services.AddTransient<JobRepository>();
            services.AddTransient<EmployeeRepository>();
            services.AddTransient<DepartmentRepository>();
            services.AddTransient<DepartmentLocationRepository>();
            services.AddTransient<JobHistoryRepository>();

            services.AddSingleton<StartMenuViewModel>();
            services.AddTransient<EmployeesMenuViewModel>();
            services.AddTransient<SalariesMenuViewModel>();
            services.AddTransient<DepartmentsMenuViewModel>();
            services.AddTransient<DepartmentLocationsMenuViewModel>();
            services.AddTransient<JobHistoryMenuViewModel>();

            services.AddTransient<DepartmentsLocationMenu>();
            services.AddTransient<JobHistoryMenu>();

            services.AddTransient<EmployeesMenu>();
            services.AddTransient<SalariesMenu>();
            services.AddTransient<DepartmentsMenu>();

            services.AddTransient<MainMenu>();

            services.AddTransient<StartMenu>();

            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
