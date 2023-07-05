﻿using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLogic.ViewModels
{
    public class EmployeeViewModel : INotifyPropertyChanged, INotifyPropertyChanging
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private IDateProvider _dateProvider;
        
        private int? _employeeId;
        public int? EmployeeId {
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

        private string _firstName;
        public string FirstName
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

        private string _phoneNumber;
        public string PhoneNumber
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

        private DateOnly? _hireDate;
        public DateOnly? HireDate
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

        private Single? _commissionPct;
        public Single? CommissionPct
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

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public EmployeeViewModel()
        {
            _dateProvider = new DateProvider();

            HireDate = _dateProvider.GetNow();
        }

        public EmployeeViewModel(IDateProvider dateProvider)
        {
            _dateProvider = dateProvider;

            HireDate = _dateProvider.GetNow();
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////

  

        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        protected async Task OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        protected async Task OnPropertyChanging([CallerMemberName] string name = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(name));
        }
        
    }
}