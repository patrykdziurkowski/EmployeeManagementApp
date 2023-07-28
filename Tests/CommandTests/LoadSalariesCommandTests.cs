using BusinessLogic.Commands;
using BusinessLogic.ViewModels;
using DataAccess;
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
    public class LoadSalariesCommandTests
    {
        private LoadSalariesCommand _subject;

        private Mock<EmployeeRepository> _mockEmployeeRepository;
        private Mock<SalariesMenuViewModel> _mockViewModel;

        public LoadSalariesCommandTests()
        {
            Mock<ISqlDataAccess> mockDataAccess = new();
            _mockEmployeeRepository = new(mockDataAccess.Object);

            _mockViewModel = new(_mockEmployeeRepository.Object);

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
            Assert.Equal(3, _mockViewModel.Object.Salaries.Count);
            Assert.Contains(_mockViewModel.Object.Salaries, employee => employee.EmployeeId == 1);
            Assert.Contains(_mockViewModel.Object.Salaries, employee => employee.EmployeeId == 2);
            Assert.Contains(_mockViewModel.Object.Salaries, employee => employee.EmployeeId == 3);
        }

    }
#pragma warning restore CS1998
}
