using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface ISQLDataAccess
    {
        Task<IEnumerable<T>> ExecuteSQLQueryAsync<T>(string query) where T : class, new();
        Task<Result> ExecuteSQLNonQueryAsync(string nonQuery);
    }
}
