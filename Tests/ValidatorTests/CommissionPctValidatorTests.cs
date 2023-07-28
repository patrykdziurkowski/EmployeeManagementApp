using BusinessLogic.Validators;
using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Repositories;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.ValidatorTests
{
#pragma warning disable CS1998
    public class CommissionPctValidatorTests
    {
        private CommissionPctValidator _subject;

        private Mock<DepartmentRepository> _mockDepartmentRepository;

        public CommissionPctValidatorTests()
        {
            Mock<ISqlDataAccess> mockDataAccess = new();
            _mockDepartmentRepository = new(mockDataAccess.Object);

            _subject = new(_mockDepartmentRepository.Object);
        }

        [Fact]
        public async Task Validator_GivenCommissionPctFromNullDepartmentIdEmployee_Fails()
        {
            //Arrange
            IEnumerable<Department> departments = new List<Department>()
            {
                new Department() { DepartmentId = 80, DepartmentName = "Sales" },
                new Department() { DepartmentId = 110, DepartmentName = "IT" }
            };
            EmployeeDto invalidEmployee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_CLERK")
            {
                CommissionPct = 0.5f
            };

            _mockDepartmentRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(departments));

            //Act
            ValidationResult result = await _subject.ValidateAsync(invalidEmployee);

            //Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validator_GivenCommissionPctFromNonSalesDepartmentEmployee_Fails()
        {
            //Arrange
            IEnumerable<Department> departments = new List<Department>()
            {
                new Department() { DepartmentId = 80, DepartmentName = "Sales" },
                new Department() { DepartmentId = 110, DepartmentName = "IT" }
            };
            EmployeeDto invalidEmployee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_CLERK")
            {
                CommissionPct = 0.5f,
                DepartmentId = 110
            };

            _mockDepartmentRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(departments));

            //Act
            ValidationResult result = await _subject.ValidateAsync(invalidEmployee);

            //Assert
            Assert.False(result.IsValid);
        }

        [Fact]
        public async Task Validator_GivenCommissionPctFromSalesDepartmentEmployee_Succeeds()
        {
            //Arrange
            IEnumerable<Department> departments = new List<Department>()
            {
                new Department() { DepartmentId = 80, DepartmentName = "Sales" },
                new Department() { DepartmentId = 110, DepartmentName = "IT" }
            };
            EmployeeDto validEmployee = new(
                1,
                "Smith",
                "JSMITH",
                "ST_CLERK")
            {
                CommissionPct = 0.5f,
                DepartmentId = 80
            };

            _mockDepartmentRepository
                .Setup(x => x.GetAllAsync())
                .Returns(Task.FromResult(departments));

            //Act
            ValidationResult result = await _subject.ValidateAsync(validEmployee);

            //Assert
            Assert.True(result.IsValid);
        }



    }
#pragma warning restore CS1998
}
