using FluentValidation;
using FluentValidation.Results;
using Models;
using Models.Entities;
using Models.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ViewModels.Validators;

namespace ViewModels
{
    public class EmployeesMenuViewModel : INotifyPropertyChanged, IEmployeeUpdatable
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private EmployeeRepository _employeeRepository;
        private IValidator<EmployeeViewModel> _employeeValidator;

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
        public EmployeesMenuViewModel(EmployeeRepository employeeRepository,
            IValidator<EmployeeViewModel> employeeValidator)
        {
            _employeeRepository = employeeRepository;
            _employeeValidator = employeeValidator;

            _employees = new ObservableCollection<EmployeeViewModel>();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task InitializeData()
        {
            List<EmployeeViewModel> employeeViewModels = (await _employeeRepository.GetAll()).ToListOfEmployeeViewModel();
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

            ValidationResult validationResult = _employeeValidator.Validate(_newEmployee);
            if (!validationResult.IsValid)
            {
                Employees.Remove(_newEmployee);
                _newEmployee = null;

                return;
            }

            Employee employeeToHire = new()
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

        public async void UpdateEmployee(object sender, PropertyChangedEventArgs e)
        {
            EmployeeViewModel changedEmployee = (EmployeeViewModel) sender;

            ValidationResult validationResult = _employeeValidator.Validate(changedEmployee);
            if (!validationResult.IsValid)
            {
                return;
            }


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

        public async Task RemoveEmployee(int id)
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
