﻿using BusinessLogic.ViewModels;
using DataAccess.Models;
using DataAccess.Repositories;
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
    public class UpdateDepartmentCommand : ICommand
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private DepartmentsMenuViewModel _viewModel;
        private EmployeeRepository _employeeRepository;

        public event EventHandler? CanExecuteChanged;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public UpdateDepartmentCommand(DepartmentsMenuViewModel departmentsMenuViewModel,
            EmployeeRepository employeeRepository)
        {
            _viewModel = departmentsMenuViewModel;
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

            _employeeRepository.Update((int)changedEmployee.EmployeeId, employeeToUpdate);
            
            MoveEmployeeToTheirDepartment(changedEmployee);
        }

        /// <summary>
        /// Moves the employee from one department to another (inside the Departments collection)
        /// </summary>
        /// <param name="changedEmployee">Employee that was updated</param>
        /// <returns></returns>
        private async Task MoveEmployeeToTheirDepartment(EmployeeViewModel changedEmployee)
        {
            foreach (DepartmentViewModel department in _viewModel.Departments)
            {
                EmployeeViewModel employeeToRemove = department.Employees
                    .FirstOrDefault(employee => employee.EmployeeId == changedEmployee.EmployeeId);
                department.Employees.Remove(employeeToRemove);

                if (department.DepartmentId == changedEmployee.DepartmentId)
                {
                    department.Employees.Add(changedEmployee);
                }

            }
        }

    }
}
