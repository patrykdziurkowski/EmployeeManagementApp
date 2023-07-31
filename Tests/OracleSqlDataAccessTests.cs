using Dapper;
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
    public class OracleSqlDataAccessTests
    {
        private OracleSqlDataAccess _subject;

        private Mock<IConnectionFactory> _mockConnectionFactory;
        private Mock<IDapperAdapter> _mockDapperAdapter;

        private Mock<IDbConnection> _mockConnection;
        private Mock<IDbTransaction> _mockTransaction;

        public OracleSqlDataAccessTests()
        {
            _mockConnection = new();
            
            _mockConnectionFactory = new();
            _mockConnectionFactory
                .Setup(x => x.GetConnection(ConnectionType.Oracle))
                .Returns(_mockConnection.Object);

            _mockDapperAdapter = new();

            _mockTransaction = new();
            _mockConnection
                .Setup(x => x.BeginTransaction())
                .Returns(_mockTransaction.Object);

            _subject = new(
                _mockConnectionFactory.Object,
                _mockDapperAdapter.Object);
        }


        [Fact]
        public async Task ExecuteSqlQueryAsync_OpensClosesAndDisposesOfConnection()
        {
            //Arrange

            //Act
            await _subject.ExecuteSqlQueryAsync<Employee>("");

            //Assert
            _mockConnection
                .Verify(x => x.Open());
            _mockConnection
                .Verify(x => x.Close());
            _mockConnection
                .Verify(x => x.Dispose());
        }

        [Fact]
        public async Task ExecuteSqlNonQueryAsync_OpensClosesAndDisposesOfConnection()
        {
            //Arrange

            //Act
            await _subject.ExecuteSqlNonQueryAsync("");

            //Assert
            _mockConnection
                .Verify(x => x.Open());
            _mockConnection
                .Verify(x => x.Close());
            _mockConnection
                .Verify(x => x.Dispose());
        }

        [Fact]
        public async Task ExecuteSqlNonQueryAsync_GivenNoExceptions_CommitsTransaction()
        {
            //Arrange

            //Act
            await _subject.ExecuteSqlNonQueryAsync("");

            //Assert
            _mockTransaction
                .Verify(x => x.Commit());
        }

        [Fact]
        public async Task ExecuteSqlNonQueryAsync_GivenException_RollsbackTransaction()
        {
            //Arrange
            _mockDapperAdapter
                .Setup(x => x.ExecuteAsync(
                    It.IsAny<IDbConnection>(),
                    It.IsAny<string>(),
                    null,
                    null,
                    null,
                    null))
                .Throws<Exception>();

            //Act
            await _subject.ExecuteSqlNonQueryAsync("");

            //Assert
            _mockTransaction
                .Verify(x => x.Rollback());
        }

        [Fact]
        public async Task ExecuteSqlNonQueryAsync_GivenMultipleCommands_ExecutesMultipleNonQueries()
        {
            //Arrange

            //Act
            await _subject.ExecuteSqlNonQueryAsync("command1;command2;command3");

            //Assert
            _mockDapperAdapter
                .Verify(x => x.ExecuteAsync(
                        It.IsAny<IDbConnection>(),
                        It.IsAny<string>(),
                        null,
                        null,
                        null,
                        null), 
                    Times.Exactly(3));
        }


    }

#pragma warning restore CS1998
}
