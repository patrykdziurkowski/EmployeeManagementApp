using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using Moq;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
#pragma warning disable CS1998
    public class OracleSQLDataAccessTests
    {
        private OracleSQLDataAccess _subject;

        private Mock<IConnectionFactory> _mockConnectionFactory;
        private Mock<ICommandFactory> _mockCommandFactory;

        private Mock<IDbConnection> _mockConnection;
        private Mock<IDbCommand> _mockCommand;

        private Mock<IDbTransaction> _mockTransaction;

        public OracleSQLDataAccessTests()
        {
            _mockConnection = new();
            _mockCommand = new();
            _mockCommand
                .Setup(x => x.ExecuteReader())
                .Returns((OracleDataReader)null!);
            
            _mockConnectionFactory = new();
            _mockConnectionFactory
                .Setup(x => x.GetConnection(ConnectionType.Oracle))
                .Returns(_mockConnection.Object);

            _mockCommandFactory = new();
            _mockCommandFactory
                .Setup(x => x.GetCommand(It.IsAny<string>(), _mockConnection.Object, ConnectionType.Oracle))
                .Returns(_mockCommand.Object);

            _mockTransaction = new();
            _mockConnection
                .Setup(x => x.BeginTransaction())
                .Returns(_mockTransaction.Object);

            _subject = new(_mockConnectionFactory.Object, _mockCommandFactory.Object);
        }


        [Fact]
        public async Task ExecuteSQLQueryAsync_OpensClosesAndDisposesOfConnection()
        {
            //Arrange

            //Act
            await _subject.ExecuteSQLQueryAsync<Employee>("");

            //Assert
            _mockConnection
                .Verify(x => x.Open());
            _mockConnection
                .Verify(x => x.Close());
            _mockConnection
                .Verify(x => x.Dispose());
        }

        [Fact]
        public async Task ExecuteSQLNonQueryAsync_OpensClosesAndDisposesOfConnection()
        {
            //Arrange

            //Act
            await _subject.ExecuteSQLNonQueryAsync("");

            //Assert
            _mockConnection
                .Verify(x => x.Open());
            _mockConnection
                .Verify(x => x.Close());
            _mockConnection
                .Verify(x => x.Dispose());
        }

        [Fact]
        public async Task ExecuteSQLNonQueryAsync_GivenNoExceptions_CommitsTransaction()
        {
            //Arrange

            //Act
            await _subject.ExecuteSQLNonQueryAsync("");

            //Assert
            _mockTransaction
                .Verify(x => x.Commit());
        }

        [Fact]
        public async Task ExecuteSQLNonQueryAsync_GivenException_RollsbackTransaction()
        {
            //Arrange
            _mockCommand
                .Setup(x => x.ExecuteNonQuery())
                .Throws<Exception>();

            //Act
            await _subject.ExecuteSQLNonQueryAsync("");

            //Assert
            _mockTransaction
                .Verify(x => x.Rollback());
        }

        [Fact]
        public async Task ExecuteSQLNonQueryAsync_GivenMultipleCommands_ExecutesMultipleNonQueries()
        {
            //Arrange

            //Act
            await _subject.ExecuteSQLNonQueryAsync("command1;command2;command3");

            //Assert
            _mockCommand
                .Verify(x => x.ExecuteNonQuery(), Times.Exactly(3));
        }


    }

#pragma warning restore CS1998
}
