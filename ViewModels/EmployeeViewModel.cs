using Core;
using Domain;
using Infrastructure;
using Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged
    {
        private int? _employeeId;
        public int? EmployeeId {
            get
            {
                return _employeeId;
            }
            set
            {
                _employeeId = value;
                OnPropertyChanged();
            }
        }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        private string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        private string _phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _hireDate;
        public DateTime? HireDate
        {
            get
            {
                return _hireDate;
            }
            set
            {
                _hireDate = value;
                OnPropertyChanged();
            }
        }

        private string _jobId;
        public string JobId
        {
            get
            {
                return _jobId;
            }
            set
            {
                _jobId = value;
                OnPropertyChanged();
            }
        }

        private double? _salary;
        public double? Salary
        {
            get
            {
                return _salary;
            }
            set
            {
                _salary = value;
                OnPropertyChanged();
            }
        }

        private Single? _commissionPct;
        public Single? CommissionPct
        {
            get
            {
                return _commissionPct;
            }
            set
            {
                _commissionPct = value;
                OnPropertyChanged();
            }
        }

        private int? _managerId;
        public int? ManagerId
        {
            get
            {
                return _managerId;
            }
            set
            {
                _managerId = value;
                OnPropertyChanged();
            }
        }

        private Int16? _departmentId;
        public Int16? DepartmentId
        {
            get
            {
                return _departmentId;
            }
            set
            {
                _departmentId = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        private LoginViewModel _loginViewModel;
        private EmployeeRepository _employeeRepository;


        public EmployeeViewModel()
        {
            _loginViewModel = LoginViewModel.GetInstance();
            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(_loginViewModel.UserName, _loginViewModel.Password);
            _employeeRepository = new(new OracleSQLDataAccess(connectionString));
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));


            if (EmployeeId is not null &&
                FirstName is not null &&
                LastName is not null &&
                Email is not null &&
                PhoneNumber is not null &&
                HireDate is not null &&
                JobId is not null &&
                Salary is not null &&
                DepartmentId is not null)
            {
                Employee employeeToUpdate = new Employee()
                {
                    EMPLOYEE_ID = EmployeeId,
                    FIRST_NAME = FirstName,
                    LAST_NAME = LastName,
                    EMAIL = Email,
                    PHONE_NUMBER = PhoneNumber,
                    HIRE_DATE = HireDate,
                    JOB_ID = JobId,
                    SALARY = Salary,
                    COMMISSION_PCT = CommissionPct,
                    MANAGER_ID = ManagerId,
                    DEPARTMENT_ID = DepartmentId
                };

                _employeeRepository.Update((int)EmployeeId, employeeToUpdate);
            }
        }

        public static List<EmployeeViewModel> ToListOfEmployeeViewModel(IEnumerable<Employee> employees)
        {
            List<EmployeeViewModel> result = new List<EmployeeViewModel>();

            foreach (Employee employee in employees)
            {
                EmployeeViewModel employeeViewModel = new()
                {
                    EmployeeId = employee.EMPLOYEE_ID,
                    FirstName = employee.FIRST_NAME,
                    LastName = employee.LAST_NAME,
                    Email = employee.EMAIL,
                    PhoneNumber = employee.PHONE_NUMBER,
                    HireDate = employee.HIRE_DATE,
                    JobId = employee.JOB_ID,
                    Salary = employee.SALARY,
                    CommissionPct = employee.COMMISSION_PCT,
                    ManagerId = employee.MANAGER_ID,
                    DepartmentId = employee.DEPARTMENT_ID
                };
                result.Add(employeeViewModel);
            }

            return result;
        }
    }
}
