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

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobHistoryViewModel()
        {
           
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public static List<JobHistoryViewModel> ToListOfJobHistoryViewModel(IEnumerable<JobHistory> jobHistories)
        {
            List<JobHistoryViewModel> result = new List<JobHistoryViewModel>();

            foreach (JobHistory jobHistory in jobHistories)
            {
                JobHistoryViewModel jobHistoryViewModel = new()
                {
                    EmployeeId = jobHistory.EmployeeId,
                    StartDate = jobHistory.StartDate,
                    EndDate = jobHistory.EndDate,
                    JobId = jobHistory.JobId,
                    DepartmentId = jobHistory.DepartmentId
                };
                result.Add(jobHistoryViewModel);
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
