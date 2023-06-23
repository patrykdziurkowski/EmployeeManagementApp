using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface ISQLDataAccess
    {
        Task<IEnumerable<T>> ExecuteSQLQueryAsync<T>(string query) where T : class, new();
        Task<int> ExecuteSQLNonQueryAsync(string nonQuery);
    }
}
