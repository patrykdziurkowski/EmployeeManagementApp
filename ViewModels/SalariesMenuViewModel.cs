using Models;
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
        private LoginViewModel _loginViewModel;

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

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public SalariesMenuViewModel()
        {
            _loginViewModel = LoginViewModel.GetInstance();
            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(_loginViewModel.UserName, _loginViewModel.Password);
            EmployeeRepository employeeRepository = new(new OracleSQLDataAccess(connectionString));

            _salaries = new ObservableCollection<SalaryViewModel>();
            List<SalaryViewModel> salaryViewModels = SalaryViewModel
                .ToListOfSalaryViewModel(employeeRepository.GetAll());
            ObservableCollection<SalaryViewModel> salaries = new ObservableCollection<SalaryViewModel>(salaryViewModels);
            _salaries = salaries;
            _salaries.CollectionChanged += Salaries_CollectionChanged;
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


