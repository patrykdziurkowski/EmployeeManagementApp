using BusinessLogic;
using BusinessLogic.Commands;
using BusinessLogic.Interfaces;
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

        private Mock<IEmployeeValidatorFactory> _mockEmployeeValidatorFactory;
 
        private Mock<IDateProvider> _mockDateProvider;
        private Mock<JobHistoryRepository> _mockJobHistoryRepository;
        private Mock<JobRepository> _mockJobRepository;
        private Mock<DepartmentRepository> _mockDepartmentRepository;
        private Mock<EmployeeRepository> _mockEmployeeRepository;
        private Mock<EmployeesMenuViewModel> _mockViewModel;

        public CreateEmployeeCommandTests()
        {
            Mock<ISqlDataAccess> mockDataAccess = new();
            _mockJobHistoryRepository = new (mockDataAccess.Object);
            _mockJobRepository = new (mockDataAccess.Object);
            _mockDepartmentRepository = new(mockDataAccess.Object);
            _mockEmployeeRepository = new (mockDataAccess.Object);

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
                _mockEmployeeValidatorFactory.Object);
        }

        [Fact]
        public async Task CanExecute_GivenValidNewEmployee_ReturnsTrue()
        {
            //Arrange
            EmployeeDto validNewEmployee = new(
                100,
                "Smith",
                "JSMITH",
                "ST_CLERK");

            _mockViewModel.Object.Employees.Add(validNewEmployee);
            _mockViewModel.Object.NewEmployee = validNewEmployee;

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.True(canExecute);
        }

        [Fact]
        public async Task CanExecute_GivenInvalidNewEmployee_ReturnsFalse()
        {
            //Arrange
            EmployeeDto invalidNewEmployee = new(
                100,
                "Smith",
                "JSMITH",
                "ST_CLERK")
            {
                Salary = -10
            };
            _mockViewModel.Object.Employees.Add(invalidNewEmployee);
            _mockViewModel.Object.NewEmployee = invalidNewEmployee;

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.False(canExecute);
        }

        [Fact]
        public async Task CanExecute_GivenInvalidNewEmployee_SetsFailMessageAndFailureIndicator()
        {
            //Arrange
            EmployeeDto invalidNewEmployee = new(
                100,
                "Smith",
                "JSMITH",
                "ST_CLERK")
            {
                Salary = -10
            };
            _mockViewModel.Object.Employees.Add(invalidNewEmployee);
            _mockViewModel.Object.NewEmployee = invalidNewEmployee;

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
            EmployeeDto validNewEmployee = new(
                100,
                "Smith",
                "JSMITH",
                "ST_CLERK");

            _mockViewModel.Object.Employees.Add(validNewEmployee);
            _mockViewModel.Object.NewEmployee = validNewEmployee;
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
            EmployeeDto validNewEmployee = new(
                100,
                "Smith",
                "JSMITH",
                "ST_CLERK");

            _mockViewModel.Object.Employees.Add(validNewEmployee);
            _mockViewModel.Object.NewEmployee = validNewEmployee;
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

        [Fact]
        public async Task Execute_GivenCommissionPctFromNonSalesDepartmentEmployee_SetsFailureIndicatorAndMessage()
        {
            //Arrange
            IEnumerable<Department> departments = new List<Department>()
            {
                new Department()
                {
                    DepartmentName = "Sales",
                    DepartmentId = 80
                }
            };

            EmployeeDto invalidNewEmployee = new(
                100,
                "Smith",
                "JSMITH",
                "ST_CLERK")
            {
                DepartmentId = 110,
                CommissionPct = 0.5f
            };

            _mockViewModel.Object.Employees.Add(invalidNewEmployee);
            _mockViewModel.Object.NewEmployee = invalidNewEmployee;
            _mockViewModel.Object.CommandFailMessage = null;
            _mockViewModel.Object.IsLastCommandSuccessful = true;


            _mockDepartmentRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(departments));
            _mockEmployeeRepository
                .Setup(x => x.HireAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Ok()));

            //Act
            _subject.CanExecute(null);
            _subject.Execute(null);

            //Assert
            Assert.False(_mockViewModel.Object.IsLastCommandSuccessful);
            Assert.NotNull(_mockViewModel.Object.CommandFailMessage);
        }

        [Fact]
        public async Task Execute_GivenCommissionPctFromSalesDepartmentEmployee_SetsSuccessIndicator()
        {
            //Arrange
            IEnumerable<Department> departments = new List<Department>()
            {
                new Department()
                {
                    DepartmentName = "Sales",
                    DepartmentId = 80
                }
            };

            EmployeeDto invalidNewEmployee = new(
                100,
                "Smith",
                "JSMITH",
                "ST_CLERK")
            {
                DepartmentId = 80,
                CommissionPct = 0.5f
            };

            _mockViewModel.Object.Employees.Add(invalidNewEmployee);
            _mockViewModel.Object.NewEmployee = invalidNewEmployee;
            _mockViewModel.Object.CommandFailMessage = null;
            _mockViewModel.Object.IsLastCommandSuccessful = true;


            _mockDepartmentRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(departments));
            _mockEmployeeRepository
                .Setup(x => x.HireAsync(It.IsAny<Employee>()))
                .Returns(Task.FromResult(Result.Ok()));

            //Act
            _subject.CanExecute(null);
            _subject.Execute(null);

            //Assert
            Assert.True(_mockViewModel.Object.IsLastCommandSuccessful);
        }

    }
#pragma warning restore CS1998
}
