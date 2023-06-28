using Models;
using Models.Entities;
using Models.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
{
    public class DepartmentsMenuViewModel : INotifyPropertyChanged, IEmployeeUpdatable
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private DepartmentRepository _departmentRepository;
        private EmployeeRepository _employeeRepository;

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
            EmployeeRepository employeeRepository)
        {
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;

            _departments = new ObservableCollection<DepartmentViewModel>();
            _employees = new ObservableCollection<EmployeeViewModel>(); 
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task InitializeData()
        {
            List<DepartmentViewModel> departmentViewModels = DepartmentViewModel
                .ToListOfDepartmentViewModel(await _departmentRepository.GetAll());
            ObservableCollection<DepartmentViewModel> departments = new ObservableCollection<DepartmentViewModel>(departmentViewModels);

            List<EmployeeViewModel> employeeViewModels = EmployeeViewModel
                .ToListOfEmployeeViewModel(await _employeeRepository.GetAll());
            ObservableCollection<EmployeeViewModel> employees = new ObservableCollection<EmployeeViewModel>(employeeViewModels);

            Employees = employees;
            Departments = departments;

            foreach(DepartmentViewModel department in Departments)
            {
                List<EmployeeViewModel> employeeDepartmentViewModels = EmployeeViewModel
                    .ToListOfEmployeeViewModel(await _departmentRepository.GetEmployeesForDepartment((int)department.DepartmentId));
                department.Employees = new ObservableCollection<EmployeeViewModel>(employeeDepartmentViewModels);

                department.Employees.CollectionChanged += Employees_CollectionChanged;
            }

            _employees.CollectionChanged += Employees_CollectionChanged;
            _departments.CollectionChanged += Departments_CollectionChanged;

            foreach (EmployeeViewModel employee in Employees)
            {
                employee.PropertyChanged += UpdateEmployee;
            }
        }


        public async void UpdateEmployee(object sender, PropertyChangedEventArgs e)
        {
            EmployeeViewModel changedEmployee = (EmployeeViewModel)sender;

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


