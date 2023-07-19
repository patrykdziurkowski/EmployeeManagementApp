using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Repositories;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BusinessLogic.Commands
{
    public class LoginCommand : ICommand
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private StartMenuViewModel _viewModel;
        private EmployeeRepository _employeeRepository;

        public event EventHandler? CanExecuteChanged;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public LoginCommand(StartMenuViewModel startMenuViewModel,
            EmployeeRepository employeeRepository)
        {
            _viewModel = startMenuViewModel;
            _employeeRepository = employeeRepository;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public bool CanExecute(object? parameter)
        {
            _viewModel.IsLoginSuccessful = false;
            return _viewModel.AreCredentialsFilledOut;
        }

        public async void Execute(object? parameter)
        {
            _viewModel.LoginErrorMessages.Clear();
            _viewModel.IsLoginSuccessful = true;

            try
            {
                await _employeeRepository.GetAllAsync();
            }
            catch (OracleException ex)
            {
                _viewModel.UserCredentials.Clear();
                _viewModel.LoginErrorMessages.Add(ex.Message);
                _viewModel.IsLoginSuccessful = false;
            }

        }
    }
}
