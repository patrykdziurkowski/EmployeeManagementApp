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


        private bool _isLastEmployeeUpdateSuccessful;
        public bool IsLastEmployeeUpdateSuccessful
        {
            get
            {
                return _isLastEmployeeUpdateSuccessful;
            }

            set
            {
                _isLastEmployeeUpdateSuccessful = value;
                IsLastUpdateFailAcknowledged = _isLastEmployeeUpdateSuccessful;

                OnPropertyChanged();
            }
        }
        
        private bool _isLastUpdateFailAcknowledged;
        public bool IsLastUpdateFailAcknowledged
        {
            get
            {
                return _isLastUpdateFailAcknowledged;
            }

            set
            {
                _isLastUpdateFailAcknowledged = value;
                OnPropertyChanged();
            }
        }
        
        public bool IsUpdatedEmployeeJobChanged => UpdatedEmployeePreviousJob.JobId != UpdatedEmployee.JobId;

        private Job _updatedEmployeePreviousJob;
        public Job UpdatedEmployeePreviousJob
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

        private EmployeeViewModel _updatedEmployee;
        public EmployeeViewModel UpdatedEmployee
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


        private EmployeeViewModel _employeeToFire;
        public EmployeeViewModel EmployeeToFire
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
            JobRepository jobRepository,
            JobHistoryRepository jobHistoryRepository,
            IValidator<EmployeeViewModel> employeeValidator,
            IDateProvider dateProvider)
        {
            _jobRepository = jobRepository;
            _employeeRepository = employeeRepository;

            _employees = new ObservableCollection<EmployeeViewModel>();
            _jobs = new ObservableCollection<string>();

            IsLastEmployeeUpdateSuccessful = true;

            DeleteEmployeeCommand = new DeleteEmployeeCommand(this, _employeeRepository);
            CreateEmployeeCommand = new CreateEmployeeCommand(this, _employeeRepository, employeeValidator);
            UpdateEmployeeCommand = new UpdateEmployeeCommand(this, _employeeRepository, employeeValidator, dateProvider, jobHistoryRepository);
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task InitializeData()
        {
            List<EmployeeViewModel> employeeViewModels = (await _employeeRepository.GetAll()).ToListOfEmployeeViewModel();
            ObservableCollection<EmployeeViewModel> employees = new ObservableCollection<EmployeeViewModel>(employeeViewModels);
            Employees = employees;
            Employees.CollectionChanged += Employees_CollectionChanged;

            List<JobViewModel> jobViewModels = (await _jobRepository.GetAll()).ToListOfJobViewModel();
            ObservableCollection<string> jobs = new ObservableCollection<string>(jobViewModels.Select(job => job.JobId));
            Jobs = jobs;
            Jobs.CollectionChanged += Jobs_CollectionChanged;

            foreach (EmployeeViewModel employee in Employees)
            {
                employee.PropertyChanging += EmployeeUpdating;
                employee.PropertyChanged += EmployeeUpdated;
            }

        }

        public async void EmployeeUpdating(object sender, PropertyChangingEventArgs e)
        {
            EmployeeViewModel employeeBeforeChange = (EmployeeViewModel) sender;
            UpdatedEmployeePreviousJob = (await _jobRepository.GetAll())
                .FirstOrDefault(job => job.JobId == employeeBeforeChange.JobId);
        }

        public async void EmployeeUpdated(object sender, PropertyChangedEventArgs e)
        {
            EmployeeViewModel changedEmployee = (EmployeeViewModel) sender;
            UpdatedEmployee = changedEmployee;

            if (UpdateEmployeeCommand.CanExecute(null))
            {
                UpdateEmployeeCommand.Execute(null);
            }
            
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

        private void Jobs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Jobs"));
        }
    }
}
