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
    public class DepartmentLocationsMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private DepartmentLocationRepository _departmentLocationRepository;

        private ObservableCollection<DepartmentLocationViewModel> _departmentLocation;
        public ObservableCollection<DepartmentLocationViewModel> DepartmentLocation
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

            _departmentLocation = new ObservableCollection<DepartmentLocationViewModel>();
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public void InitializeData()
        {
            List<DepartmentLocationViewModel> departmentLocationViewModels = DepartmentLocationViewModel
                .ToListOfDepartmentLocationViewModel(_departmentLocationRepository.GetAll());
            ObservableCollection<DepartmentLocationViewModel> departmentLocation = new ObservableCollection<DepartmentLocationViewModel>(departmentLocationViewModels);

            DepartmentLocation = departmentLocation;
            DepartmentLocation.CollectionChanged += DepartmentLocation_CollectionChanged;
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
