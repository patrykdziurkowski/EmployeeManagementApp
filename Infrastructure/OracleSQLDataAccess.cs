using Domain;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    //A class that abstracts Oracle's framework to simplify CRUD operations
    public class OracleSQLDataAccess : ISQLDataAccess
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private readonly string _connectionString;
        private OracleConnection _connection;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public OracleSQLDataAccess(string connectionString)
        {
            _connectionString = connectionString;
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
        public IEnumerable<T> ExecuteSQLQuery<T>(string query) where T : class, new()
        {
            List<T> result = new();
            Open();

            OracleCommand command = new(query, _connection);
            OracleDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                result.Add(reader.ConvertToObject<T>());
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
        public int ExecuteSQLNonQuery(string nonQueries)
        {
            Open();
            OracleTransaction transaction = _connection.BeginTransaction();

            int affectedRows = 0;
            try
            {
                affectedRows = ExecuteCommands(nonQueries);

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

        private int ExecuteCommands(string commands)
        {
            string[] nonQueries = commands.Split(';');

            int affectedRows = 0;
            foreach (string nonQuery in nonQueries)
            {
                OracleCommand command = new(nonQuery, _connection);
                affectedRows += command.ExecuteNonQuery();
            }

            return affectedRows;
        }

        private void Open()
        {
            _connection = new OracleConnection(_connectionString);
            _connection.Open();
        }

        private void Close()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}
