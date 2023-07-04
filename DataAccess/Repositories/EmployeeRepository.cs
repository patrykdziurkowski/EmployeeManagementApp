using DataAccess.Models;

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

        public async Task<Employee> Get(int employeeId)
        {
            string query = $"SELECT * FROM employees WHERE employee_id = {employeeId}";

            Employee employeeWithGivenId = (await _dataAccess
                .ExecuteSQLQueryAsync<Employee>(query))
                .FirstOrDefault();

            return employeeWithGivenId;
        }

        public async Task<bool> Hire(Employee employee)
        {
            string hireDate = employee.HireDate.Value.ToString("yyyy-MM-dd");
            string commissionPct = employee.CommissionPct.ToString().Replace(",", ".");
            commissionPct = (commissionPct == String.Empty) ? "null" : commissionPct;
            string managerId = (employee.ManagerId is null) ? "null" : employee.ManagerId.ToString();

            string nonQuery = $"INSERT INTO employees VALUES ({employee.EmployeeId}, '{employee.FirstName}', " +
                $"'{employee.LastName}', '{employee.Email}', '{employee.PhoneNumber}', '{hireDate}', '{employee.JobId}', " +
                $"{employee.Salary}, {commissionPct}, {managerId}, {employee.DepartmentId})";
            
            int rowsAffected = await _dataAccess
                .ExecuteSQLNonQueryAsync(nonQuery);

            return rowsAffected > 0;
        }

        public async Task<bool> Fire(int employeeId)
        {
            string nonQuery = $"DELETE FROM employees WHERE employee_id = {employeeId}";
            int rowsAffected = await _dataAccess
                .ExecuteSQLNonQueryAsync(nonQuery);
            return rowsAffected > 0;
        }

        public async Task<bool> Update(int targetEmployeeId, Employee newEmployeeData)
        {
            string hireDate = newEmployeeData.HireDate.Value.ToString("yyyy-MM-dd");
            string commissionPct = newEmployeeData.CommissionPct.ToString().Replace(",", ".");
            commissionPct = (commissionPct == String.Empty) ? "null": commissionPct;
            string managerId = (newEmployeeData.ManagerId is null) ? "null" : newEmployeeData.ManagerId.ToString();
            string departmentId = (newEmployeeData.DepartmentId is null) ? "null" : newEmployeeData.DepartmentId.ToString();

            string nonQuery = $"UPDATE employees SET employee_id = {newEmployeeData.EmployeeId}, first_name = " +
                $"'{newEmployeeData.FirstName}', last_name = '{newEmployeeData.LastName}', email = '{newEmployeeData.Email}', phone_number = " +
                $"'{newEmployeeData.PhoneNumber}', hire_date = '{hireDate}', job_id = '{newEmployeeData.JobId}', salary = " +
                $"{newEmployeeData.Salary}, commission_pct = {commissionPct}, manager_id = {managerId}, " +
                $"department_id = {departmentId} WHERE employee_id = {targetEmployeeId}";
            
            int rowsAffected = await _dataAccess
                .ExecuteSQLNonQueryAsync(nonQuery);

            return rowsAffected > 0;
        }
    }
}
