﻿using BusinessLogic.Validators;
using BusinessLogic.ViewModels;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
#pragma warning disable CS1998

    public class EmployeeValidatorTests
    {
        private EmployeeValidator _subject;

        public EmployeeValidatorTests()
        {
            _subject = new EmployeeValidator();
        }

        [Fact]
        public async Task Validator_GivenValidEmployee_Succeeds()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "ST_CLERK"
            };

            //Act
            ValidationResult result = _subject.Validate(employee);
            
            //Assert
            if (result.IsValid)
            {
                Assert.True(true);
            }
            else
            {
                Assert.Fail("Employee validation should have passed");
            }
        }

        [Theory]
        [InlineData(0, null, null, null)]
        [InlineData(1, "Smith", "JSMITH", null)]
        [InlineData(1, "Smith", null, "ST_CLERK")]
        [InlineData(1, null, "JSMITH", "ST_CLERK")]
        [InlineData(0, "Smith", "JSMITH", "ST_CLERK")]
        public async Task Validator_GivenEmptyCoreProperties_Fails(
            int employeeId,
            string lastName,
            string email,
            string jobId)

        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = employeeId,
                LastName = lastName,
                Email = email,
                JobId = jobId
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenNegativeEmployeeId_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = -1,
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "ST_CLERK"
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenTooLongFirstName_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                FirstName = "012345678901234567890",
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "ST_CLERK"
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenTooLongLastName_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "01234567890123456789012345",
                Email = "JSMITH",
                JobId = "ST_CLERK"
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenTooLongEmail_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "01234567890123456789012345",
                JobId = "ST_CLERK"
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenTooLongPhoneNumber_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                PhoneNumber = "012345678901234567890",
                JobId = "ST_CLERK"
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenTooLongJobId_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "01234567890"
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenNegativeSalary_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                Salary = -5,
                JobId = "ST_CLERK"
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenSalaryEqualToZero_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                Salary = 0,
                JobId = "ST_CLERK"
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenNegativeCommissionPctWhileInSalesDepartment_Fails()
        {
            //Arrange
            short salesDepartmentId = 80;

            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "ST_CLERK",
                CommissionPct = -0.5f,
                DepartmentId = salesDepartmentId
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenNullCommissionPctWhileInSalesDepartment_Succeeds()
        {
            //Arrange
            short salesDepartmentId = 80;

            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "ST_CLERK",
                CommissionPct = null,
                DepartmentId = salesDepartmentId
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.True(true);
            }
            else
            {
                Assert.Fail("Employee validation should not have passed");
            }
        }

        [Fact]
        public async Task Validator_GivenCommissionPctEqualToZeroWhileInSalesDepartment_Fails()
        {
            //Arrange
            short salesDepartmentId = 80;

            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "ST_CLERK",
                CommissionPct = 0f,
                DepartmentId = salesDepartmentId
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenNegativeManagerId_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "ST_CLERK",
                ManagerId = -5
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenManagerIdEqualToZero_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "ST_CLERK",
                ManagerId = 0
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenNegativeDepartmentId_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "ST_CLERK",
                DepartmentId = -5
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async Task Validator_GivenDepartmentIdEqualToZero_Fails()
        {
            //Arrange
            EmployeeViewModel employee = new()
            {
                EmployeeId = 1,
                LastName = "Smith",
                Email = "JSMITH",
                JobId = "ST_CLERK",
                DepartmentId = 0
            };

            //Act
            ValidationResult result = _subject.Validate(employee);

            //Assert
            if (result.IsValid)
            {
                Assert.Fail("Employee validation should not have passed");
            }
            else
            {
                Assert.True(true);
            }
        }

    }

#pragma warning restore CS1998
}
