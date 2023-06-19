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
        private LoginViewModel _loginViewModel;

        private readonly ObservableCollection<EmployeeViewModel> _employees;
        public ObservableCollection<EmployeeViewModel> Employees
        {
            get
            {
                return _employees;
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
        public EmployeesMenuViewModel()
        {
            _loginViewModel = LoginViewModel.GetInstance();
            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(_loginViewModel.UserName, _loginViewModel.Password);
            _employeeRepository = new(new OracleSQLDataAccess(connectionString));

            _employees = new ObservableCollection<EmployeeViewModel>();
            List<EmployeeViewModel> employeeViewModels = EmployeeViewModel
                .ToListOfEmployeeViewModel(_employeeRepository.GetAll());
            ObservableCollection<EmployeeViewModel> employees = new ObservableCollection<EmployeeViewModel>(employeeViewModels);

            _employees = employees;
            _employees.CollectionChanged += Employees_CollectionChanged;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public void AddEmployee()
        {
            _newEmployee = Employees.LastOrDefault();
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
            //TODO: validate params
            _employeeRepository.Hire(employeeToHire);

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
