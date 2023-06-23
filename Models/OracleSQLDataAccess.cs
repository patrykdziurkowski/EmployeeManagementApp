using Oracle.ManagedDataAccess.Client;

namespace Models
{
    //A class that abstracts Oracle's framework to simplify CRUD operations
    public class OracleSQLDataAccess : ISQLDataAccess
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ConnectionStringProvider _connectionStringProvider;
        
        private string _connectionString;
        private OracleConnection _connection;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public OracleSQLDataAccess(ConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
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
                result.Add(await reader.ConvertToObject<T>());
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
        public async Task<int> ExecuteSQLNonQueryAsync(string nonQueries)
        {
            Open();
            OracleTransaction transaction = _connection.BeginTransaction();

            int affectedRows = 0;
            try
            {
                affectedRows = await ExecuteCommandsAsync(nonQueries);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            finally
            {
                Close();
            }
            return affectedRows;
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
            UpdateConnectionString();
            _connection = new OracleConnection(_connectionString);
            _connection.Open();
        }

        private void Close()
        {
            _connection.Close();
            _connection.Dispose();
        }

        private void UpdateConnectionString()
        {
            _connectionString = _connectionStringProvider.GetConnectionString();
        }
    }
}
