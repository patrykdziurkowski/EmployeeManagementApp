using DataAccess.Repositories;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewModels
{
    public class DepartmentLocationsMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private DepartmentLocationRepository _departmentLocationRepository;

        private ObservableCollection<DepartmentLocationDto> _departmentLocation;
        public ObservableCollection<DepartmentLocationDto> DepartmentLocation
        {
            get
            {
                return _departmentLocation;
            }
            set
            {
                _departmentLocation = value;
                OnPropertyChanged();
            }
        }
        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentLocationsMenuViewModel(DepartmentLocationRepository departmentLocationRepository)
        {
            _departmentLocationRepository = departmentLocationRepository;

            _departmentLocation = new();
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task InitializeDataAsync()
        {
            List<DepartmentLocationDto> departmentLocationViewModels = (await _departmentLocationRepository.GetAllAsync()).ToListOfDepartmentLocationViewModel();
            ObservableCollection<DepartmentLocationDto> departmentLocation = new(departmentLocationViewModels);

            DepartmentLocation = departmentLocation;
            DepartmentLocation.CollectionChanged += DepartmentLocation_CollectionChanged;
        }


        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void DepartmentLocation_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DepartmentLocation"));
        }
    }
}
