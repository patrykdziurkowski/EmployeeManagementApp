using DataAccess.Models;
using FluentResults;

namespace DataAccess.Repositories
{
    public class DepartmentRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ISqlDataAccess _dataAccess;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            string query = "SELECT * FROM departments";

            return await _dataAccess
                .ExecuteSqlQueryAsync<Department>(query);
        }

        public async Task<Result<Department>> GetAsync(int departmentId)
        {
            string query = $"SELECT * FROM departments WHERE department_id = { departmentId}";

            Department? departmentWithGivenId = (await _dataAccess
                .ExecuteSqlQueryAsync<Department>(query))
                .FirstOrDefault();

            if (departmentWithGivenId is null)
            {
                return Result.Fail($"No user with id {departmentId} was found");
            }

            return Result.Ok(departmentWithGivenId);
        }

        public async Task<IEnumerable<Employee>> GetEmployeesForDepartmentAsync(int departmentId)
        {
            string query = $"SELECT * FROM employees WHERE department_id = {departmentId}";
            
            return await _dataAccess
                .ExecuteSqlQueryAsync<Employee>(query);
        }
    }
}
