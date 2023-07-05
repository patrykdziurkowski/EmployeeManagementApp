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
        private JobHistoryRepository _jobHistoryRepository;
        private IValidator<EmployeeViewModel> _employeeValidator;
        private IDateProvider _dateProvider;

        private Job _updatedEmployeePreviousJob;

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

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public EmployeesMenuViewModel(EmployeeRepository employeeRepository,
            JobRepository jobRepository,
            JobHistoryRepository jobHistoryRepository,
            IValidator<EmployeeViewModel> employeeValidator,
            IDateProvider dateProvider)
        {
            _jobHistoryRepository = jobHistoryRepository;
            _jobRepository = jobRepository;
            _employeeRepository = employeeRepository;
            _employeeValidator = employeeValidator;
            _dateProvider = dateProvider;

            _employees = new ObservableCollection<EmployeeViewModel>();
            _jobs = new ObservableCollection<string>();

            DeleteEmployeeCommand = new DeleteEmployeeCommand(this, _employeeRepository);
            CreateEmployeeCommand = new CreateEmployeeCommand(this, _employeeRepository, _employeeValidator);
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
                employee.PropertyChanging += GetPreviousJob;
                employee.PropertyChanged += UpdateEmployee;
            }

        }

        public async void GetPreviousJob(object sender, PropertyChangingEventArgs e)
        {
            EmployeeViewModel employeeBeforeChange = (EmployeeViewModel) sender;
            _updatedEmployeePreviousJob = (await _jobRepository.GetAll())
                .FirstOrDefault(job => job.JobId == employeeBeforeChange.JobId);
        }

        public async void UpdateEmployee(object sender, PropertyChangedEventArgs e)
        {
            EmployeeViewModel changedEmployee = (EmployeeViewModel) sender;

            ValidationResult validationResult = _employeeValidator.Validate(changedEmployee);
            if (!validationResult.IsValid)
            {
                return;
            }

            Employee employeeToUpdate = new Employee()
            {
                EmployeeId = changedEmployee.EmployeeId,
                FirstName = changedEmployee.FirstName,
                LastName = changedEmployee.LastName,
                Email = changedEmployee.Email,
                PhoneNumber = changedEmployee.PhoneNumber,
                HireDate = changedEmployee.HireDate.Value.ToDateTime(TimeOnly.MinValue),
                JobId = changedEmployee.JobId,
                Salary = changedEmployee.Salary,
                CommissionPct = changedEmployee.CommissionPct,
                ManagerId = changedEmployee.ManagerId,
                DepartmentId = changedEmployee.DepartmentId
            };

            if (_updatedEmployeePreviousJob.JobId != employeeToUpdate.JobId)
            {
                await CreateJobHistoryEntry(employeeToUpdate);
            }

            _employeeRepository.Update((int)changedEmployee.EmployeeId, employeeToUpdate);
        }

        private async Task CreateJobHistoryEntry(Employee employeeToUpdate)
        {
            DateTime? lastJobStartdate = (await _jobHistoryRepository.GetAll())
                                .Where(jobHistoryEntry => jobHistoryEntry.EmployeeId == employeeToUpdate.EmployeeId)
                                .Select(jobHistoryEntry => jobHistoryEntry.EndDate)
                                .Max();
            DateOnly? updatedJobStartDate = DateOnly.FromDateTime(lastJobStartdate.Value);

            if (updatedJobStartDate is null)
            {
                updatedJobStartDate = DateOnly.FromDateTime(employeeToUpdate.HireDate.Value);
            }

            JobHistory jobHistoryEntry = new()
            {
                EmployeeId = employeeToUpdate.EmployeeId,
                StartDate = lastJobStartdate,
                EndDate = _dateProvider.GetNow().ToDateTime(TimeOnly.MinValue),
                JobId = _updatedEmployeePreviousJob.JobId,
                DepartmentId = employeeToUpdate.DepartmentId
            };

            _jobHistoryRepository.Insert(jobHistoryEntry);
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
