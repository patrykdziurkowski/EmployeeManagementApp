using DataAccess;
using DataAccess.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace BusinessLogic.ViewModels
{
    public class SalariesMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private EmployeeRepository _employeeRepository;

        private ObservableCollection<SalaryDto> _salaries;
        public ObservableCollection<SalaryDto> Salaries
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

            _salaries = new ObservableCollection<SalaryDto>(); 
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task InitializeDataAsync()
        {
            List<SalaryDto> salaryViewModels = (await _employeeRepository.GetAllAsync()).ToListOfSalaryViewModel();
            ObservableCollection<SalaryDto> salaries = new ObservableCollection<SalaryDto>(salaryViewModels);
            _salaries = salaries;
            _salaries.CollectionChanged += Salaries_CollectionChanged;
        }
        
        public double GetAverageSalary()
        {
            double? averageSalary = Salaries.Average(employee => employee.Salary);
            if (averageSalary is null)
            {
                return 0;
            }

            return (double)averageSalary;
        }
        public double GetMaxSalary()
        {
            double? maxSalary = Salaries.Max(employee => employee.Salary);
            if (maxSalary is null)
            {
                return 0;
            }

            return (double)maxSalary;
        }
        public double GetMinSalary()
        {
            double? minSalary = Salaries.Min(employee => employee.Salary);
            if (minSalary is null)
            {
                return 0;
            }

            return (double)minSalary;
        }
        public double GetSumOfSalaries()
        {
            double? sumOfSalaries = Salaries.Sum(employee => employee.Salary);
            if (sumOfSalaries is null)
            {
                return 0;
            }

            return (double)sumOfSalaries;
        }

        private string DoubleToStringMoney(double number)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return string.Format(nfi, "${0:0.00}", number);
        }

        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void Salaries_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Salaries"));
        }
    }
}


