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
        private IConnectionFactory _connectionFactory;
        private ICommandFactory _commandFactory;
        
        private IDbConnection? _connection;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public OracleSqlDataAccess(IConnectionFactory connectionFactory,
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
        public async Task<IEnumerable<T>> ExecuteSqlQueryAsync<T>(string query) where T : class, new()
        {
            List<T> result = new();
            Open();

            IDbCommand command = _commandFactory.GetCommand(query, _connection!, ConnectionType.Oracle);
            
            //Note: the nullcheck here is purely for unit testing purposes, as it's the only
            //workaround for Oracle's library being largely sealed an internal and thus unmockable
            OracleDataReader? reader = (OracleDataReader)command.ExecuteReader();

            if (reader is not null)
            {
                while (reader.Read())
                {
                    result.Add(await reader.ConvertToObjectAsync<T>());
                }
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
        public async Task<Result> ExecuteSqlNonQueryAsync(string nonQueries)
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
                IDbCommand command = _commandFactory.GetCommand(nonQuery, _connection!, ConnectionType.Oracle);
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
