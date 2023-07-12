using BusinessLogic.ViewModels;
using DataAccess.Repositories;
using FluentResults;
using System.Windows.Input;

namespace BusinessLogic.Commands
{
    public class DeleteEmployeeCommand : ICommand
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private EmployeesMenuViewModel _viewModel;
        private EmployeeRepository _employeeRepository;
        
        public event EventHandler? CanExecuteChanged;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DeleteEmployeeCommand(EmployeesMenuViewModel employeesMenuViewModel,
            EmployeeRepository employeeRepository)
        {
            _viewModel = employeesMenuViewModel;
            _employeeRepository = employeeRepository;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public async void Execute(object? parameter)
        {
            if (parameter is null)
            {
                return;
            }
            int employeeToDeleteId = (int)parameter;

            Result deletionResult = await _employeeRepository.Fire(employeeToDeleteId);
            _viewModel.IsLastCommandSuccessful = deletionResult.IsSuccess;
            
            if (deletionResult.IsSuccess)
            {
                EmployeeViewModel employeeToRemove = _viewModel.Employees
                .First(employee => employee.EmployeeId == employeeToDeleteId);
                
                _viewModel.Employees.Remove(employeeToRemove);
            }
            else
            {
                _viewModel.CommandFailMessage = deletionResult.Reasons.First().Message;
            }
        }
    }
}
