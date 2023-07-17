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

        private EmployeeViewModel? _updatedEmployee;
        public EmployeeViewModel? UpdatedEmployee
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

            _departments = new ObservableCollection<DepartmentViewModel>();
            _employees = new ObservableCollection<EmployeeViewModel>();

            IsLastCommandSuccessful = true;

            UpdateDepartmentCommand = new UpdateDepartmentCommand(this, _employeeRepository);
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
                List<EmployeeViewModel> employeeDepartmentViewModels = (await _departmentRepository.GetEmployeesForDepartment(department.DepartmentId)).ToListOfEmployeeViewModel();
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


        public void EmployeeUpdated(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not null)
            {
                UpdatedEmployee = (EmployeeViewModel)sender;

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


