using FluentValidation;
using FluentValidation.Results;
using DataAccess.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using DataAccess.Models;
using System.Windows.Input;
using BusinessLogic.Commands;
using BusinessLogic.Interfaces;

namespace BusinessLogic.ViewModels
{
    public class DepartmentsMenuViewModel : INotifyPropertyChanged, IEmployeeUpdatable
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private DepartmentRepository _departmentRepository;
        private EmployeeRepository _employeeRepository;

        private ObservableCollection<DepartmentDto> _departments;
        public ObservableCollection<DepartmentDto> Departments
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


        private ObservableCollection<EmployeeDto> _employees;
        public ObservableCollection<EmployeeDto> Employees
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

        private bool _isLastCommandSuccessful;
        public bool IsLastCommandSuccessful
        {
            get
            {
                return _isLastCommandSuccessful;
            }

            set
            {
                _isLastCommandSuccessful = value;
                IsLastCommandFailAcknowledged = _isLastCommandSuccessful;

                OnPropertyChanged();
            }
        }

        private bool _isLastCommandFailAcknowledged;
        public bool IsLastCommandFailAcknowledged
        {
            get
            {
                return _isLastCommandFailAcknowledged;
            }

            set
            {
                _isLastCommandFailAcknowledged = value;
                OnPropertyChanged();
            }
        }

        private string? _commandFailMessage;
        public string? CommandFailMessage
        {
            get
            {
                return _commandFailMessage;
            }
            set
            {
                _commandFailMessage = value;
                OnPropertyChanged();
            }
        }

        private EmployeeDto? _updatedEmployee;
        public EmployeeDto? UpdatedEmployee
        {
            get
            {
                return _updatedEmployee;
            }

            set
            {
                _updatedEmployee = value;
            }
        }

        public ICommand UpdateDepartmentCommand { get; }
        public ICommand LoadDepartmentsCommand { get; }

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentsMenuViewModel(DepartmentRepository departmentRepository,
            EmployeeRepository employeeRepository)
        {
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;

            _departments = new();
            _employees = new();

            IsLastCommandSuccessful = true;

            LoadDepartmentsCommand = new LoadDepartmentsCommand(this, departmentRepository, employeeRepository);
            UpdateDepartmentCommand = new UpdateDepartmentCommand(this, _employeeRepository);
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////


        public void EmployeeUpdated(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not null)
            {
                UpdatedEmployee = (EmployeeDto)sender;

                if (UpdateDepartmentCommand.CanExecute(null))
                {
                    UpdateDepartmentCommand.Execute(null);
                }
            }
        }

        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public void Departments_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Departments)));
        }

        public void Employees_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Employees)));
        }
    }
}


