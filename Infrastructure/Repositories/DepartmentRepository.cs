using Core;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class DepartmentRepository
    {
        private ISQLDataAccess _dataAccess;
        public DepartmentRepository(ISQLDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public IEnumerable<Department> GetAll()
        {
            return _dataAccess
                .ExecuteSQLQuery<Department>("SELECT * FROM departments");
        }

        public Department Get(int departmentId)
        {
            return _dataAccess
                .ExecuteSQLQuery<Department>($"SELECT * FROM departments WHERE department_id = {departmentId}")
                .FirstOrDefault();
        }
    }
}
