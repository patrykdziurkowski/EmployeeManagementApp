using Dapper;
using DataAccess.Interfaces;
using DataAccess.Models;
using FluentResults;
using Oracle.ManagedDataAccess.Client;
using System.Data;

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
            OracleDynamicParameters parameters = new();
            parameters.Add("out_employees_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            return await _dataAccess
                .QueryStoredProcedureAsync<Employee>("EMPLOYEEPROCEDURES.getEmployees", parameters);
        }

        public virtual async Task<Result> HireAsync(Employee employee)
        {
            DynamicParameters parameters = new();
            parameters.Add(":in_employee_id", employee.EmployeeId);
            parameters.Add(":in_first_name", employee.FirstName);
            parameters.Add(":in_last_name", employee.LastName);
            parameters.Add(":in_email", employee.Email);
            parameters.Add(":in_phone_number", employee.PhoneNumber);
            parameters.Add(":in_hire_date", employee.HireDate);
            parameters.Add(":in_job_id", employee.JobId);
            parameters.Add(":in_salary", employee.Salary);
            parameters.Add(":in_commission_pct", employee.CommissionPct);
            parameters.Add(":inout_manager_id", employee.ManagerId);
            parameters.Add(":inout_department_id", employee.DepartmentId);

            Result updateResult = await _dataAccess.ExecuteStoredProcedureAsync("EMPLOYEEPROCEDURES.createEmployee", parameters);

            if (updateResult.IsFailed)
            {
                return Result.Fail(updateResult.Errors.First().Message);
            }
            return Result.Ok();
        }

        public virtual async Task<Result> FireAsync(int employeeId)
        {
            DynamicParameters parameters = new();
            parameters.Add(":in_employee_id", employeeId);

            Result deletionResult = await _dataAccess
                .ExecuteStoredProcedureAsync("EMPLOYEEPROCEDURES.deleteEmployee", parameters);

            return deletionResult;
        }

        public virtual async Task<Result> UpdateAsync(Employee employee)
        {
            DynamicParameters parameters = new();
            parameters.Add(":in_employee_id", employee.EmployeeId);
            parameters.Add(":in_first_name", employee.FirstName);
            parameters.Add(":in_last_name", employee.LastName);
            parameters.Add(":in_email", employee.Email);
            parameters.Add(":in_phone_number", employee.PhoneNumber);
            parameters.Add(":in_hire_date", employee.HireDate);
            parameters.Add(":in_job_id", employee.JobId);
            parameters.Add(":in_salary", employee.Salary);
            parameters.Add(":in_commission_pct", employee.CommissionPct);
            parameters.Add(":inout_manager_id", employee.ManagerId);
            parameters.Add(":inout_department_id", employee.DepartmentId);

            Result updateResult = await _dataAccess.ExecuteStoredProcedureAsync("EMPLOYEEPROCEDURES.updateEmployee", parameters);

            if (updateResult.IsFailed)
            {
                return Result.Fail(updateResult.Errors.First().Message);
            }
            return Result.Ok();
        }
    }
}
