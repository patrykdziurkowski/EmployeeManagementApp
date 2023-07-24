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
        private DepartmentRepository _departmentRepository;
        private IValidator<EmployeeViewModel> _employeeValidator;

        public event EventHandler? CanExecuteChanged;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public CreateEmployeeCommand(EmployeesMenuViewModel employeesMenuViewModel,
            EmployeeRepository employeeRepository,
            DepartmentRepository departmentRepository,
            IValidator<EmployeeViewModel> employeeValidator)
        {
            _viewModel = employeesMenuViewModel;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _employeeValidator = employeeValidator;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public bool CanExecute(object? parameter)
        {
            _viewModel.NewEmployee = _viewModel.Employees.Last();

            ValidationResult validationResult = _employeeValidator.Validate(_viewModel.NewEmployee);
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
            if (_viewModel.NewEmployee!.CommissionPct is not null)
            {
                IEnumerable<Department> departments = await _departmentRepository.GetAllAsync();
                short salesDepartmentId = departments
                                            .First(department => department.DepartmentName == "Sales")
                                            .DepartmentId;
                if (_viewModel.NewEmployee.DepartmentId != salesDepartmentId)
                {
                    _viewModel.IsLastCommandSuccessful = false;
                    _viewModel.CommandFailMessage = "Only an employee from the Sales department can have a commission percentage";
                    return;
                }
            }
            

            Employee employeeToHire = new()
            {
                EmployeeId = _viewModel.NewEmployee.EmployeeId,
                FirstName = _viewModel.NewEmployee.FirstName,
                LastName = _viewModel.NewEmployee.LastName,
                Email = _viewModel.NewEmployee.Email,
                PhoneNumber = _viewModel.NewEmployee.PhoneNumber,
                HireDate = _viewModel.NewEmployee.HireDate.ToDateTime(TimeOnly.MinValue),
                JobId = _viewModel.NewEmployee.JobId,
                Salary = _viewModel.NewEmployee.Salary,
                CommissionPct = _viewModel.NewEmployee.CommissionPct,
                ManagerId = _viewModel.NewEmployee.ManagerId,
                DepartmentId = _viewModel.NewEmployee.DepartmentId
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
        }
    }
}
