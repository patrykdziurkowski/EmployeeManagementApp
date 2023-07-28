using BusinessLogic.Commands;
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
    public class LoadDepartmentsCommandTests
    {
        private LoadDepartmentsCommand _subject;

        private Mock<DepartmentsMenuViewModel> _mockViewModel;
        private Mock<EmployeeRepository> _mockEmployeeRepository;
        private Mock<DepartmentRepository> _mockDepartmentRepository;

        public LoadDepartmentsCommandTests()
        {
            Mock<ISqlDataAccess> mockDataAccess = new();
            _mockEmployeeRepository = new(mockDataAccess.Object);
            _mockDepartmentRepository = new(mockDataAccess.Object);

            _mockViewModel = new(
                _mockDepartmentRepository.Object,
                _mockEmployeeRepository.Object);

            _subject = new(
                _mockViewModel.Object,
                _mockDepartmentRepository.Object,
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
        public async Task Execute_GivenDepartmentsFromDatabase_AssignsThemToViewModelProperty()
        {
            //Arrange
            IEnumerable<Department> departments = new List<Department>()
            {
                new Department { DepartmentId = 1 },
                new Department { DepartmentId = 2 },
                new Department { DepartmentId = 3 }
            };

            _mockDepartmentRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(departments));

            //Act
            bool canExecute = _subject.CanExecute(null);
            _subject.Execute(null);

            //Assert
            Assert.True(canExecute);
            Assert.Equal(3, _mockViewModel.Object.Departments.Count);
            Assert.Contains(_mockViewModel.Object.Departments, department => department.DepartmentId == 1);
            Assert.Contains(_mockViewModel.Object.Departments, department => department.DepartmentId == 2);
            Assert.Contains(_mockViewModel.Object.Departments, department => department.DepartmentId == 3);
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
        public async Task Execute_GivenEmployeesFromDatabase_AssignsThemToTheirDepartments()
        {
            //Arrange
            IEnumerable<Department> departments = new List<Department>()
            {
                new Department { DepartmentId = 10 },
                new Department { DepartmentId = 20 },
                new Department { DepartmentId = 30 }
            };

            IEnumerable<Employee> department10Employees = new List<Employee>()
            {
                new Employee
                {
                    EmployeeId = 1,
                    LastName = "LastName1",
                    Email = "EMAIL1",
                    JobId = "JOB1",
                    DepartmentId = 10
                }
            };
            IEnumerable<Employee> department20Employees = new List<Employee>()
            {
                new Employee
                {
                    EmployeeId = 2,
                    LastName = "LastName2",
                    Email = "EMAIL2",
                    JobId = "JOB2",
                    DepartmentId = 20
                }
            };
            IEnumerable<Employee> department30Employees = new List<Employee>()
            {
                new Employee
                {
                    EmployeeId = 3,
                    LastName = "LastName3",
                    Email = "EMAIL3",
                    JobId = "JOB3",
                    DepartmentId = 30
                }
            };

            _mockDepartmentRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(departments));
            _mockDepartmentRepository
                .SetupSequence(x => x.GetEmployeesForDepartmentAsync(It.IsAny<int>()))
                .Returns(Task.FromResult(department10Employees))
                .Returns(Task.FromResult(department20Employees))
                .Returns(Task.FromResult(department30Employees));

            //Act
            bool canExecute = _subject.CanExecute(null);
            _subject.Execute(null);

            //Assert
            DepartmentDto firstDepartment = _mockViewModel.Object.Departments.First(department => department.DepartmentId == 10);
            DepartmentDto secondDepartment = _mockViewModel.Object.Departments.First(department => department.DepartmentId == 20);
            DepartmentDto thirdDepartment = _mockViewModel.Object.Departments.First(department => department.DepartmentId == 30);

            Assert.True(canExecute);
            Assert.Contains(firstDepartment.Employees, employee => employee.EmployeeId == 1);
            Assert.Contains(secondDepartment.Employees, employee => employee.EmployeeId == 2);
            Assert.Contains(thirdDepartment.Employees, employee => employee.EmployeeId == 3);
        }

    }
#pragma warning restore CS1998
}
