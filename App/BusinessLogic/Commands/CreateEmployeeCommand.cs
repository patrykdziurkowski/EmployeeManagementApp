using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using BusinessLogic.ViewModels;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BusinessLogic.Commands
{
    public class CreateEmployeeCommand : ICommand
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
        public CreateEmployeeCommand(EmployeesMenuViewModel employeesMenuViewModel,
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
            if (_viewModel.NewEmployee is null)
            {
                _viewModel.IsLastCommandSuccessful = false;
                _viewModel.CommandFailMessage = "You must first add an employee to the table before adding them";

                return false;
            }
            ValidationResult validationResult = _employeeValidator.Validate(_viewModel.NewEmployee);
            if (!validationResult.IsValid)
            {
                _viewModel.IsLastCommandSuccessful = false;
                _viewModel.NewEmployee = null;
                _viewModel.CommandFailMessage = validationResult.Errors.First().ErrorMessage;

                return false;
            }
            return true;
        }

        public async void Execute(object? parameter)
        {
            EmployeeDto newEmployee = _viewModel.NewEmployee!;

            ValidationResult result = await _commissionPctValidator.ValidateAsync(newEmployee);
            if (!result.IsValid)
            {
                _viewModel.IsLastCommandSuccessful = false;
                _viewModel.CommandFailMessage = result.Errors.First().ErrorMessage;
                return;
            }


            Employee employeeToHire = new()
            {
                EmployeeId = newEmployee.EmployeeId,
                FirstName = newEmployee.FirstName,
                LastName = newEmployee.LastName,
                Email = newEmployee.Email,
                PhoneNumber = newEmployee.PhoneNumber,
                HireDate = newEmployee.HireDate.ToDateTime(TimeOnly.MinValue),
                JobId = newEmployee.JobId,
                Salary = newEmployee.Salary,
                CommissionPct = newEmployee.CommissionPct,
                ManagerId = newEmployee.ManagerId,
                DepartmentId = newEmployee.DepartmentId
            };
            Result hireResult = await _employeeRepository.HireAsync(employeeToHire);

            _viewModel.IsLastCommandSuccessful = hireResult.IsSuccess;
            _viewModel.NewEmployee = null;
            if (hireResult.IsFailed)
            {
                _viewModel.CommandFailMessage = hireResult.Reasons.First().Message;
                return;
            }
            _viewModel.NewEmployeeAlreadyExists = false;
            _viewModel.NewEmployee = null;
        }
    }
}
