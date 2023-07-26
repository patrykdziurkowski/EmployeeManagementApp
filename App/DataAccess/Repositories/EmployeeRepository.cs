using DataAccess.Models;
using FluentResults;

namespace DataAccess.Repositories
{
    public class EmployeeRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ISqlDataAccess _dataAccess;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public EmployeeRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public virtual async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _dataAccess
                .ExecuteSqlQueryAsync<Employee>("SELECT * FROM employees");
        }

        public virtual async Task<Result<Employee>> GetAsync(int employeeId)
        {
            string query = $"SELECT * FROM employees WHERE employee_id = {employeeId}";

            Employee? employeeWithGivenId = (await _dataAccess
                .ExecuteSqlQueryAsync<Employee>(query))
                .FirstOrDefault();
            if (employeeWithGivenId is null)
            {
                return Result.Fail($"No user with id {employeeId} was found");
            }

            return Result.Ok(employeeWithGivenId);
        }

        public virtual async Task<Result> HireAsync(Employee employee)
        {
            string nonQuery = $"INSERT INTO employees VALUES (" +
                $"{employee.EmployeeId}," +
                $"'{employee.FirstName ?? "null"}', " +
                $"'{employee.LastName}'," +
                $"'{employee.Email}'," +
                $"'{employee.PhoneNumber ?? "null"}'," +
                $"'{employee.HireDate.ToString("yyyy-MM-dd")}'," +
                $"'{employee.JobId}', " +
                $"{((employee.Salary is null) ? "null" : employee.Salary)}," +
                $"{((employee.CommissionPct is null) ? "null" : employee.CommissionPct.ToString()!.Replace(",", "."))}," +
                $"{((employee.ManagerId is null) ? "null" : employee.ManagerId)}," +
                $"{((employee.DepartmentId is null) ? "null" : employee.DepartmentId)})";
            
            Result insertionResult = await _dataAccess
                .ExecuteSqlNonQueryAsync(nonQuery);

            return insertionResult;
        }

        public virtual async Task<Result> FireAsync(int employeeId)
        {
            string nonQuery = $"DELETE FROM employees WHERE employee_id = {employeeId}";
            
            Result deletionResult = await _dataAccess
                .ExecuteSqlNonQueryAsync(nonQuery);

            return deletionResult;
        }

        public virtual async Task<Result> UpdateAsync(Employee employee)
        {
            string nonQuery = $"UPDATE employees SET " +
                $"employee_id = {employee.EmployeeId}," +
                $"first_name = '{employee.FirstName ?? "null"}'," +
                $"last_name = '{employee.LastName}'," +
                $"email = '{employee.Email}'," +
                $"phone_number = '{employee.PhoneNumber ?? "null"}'," +
                $"hire_date = '{employee.HireDate.ToString("yyyy-MM-dd")}'," +
                $"job_id = '{employee.JobId}'," +
                $"salary = {((employee.Salary is null) ? "null" : employee.Salary)}," +
                $"commission_pct = {((employee.CommissionPct is null) ? "null" : employee.CommissionPct.ToString()!.Replace(",", "."))}," +
                $"manager_id = {((employee.ManagerId is null) ? "null" : employee.ManagerId)}, " +
                $"department_id = {((employee.DepartmentId is null) ? "null" : employee.DepartmentId)}" +
                $" WHERE employee_id = {employee.EmployeeId}";
            
            Result updateResult = await _dataAccess
                .ExecuteSqlNonQueryAsync(nonQuery);

            return updateResult;
        }
    }
}
