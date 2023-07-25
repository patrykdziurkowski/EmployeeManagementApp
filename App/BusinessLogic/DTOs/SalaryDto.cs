using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLogic.ViewModels
{
    public class SalaryDto : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////

        private int _employeeId;
        public int EmployeeId
        {
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

        private string? _firstName;
        public string? FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
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
                _lastName = value;
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

        private double? _salary;
        public double? Salary
        {
            get
            {
                return _salary;
            }
            set
            {
                _salary = value;
                OnPropertyChanged();
            }
        }

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public SalaryDto(
            int employeeId,
            string lastName,
            string jobId)
        {
            _employeeId = employeeId;
            _lastName = lastName;
            _jobId = jobId;
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



