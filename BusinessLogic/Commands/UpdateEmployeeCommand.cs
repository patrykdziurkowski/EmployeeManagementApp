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
        private IValidator<EmployeeViewModel> _employeeValidator;
        private IDateProvider _dateProvider;
        private JobHistoryRepository _jobHistoryRepository;

        public event EventHandler? CanExecuteChanged;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public UpdateEmployeeCommand(EmployeesMenuViewModel employeesMenuViewModel,
            EmployeeRepository employeeRepository,
            IValidator<EmployeeViewModel> employeeValidator,
            IDateProvider dateProvider,
            JobHistoryRepository jobHistoryRepository)
        {
            _viewModel = employeesMenuViewModel;
            _employeeRepository = employeeRepository;
            _employeeValidator = employeeValidator;
            _dateProvider = dateProvider;
            _jobHistoryRepository = jobHistoryRepository;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public bool CanExecute(object? parameter)
        {
            ValidationResult validationResult = _employeeValidator.Validate(_viewModel.UpdatedEmployee);
            if (!validationResult.IsValid)
            {
                _viewModel.IsLastCommandSuccessful = false;
                _viewModel.CommandFailMessage = validationResult.Errors.FirstOrDefault().ErrorMessage;
                return false;
            }
            return true;
        }

        public async void Execute(object? parameter)
        {
            EmployeeViewModel changedEmployee = _viewModel.UpdatedEmployee;
            Employee employeeToUpdate = new()
            {
                EmployeeId = changedEmployee.EmployeeId,
                FirstName = changedEmployee.FirstName,
                LastName = changedEmployee.LastName,
                Email = changedEmployee.Email,
                PhoneNumber = changedEmployee.PhoneNumber,
                HireDate = changedEmployee.HireDate.Value.ToDateTime(TimeOnly.MinValue),
                JobId = changedEmployee.JobId,
                Salary = changedEmployee.Salary,
                CommissionPct = changedEmployee.CommissionPct,
                ManagerId = changedEmployee.ManagerId,
                DepartmentId = changedEmployee.DepartmentId
            };

            if (_viewModel.IsUpdatedEmployeeJobChanged)
            {
                await CreateJobHistoryEntry(employeeToUpdate);
            }

            Result updateResult = await _employeeRepository.Update(employeeToUpdate);
            _viewModel.IsLastCommandSuccessful = updateResult.IsSuccess;
            if (updateResult.IsFailed)
            {
                _viewModel.CommandFailMessage = updateResult.Reasons.FirstOrDefault().Message;
            }
        }


        private async Task CreateJobHistoryEntry(Employee employeeToUpdate)
        {
            DateTime? previousJobStartDateTime = (await _jobHistoryRepository.GetAll())
                                .Where(jobHistoryEntry => jobHistoryEntry.EmployeeId == employeeToUpdate.EmployeeId)
                                .Select(jobHistoryEntry => jobHistoryEntry.EndDate)
                                .Max();

            if (previousJobStartDateTime is null)
            {
                previousJobStartDateTime = employeeToUpdate.HireDate.Value;
            }
            DateOnly? previousJobStartDate = DateOnly.FromDateTime(previousJobStartDateTime.Value);


            JobHistory jobHistoryEntry = new()
            {
                EmployeeId = employeeToUpdate.EmployeeId,
                StartDate = previousJobStartDateTime,
                EndDate = _dateProvider.GetNow().ToDateTime(TimeOnly.MinValue),
                JobId = _viewModel.UpdatedEmployeePreviousJob.JobId,
                DepartmentId = employeeToUpdate.DepartmentId
            };

            _jobHistoryRepository.Insert(jobHistoryEntry);
        }
    }
}
