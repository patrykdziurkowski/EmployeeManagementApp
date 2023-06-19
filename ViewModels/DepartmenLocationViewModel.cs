using Models.Entities;
using Models.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class DepartmentLocationViewModel
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////

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

        private string _stateProvince;

        public string StateProvince
        {
            get
            {
                return _stateProvince;
            }
            set
            {
                _stateProvince = value;
                OnPropertyChanged();
            }
        }

        private string _city;

        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                OnPropertyChanged();
            }
        }

        private string _streetAddress;

        public string StreetAddress
        {
            get
            {
                return _streetAddress;
            }
            set
            {
                _streetAddress = value;
                OnPropertyChanged();
            }
        }

        private string _regionName;

        public string RegionName
        {
            get
            {
                return _regionName;
            }
            set
            {
                _regionName = value;
                OnPropertyChanged();
            }
        }

        private string _countryName;

        public string CountryName
        {
            get
            {
                return _countryName;
            }
            set
            {
                _countryName = value;
                OnPropertyChanged();
            }
        }


        private LoginViewModel _loginViewModel;
        private DepartmentLocationRepository _departmentLocationRepository;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentLocationViewModel()
        {
            _loginViewModel = LoginViewModel.GetInstance();
            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(_loginViewModel.UserName, _loginViewModel.Password);
            _departmentLocationRepository = new(new OracleSQLDataAccess(connectionString));
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public static List<DepartmentLocationViewModel> ToListOfDepartmentLocationViewModel(IEnumerable<DepartmentLocation> departmentLocations)
        {
            List<DepartmentLocationViewModel> result = new List<DepartmentLocationViewModel>();

            foreach (DepartmentLocation departmentLocation in departmentLocations)
            {
                DepartmentLocationViewModel departmentLocationViewModel = new()
                {
                    DepartmentId = departmentLocation.DEPARTMENT_ID,
                    DepartmentName = departmentLocation.DEPARTMENT_NAME,
                    StateProvince = departmentLocation.STATE_PROVINCE,
                    City = departmentLocation.CITY,
                    StreetAddress = departmentLocation.STREET_ADDRESS,
                    RegionName = departmentLocation.REGION_NAME,
                    CountryName = departmentLocation.COUNTRY_NAME
                };
                result.Add(departmentLocationViewModel);
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
