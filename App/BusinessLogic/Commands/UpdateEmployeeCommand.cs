﻿using BusinessLogic.ViewModels;
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
            EmployeeViewModel changedEmployee = _viewModel.UpdatedEmployee!;
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

            DateTime previousJobStartDateTime = employeeToUpdate.HireDate;
            if (employeeToUpdatePastJobs.Any())
            {
                previousJobStartDateTime = employeeToUpdatePastJobs
                                                    .Select(jobHistoryEntry => jobHistoryEntry.EndDate)
                                                    .Max();
            }

            JobHistory jobHistoryEntry = new()
            {
                EmployeeId = employeeToUpdate.EmployeeId,
                StartDate = previousJobStartDateTime,
                EndDate = _dateProvider.GetNow().ToDateTime(TimeOnly.MinValue),
                JobId = _viewModel.UpdatedEmployeePreviousJob!.JobId,
                DepartmentId = employeeToUpdate.DepartmentId
            };

            if (jobHistoryEntry.StartDate.Date == jobHistoryEntry.EndDate.Date)
            {
                return Result.Fail("Cannot change an employee's job twice in one day");
            }

            Result insertionResult = await _jobHistoryRepository.InsertAsync(jobHistoryEntry);

            return insertionResult;
        }
    }
}
