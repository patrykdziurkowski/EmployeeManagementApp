using BusinessLogic.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLogic.ViewModels
{
    public class EmployeeDto : INotifyPropertyChanged, INotifyPropertyChanging
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////

        private int _employeeId;
        public int EmployeeId {
            get
            {
                return _employeeId;
            }
            set
            {
                OnPropertyChanging();
                _employeeId = value;
                OnPropertyChanged();
            }
        }

        private string? _firstName;
        public string? FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                OnPropertyChanging();
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
                OnPropertyChanging();
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
                OnPropertyChanging();
                _email = value;
                OnPropertyChanged();
            }
        }

        private string? _phoneNumber;
        public string? PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                OnPropertyChanging();
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        private DateOnly _hireDate;
        public DateOnly HireDate
        {
            get
            {
                return _hireDate;
            }
            set
            {
                OnPropertyChanging();
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
                OnPropertyChanging();
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
                OnPropertyChanging();
                _salary = value;
                OnPropertyChanged();
            }
        }

        private float? _commissionPct;
        public float? CommissionPct
        {
            get
            {
                return _commissionPct;
            }
            set
            {
                OnPropertyChanging();
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
                OnPropertyChanging();
                _managerId = value;
                OnPropertyChanged();
            }
        }

        private short? _departmentId;
        public short? DepartmentId
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

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        //Empty constructor is required for datagrid initialization
        public EmployeeDto()
        {
            _employeeId = 0;
            _lastName = string.Empty;
            _email = string.Empty;
            _jobId = string.Empty;
            HireDate = new DateProvider().GetNow();
        }
        
        public EmployeeDto(
            int employeeId,
            string lastName,
            string email,
            string jobId)
        {
            _employeeId = employeeId;
            _lastName = lastName;
            _email = email;
            _jobId = jobId;
            HireDate = new DateProvider().GetNow();
        }

        public EmployeeDto(
            int employeeId,
            string lastName,
            string email,
            string jobId,
            IDateProvider dateProvider)
        {
            _employeeId = employeeId;
            _lastName = lastName;
            _email = email;
            _jobId = jobId;
            HireDate = dateProvider.GetNow();
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////

  

        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        protected void OnPropertyChanging([CallerMemberName] string? name = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
        }
        
    }
}
