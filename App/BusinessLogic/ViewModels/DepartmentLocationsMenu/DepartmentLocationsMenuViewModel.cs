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
using System.Windows.Input;
using BusinessLogic.Commands;

namespace BusinessLogic.ViewModels
{
    public class DepartmentLocationsMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ObservableCollection<DepartmentLocationDto> _departmentLocations;
        public ObservableCollection<DepartmentLocationDto> DepartmentLocations
        {
            get
            {
                return _departmentLocations;
            }
            set
            {
                _departmentLocations = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadDepartmentLocationsCommand { get; }

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentLocationsMenuViewModel(DepartmentLocationRepository departmentLocationRepository)
        {
            _departmentLocations = new();
            LoadDepartmentLocationsCommand = new LoadDepartmentLocationsCommand(this, departmentLocationRepository);
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

        public void DepartmentLocation_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DepartmentLocation"));
        }
    }
}
