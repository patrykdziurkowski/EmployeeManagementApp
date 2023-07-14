using DataAccess.Models;
using FluentResults;

namespace DataAccess.Repositories
{
    public class EmployeeRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ISQLDataAccess _dataAccess;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public EmployeeRepository(ISQLDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task<IEnumerable<Employee>> GetAll()
        {
            return await _dataAccess
                .ExecuteSQLQueryAsync<Employee>("SELECT * FROM employees");
        }

        public async Task<Result<Employee>> Get(int employeeId)
        {
            string query = $"SELECT * FROM employees WHERE employee_id = {employeeId}";

            Employee? employeeWithGivenId = (await _dataAccess
                .ExecuteSQLQueryAsync<Employee>(query))
                .FirstOrDefault();
            if (employeeWithGivenId is null)
            {
                return Result.Fail($"No user with id {employeeId} was found");
            }

            return Result.Ok(employeeWithGivenId);
        }

        public virtual async Task<Result> Hire(Employee employee)
        {
            string hireDate = employee.HireDate.ToString("yyyy-MM-dd");

            string commissionPct = "null";
            if (employee.CommissionPct is not null)
            {
                commissionPct = employee.CommissionPct.ToString()!.Replace(",", ".");
            }

            string managerId = "null";
            if (employee.ManagerId is not null)
            {
                managerId = employee.ManagerId.ToString()!;
            }



            string nonQuery = $"INSERT INTO employees VALUES ({employee.EmployeeId}, '{employee.FirstName}', " +
                $"'{employee.LastName}', '{employee.Email}', '{employee.PhoneNumber}', '{hireDate}', '{employee.JobId}', " +
                $"{employee.Salary}, {commissionPct}, {managerId}, {employee.DepartmentId})";
            
            Result insertionResult = await _dataAccess
                .ExecuteSQLNonQueryAsync(nonQuery);

            return insertionResult;
        }

        public async Task<Result> Fire(int employeeId)
        {
            string nonQuery = $"DELETE FROM employees WHERE employee_id = {employeeId}";
            Result deletionResult = await _dataAccess
                .ExecuteSQLNonQueryAsync(nonQuery);

            return deletionResult;
        }

        public async Task<Result> Update(Employee newEmployeeData)
        {
            string hireDate = newEmployeeData.HireDate.ToString("yyyy-MM-dd");

            string commissionPct = "null";
            if (newEmployeeData.CommissionPct is not null)
            {
                commissionPct = newEmployeeData.CommissionPct.ToString()!.Replace(",", ".");
            }

            string managerId = "null";
            if (newEmployeeData.ManagerId is not null)
            {
                managerId = newEmployeeData.ManagerId.ToString()!;
            }

            string departmentId = "null";
            if (newEmployeeData.DepartmentId is not null)
            {
                departmentId = newEmployeeData.DepartmentId.ToString()!;
            }

            string nonQuery = $"UPDATE employees SET employee_id = {newEmployeeData.EmployeeId}, first_name = " +
                $"'{newEmployeeData.FirstName}', last_name = '{newEmployeeData.LastName}', email = '{newEmployeeData.Email}', phone_number = " +
                $"'{newEmployeeData.PhoneNumber}', hire_date = '{hireDate}', job_id = '{newEmployeeData.JobId}', salary = " +
                $"{newEmployeeData.Salary}, commission_pct = {commissionPct}, manager_id = {managerId}, " +
                $"department_id = {departmentId} WHERE employee_id = {newEmployeeData.EmployeeId}";
            
            Result updateResult = await _dataAccess
                .ExecuteSQLNonQueryAsync(nonQuery);

            return updateResult;
        }
    }
}
