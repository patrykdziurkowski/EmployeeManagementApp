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
        public void InitializeData()
        {
            List<EmployeeViewModel> employeeViewModels = EmployeeViewModel
                .ToListOfEmployeeViewModel(_employeeRepository.GetAll(), _employeeRepository);
            ObservableCollection<EmployeeViewModel> employees = new ObservableCollection<EmployeeViewModel>(employeeViewModels);

            Employees = employees;
            Employees.CollectionChanged += Employees_CollectionChanged;
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
                    EMPLOYEE_ID = _newEmployee.EmployeeId,
                    FIRST_NAME = _newEmployee.FirstName,
                    LAST_NAME = _newEmployee.LastName,
                    EMAIL = _newEmployee.Email,
                    PHONE_NUMBER = _newEmployee.PhoneNumber,
                    HIRE_DATE = _newEmployee.HireDate,
                    JOB_ID = _newEmployee.JobId,
                    SALARY = _newEmployee.Salary,
                    COMMISSION_PCT = _newEmployee.CommissionPct,
                    MANAGER_ID = _newEmployee.ManagerId,
                    DEPARTMENT_ID = _newEmployee.DepartmentId
                };

                _employeeRepository.Hire(employeeToHire);
            }
        }
        

        public void RemoveEmployee(int id)
        {
            _employeeRepository.Fire(id);
            EmployeeViewModel employeeToRemove = Employees
                .FirstOrDefault(employee => employee.EmployeeId == id);
            Employees.Remove(employeeToRemove);
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
