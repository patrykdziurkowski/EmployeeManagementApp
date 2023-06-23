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
            double? averageSalary = Salaries.Average(employee => employee.Salary);

            return (double)averageSalary;
        }
        public double GetMaxSalary()
        {
            double? maxSalary = Salaries.Max(employee => employee.Salary);

            return (double)maxSalary;
        }
        public double GetMinSalary()
        {
            double? minSalary = Salaries.Min(employee => employee.Salary);

            return (double)minSalary;
        }
        public double GetSumOfSalaries()
        {
            double? sumOfSalaries = Salaries.Sum(employee => employee.Salary);

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


