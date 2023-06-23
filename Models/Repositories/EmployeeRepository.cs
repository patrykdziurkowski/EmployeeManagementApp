using Models.Entities;

namespace Models.Repositories
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
            string hireDate = employee.HIRE_DATE.Value.ToString("yyyy-MM-dd");
            string commissionPct = employee.COMMISSION_PCT.ToString().Replace(",", ".");
            commissionPct = (commissionPct == String.Empty) ? "null" : commissionPct;
            string managerId = (employee.MANAGER_ID is null) ? "null" : employee.MANAGER_ID.ToString();

            string nonQuery = $"INSERT INTO employees VALUES ({employee.EMPLOYEE_ID}, '{employee.FIRST_NAME}', " +
                $"'{employee.LAST_NAME}', '{employee.EMAIL}', '{employee.PHONE_NUMBER}', '{hireDate}', '{employee.JOB_ID}', " +
                $"{employee.SALARY}, {commissionPct}, {managerId}, {employee.DEPARTMENT_ID})";
            
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
            string hireDate = newEmployeeData.HIRE_DATE.Value.ToString("yyyy-MM-dd");
            string commissionPct = newEmployeeData.COMMISSION_PCT.ToString().Replace(",", ".");
            commissionPct = (commissionPct == String.Empty) ? "null": commissionPct;
            string managerId = (newEmployeeData.MANAGER_ID is null) ? "null" : newEmployeeData.MANAGER_ID.ToString();

            string nonQuery = $"UPDATE employees SET employee_id = {newEmployeeData.EMPLOYEE_ID}, first_name = " +
                $"'{newEmployeeData.FIRST_NAME}', last_name = '{newEmployeeData.LAST_NAME}', email = '{newEmployeeData.EMAIL}', phone_number = " +
                $"'{newEmployeeData.PHONE_NUMBER}', hire_date = '{hireDate}', job_id = '{newEmployeeData.JOB_ID}', salary = " +
                $"{newEmployeeData.SALARY}, commission_pct = {commissionPct}, manager_id = {managerId}, " +
                $"department_id = {newEmployeeData.DEPARTMENT_ID} WHERE employee_id = {targetEmployeeId}";
            
            int rowsAffected = await _dataAccess
                .ExecuteSQLNonQueryAsync(nonQuery);

            return rowsAffected > 0;
        }
    }
}
