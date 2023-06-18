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
        public IEnumerable<Employee> GetAll()
        {
            return _dataAccess
                .ExecuteSQLQuery<Employee>("SELECT * FROM employees");
        }

        public Employee Get(int employeeId)
        {
            return _dataAccess
                .ExecuteSQLQuery<Employee>($"SELECT * FROM employees WHERE employee_id = {employeeId}")
                .FirstOrDefault();
        }

        public bool Hire(Employee employee)
        {
            string hireDate = employee.HIRE_DATE.Value.ToString("yyyy-MM-dd");
            string commissionPct = employee.COMMISSION_PCT.ToString().Replace(",", ".");

            string nonQuery = $"INSERT INTO employees VALUES ({employee.EMPLOYEE_ID}, '{employee.FIRST_NAME}', " +
                $"'{employee.LAST_NAME}', '{employee.EMAIL}', '{employee.PHONE_NUMBER}', '{hireDate}', '{employee.JOB_ID}', " +
                $"{employee.SALARY}, {commissionPct}, {employee.MANAGER_ID}, {employee.DEPARTMENT_ID})";
            
            int rowsAffected = _dataAccess
                .ExecuteSQLNonQuery(nonQuery);

            return rowsAffected > 0;
        }

        public bool Fire(int employeeId)
        {
            string nonQuery = $"DELETE FROM employees WHERE employee_id = {employeeId}";
            int rowsAffected = _dataAccess
                .ExecuteSQLNonQuery(nonQuery);
            return rowsAffected > 0;
        }

        public bool Update(int targetEmployeeId, Employee newEmployeeData)
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
            int rowsAffected = _dataAccess
                .ExecuteSQLNonQuery(nonQuery);

            return rowsAffected > 0;
        }

    }
}
