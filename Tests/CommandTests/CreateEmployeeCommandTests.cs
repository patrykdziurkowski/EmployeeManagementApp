using BusinessLogic;
using BusinessLogic.Commands;
using BusinessLogic.Validators;
using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentResults;
using FluentValidation;
using FluentValidation.Results;
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
    public class CreateEmployeeCommandTests
    {
        private CreateEmployeeCommand _subject;

        private EmployeeValidator _employeeValidator;
 
        private Mock<IDateProvider> _mockDateProvider;
        private Mock<JobHistoryRepository> _mockJobHistoryRepository;
        private Mock<JobRepository> _mockJobRepository;
        private Mock<EmployeeRepository> _mockEmployeeRepository;
        private Mock<EmployeesMenuViewModel> _mockViewModel;

        public CreateEmployeeCommandTests()
        {
            _employeeValidator = new();

            Mock<ISqlDataAccess> mockDataAccess = new();
            _mockJobHistoryRepository = new (mockDataAccess.Object);
            _mockJobRepository = new (mockDataAccess.Object);
            _mockEmployeeRepository = new (mockDataAccess.Object);

            _mockDateProvider = new();

            _mockViewModel = new(_mockEmployeeRepository.Object,
                                _mockJobRepository.Object,
                                _mockJobHistoryRepository.Object,
                                _employeeValidator,
                                _mockDateProvider.Object);

            _subject = new(_mockViewModel.Object, _mockEmployeeRepository.Object, _employeeValidator);
        }

        [Fact]
        public async Task CanExecute_GivenValidNewEmployee_ReturnsTrue()
        {
            //Arrange
            EmployeeViewModel validNewEmployee = new()
            {
                EmployeeId = 100,
                FirstName = "John",
                LastName = "Smith",
                Email = "JSMITH",
                PhoneNumber = "111 111 1111",
                HireDate = DateOnly.MaxValue,
                JobId = "ST_CLERK"
            };

            _mockViewModel.Object.Employees.Add(validNewEmployee);

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.True(canExecute);
        }

        [Fact]
        public async Task CanExecute_GivenInvalidNewEmployee_ReturnsFalse()
        {
            //Arrange
            EmployeeViewModel invalidNewEmployee = new();
            _mockViewModel.Object.Employees.Add(invalidNewEmployee);

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.False(canExecute);
        }

        [Fact]
        public async Task CanExecute_GivenInvalidNewEmployee_SetsFailMessageAndFailureIndicator()
        {
            //Arrange
            EmployeeViewModel invalidNewEmployee = new();
            _mockViewModel.Object.Employees.Add(invalidNewEmployee);

            _mockViewModel.Object.CommandFailMessage = null;
            _mockViewModel.Object.IsLastCommandSuccessful = true;

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.False(_mockViewModel.Object.IsLastCommandSuccessful);
            Assert.NotNull(_mockViewModel.Object.CommandFailMessage);
        }

        [Fact]
        public async Task Execute_GivenSuccessfulDatabaseCreation_SetsSuccessIndicator()
        {
            //Arrange
            EmployeeViewModel validNewEmployee = new()
            {
                EmployeeId = 100,
                FirstName = "John",
                LastName = "Smith",
                Email = "JSMITH",
                PhoneNumber = "111 111 1111",
                HireDate = DateOnly.MaxValue,
                JobId = "ST_CLERK"
            };
            _mockViewModel.Object.Employees.Add(validNewEmployee);
            _mockViewModel.Object.CommandFailMessage = null;
            _mockViewModel.Object.IsLastCommandSuccessful = true;


            _mockEmployeeRepository
                .Setup(x => x.HireAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Ok()));

            //Act
            _subject.CanExecute(null);
            _subject.Execute(null);

            //Assert
            Assert.True(_mockViewModel.Object.IsLastCommandSuccessful);
        }

        [Fact]
        public async Task Execute_GivenUnsuccessfulDatabaseCreation_SetsFailureIndicatorAndMessage()
        {
            //Arrange
            EmployeeViewModel validNewEmployee = new()
            {
                EmployeeId = 100,
                FirstName = "John",
                LastName = "Smith",
                Email = "JSMITH",
                PhoneNumber = "111 111 1111",
                HireDate = DateOnly.MaxValue,
                JobId = "ST_CLERK"
            };
            _mockViewModel.Object.Employees.Add(validNewEmployee);
            _mockViewModel.Object.CommandFailMessage = null;
            _mockViewModel.Object.IsLastCommandSuccessful = true;


            _mockEmployeeRepository
                .Setup(x => x.HireAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Fail("")));

            //Act
            _subject.CanExecute(null);
            _subject.Execute(null);

            //Assert
            Assert.False(_mockViewModel.Object.IsLastCommandSuccessful);
            Assert.NotNull(_mockViewModel.Object.CommandFailMessage);
        }
    }
#pragma warning restore CS1998
}
