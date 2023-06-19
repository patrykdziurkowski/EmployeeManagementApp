using Models;
using Models.Repositories;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private static LoginViewModel _instance = null;
        private EmployeeRepository _employeeRepository;

        private string _userName;
        public string UserName {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _password;
        public string Password {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        private LoginViewModel()
        {
            
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public bool TryLogIn(string userName, string password)
        {
            _instance.UserName = userName;
            _instance.Password = password;


            ConnectionStringProvider provider = new ConnectionStringProvider();
            string connectionString = provider
                .GetConnectionString(UserName, Password);

            _employeeRepository = new(new OracleSQLDataAccess(connectionString));


            bool isSuccessful;
            try
            {
                _employeeRepository.GetAll();
                isSuccessful = true;
            }
            catch (Exception ex)
            {
                //Unable to log in to the server
                isSuccessful = false;
            }

            return isSuccessful;
        }

        public static LoginViewModel GetInstance()
        {
            if (_instance is null)
            {
                _instance = new LoginViewModel();
            }
            return _instance;
        }

        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
