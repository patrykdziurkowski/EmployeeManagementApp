using Models;
using Models.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
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

        public string AverageSalaryText => DoubleToStringMoney(GetAverageSalary());
        public string MaxSalaryText => DoubleToStringMoney(GetMaxSalary());
        public string MinSalaryText => DoubleToStringMoney(GetMinSalary());
        public string SumOfSalariesText => DoubleToStringMoney(GetSumOfSalaries());

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
        public async Task InitializeData()
        {
            List<SalaryViewModel> salaryViewModels = SalaryViewModel
                .ToListOfSalaryViewModel(await _employeeRepository.GetAll());
            ObservableCollection<SalaryViewModel> salaries = new ObservableCollection<SalaryViewModel>(salaryViewModels);
            _salaries = salaries;
            _salaries.CollectionChanged += Salaries_CollectionChanged;
        }
        
        public double GetAverageSalary()
        {
            double sum = 0;
            foreach (SalaryViewModel employee in Salaries)
            {
                sum += (double) employee.Salary;
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
                    maxSalary = (double) employee.Salary;
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
                    minSalary = (double) employee.Salary;
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
                sumOfSalaries += (double) employee.Salary;
            }

            return sumOfSalaries;
        }

        public string DoubleToStringMoney(double number)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return string.Format(nfi, "${0:0.00}", number);
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


