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
    public class EmployeesMenuViewModel : INotifyPropertyChanged, IEmployeeUpdatable
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private EmployeeRepository _employeeRepository;
        private JobRepository _jobRepository;
        private DepartmentRepository _departmentRepository;


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

        public bool IsUpdatedEmployeeJobChanged => UpdatedEmployeePreviousJob?.JobId != UpdatedEmployee?.JobId;

        private Job? _updatedEmployeePreviousJob;
        public Job? UpdatedEmployeePreviousJob
        {
            get
            {
                return _updatedEmployeePreviousJob;
            }
            set
            {
                _updatedEmployeePreviousJob = value;
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


        private EmployeeDto? _newEmployee;
        public EmployeeDto? NewEmployee
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
        
        private bool _newEmployeeAlreadyExists;
        public bool NewEmployeeAlreadyExists
        {
            get
            {
                return _newEmployeeAlreadyExists;
            }
            set
            {
                _newEmployeeAlreadyExists = value;
                OnPropertyChanged();
            }
        }


        private EmployeeDto? _employeeToFire;
        public EmployeeDto? EmployeeToFire
        {
            get
            {
                return _employeeToFire;
            }
            set
            {
               _employeeToFire = value;
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

        private ObservableCollection<string> _jobs;
        public ObservableCollection<string> Jobs
        {
            get
            {
                return _jobs;
            }
            set
            {
                _jobs = value;
                OnPropertyChanged();
            }
        }


        public ICommand DeleteEmployeeCommand { get; }
        public ICommand CreateEmployeeCommand { get; }
        public ICommand UpdateEmployeeCommand { get; }

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public EmployeesMenuViewModel(EmployeeRepository employeeRepository,
            DepartmentRepository departmentRepository,
            JobRepository jobRepository,
            JobHistoryRepository jobHistoryRepository,
            IValidator<EmployeeDto> employeeValidator,
            IDateProvider dateProvider)
        {
            _jobRepository = jobRepository;
            _departmentRepository = departmentRepository;
            _employeeRepository = employeeRepository;

            _employees = new();
            _jobs = new();

            NewEmployeeAlreadyExists = false;
            IsLastCommandSuccessful = true;

            DeleteEmployeeCommand = new DeleteEmployeeCommand(this, _employeeRepository);
            CreateEmployeeCommand = new CreateEmployeeCommand(this, _employeeRepository, _departmentRepository, employeeValidator);
            UpdateEmployeeCommand = new UpdateEmployeeCommand(this, _employeeRepository, _departmentRepository, employeeValidator, dateProvider, jobHistoryRepository);
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task InitializeDataAsync()
        {
            NewEmployeeAlreadyExists = false;

            List<EmployeeDto> employeeViewModels = (await _employeeRepository.GetAllAsync()).ToListOfEmployeeViewModel();
            ObservableCollection<EmployeeDto> employees = new(employeeViewModels);
            Employees = employees;
            Employees.CollectionChanged += Employees_CollectionChanged;

            List<JobDto> jobViewModels = (await _jobRepository.GetAllAsync()).ToListOfJobViewModel();
            ObservableCollection<string> jobs = new(jobViewModels.Select(job => job.JobId));
            Jobs = jobs;
            Jobs.CollectionChanged += Jobs_CollectionChanged;

            foreach (EmployeeDto employee in Employees)
            {
                employee.PropertyChanging += EmployeeUpdating;
                employee.PropertyChanged += EmployeeUpdated;
            }

        }

        public async void EmployeeUpdating(object? sender, PropertyChangingEventArgs e)
        {
            if (sender is not null)
            {
                EmployeeDto employeeBeforeChange = (EmployeeDto)sender;
                UpdatedEmployeePreviousJob = (await _jobRepository.GetAllAsync())
                    .First(job => job.JobId == employeeBeforeChange.JobId);
            }
        }

        public void EmployeeUpdated(object? sender, PropertyChangedEventArgs e)
        {
            if (sender is not null)
            {
                UpdatedEmployee = (EmployeeDto)sender;

                if (UpdateEmployeeCommand.CanExecute(null))
                {
                    UpdateEmployeeCommand.Execute(null);
                }
            }

        }

        private int GenerateUniqueEmployeeId()
        {
            IEnumerable<int> employeeIds = Employees.Select(employee => employee.EmployeeId);

            if (employeeIds.Any())
            {
                return (employeeIds.Max() + 1);
            }

            return 1;
        }


        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Employees_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Employees"));

            if (e.NewItems is not null)
            {
                foreach (EmployeeDto employee in e.NewItems)
                {
                    employee.EmployeeId = GenerateUniqueEmployeeId();
                }
            }
            
        }

        private void Jobs_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Jobs"));
        }
    }
}
