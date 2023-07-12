using BusinessLogic.Commands;
using DataAccess;
using DataAccess.Repositories;
using Oracle.ManagedDataAccess.Client;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace BusinessLogic.ViewModels
{
    public class StartMenuViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        public UserCredentials UserCredentials;

        public bool IsLoginSuccessful { get; set; } 
        public bool AreCredentialsFilledOut => !string.IsNullOrEmpty(UserCredentials.UserName) && !string.IsNullOrEmpty(UserCredentials.Password);
        public ObservableCollection<string> LoginErrorMessages { get; }


        public ICommand LoginCommand { get; }

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public StartMenuViewModel(UserCredentials userCredentials,
            EmployeeRepository employeeRepository)
        {
            UserCredentials = userCredentials;

            LoginErrorMessages = new AsyncObservableCollection<string>();
            IsLoginSuccessful = false;
            LoginCommand = new LoginCommand(this, employeeRepository);
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////


        ////////////////////////////////////////////
        //  Events and Data Binding
        ////////////////////////////////////////////
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
