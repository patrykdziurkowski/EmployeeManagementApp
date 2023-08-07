using Dapper;
using DataAccess.Interfaces;
using FluentResults;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DataAccess
{
    //A class that abstracts Oracle's framework to simplify CRUD operations
    public class OracleSqlDataAccess : ISqlDataAccess
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private readonly IConnectionFactory _connectionFactory;
        private readonly IDapperAdapter _dapperAdapter;

        private IDbConnection? _connection;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public OracleSqlDataAccess(
            IConnectionFactory connectionFactory,
            IDapperAdapter dapperAdapter)
        {
            _connectionFactory = connectionFactory;
            _dapperAdapter = dapperAdapter;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task<Result> ExecuteStoredProcedureAsync(
            string procedureName,
            object? data = null)
        {
            Open();

            try
            {
                await _connection.ExecuteAsync(procedureName, data, commandType: CommandType.StoredProcedure);
                return Result.Ok();
            }
            catch (OracleException ex)
            {
                return Result.Fail(ex.Message);
            }
            finally
            {
                Close();
            }

        }

        public async Task<IEnumerable<T>> QueryStoredProcedureAsync<T>(
            string procedureName,
            object? data = null)
        {
            Open();

            IEnumerable<T> result = await _connection.QueryAsync<T>(procedureName, data, commandType: CommandType.StoredProcedure);

            Close();
            return result;
        }
        
        /// <summary>
        /// Executes a SELECT type Oracle SQL command that returns matching rows
        /// </summary>
        /// <typeparam name="T">The type of database entity to be returned</typeparam>
        /// <param name="query">The query in string format</param>
        /// <returns>IEnumerable of given entity, can be empty.</returns>
        public async Task<IEnumerable<T>> ExecuteSqlQueryAsync<T>(
            string query,
            object? parameters = null) where T : class, new()
        {
            Open();
            
            IEnumerable<T> result = await _dapperAdapter.QueryAsync<T>(_connection!, query, parameters);

            Close();
            return result;
        }

        /// <summary>
        /// Executes INSERT, UPDATE, DELETE type Oracle SQL commands on the database
        /// </summary>
        /// <param name="nonQuery">
        ///     An Oracle SQL command to be executed on the database.
        ///     Multiple queries can be used in a transaction by delimiting them with a semi-colon: ";"
        /// </param>
        /// <param name="parameters">
        ///     An object containing values for the parameterized queries.
        /// </param>
        /// <returns>Number of affected rows</returns>
        public async Task<Result> ExecuteSqlNonQueryAsync(
            string nonQueries,
            object? parameters = null)
        {
            Open();
            IDbTransaction transaction = _connection!.BeginTransaction();

            Result executionResult = await ExecuteCommandsAsync(nonQueries, parameters);
            if (executionResult.IsFailed)
            {
                transaction.Rollback();
            }
            else
            {
                transaction.Commit();
            }
            Close();

            return executionResult;
        }

        private async Task<Result> ExecuteCommandsAsync(
            string commands,
            object? parameters = null)
        {
            string[] nonQueries = commands.Split(';');
            Result result = Result.Ok();

            foreach (string nonQuery in nonQueries)
            {   
                try
                {
                    await _dapperAdapter.ExecuteAsync(_connection!, nonQuery, parameters);
                }
                catch (Exception ex)
                {
                    result = Result.Fail(ex.Message);
                }
            }
            return result;
        }

        private void Open()
        {
            _connection = _connectionFactory.GetConnection(ConnectionType.Oracle);
            _connection.Open();
        }

        private void Close()
        {
            _connection!.Close();
            _connection.Dispose();
        }
    }
}
