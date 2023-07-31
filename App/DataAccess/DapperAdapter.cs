using Dapper;
using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DapperAdapter : IDapperAdapter
    {
        public async Task<IEnumerable<T>> QueryAsync<T>(
            IDbConnection connection,
            string sql,
            object? param = null,
            IDbTransaction? transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null) where T : class, new()
        {
            return await connection.QueryAsync<T>(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType);
        }

        public async Task ExecuteAsync(
            IDbConnection connection,
            string sql,
            object? param = null,
            IDbTransaction? transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null)
        {
            await connection.ExecuteAsync(
                sql,
                param,
                transaction,
                commandTimeout,
                commandType);
        }
    }
}
