using DataAccess.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CommandFactory : ICommandFactory
    {
        public IDbCommand GetCommand(string query, IDbConnection connection, ConnectionType connectionType)
        {
            if (connectionType == ConnectionType.Oracle)
            {
                return new OracleCommand(query, (OracleConnection)connection);
            }

            throw new NotImplementedException($"Connection of type ConnectionType.{Enum.GetName(typeof(ConnectionType), connectionType)} is not implemented");
        }
    }
}
