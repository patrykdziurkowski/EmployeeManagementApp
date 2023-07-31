using BusinessLogic;
using BusinessLogic.Commands;
using BusinessLogic.Interfaces;
using BusinessLogic.Validators;
using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
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
    public class LoadEmployeesCommandTests
    {
        private readonly LoadEmployeesCommand _subject;

        private readonly Mock<IEmployeeValidatorFactory> _mockEmployeeValidatorFactory;
        private readonly Mock<IDateProvider> _mockDateProvider;
        private readonly Mock<JobHistoryRepository> _mockJobHistoryRepository;
        private readonly Mock<JobRepository> _mockJobRepository;
        private readonly Mock<EmployeeRepository> _mockEmployeeRepository;

        private readonly Mock<EmployeesMenuViewModel> _mockViewModel;

        public LoadEmployeesCommandTests()
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
                _mockEmployeeRepository.Object,
                _mockJobRepository.Object);
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
        public async Task Execute_GivenEmployeesFromDatabase_AssignsThemToViewModelProperty()
        {
            //Arrange
            IEnumerable<Employee> employees = new List<Employee>()
            {
                new Employee { EmployeeId = 1 },
                new Employee { EmployeeId = 2 },
                new Employee { EmployeeId = 3 }
            };
            
            _mockEmployeeRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(employees));

            //Act
            bool canExecute = _subject.CanExecute(null);
            _subject.Execute(null);

            //Assert
            Assert.True(canExecute);
            Assert.Equal(3, _mockViewModel.Object.Employees.Count);
            Assert.Contains(_mockViewModel.Object.Employees, employee => employee.EmployeeId == 1);
            Assert.Contains(_mockViewModel.Object.Employees, employee => employee.EmployeeId == 2);
            Assert.Contains(_mockViewModel.Object.Employees, employee => employee.EmployeeId == 3);
        }

        [Fact]
        public async Task Execute_GivenJobsFromDatabase_AssignsThemToViewModelProperty()
        {
            //Arrange
            IEnumerable<Job> jobs = new List<Job>()
            {
                new Job { JobId = "ST_CLERK" },
                new Job { JobId = "AD_VP" },
                new Job { JobId = "ST_MGR" }
            };

            _mockJobRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(jobs));

            //Act
            bool canExecute = _subject.CanExecute(null);
            _subject.Execute(null);

            //Assert
            Assert.True(canExecute);
            Assert.Equal(3, _mockViewModel.Object.Jobs.Count);
            Assert.Contains("ST_CLERK", _mockViewModel.Object.Jobs);
            Assert.Contains("AD_VP", _mockViewModel.Object.Jobs);
            Assert.Contains("ST_MGR", _mockViewModel.Object.Jobs);
        }

    }
#pragma warning restore CS1998
}
