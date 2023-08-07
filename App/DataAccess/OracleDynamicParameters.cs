using Dapper;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace DataAccess
{
    public class OracleDynamicParameters : IDynamicParameters
    {
        private readonly DynamicParameters dynamicParameters = new();
        private readonly List<OracleParameter> oracleParameters = new();

        public void Add(string name, object? value = null, DbType? dbType = null, ParameterDirection? direction = null, int? size = null)
        {
            dynamicParameters.Add(name, value, dbType, direction, size);
        }

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection direction)
        {
            OracleParameter oracleParameter = new(name, oracleDbType, direction);
            oracleParameters.Add(oracleParameter);
        }

        public void AddParameters(IDbCommand command, Identity identity)
        {
            ((IDynamicParameters)dynamicParameters).AddParameters(command, identity);

            OracleCommand oracleCommand = (OracleCommand)command;
            oracleCommand.Parameters.AddRange(oracleParameters.ToArray());
        }
    }
}
