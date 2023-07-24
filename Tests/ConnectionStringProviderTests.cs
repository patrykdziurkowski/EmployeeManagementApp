using DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
#pragma warning disable CS1998
    public class ConnectionStringProviderTests
    {

        public ConnectionStringProviderTests()
        {

        }

        [Fact]
        public async Task Constructor_GivenNonNullOrEmptyHostPortSid_DoesntThrowException()
        {
            //Arrange
            ConfigurationManager.AppSettings["host"] = "host";
            ConfigurationManager.AppSettings["port"] = "port";
            ConfigurationManager.AppSettings["sid"] = "sid";

            UserCredentials userCredentials = new();

            try
            {
                //Act
                ConnectionStringProvider connectionStringProvider = new(userCredentials);

                //Assert
                Assert.True(true);

            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, null, "test")]
        [InlineData(null, "test", null)]
        [InlineData(null, "test", "test")]
        [InlineData("test", null, null)]
        [InlineData("test", null, "test")]
        [InlineData("test", "test", null)]
        public async Task Constructor_GivenNullHostPortSid_ThrowsException(string host, string port, string sid)
        {
            //Arrange
            ConfigurationManager.AppSettings["host"] = host;
            ConfigurationManager.AppSettings["port"] = port;
            ConfigurationManager.AppSettings["sid"] = sid;

            UserCredentials userCredentials = new();

            try
            {
                //Act
                ConnectionStringProvider connectionStringProvider = new(userCredentials);

                //Assert
                Assert.Fail("The construction didn't throw despite one of the parameters being null");

            }
            catch (Exception)
            {
                Assert.True(true);
            }

        }

        [Theory]
        [InlineData("", "", "")]
        [InlineData("", "", "test")]
        [InlineData("", "test", "")]
        [InlineData("", "test", "test")]
        [InlineData("test", "", "")]
        [InlineData("test", "", "test")]
        [InlineData("test", "test", "")]
        public async Task Constructor_GivenEmptyHostPortSid_ThrowsException(string host, string port, string sid)
        {
            //Arrange
            ConfigurationManager.AppSettings["host"] = host;
            ConfigurationManager.AppSettings["port"] = port;
            ConfigurationManager.AppSettings["sid"] = sid;

            UserCredentials userCredentials = new();

            try
            {
                //Act
                ConnectionStringProvider connectionStringProvider = new(userCredentials);

                //Assert
                Assert.Fail("The construction didn't throw despite one of the parameters being empty");

            }
            catch (Exception)
            {
                Assert.True(true);
            }

        }

        [Fact]
        public async Task GetConnectionString_GivenValidUsernamePassword_ReturnsConnectionStringContainingHostPortSidUsernamePassword()
        {
            //Arrange
            string host = "host";
            string port = "port";
            string sid = "sid";

            ConfigurationManager.AppSettings["host"] = host;
            ConfigurationManager.AppSettings["port"] = port;
            ConfigurationManager.AppSettings["sid"] = sid;

            UserCredentials userCredentials = new();
            userCredentials.UserName = "userName";
            userCredentials.Password = "password";

            ConnectionStringProvider connectionStringProvider = new(userCredentials);

            //Act
            string connectionString = connectionStringProvider.GetConnectionString();

            //Assert
            Assert.False(string.IsNullOrEmpty(connectionString));
            Assert.Contains(host, connectionString);
            Assert.Contains(port, connectionString);
            Assert.Contains(sid, connectionString);
            Assert.Contains(userCredentials.UserName, connectionString);
            Assert.Contains(userCredentials.Password, connectionString);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "password")]
        [InlineData("userName", null)]
        public async Task GetConnectionString_GivenNullUsernameOrPassword_Throws(string userName, string password)
        {
            //Arrange
            ConfigurationManager.AppSettings["host"] = "host";
            ConfigurationManager.AppSettings["port"] = "port";
            ConfigurationManager.AppSettings["sid"] = "sid";

            UserCredentials userCredentials = new();
            userCredentials.UserName = userName;
            userCredentials.Password = password;

            ConnectionStringProvider connectionStringProvider = new(userCredentials);

            //Act
            try
            {
                string connectionString = connectionStringProvider.GetConnectionString();

                Assert.Fail("GetConnectionString() should have thrown an exception but didn't");
            }
            catch (InvalidOperationException)
            {
                Assert.True(true);
            }

        }

    }

#pragma warning restore CS1998
}
