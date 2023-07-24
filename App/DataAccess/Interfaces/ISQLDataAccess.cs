using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> ExecuteSqlQueryAsync<T>(string query) where T : class, new();
        Task<Result> ExecuteSqlNonQueryAsync(string nonQuery);
    }
}
