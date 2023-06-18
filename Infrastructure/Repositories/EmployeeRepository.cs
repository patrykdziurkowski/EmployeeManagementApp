using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class EmployeeRepository
    {
        private ISQLDataAccess _dataAccess;
        public EmployeeRepository(ISQLDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

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
            int rowsAffected = _dataAccess
                .ExecuteSQLNonQuery($"INSERT INTO employees VALUES ({employee.EMPLOYEE_ID}, {employee.FIRST_NAME}, " +
                $"{employee.LAST_NAME}, {employee.EMAIL}, {employee.PHONE_NUMBER}, {employee.HIRE_DATE}, {employee.JOB_ID}, " +
                $"{employee.SALARY}, {employee.COMMISSION_PCT}, {employee.MANAGER_ID}, {employee.DEPARTMENT_ID})");

            return rowsAffected > 0;
        }

        public bool Fire(int employeeId)
        {
            int rowsAffected = _dataAccess
                .ExecuteSQLNonQuery($"DELETE FROM employees WHERE employee_id = {employeeId}");
            return rowsAffected > 0;
        }

        public bool Update(int targetEmployeeId, Employee newEmployeeData)
        {
            int rowsAffected = _dataAccess
                .ExecuteSQLNonQuery($"UPDATE employees SET employee_id = {newEmployeeData.EMPLOYEE_ID}, first_name = " +
                $"{newEmployeeData.FIRST_NAME}, last_name = {newEmployeeData.LAST_NAME}, email = {newEmployeeData.EMAIL}, phone_number = " +
                $"{newEmployeeData.PHONE_NUMBER}, hire_date = {newEmployeeData.HIRE_DATE}, job_id = {newEmployeeData.JOB_ID}, salary = " +
                $"{newEmployeeData.SALARY}, commission_pct = {newEmployeeData.COMMISSION_PCT}, manager_id = {newEmployeeData.MANAGER_ID}, " +
                $"department_id = {newEmployeeData.DEPARTMENT_ID} WHERE employee_id = {targetEmployeeId}");

            return rowsAffected > 0;
        }


    }
}
