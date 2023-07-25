using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLogic.ViewModels
{
    public class DepartmentDto : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
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

        private short _departmentId;
        public short DepartmentId
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

        private string _departmentName;
        public string DepartmentName
        {
            get
            {
                return _departmentName;
            }
            set
            {
                _departmentName = value;
                OnPropertyChanged();
            }
        }

        private int? _managerId;
        public int? ManagerId
        {
            get
            {
                return _managerId;
            }
            set
            {
                _managerId = value;
                OnPropertyChanged();
            }
        }

        private int? _locationId;
        public int? LocationId
        {
            get
            {
                return _locationId;
            }
            set
            {
                _locationId = value;
                OnPropertyChanged();
            }
        }


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentDto(
            short departmentId,
            string departmentName)
        {
            _departmentId = departmentId;
            _departmentName = departmentName;

            _employees = new();
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

        private void Employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Employees"));
        }
    }
}



