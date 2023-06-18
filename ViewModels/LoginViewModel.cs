using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private static LoginViewModel _instance = null;

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
        public void Login(string userName, string password)
        {
            _instance.UserName = userName;
            _instance.Password = password;
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
