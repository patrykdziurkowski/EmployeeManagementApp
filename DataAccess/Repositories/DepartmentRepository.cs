using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class DepartmentRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ISQLDataAccess _dataAccess;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentRepository(ISQLDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task<IEnumerable<Department>> GetAll()
        {
            string query = "SELECT * FROM departments";

            return await _dataAccess
                .ExecuteSQLQueryAsync<Department>(query);
        }

        public async Task<Department> Get(int departmentId)
        {
            string query = $"SELECT * FROM departments WHERE department_id = { departmentId}";

            Department departmentWithGivenId = (await _dataAccess
                .ExecuteSQLQueryAsync<Department>(query))
                .First();

            return departmentWithGivenId;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesForDepartment(int departmentId)
        {
            string query = $"SELECT * FROM employees WHERE department_id = {departmentId}";
            
            return await _dataAccess
                .ExecuteSQLQueryAsync<Employee>(query);
        }
    }
}
