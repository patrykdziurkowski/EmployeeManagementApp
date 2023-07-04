using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BusinessLogic.ViewModels
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

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentLocationViewModel()
        {

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

    }
}
