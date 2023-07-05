using BusinessLogic.ViewModels;
using DataAccess.Repositories;
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

            bool employeeWasRemovedFromDatabase = await _employeeRepository.Fire(employeeToDeleteId);
            if (employeeWasRemovedFromDatabase)
            {
                EmployeeViewModel employeeToRemove = _viewModel.Employees
                .FirstOrDefault(employee => employee.EmployeeId == employeeToDeleteId);
                
                _viewModel.Employees.Remove(employeeToRemove);
            }
        }
    }
}
