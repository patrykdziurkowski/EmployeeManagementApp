using Models;
using Models.Entities;
using Models.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
{
    public class EmployeesMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private EmployeeRepository _employeeRepository;

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

        private EmployeeViewModel _newEmployee;
        public EmployeeViewModel NewEmployee
        {
            get
            {
                return _newEmployee;
            }
            set
            {
                _newEmployee = value;
                OnPropertyChanged();
            }
        }




        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public EmployeesMenuViewModel(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

            _employees = new ObservableCollection<EmployeeViewModel>();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task InitializeData()
        {
            List<EmployeeViewModel> employeeViewModels = EmployeeViewModel
                .ToListOfEmployeeViewModel(await _employeeRepository.GetAll());
            ObservableCollection<EmployeeViewModel> employees = new ObservableCollection<EmployeeViewModel>(employeeViewModels);

            Employees = employees;
            Employees.CollectionChanged += Employees_CollectionChanged;
            
            foreach (EmployeeViewModel employee in Employees)
            {
                employee.PropertyChanged += UpdateEmployee;
            }
        }

        public void AddEmployee()
        {
            _newEmployee = Employees.LastOrDefault();
            if (_newEmployee.EmployeeId is not null &&
                _newEmployee.FirstName is not null &&
                _newEmployee.LastName is not null &&
                _newEmployee.Email is not null &&
                _newEmployee.PhoneNumber is not null &&
                _newEmployee.HireDate is not null &&
                _newEmployee.JobId is not null &&
                _newEmployee.Salary is not null &&
                _newEmployee.DepartmentId is not null)
            {
                Employee employeeToHire = new Employee()
                {
                    EmployeeId = _newEmployee.EmployeeId,
                    FirstName = _newEmployee.FirstName,
                    LastName = _newEmployee.LastName,
                    Email = _newEmployee.Email,
                    PhoneNumber = _newEmployee.PhoneNumber,
                    HireDate = _newEmployee.HireDate,
                    JobId = _newEmployee.JobId,
                    Salary = _newEmployee.Salary,
                    CommissionPct = _newEmployee.CommissionPct,
                    ManagerId = _newEmployee.ManagerId,
                    DepartmentId = _newEmployee.DepartmentId
                };

                _employeeRepository.Hire(employeeToHire);
            }
        }

        public async void UpdateEmployee(object sender, PropertyChangedEventArgs e)
        {
            EmployeeViewModel changedEmployee = (EmployeeViewModel) sender;

            if (changedEmployee.EmployeeId is not null &&
                changedEmployee.FirstName is not null &&
                changedEmployee.LastName is not null &&
                changedEmployee.Email is not null &&
                changedEmployee.PhoneNumber is not null &&
                changedEmployee.HireDate is not null &&
                changedEmployee.JobId is not null &&
                changedEmployee.Salary is not null &&
                changedEmployee.DepartmentId is not null)
            {
                Employee employeeToUpdate = new Employee()
                {
                    EmployeeId = changedEmployee.EmployeeId,
                    FirstName = changedEmployee.FirstName,
                    LastName = changedEmployee.LastName,
                    Email = changedEmployee.Email,
                    PhoneNumber = changedEmployee.PhoneNumber,
                    HireDate = changedEmployee.HireDate,
                    JobId = changedEmployee.JobId,
                    Salary = changedEmployee.Salary,
                    CommissionPct = changedEmployee.CommissionPct,
                    ManagerId = changedEmployee.ManagerId,
                    DepartmentId = changedEmployee.DepartmentId
                };

                _employeeRepository.Update((int)changedEmployee.EmployeeId, employeeToUpdate);
            }
        }

        public async void RemoveEmployee(int id)
        {
            bool employeeWasRemovedFromDatabase = await _employeeRepository.Fire(id);

            if (employeeWasRemovedFromDatabase)
            {
                EmployeeViewModel employeeToRemove = Employees
                .FirstOrDefault(employee => employee.EmployeeId == id);
                Employees.Remove(employeeToRemove);
            }
        }

        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Employees"));
        }


    }
}
