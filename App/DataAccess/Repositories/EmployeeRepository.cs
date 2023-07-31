using DataAccess.Interfaces;
using DataAccess.Models;
using FluentResults;

namespace DataAccess.Repositories
{
    public class EmployeeRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private readonly ISqlDataAccess _dataAccess;

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
            string query = $"SELECT * FROM employees WHERE employee_id = :EmployeeId";

            Employee? employeeWithGivenId = (await _dataAccess
                .ExecuteSqlQueryAsync<Employee>(query, new { EmployeeId = employeeId }))
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
                $":EmployeeId, " +
                $":FirstName, " +
                $":LastName, " +
                $":Email, " +
                $":PhoneNumber, " +
                $":HireDate, " +
                $":JobId, " +
                $":Salary, " +
                $":CommissionPct, " +
                $":ManagerId, " +
                $":DepartmentId" +
                $")";
            
            Result insertionResult = await _dataAccess
                .ExecuteSqlNonQueryAsync(nonQuery, employee);

            return insertionResult;
        }

        public virtual async Task<Result> FireAsync(int employeeId)
        {
            string nonQuery = $"DELETE FROM employees WHERE employee_id = :EmployeeId";
            
            Result deletionResult = await _dataAccess
                .ExecuteSqlNonQueryAsync(nonQuery, new { EmployeeId = employeeId });

            return deletionResult;
        }

        public virtual async Task<Result> UpdateAsync(Employee employee)
        {
            string nonQuery = $"UPDATE employees SET " +
                $"employee_id = :EmployeeId, " +
                $"first_name = :FirstName, " +
                $"last_name = :LastName, " +
                $"email = :Email, " +
                $"phone_number = :PhoneNumber, " +
                $"hire_date = :HireDate, " +
                $"job_id = :JobId, " +
                $"salary = :Salary, " +
                $"commission_pct = :CommissionPct, " +
                $"manager_id = :ManagerId, " +
                $"department_id = :DepartmentId " +
                $"WHERE employee_id = :EmployeeId";
            
            Result updateResult = await _dataAccess
                .ExecuteSqlNonQueryAsync(nonQuery, employee);

            return updateResult;
        }
    }
}
