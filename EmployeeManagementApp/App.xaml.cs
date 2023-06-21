using Microsoft.Extensions.DependencyInjection;
using Models;
using Models.Repositories;
using System.Windows;
using ViewModels;

namespace EmployeeManagementApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
        }
        
        private void ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<UserCredentials>();
            services.AddSingleton<ConnectionStringProvider>();
            services.AddSingleton<ISQLDataAccess, OracleSQLDataAccess>();

            services.AddTransient<EmployeeRepository>();
            services.AddTransient<DepartmentRepository>();
            services.AddTransient<DepartmentLocationRepository>();
            services.AddTransient<JobHistoryRepository>();

            services.AddSingleton<StartMenuViewModel>();
            services.AddTransient<EmployeesMenuViewModel>();
            services.AddTransient<SalariesMenuViewModel>();
            services.AddTransient<DepartmentsMenuViewModel>();
            services.AddTransient<DepartmentLocationMenuViewModel>();
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

        protected override async void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}
