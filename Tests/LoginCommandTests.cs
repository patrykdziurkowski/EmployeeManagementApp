using BusinessLogic.Commands;
using BusinessLogic.ViewModels;
using DataAccess;
using DataAccess.Repositories;
using Moq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
#pragma warning disable CS1998
    public class LoginCommandTests
    {
        private LoginCommand _subject;

        private Mock<UserCredentials> _mockUserCredentials;
        private Mock<StartMenuViewModel> _mockViewModel;
        private Mock<EmployeeRepository> _mockEmployeeRepository;

        public LoginCommandTests()
        {
            Mock<ISQLDataAccess> dataAccess = new();

            _mockEmployeeRepository = new(dataAccess.Object);
            _mockUserCredentials = new();
            _mockViewModel = new(_mockUserCredentials.Object, _mockEmployeeRepository.Object);

            _subject = new LoginCommand(_mockViewModel.Object, _mockEmployeeRepository.Object);
        }

        [Fact]
        public async Task CanExecute_GivenFilledOutCredentials_ReturnsTrue()
        {
            //Arrange
            _mockUserCredentials.Object.UserName = "testUserName";
            _mockUserCredentials.Object.Password = "testPassword";

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.True(canExecute);
        }

        [Fact]
        public async Task CanExecute_GivenBothStringEmptyCredentials_ReturnsFalse()
        {
            //Arrange
            _mockUserCredentials.Object.UserName = "";
            _mockUserCredentials.Object.Password = "";

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.False(canExecute);
        }

        [Fact]
        public async Task CanExecute_GivenBothNullCredentials_ReturnsFalse()
        {
            //Arrange
            _mockUserCredentials.Object.UserName = null;
            _mockUserCredentials.Object.Password = null;

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.False(canExecute);
        }

        [Fact]
        public async Task CanExecute_GivenNullPasswordButFilledOutUserName_ReturnsFalse()
        {
            //Arrange
            _mockUserCredentials.Object.UserName = "test";
            _mockUserCredentials.Object.Password = null;

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.False(canExecute);
        }

        [Fact]
        public async Task CanExecute_GivenNullUserNameButFilledOutPassword_ReturnsFalse()
        {
            //Arrange
            _mockUserCredentials.Object.UserName = null;
            _mockUserCredentials.Object.Password = "test";

            //Act
            bool canExecute = _subject.CanExecute(null);

            //Assert
            Assert.False(canExecute);
        }

        [Fact]
        public async Task CanExecute_SetsLoginAttemptIndicatorToFalse()
        {
            //Arrange


            //Act
            _subject.CanExecute(null);
            bool isLoginSuccessful = _mockViewModel.Object.IsLoginSuccessful;

            //Assert
            Assert.False(isLoginSuccessful);
        }


        [Fact]
        public async Task Execute_GivenSuccessfulLogin_ClearsErrorMessagesAndSetsLoginIndicatorToTrue()
        {
            //Arrange

            //Act
            _subject.Execute(null);

            //Assert
            Assert.True(_mockViewModel.Object.IsLoginSuccessful);
            Assert.Empty(_mockViewModel.Object.LoginErrorMessages);
        }

        [Fact]
        public async Task Execute_GivenUnsuccessfulLogin_SetsErrorMessagesAndLoginIndicatorToFalseAndClearsCredentials()
        {
            //Arrange
            _mockEmployeeRepository
                .Setup(x => x.GetAllAsync())
                .Callback(() =>
                {
                    ConstructorInfo? constructorInfo = typeof(OracleException).GetConstructor(
                        BindingFlags.NonPublic | BindingFlags.Instance,
                        null,
                        new Type[] { typeof(int), typeof(string), typeof(string), typeof(string), typeof(int) },
                        null);

                    if (constructorInfo is null)
                    {
                        Assert.Fail("Given constructor for OracleException was not found!");
                    }

                    OracleException oracleException = (OracleException)constructorInfo
                        .Invoke(new object[] { 1234, "", "", "", -1 });
                    throw oracleException;
                });

            //Act
            _subject.Execute(null);

            //Assert
            Assert.False(_mockViewModel.Object.IsLoginSuccessful);
            Assert.NotEmpty(_mockViewModel.Object.LoginErrorMessages);
            Assert.Null(_mockUserCredentials.Object.UserName);
            Assert.Null(_mockUserCredentials.Object.Password);
        }
    }
#pragma warning restore CS1998
}
