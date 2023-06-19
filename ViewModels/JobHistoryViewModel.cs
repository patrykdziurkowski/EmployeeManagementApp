using Models;
using Models.Entities;
using Models.Repositories;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
{
    public class JobHistoryViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
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

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get
            {
                return _endDate;
            }
            set
            {
                _endDate = value;
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

        
        private LoginViewModel _loginViewModel;
        private JobHistoryRepository _JobHistoryRepository;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobHistoryViewModel()
        {
            _loginViewModel = LoginViewModel.GetInstance();
            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(_loginViewModel.UserName, _loginViewModel.Password);
            _JobHistoryRepository = new(new OracleSQLDataAccess(connectionString));
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public static List<JobHistoryViewModel> ToListOfJobHistoryViewModel(IEnumerable<JobHistory> JobHistories)
        {
            List<JobHistoryViewModel> result = new List<JobHistoryViewModel>();

            foreach (JobHistory JobHistory in JobHistories)
            {
                JobHistoryViewModel JobHistoryViewModel = new()
                {
                    EmployeeId = JobHistory.EMPLOYEE_ID,
                    StartDate = JobHistory.START_DATE,
                    EndDate = JobHistory.END_DATE,
                    JobId = JobHistory.JOB_ID,
                    DepartmentId = JobHistory.DEPARTMENT_ID
                };
                result.Add(JobHistoryViewModel);
            }

            return result;
        }

        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
    }
}
