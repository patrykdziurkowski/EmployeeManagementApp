using DataAccess.Interfaces;
using FluentResults;
using Oracle.ManagedDataAccess.Client;

namespace DataAccess
{
    //A class that abstracts Oracle's framework to simplify CRUD operations
    public class OracleSQLDataAccess : ISQLDataAccess
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private IConnectionFactory _connectionFactory;
        
        private OracleConnection? _connection;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public OracleSQLDataAccess(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
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

            OracleCommand command = new(query, _connection);
            OracleDataReader reader = (OracleDataReader) await command.ExecuteReaderAsync();
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
            OracleTransaction transaction = _connection!.BeginTransaction();

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
                OracleCommand command = new(nonQuery, _connection);
                affectedRows += await command.ExecuteNonQueryAsync();
            }

            return affectedRows;
        }

        private void Open()
        {
            _connection = (OracleConnection)_connectionFactory.GetConnection(ConnectionType.Oracle);
            _connection.Open();
        }

        private void Close()
        {
            _connection!.Close();
            _connection.Dispose();
        }
    }
}
