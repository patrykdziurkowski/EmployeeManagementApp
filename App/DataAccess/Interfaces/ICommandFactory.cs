using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ICommandFactory
    {
        IDbCommand GetCommand(string query, IDbConnection connection, ConnectionType connectionType);
    }
}
