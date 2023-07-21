using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IConnectionFactory
    {
        DbConnection GetConnection(ConnectionType connectionType);
    }
}
