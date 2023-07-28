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
        private IDateProvider _dateProvider;
        private JobHistoryRepository _jobHistoryRepository;

        public event EventHandler? CanExecuteChanged;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public UpdateEmployeeCommand(EmployeesMenuViewModel employeesMenuViewModel,
            EmployeeRepository employeeRepository,
            IEmployeeValidatorFactory employeeValidatorFactory,
            IDateProvider dateProvider,
            JobHistoryRepository jobHistoryRepository)
        {
            _viewModel = employeesMenuViewModel;
            _employeeRepository = employeeRepository;
            _employeeValidator = employeeValidatorFactory.GetValidator(typeof(EmployeeValidator));
            _commissionPctValidator = employeeValidatorFactory.GetValidator(typeof(CommissionPctValidator));
            _dateProvider = dateProvider;
            _jobHistoryRepository = jobHistoryRepository;
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

            if (_viewModel.IsUpdatedEmployeeJobChanged)
            {
                Result jobHistoryEntryCreationResult = await CreateJobHistoryEntryAsync(employeeToUpdate);
                if (jobHistoryEntryCreationResult.IsFailed)
                {
                    _viewModel.IsLastCommandSuccessful = false;
                    _viewModel.CommandFailMessage = jobHistoryEntryCreationResult.Reasons.First().Message;

                    return;
                }
            }

            Result updateResult = await _employeeRepository.UpdateAsync(employeeToUpdate);
            _viewModel.IsLastCommandSuccessful = updateResult.IsSuccess;
            if (updateResult.IsFailed)
            {
                _viewModel.CommandFailMessage = updateResult.Reasons.First().Message;
            }
        }


        private async Task<Result> CreateJobHistoryEntryAsync(Employee employeeToUpdate)
        {
            IEnumerable<JobHistory> employeeToUpdatePastJobs = (await _jobHistoryRepository.GetAllAsync())
                                                                    .Where(jobHistoryEntry => jobHistoryEntry.EmployeeId == employeeToUpdate.EmployeeId);

            DateTime previousJobStart = employeeToUpdate.HireDate;
            if (employeeToUpdatePastJobs.Any())
            {
                previousJobStart = employeeToUpdatePastJobs
                                                    .Select(jobHistoryEntry => jobHistoryEntry.EndDate)
                                                    .Max();
            }

            JobHistory previousJobEntry = new()
            {
                EmployeeId = employeeToUpdate.EmployeeId,
                StartDate = previousJobStart,
                EndDate = _dateProvider.GetNow().ToDateTime(TimeOnly.MinValue),
                JobId = _viewModel.UpdatedEmployeePreviousJob!.JobId,
                DepartmentId = employeeToUpdate.DepartmentId
            };

            if (previousJobEntry.StartDate.Date == previousJobEntry.EndDate.Date)
            {
                return Result.Fail("Cannot change an employee's job twice in one day");
            }

            Result insertionResult = await _jobHistoryRepository.InsertAsync(previousJobEntry);

            return insertionResult;
        }
    }
}
