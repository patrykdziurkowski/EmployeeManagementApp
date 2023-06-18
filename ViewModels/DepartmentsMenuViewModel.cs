using Core;
using Domain;
using Infrastructure;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class DepartmentsMenuViewModel : INotifyPropertyChanged
    {
        private LoginViewModel _loginViewModel;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private ObservableCollection<DepartmentViewModel> _departments;

        public ObservableCollection<DepartmentViewModel> Departments
        {
            get
            {
                return _departments;
            }
            set
            {
                _departments = value;
                OnPropertyChanged();
            }
        }

        public DepartmentsMenuViewModel()
        {
            _loginViewModel = LoginViewModel.GetInstance();
            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(_loginViewModel.UserName, _loginViewModel.Password);
            EmployeeRepository employeeRepository = new(new OracleSQLDataAccess(connectionString));

            _departments = new ObservableCollection<DepartmentViewModel>();
            List<DepartmentViewModel> DepartmentViewModels = DepartmentViewModel
                .ToListOfDepartmentViewModel(employeeRepository
                .GetAll());
            ObservableCollection<DepartmentViewModel> Departments = new ObservableCollection<DepartmentViewModel>(DepartmentViewModels);
            _departments = Departments;
            _departments.CollectionChanged += Departments_CollectionChanged;
        }

        private void Departments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Departments"));
        }
    }
}


