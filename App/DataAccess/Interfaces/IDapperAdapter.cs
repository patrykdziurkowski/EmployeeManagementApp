using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IDapperAdapter
    {
        Task<IEnumerable<T>> QueryAsync<T>(
            IDbConnection connection,
            string sql,
            object? param = null,
            IDbTransaction? transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null) where T : class, new();

        Task ExecuteAsync(
            IDbConnection connection,
            string sql,
            object? param = null,
            IDbTransaction? transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null);
    }
}
