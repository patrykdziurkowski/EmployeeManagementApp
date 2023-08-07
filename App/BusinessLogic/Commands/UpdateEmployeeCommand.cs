using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using BusinessLogic.ViewModels;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using System.Windows.Input;

namespace BusinessLogic.Commands
{
    public class UpdateEmployeeCommand : ICommand
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private EmployeesMenuViewModel _viewModel;
        private EmployeeRepository _employeeRepository;
        private IValidator<EmployeeDto> _employeeValidator;
        private IValidator<EmployeeDto> _commissionPctValidator;

        public event EventHandler? CanExecuteChanged;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public UpdateEmployeeCommand(
            EmployeesMenuViewModel employeesMenuViewModel,
            EmployeeRepository employeeRepository,
            IEmployeeValidatorFactory employeeValidatorFactory)
        {
            _viewModel = employeesMenuViewModel;
            _employeeRepository = employeeRepository;
            _employeeValidator = employeeValidatorFactory.GetValidator(typeof(EmployeeValidator));
            _commissionPctValidator = employeeValidatorFactory.GetValidator(typeof(CommissionPctValidator));
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public bool CanExecute(object? parameter)
        {
            ValidationResult validationResult = _employeeValidator.Validate(_viewModel.UpdatedEmployee!);
            if (!validationResult.IsValid)
            {
                _viewModel.IsLastCommandSuccessful = false;
                _viewModel.CommandFailMessage = validationResult.Errors.First().ErrorMessage;
                return false;
            }
            return true;
        }

        public async void Execute(object? parameter)
        {
            EmployeeDto changedEmployee = _viewModel.UpdatedEmployee!;

            ValidationResult result = await _commissionPctValidator.ValidateAsync(changedEmployee);
            if (!result.IsValid)
            {
                _viewModel.IsLastCommandSuccessful = false;
                _viewModel.CommandFailMessage = result.Errors.First().ErrorMessage;
                return;
            }    

            Employee employeeToUpdate = new()
            {
                EmployeeId = changedEmployee.EmployeeId,
                FirstName = changedEmployee.FirstName,
                LastName = changedEmployee.LastName,
                Email = changedEmployee.Email,
                PhoneNumber = changedEmployee.PhoneNumber,
                HireDate = changedEmployee.HireDate.ToDateTime(TimeOnly.MinValue),
                JobId = changedEmployee.JobId,
                Salary = changedEmployee.Salary,
                CommissionPct = changedEmployee.CommissionPct,
                ManagerId = changedEmployee.ManagerId,
                DepartmentId = changedEmployee.DepartmentId
            };

            Result updateResult = await _employeeRepository.UpdateAsync(employeeToUpdate);
            _viewModel.IsLastCommandSuccessful = updateResult.IsSuccess;
            if (updateResult.IsFailed)
            {
                _viewModel.CommandFailMessage = updateResult.Reasons.First().Message;
            }
        }

    }
}
