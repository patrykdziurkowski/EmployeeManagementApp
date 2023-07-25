﻿using BusinessLogic;
using BusinessLogic.Commands;
using BusinessLogic.Validators;
using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Repositories;
using FluentResults;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
#pragma warning disable CS1998
    public class DeleteEmployeeCommandTests
    {
        private DeleteEmployeeCommand _subject;

        private EmployeeValidator _employeeValidator;

        private Mock<IDateProvider> _mockDateProvider;
        private Mock<JobHistoryRepository> _mockJobHistoryRepository;
        private Mock<JobRepository> _mockJobRepository;
        private Mock<DepartmentRepository> _mockDepartmentRepository;
        private Mock<EmployeeRepository> _mockEmployeeRepository;
        private Mock<EmployeesMenuViewModel> _mockViewModel;

        public DeleteEmployeeCommandTests()
        {
            _employeeValidator = new();

            Mock<ISqlDataAccess> mockDataAccess = new();
            _mockJobHistoryRepository = new(mockDataAccess.Object);
            _mockJobRepository = new(mockDataAccess.Object);
            _mockDepartmentRepository = new(mockDataAccess.Object);
            _mockEmployeeRepository = new(mockDataAccess.Object);

            _mockDateProvider = new();

            _mockViewModel = new(
                _mockEmployeeRepository.Object,
                _mockDepartmentRepository.Object,
                _mockJobRepository.Object,
                _mockJobHistoryRepository.Object,
                _employeeValidator,
                _mockDateProvider.Object);

            _subject = new(
                _mockViewModel.Object,
                _mockEmployeeRepository.Object);
        }

        [Fact]
        public async Task Execute_UponSuccessfulDatabaseDeletion_RemovesEmployeeFromViewModelCollection()
        {
            //Arrange
            int employeeToDeleteId = 100;
            EmployeeDto validNewEmployee = new(
                employeeToDeleteId,
                "Smith",
                "JSMITH",
                "ST_CLERK");

            _mockViewModel.Object.Employees.Add(validNewEmployee);

            _mockEmployeeRepository
                .Setup(x => x.FireAsync(employeeToDeleteId))
                .Returns(Task.FromResult(Result.Ok()));

            //Act
            _subject.Execute(100);

            //Assert
            Assert.True(_mockViewModel.Object.IsLastCommandSuccessful);
            Assert.Empty(_mockViewModel.Object.Employees);
        }

        [Fact]
        public async Task Execute_UponUnsuccessfulDatabaseDeletion_DoesNotRemoveEmployeeFromViewModelCollection()
        {
            //Arrange
            int employeeToDeleteId = 100;
            EmployeeDto validNewEmployee = new(
                employeeToDeleteId,
                "Smith",
                "JSMITH",
                "ST_CLERK");

            _mockViewModel.Object.Employees.Add(validNewEmployee);

            _mockEmployeeRepository
                .Setup(x => x.FireAsync(employeeToDeleteId))
                .Returns(Task.FromResult(Result.Fail("")));

            //Act
            _subject.Execute(100);

            //Assert
            Assert.False(_mockViewModel.Object.IsLastCommandSuccessful);
            Assert.NotEmpty(_mockViewModel.Object.Employees);
            Assert.NotNull(_mockViewModel.Object.CommandFailMessage);
        }
    }
#pragma warning restore CS1998
}
