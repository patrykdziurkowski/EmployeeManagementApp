using DataAccess.Interfaces;
using FluentResults;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DataAccess
{
    //A class that abstracts Oracle's framework to simplify CRUD operations
    public class OracleSQLDataAccess : ISQLDataAccess
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private IConnectionFactory _connectionFactory;
        private ICommandFactory _commandFactory;
        
        private IDbConnection? _connection;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public OracleSQLDataAccess(IConnectionFactory connectionFactory,
            ICommandFactory commandFactory)
        {
            _connectionFactory = connectionFactory;
            _commandFactory = commandFactory;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        /// <summary>
        /// Executes a SELECT type Oracle SQL command that returns matching rows
        /// </summary>
        /// <typeparam name="T">The type of database entity to be returned</typeparam>
        /// <param name="query">The query in string format</param>
        /// <returns>IEnumerable of given entity, can be empty.</returns>
        public async Task<IEnumerable<T>> ExecuteSQLQueryAsync<T>(string query) where T : class, new()
        {
            List<T> result = new();
            Open();

            IDbCommand command = _commandFactory.GetCommand(query, (OracleConnection)_connection!, ConnectionType.Oracle);
            OracleDataReader reader = (OracleDataReader) await Task.Run(() => command.ExecuteReader());
            while (reader.Read())
            {
                result.Add(await reader.ConvertToObjectAsync<T>());
            }

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
        /// <returns>Number of affected rows</returns>
        public async Task<Result> ExecuteSQLNonQueryAsync(string nonQueries)
        {
            Open();
            IDbTransaction transaction = _connection!.BeginTransaction();

            try
            {
                await ExecuteCommandsAsync(nonQueries);

                transaction.Commit();
            }
            catch (Exception ex)
             {
                transaction.Rollback();

                return Result.Fail(ex.Message);
            }
            finally
            {
                Close();
            }

            return Result.Ok();
        }

        private async Task<int> ExecuteCommandsAsync(string commands)
        {
            string[] nonQueries = commands.Split(';');

            int affectedRows = 0;
            foreach (string nonQuery in nonQueries)
            {
                IDbCommand command = _commandFactory.GetCommand(nonQuery, (OracleConnection)_connection!, ConnectionType.Oracle);
                affectedRows += await Task.Run(() => command.ExecuteNonQuery());
            }

            return affectedRows;
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
