using Models;
using Models.Entities;
using Models.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
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

        private LoginViewModel _loginViewModel;
        private DepartmentRepository _departmentRepository;

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
            _loginViewModel = LoginViewModel.GetInstance();
            _employees = new ObservableCollection<EmployeeViewModel>();

            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(_loginViewModel.UserName, _loginViewModel.Password);
            OracleSQLDataAccess dataAccess = new(connectionString);

            _departmentRepository = new(dataAccess);
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

            if (name == "DepartmentId" && DepartmentId is not null)
            {
                List<EmployeeViewModel> employeeViewModels = EmployeeViewModel
               .ToListOfEmployeeViewModel(_departmentRepository.GetEmployeesForDepartment((int)DepartmentId));
                ObservableCollection<EmployeeViewModel> employees = new ObservableCollection<EmployeeViewModel>(employeeViewModels);

                _employees = employees;
                _employees.CollectionChanged += Employees_CollectionChanged;
            }
        }

        private void Employees_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Employees"));
        }
    }
}



