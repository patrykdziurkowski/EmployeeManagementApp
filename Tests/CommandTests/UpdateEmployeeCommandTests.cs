using BusinessLogic;
using BusinessLogic.Commands;
using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
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
    public class UpdateEmployeeCommandTests
    {
        private UpdateEmployeeCommand _subject;

        private Mock<IEmployeeValidatorFactory> _mockEmployeeValidatorFactory;

        private Mock<IDateProvider> _mockDateProvider;
        private Mock<JobHistoryRepository> _mockJobHistoryRepository;
        private Mock<JobRepository> _mockJobRepository;
        private Mock<EmployeeRepository> _mockEmployeeRepository;
        private Mock<DepartmentRepository> _mockDepartmentRepository;
        private Mock<EmployeesMenuViewModel> _mockViewModel;

        public UpdateEmployeeCommandTests()
        {
            Mock<ISqlDataAccess> mockDataAccess = new();
            _mockJobHistoryRepository = new(mockDataAccess.Object);
            _mockJobRepository = new(mockDataAccess.Object);
            _mockEmployeeRepository = new(mockDataAccess.Object);
            _mockDepartmentRepository = new(mockDataAccess.Object);

            _mockEmployeeValidatorFactory = new();
            _mockEmployeeValidatorFactory
                .Setup(x => x.GetValidator(typeof(EmployeeValidator)))
                .Returns(new EmployeeValidator());
            _mockEmployeeValidatorFactory
                .Setup(x => x.GetValidator(typeof(CommissionPctValidator)))
                .Returns(new CommissionPctValidator(_mockDepartmentRepository.Object));

            _mockDateProvider = new();

            _mockViewModel = new(
                _mockEmployeeRepository.Object,
                _mockJobRepository.Object,
                _mockJobHistoryRepository.Object,
                _mockEmployeeValidatorFactory.Object,
                _mockDateProvider.Object);

            _subject = new(
                _mockViewModel.Object,
                _mockEmployeeRepository.Object,
                _mockEmployeeValidatorFactory.Object,
                _mockDateProvider.Object,
                _mockJobHistoryRepository.Object);
        }

        [Fact]
        public async Task CanExecute_GivenValidEmployee_ReturnsTrue()
        {
            //Arrange
            EmployeeDto validEmployee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_CLERK");

            _mockViewModel.Object.UpdatedEmployee = validEmployee;

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.True(canExecute);
        }

        [Fact]
        public async Task CanExecute_GivenInvalidEmployee_ReturnsFalse()
        {
            //Arrange
            EmployeeDto invalidEmployee = new(
                -1,
                "Smith",
                "JSMITH",
                "ST_CLERK");

            _mockViewModel.Object.UpdatedEmployee = invalidEmployee;

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.False(canExecute);
        }

        [Fact]
        public async Task CanExecute_GivenInvalidEmployee_SetsFailMessageAndIndicator()
        {
            //Arrange
            EmployeeDto invalidEmployee = new(
                -1,
                "Smith",
                "JSMITH",
                "ST_CLERK");
            _mockViewModel.Object.UpdatedEmployee = invalidEmployee;
            _mockViewModel.Object.CommandFailMessage = null;
            _mockViewModel.Object.IsLastCommandSuccessful = true;

            //Act
            _subject.CanExecute(null);

            //Assert
            Assert.False(_mockViewModel.Object.IsLastCommandSuccessful);
            Assert.NotNull(_mockViewModel.Object.CommandFailMessage);
        }

        [Fact]
        public async Task Execute_GivenSuccessfulDatabaseUpdateAndNoJobChange_SetsSuccessIndicator()
        {
            //Arrange
            EmployeeDto validEmployee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_CLERK");

            _mockViewModel.Object.UpdatedEmployee = validEmployee;
            _mockViewModel.Object.IsLastCommandSuccessful = false;
            _mockViewModel.Object.UpdatedEmployeePreviousJob = new Job()
            {
                JobId = validEmployee.JobId
            };

            _mockEmployeeRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Ok()));

            //Act
            _subject.Execute(null);

            //Assert
            Assert.True(_mockViewModel.Object.IsLastCommandSuccessful);
        }

        [Fact]
        public async Task Execute_GivenUnsuccessfulDatabaseUpdateAndNoJobChange_SetsFailureIndicatorAndMessage()
        {
            //Arrange
            EmployeeDto validEmployee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_CLERK");

            _mockViewModel.Object.UpdatedEmployee = validEmployee;
            _mockViewModel.Object.IsLastCommandSuccessful = true;
            _mockViewModel.Object.CommandFailMessage = null;
            _mockViewModel.Object.UpdatedEmployeePreviousJob = new Job()
            {
                JobId = validEmployee.JobId
            };

            _mockEmployeeRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Fail("")));

            //Act
            _subject.Execute(null);

            //Assert
            Assert.False(_mockViewModel.Object.IsLastCommandSuccessful);
            Assert.NotNull(_mockViewModel.Object.CommandFailMessage);
        }

        [Fact]
        public async Task Execute_GivenJobChangeAndSuccessfulDatabaseJobEntryCreation_SetsSuccessIndicator()
        {
            //Arrange
            EmployeeDto validEmployee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_MGR");

            _mockViewModel.Object.UpdatedEmployee = validEmployee;
            _mockViewModel.Object.IsLastCommandSuccessful = false;
            _mockViewModel.Object.UpdatedEmployeePreviousJob = new Job()
            {
                JobId = "ST_CLERK"
            };

            _mockEmployeeRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Ok()));
            _mockJobHistoryRepository
                .Setup(x => x.InsertAsync(It.IsAny<JobHistory>()))
                .Returns(Task.FromResult(Result.Ok()));

            //Act
            _subject.Execute(null);

            //Assert
            Assert.True(_mockViewModel.Object.IsLastCommandSuccessful);
        }

        [Fact]
        public async Task Execute_GivenJobChangeAndUnsuccessfulDatabaseJobEntryCreation_SetsFailMessageAndIndicator()
        {
            //Arrange
            EmployeeDto validEmployee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_MGR");

            _mockViewModel.Object.UpdatedEmployee = validEmployee;
            _mockViewModel.Object.IsLastCommandSuccessful = false;
            _mockViewModel.Object.UpdatedEmployeePreviousJob = new Job()
            {
                JobId = "ST_CLERK"
            };

            _mockEmployeeRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Ok()));
            _mockJobHistoryRepository
                .Setup(x => x.InsertAsync(It.IsAny<JobHistory>()))
                .Returns(Task.FromResult(Result.Fail("")));

            //Act
            _subject.Execute(null);

            //Assert
            Assert.False(_mockViewModel.Object.IsLastCommandSuccessful);
            Assert.NotNull(_mockViewModel.Object.CommandFailMessage);
        }

        [Fact]
        public async Task Execute_GivenTwoJobChangesInOneDay_SetsFailMessageAndIndicator()
        {
            //Arrange
            EmployeeDto validEmployee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_MGR");

            _mockViewModel.Object.UpdatedEmployee = validEmployee;
            _mockViewModel.Object.IsLastCommandSuccessful = true;
            _mockViewModel.Object.UpdatedEmployeePreviousJob = new Job()
            {
                JobId = "ST_CLERK"
            };

            IEnumerable<JobHistory> employeesPastJobs = new List<JobHistory>()
            {
                new JobHistory()
                {
                    EmployeeId = validEmployee.EmployeeId,
                    StartDate = validEmployee.HireDate.ToDateTime(TimeOnly.MinValue),
                    EndDate = new DateTime(2008, 4, 27)
                }
            };
            _mockDateProvider
                .Setup(x => x.GetNow())
                .Returns(new DateOnly(2008, 4, 27));
            _mockEmployeeRepository
                .Setup(x => x.UpdateAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Ok()));
            _mockJobHistoryRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(employeesPastJobs));
                

            //Act
            _subject.Execute(null);

            //Assert
            Assert.False(_mockViewModel.Object.IsLastCommandSuccessful);
            Assert.NotNull(_mockViewModel.Object.CommandFailMessage);
        }


    }
#pragma warning restore CS1998
}
