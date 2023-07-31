using BusinessLogic;
using BusinessLogic.Commands;
using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Interfaces;
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
    public class DeleteEmployeeCommandTests
    {
        private readonly DeleteEmployeeCommand _subject;

        private readonly Mock<IEmployeeValidatorFactory> _mockEmployeeValidatorFactory;

        private readonly Mock<IDateProvider> _mockDateProvider;
        private readonly Mock<JobHistoryRepository> _mockJobHistoryRepository;
        private readonly Mock<JobRepository> _mockJobRepository;
        private readonly Mock<EmployeeRepository> _mockEmployeeRepository;
        private readonly Mock<EmployeesMenuViewModel> _mockViewModel;

        public DeleteEmployeeCommandTests()
        {
            Mock<ISqlDataAccess> mockDataAccess = new();
            _mockJobHistoryRepository = new(mockDataAccess.Object);
            _mockJobRepository = new(mockDataAccess.Object);
            _mockEmployeeRepository = new(mockDataAccess.Object);

            _mockEmployeeValidatorFactory = new();
            _mockEmployeeValidatorFactory
                .Setup(x => x.GetValidator(typeof(EmployeeValidator)))
                .Returns(new EmployeeValidator());

            _mockDateProvider = new();

            _mockViewModel = new(
                _mockEmployeeRepository.Object,
                _mockJobRepository.Object,
                _mockJobHistoryRepository.Object,
                _mockEmployeeValidatorFactory.Object,
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
