using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLogic.ViewModels
{
    public class DepartmentViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
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

        private int? _departmentId;
        public int? DepartmentId
        {
            get
            {
                return _departmentId;
            }
            set
            {
                _departmentId = value;
                OnPropertyChanged("DepartmentId");
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
        public DepartmentViewModel()
        {
            _employees = new ObservableCollection<EmployeeViewModel>();
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        

        

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
    }
}



