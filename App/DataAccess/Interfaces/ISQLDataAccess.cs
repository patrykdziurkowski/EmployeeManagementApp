using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> ExecuteSqlQueryAsync<T>(string query, object? parameters = null) where T : class, new();
        Task<Result> ExecuteSqlNonQueryAsync(string nonQuery, object? parameters = null);
    }
}
