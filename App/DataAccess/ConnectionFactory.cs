using DataAccess.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public enum ConnectionType
    {
        Oracle
    }

    public class ConnectionFactory : IConnectionFactory
    {
        private ConnectionStringProvider _connectionStringProvider;

        public ConnectionFactory(ConnectionStringProvider connectionStringProvider)
        {
            _connectionStringProvider = connectionStringProvider;
        }


        public DbConnection GetConnection(ConnectionType connectionType)
        {
            string connectionString = _connectionStringProvider.GetConnectionString();
            if (connectionType == ConnectionType.Oracle)
            {
                return new OracleConnection(connectionString);
            }

            throw new NotImplementedException($"Connection of type ${Enum.GetName(typeof(ConnectionType), connectionType)} is not implemented");
        }
    }
}
