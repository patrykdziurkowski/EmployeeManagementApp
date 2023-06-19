using Models.Repositories;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class DepartmentLocationMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private DepartmentLocationRepository _departmentLocationRepository;
        private LoginViewModel _loginViewModel;

        private readonly ObservableCollection<DepartmentLocationViewModel> _departmentLocation;
        public ObservableCollection<DepartmentLocationViewModel> DepartmentLocation
        {
            get
            {
                return _departmentLocation;
            }
        }
        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentLocationMenuViewModel()
        {
            _loginViewModel = LoginViewModel.GetInstance();
            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(_loginViewModel.UserName, _loginViewModel.Password);
            _departmentLocationRepository = new(new OracleSQLDataAccess(connectionString));

            _departmentLocation = new ObservableCollection<DepartmentLocationViewModel>();
            List<DepartmentLocationViewModel> departmentLocationViewModels = DepartmentLocationViewModel
                .ToListOfDepartmentLocationViewModel(_departmentLocationRepository.GetAll());
            ObservableCollection<DepartmentLocationViewModel> departmentLocation = new ObservableCollection<DepartmentLocationViewModel>(departmentLocationViewModels);

            _departmentLocation = departmentLocation;
            _departmentLocation.CollectionChanged += DepartmentLocation_CollectionChanged;
        }
        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void DepartmentLocation_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DepartmentLocation"));
        }
    }
}
