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
        private DepartmentRepository _departmentRepository;
        private IValidator<EmployeeViewModel> _employeeValidator;
        private IDateProvider _dateProvider;
        private JobHistoryRepository _jobHistoryRepository;

        public event EventHandler? CanExecuteChanged;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public UpdateEmployeeCommand(EmployeesMenuViewModel employeesMenuViewModel,
            EmployeeRepository employeeRepository,
            DepartmentRepository departmentRepository,
            IValidator<EmployeeViewModel> employeeValidator,
            IDateProvider dateProvider,
            JobHistoryRepository jobHistoryRepository)
        {
            _viewModel = employeesMenuViewModel;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _employeeValidator = employeeValidator;
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
            if (_viewModel.UpdatedEmployee!.CommissionPct is not null)
            {
                IEnumerable<Department> departments = await _departmentRepository.GetAllAsync();
                short salesDepartmentId = departments
                                            .First(department => department.DepartmentName == "Sales")
                                            .DepartmentId;
                if (_viewModel.UpdatedEmployee.DepartmentId != salesDepartmentId)
                {
                    _viewModel.IsLastCommandSuccessful = false;
                    _viewModel.CommandFailMessage = "Only an employee from the Sales department can have a commission percentage";
                    return;
                }
            }

            EmployeeViewModel changedEmployee = _viewModel.UpdatedEmployee;
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
