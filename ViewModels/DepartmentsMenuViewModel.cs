﻿using Models;
using Models.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
{
    public class DepartmentsMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private LoginViewModel _loginViewModel;


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


        private ObservableCollection<EmployeeViewModel> _employees;
        public ObservableCollection<EmployeeViewModel> Employees
        {
            get
            {
                return _employees;
            }
            set
            {
                _employees = value;
                OnPropertyChanged();
            }
        }


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentsMenuViewModel()
        {
            _loginViewModel = LoginViewModel.GetInstance();
            _departments = new ObservableCollection<DepartmentViewModel>();
            _employees = new ObservableCollection<EmployeeViewModel>();

            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(_loginViewModel.UserName, _loginViewModel.Password);
            OracleSQLDataAccess dataAccess = new(connectionString);

            DepartmentRepository departmentRepository = new(dataAccess);
            EmployeeRepository employeeRepository = new(dataAccess);

            List<DepartmentViewModel> departmentViewModels = DepartmentViewModel
                .ToListOfDepartmentViewModel(departmentRepository.GetAll());
            ObservableCollection<DepartmentViewModel> departments = new ObservableCollection<DepartmentViewModel>(departmentViewModels);
            List<EmployeeViewModel> employeeViewModels = EmployeeViewModel
                .ToListOfEmployeeViewModel(employeeRepository.GetAll());
            ObservableCollection<EmployeeViewModel> employees = new ObservableCollection<EmployeeViewModel>(employeeViewModels);

            _employees = employees;
            _employees.CollectionChanged += Employees_CollectionChanged;
            _departments = departments;
            _departments.CollectionChanged += Departments_CollectionChanged;

            
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public void UpdateEmployeesDepartments(EmployeeViewModel employeeToUpdate, int targetDepartmentId)
        {
            ObservableCollection<EmployeeViewModel> employeesAfterFiltering = new();
            ObservableCollection<EmployeeViewModel> employeesToFilter = _departments
                .FirstOrDefault(department => department.DepartmentId == employeeToUpdate.DepartmentId)
                .Employees;

            foreach(EmployeeViewModel employee in employeesToFilter)
            {
                if (employee.EmployeeId != employeeToUpdate.EmployeeId)
                {
                    employeesAfterFiltering.Add(employee);
                }
            }
            _departments
                .FirstOrDefault(department => department.DepartmentId == employeeToUpdate.DepartmentId)
                .Employees = employeesAfterFiltering;

            employeeToUpdate.DepartmentId = (short?)targetDepartmentId;
            _departments
                .FirstOrDefault(department => department.DepartmentId == targetDepartmentId)
                .Employees
                .Add(employeeToUpdate); 
        }

        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Departments_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Departments"));
        }

        private void Employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Employees"));
        }
    }
}


