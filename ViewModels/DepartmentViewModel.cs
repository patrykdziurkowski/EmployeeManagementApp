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
        private int? _employeeId;

        public int? EmployeeId
        {
            get
            {
                return _employeeId;
            }
            set
            {
                _employeeId = value;
                OnPropertyChanged();
            }
        }

        private string _firstName;
        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        private string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }
        private string _jobId;
        public string JobId
        {
            get
            {
                return _jobId;
            }
            set
            {
                _jobId = value;
                OnPropertyChanged();
            }
        }

        private double? _department;
        public double? Department
        {
            get
            {
                return _department;
            }
            set
            {
                _department = value;
                OnPropertyChanged();
            }
        }

        public DepartmentViewModel()
        {

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public static List<DepartmentViewModel> ToListOfDepartmentViewModel(IEnumerable<Employee> departments)
        {
            List<DepartmentViewModel> result = new List<DepartmentViewModel>();

            foreach (Employee employee in departments)
            {
                DepartmentViewModel DepartmentViewModel = new()
                {
                    EmployeeId = employee.EMPLOYEE_ID,
                    FirstName = employee.FIRST_NAME,
                    LastName = employee.LAST_NAME,
                    JobId = employee.JOB_ID,
                    Department = employee.DEPARTMENT_ID,
                };
                result.Add(DepartmentViewModel);
            }

            return result;
        }
    }
}



