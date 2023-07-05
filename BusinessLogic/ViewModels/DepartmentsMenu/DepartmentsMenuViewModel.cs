using FluentValidation;
using FluentValidation.Results;
using DataAccess.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DataAccess.Models;

namespace BusinessLogic.ViewModels
{
    public class DepartmentsMenuViewModel : INotifyPropertyChanged, IEmployeeUpdatable
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private DepartmentRepository _departmentRepository;
        private EmployeeRepository _employeeRepository;
        private IValidator<EmployeeViewModel> _employeeValidator;

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
        public DepartmentsMenuViewModel(DepartmentRepository departmentRepository,
            EmployeeRepository employeeRepository,
            IValidator<EmployeeViewModel> employeeVaidator)
        {
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;
            _employeeValidator = employeeVaidator;

            _departments = new ObservableCollection<DepartmentViewModel>();
            _employees = new ObservableCollection<EmployeeViewModel>(); 
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task InitializeData()
        {
            List<DepartmentViewModel> departmentViewModels = (await _departmentRepository.GetAll()).ToListOfDepartmentViewModel();
            ObservableCollection<DepartmentViewModel> departments = new ObservableCollection<DepartmentViewModel>(departmentViewModels);

            List<EmployeeViewModel> employeeViewModels = (await _employeeRepository.GetAll()).ToListOfEmployeeViewModel();
            ObservableCollection<EmployeeViewModel> employees = new ObservableCollection<EmployeeViewModel>(employeeViewModels);

            Employees = employees;
            Departments = departments;

            foreach(DepartmentViewModel department in Departments)
            {
                List<EmployeeViewModel> employeeDepartmentViewModels = (await _departmentRepository.GetEmployeesForDepartment((int)department.DepartmentId)).ToListOfEmployeeViewModel();
                department.Employees = new ObservableCollection<EmployeeViewModel>(employeeDepartmentViewModels);

                department.Employees.CollectionChanged += Employees_CollectionChanged;
            }

            _employees.CollectionChanged += Employees_CollectionChanged;
            _departments.CollectionChanged += Departments_CollectionChanged;

            foreach (EmployeeViewModel employee in Employees)
            {
                employee.PropertyChanged += EmployeeUpdated;
            }
        }


        public async void EmployeeUpdated(object sender, PropertyChangedEventArgs e)
        {
            EmployeeViewModel changedEmployee = (EmployeeViewModel)sender;

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
                HireDate = changedEmployee.HireDate.Value.ToDateTime(TimeOnly.MinValue),
                JobId = changedEmployee.JobId,
                Salary = changedEmployee.Salary,
                CommissionPct = changedEmployee.CommissionPct,
                ManagerId = changedEmployee.ManagerId,
                DepartmentId = changedEmployee.DepartmentId
            };

            _employeeRepository.Update((int)changedEmployee.EmployeeId, employeeToUpdate);
        }

        public void UpdateEmployeesDepartments(EmployeeViewModel employeeToUpdate, int targetDepartmentId)
        {
            foreach (DepartmentViewModel department in Departments)
            {
                ObservableCollection<EmployeeViewModel> employeesToFilter = department.Employees;

                ObservableCollection<EmployeeViewModel> employeesToKeep = new();
                foreach(EmployeeViewModel employee in department.Employees)
                {
                    if (employee.EmployeeId != employeeToUpdate.EmployeeId)
                    {
                        employeesToKeep.Add(employee);
                    }
                }
                department.Employees = employeesToKeep;
            }

            employeeToUpdate.DepartmentId = (short?)targetDepartmentId;

             Employees
                .FirstOrDefault(employee => employee.EmployeeId == employeeToUpdate.EmployeeId)
                .DepartmentId = (short?)targetDepartmentId;
            

            Departments
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


