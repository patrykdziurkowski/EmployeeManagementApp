﻿using Models;
using Models.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
{
    public class SalariesMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private EmployeeRepository _employeeRepository;

        private ObservableCollection<SalaryViewModel> _salaries;
        public ObservableCollection<SalaryViewModel> Salaries
        {
            get
            {
                return _salaries;
            }
            set
            {
                _salaries = value;
                OnPropertyChanged();
            }
        }

        public double AverageSalary => GetAverageSalary();
        public double MaxSalary => GetMaxSalary();
        public double MinSalary => GetMinSalary();
        public double SumOfSalaries => GetSumOfSalaries();

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public SalariesMenuViewModel(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

            _salaries = new ObservableCollection<SalaryViewModel>(); 
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public void InitializeData()
        {
            List<SalaryViewModel> salaryViewModels = SalaryViewModel
                .ToListOfSalaryViewModel(_employeeRepository.GetAll());
            ObservableCollection<SalaryViewModel> salaries = new ObservableCollection<SalaryViewModel>(salaryViewModels);
            _salaries = salaries;
            _salaries.CollectionChanged += Salaries_CollectionChanged;
        }
        
        public double GetAverageSalary()
        {
            double sum = 0;
            foreach (SalaryViewModel employee in Salaries)
            {
                sum += (double)employee.Salary;
            }
            return sum / Salaries.Count;
        }
        public double GetMaxSalary()
        {
            double maxSalary = 0;
            foreach (SalaryViewModel employee in Salaries)
            {
                if (employee.Salary > maxSalary)
                {
                    maxSalary = (double)employee.Salary;
                }
            }

            return maxSalary;
        }
        public double GetMinSalary()
        {
            double? minSalary = null;
            foreach (SalaryViewModel employee in Salaries)
            {
                if ((employee.Salary < minSalary) || (minSalary is null))
                {
                    minSalary = (double)employee.Salary;
                }
            }
            minSalary = (minSalary is null) ? 0 : minSalary;

            return (double)minSalary;
        }
        public double GetSumOfSalaries()
        {
            double sumOfSalaries = 0;

            foreach (SalaryViewModel employee in Salaries)
            {
                sumOfSalaries += (double)employee.Salary;
            }

            return sumOfSalaries;
        }

        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void Salaries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Salaries"));
        }
    }
}


