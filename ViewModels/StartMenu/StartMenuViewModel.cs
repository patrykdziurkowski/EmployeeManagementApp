using Models;
using Models.Repositories;
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
            catch (Exception ex)
            {
                return false;
            }
            return true;
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
