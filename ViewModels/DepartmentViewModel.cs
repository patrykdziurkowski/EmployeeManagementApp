using Models.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
{
    public class DepartmentViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
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

        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public static List<DepartmentViewModel> ToListOfDepartmentViewModel(IEnumerable<Department> departments)
        {
            List<DepartmentViewModel> convertedDepartments = new List<DepartmentViewModel>();

            foreach (Department department in departments)
            {
                DepartmentViewModel DepartmentViewModel = new()
                {
                    DepartmentId = department.DEPARTMENT_ID,
                    DepartmentName = department.DEPARTMENT_NAME,
                    ManagerId = department.MANAGER_ID,
                    LocationId = department.LOCATION_ID
                };
                convertedDepartments.Add(DepartmentViewModel);
            }

            return convertedDepartments;
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



