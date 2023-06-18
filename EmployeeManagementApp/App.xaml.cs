using Infrastructure;
using Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ViewModels;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IHost? AppHost { get; private set; }
        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<MainWindow>();

                    services.AddTransient<StartMenu>();
                    services.AddTransient<SalariesMenu>();
                    services.AddTransient<DepartmentsMenu>();
                    services.AddTransient<EmployeesMenu>();
                    services.AddTransient<MainMenu>();

                    services.AddTransient<ISQLDataAccess, OracleSQLDataAccess>();
                    services.AddTransient<EmployeeRepository>();
                    services.AddSingleton<LoginViewModel>();
                })
                .Build();
        }
        
        protected override async void OnStartup(StartupEventArgs e)
        {

            await AppHost!.StartAsync();

            //Start the window
            var mainWindow = AppHost.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost!.StopAsync();
            base.OnExit(e);
        }
    }
}
