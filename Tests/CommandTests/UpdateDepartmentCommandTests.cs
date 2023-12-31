﻿using BusinessLogic.Commands;
using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentResults;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.CommandTests
{
#pragma warning disable CS1998
    public class UpdateDepartmentCommandTests
    {
        private readonly UpdateDepartmentCommand _subject;

        private readonly Mock<DepartmentRepository> _mockDepartmentRepository;
        private readonly Mock<EmployeeRepository> _mockEmployeeRepository;
        private readonly Mock<DepartmentsMenuViewModel> _mockViewModel;

        public UpdateDepartmentCommandTests()
        {
            Mock<ISqlDataAccess> dataAccess = new();

            _mockEmployeeRepository = new(dataAccess.Object);
            _mockDepartmentRepository = new(dataAccess.Object);

            _mockViewModel = new(
                _mockDepartmentRepository.Object,
                _mockEmployeeRepository.Object);

            _subject = new(
                _mockViewModel.Object,
                _mockEmployeeRepository.Object);
        }

        [Fact]
        public async Task CanExecute_ReturnsTrue()
        {
            //Arrange

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.True(canExecute);
        }

        [Fact]
        public async Task Execute_GivenSuccessfulDatabaseUpdate_MovesEmployeeBetweenDepartments()
        {
            //Arrange
            short previousDepartmentId = 110;
            short nextDepartmentId = 20;

            DepartmentDto previousDepartment = new(
                previousDepartmentId,
                "Sales");

            DepartmentDto nextDepartment = new(
                nextDepartmentId,
                "IT");

            _mockViewModel.Object.Departments.Add(previousDepartment);
            _mockViewModel.Object.Departments.Add(nextDepartment);


            EmployeeDto employee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_CLERK")
            {
                DepartmentId = previousDepartmentId
            };

            _mockViewModel.Object.Departments
                .First(department => department.DepartmentId == 110)
                .Employees.Add(employee);

            _mockEmployeeRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Ok()));

            //Act
            employee.DepartmentId = nextDepartmentId;
            _mockViewModel.Object.UpdatedEmployee = employee;

            _subject.Execute(null);

            //Assert
            Assert.Contains(employee, nextDepartment.Employees);
            Assert.DoesNotContain(employee, previousDepartment.Employees);
        }

        [Fact]
        public async Task Execute_GivenUnsuccessfulDatabaseUpdate_DoesntMoveEmployeeBetweenDepartments()
        {
            //Arrange
            short previousDepartmentId = 110;
            short nextDepartmentId = 20;

            DepartmentDto previousDepartment = new(
                previousDepartmentId,
                "Sales");

            DepartmentDto nextDepartment = new(
                nextDepartmentId,
                "IT");

            _mockViewModel.Object.Departments.Add(previousDepartment);
            _mockViewModel.Object.Departments.Add(nextDepartment);


            EmployeeDto employee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_CLERK")
            {
                DepartmentId = previousDepartmentId
            };

            _mockViewModel.Object.Departments
                .First(department => department.DepartmentId == 110)
                .Employees.Add(employee);

            _mockEmployeeRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Fail("")));

            //Act
            employee.DepartmentId = nextDepartmentId;
            _mockViewModel.Object.UpdatedEmployee = employee;

            _subject.Execute(null);

            //Assert
            Assert.DoesNotContain(employee, nextDepartment.Employees);
            Assert.Contains(employee, previousDepartment.Employees);
        }

        [Fact]
        public async Task Execute_GivenUnsuccessfulDatabaseUpdate_SetsFailIndicatorAndMessage()
        {
            //Arrange
            short previousDepartmentId = 110;
            short nextDepartmentId = 20;

            DepartmentDto previousDepartment = new(
                previousDepartmentId,
                "Sales");

            DepartmentDto nextDepartment = new(
                nextDepartmentId,
                "IT");

            _mockViewModel.Object.Departments.Add(previousDepartment);
            _mockViewModel.Object.Departments.Add(nextDepartment);


            EmployeeDto employee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_CLERK")
            {
                DepartmentId = previousDepartmentId
            };

            _mockViewModel.Object.Departments
                .First(department => department.DepartmentId == 110)
                .Employees.Add(employee);

            _mockEmployeeRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Fail("")));

            //Act
            employee.DepartmentId = nextDepartmentId;
            _mockViewModel.Object.UpdatedEmployee = employee;

            _subject.Execute(null);

            //Assert
            Assert.False(_mockViewModel.Object.IsLastCommandSuccessful);
            Assert.NotNull(_mockViewModel.Object.CommandFailMessage);
        }


    }
#pragma warning restore CS1998
}
