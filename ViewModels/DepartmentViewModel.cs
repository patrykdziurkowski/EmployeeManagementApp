using Core;
using Domain;
using Infrastructure;
using Infrastructure.Repositories;
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
    public class DepartmentViewModel : INotifyPropertyChanged
    {
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


        public event PropertyChangedEventHandler? PropertyChanged;

        public DepartmentViewModel()
        {

        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
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
    }
}



