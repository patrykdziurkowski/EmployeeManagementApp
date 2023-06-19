using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public interface ISQLDataAccess
    {
        IEnumerable<T> ExecuteSQLQuery<T>(string query) where T : class, new();
        int ExecuteSQLNonQuery(string nonQuery);
    }
}
