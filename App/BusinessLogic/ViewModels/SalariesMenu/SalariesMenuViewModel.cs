using BusinessLogic.Commands;
using DataAccess;
using DataAccess.Repositories;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BusinessLogic.ViewModels
{
    public class SalariesMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
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

        public ICommand LoadSalariesCommand { get; }

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public SalariesMenuViewModel(EmployeeRepository employeeRepository)
        {
            _salaries = new(); 
            
            LoadSalariesCommand = new LoadSalariesCommand(this, employeeRepository);
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////        
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
            NumberFormatInfo nfi = new();
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
        public void Salaries_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Salaries"));
        }
    }
}


