using Models;
using Models.Repositories;
using Oracle.ManagedDataAccess.Client;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ViewModels
{
    public class StartMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private UserCredentials _userCredentials;
        private EmployeeRepository _employeeRepository;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public StartMenuViewModel(UserCredentials userCredentials,
            EmployeeRepository employeeRepository)
        {
            _userCredentials = userCredentials;
            _employeeRepository = employeeRepository;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task<bool> LogIn(string userName, string password)
        {
            _userCredentials.UserName = userName;
            _userCredentials.Password = password;

            try
            {
                await _employeeRepository.GetAll();
            }
            catch (OracleException ex)
            {
                LogOut();
                return false;
            }
            return true;
        }

        public void LogOut()
        {
            _userCredentials.UserName = null;
            _userCredentials.Password = null;
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
