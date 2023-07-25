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

            UpdateDepartmentCommand = new UpdateDepartmentCommand(this, _employeeRepository);
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task InitializeDataAsync()
        {
            List<DepartmentDto> departmentDtos = (await _departmentRepository.GetAllAsync()).ToListOfDepartmentDto();
            ObservableCollection<DepartmentDto> departments = new(departmentDtos);

            List<EmployeeDto> employeeDtos = (await _employeeRepository.GetAllAsync()).ToListOfEmployeeDto();
            ObservableCollection<EmployeeDto> employees = new(employeeDtos);

            Employees = employees;
            Departments = departments;

            foreach(DepartmentDto department in Departments)
            {
                List<EmployeeDto> departmentEmployeesDtos = (await _departmentRepository.GetEmployeesForDepartmentAsync(department.DepartmentId)).ToListOfEmployeeDto();
                department.Employees = new(departmentEmployeesDtos);

                department.Employees.CollectionChanged += Employees_CollectionChanged;
            }

            _employees.CollectionChanged += Employees_CollectionChanged;
            _departments.CollectionChanged += Departments_CollectionChanged;

            foreach (EmployeeDto employee in Employees)
            {
                employee.PropertyChanged += EmployeeUpdated;
            }
        }


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

        private void Departments_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Departments"));
        }

        private void Employees_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Employees"));
        }
    }
}


