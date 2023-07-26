using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLogic.ViewModels
{
    public class JobHistoryDto : INotifyPropertyChanged
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
                _employeeId = value;
                OnPropertyChanged();
            }
        }

        private DateOnly _startDate;
        public DateOnly StartDate
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

        private DateOnly _endDate;
        public DateOnly EndDate
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
        public JobHistoryDto(
            int employeeId,
            string jobId,
            DateOnly startDate,
            DateOnly endDate)
        {
            _employeeId = employeeId;
            _jobId = jobId;
            _startDate = startDate;
            _endDate = endDate;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        

        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        
    }
}
