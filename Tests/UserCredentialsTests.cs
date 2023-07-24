using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
#pragma warning disable CS1998
    public class UserCredentialsTests
    {
        [Fact]
        public async Task Clear_SetsUserNameAndPasswordToNull()
        {
            //Arrange
            UserCredentials userCredentials = new();
            userCredentials.UserName = "userName";
            userCredentials.Password = "password";

            //Act
            userCredentials.Clear();

            //Assert
            Assert.Null(userCredentials.UserName);
            Assert.Null(userCredentials.Password);
        }

        [Fact]
        public async Task AreFilledOut_GivenNonNullUserNameAndPassword_ReturnsTrue()
        {
            //Arrange
            UserCredentials userCredentials = new();
            userCredentials.UserName = "userName";
            userCredentials.Password = "password";

            //Act
            bool areFilledOut = userCredentials.AreFilledOut();

            //Assert
            Assert.True(areFilledOut);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "userName")]
        [InlineData("userName", null)]
        public async Task AreFilledOut_GivenNullUserNameOrPassword_ReturnsFalse(string userName, string password)
        {
            //Arrange
            UserCredentials userCredentials = new();
            userCredentials.UserName = userName;
            userCredentials.Password = password;

            //Act
            bool areFilledOut = userCredentials.AreFilledOut();

            //Assert
            Assert.False(areFilledOut);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "userName")]
        [InlineData("userName", null)]
        public async Task AreNotFilledOut_GivenNullUserNameOrPassword_ReturnsTrue(string userName, string password)
        {
            //Arrange
            UserCredentials userCredentials = new();
            userCredentials.UserName = userName;
            userCredentials.Password = password;

            //Act
            bool areFilledOut = userCredentials.AreNotFilledOut();

            //Assert
            Assert.True(areFilledOut);
        }

        [Fact]
        public async Task AreNotFilledOut_GivenNonNullUserNameAndPassword_ReturnsFalse()
        {
            //Arrange
            UserCredentials userCredentials = new();
            userCredentials.UserName = "userName";
            userCredentials.Password = "password";

            //Act
            bool areFilledOut = userCredentials.AreNotFilledOut();

            //Assert
            Assert.False(areFilledOut);
        }

    }

#pragma warning restore CS1998
}
