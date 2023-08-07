using DataAccess.Interfaces;
using DataAccess.Models;
using FluentResults;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DataAccess.Repositories
{
    public class DepartmentRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private readonly ISqlDataAccess _dataAccess;


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
        public virtual async Task<IEnumerable<Department>> GetAllAsync()
        {
            OracleDynamicParameters parameters = new();
            parameters.Add("out_departments_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            return await _dataAccess
                .QueryStoredProcedureAsync<Department>("DEPARTMENTPROCEDURES.getDepartments", parameters);
        }

        public virtual async Task<Result<Department>> GetAsync(int departmentId)
        {
            OracleDynamicParameters parameters = new();
            parameters.Add(":in_department_id", departmentId);
            parameters.Add("out_departments_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            Department? department = (await _dataAccess
                .QueryStoredProcedureAsync<Department>("DEPARTMENTPROCEDURES.getDepartment", parameters))
                .FirstOrDefault();

            if (department is null)
            {
                return Result.Fail("No department with such id was found");
            }
            return Result.Ok(department);
        }

        public virtual async Task<IEnumerable<Employee>> GetEmployeesForDepartmentAsync(int departmentId)
        {
            OracleDynamicParameters parameters = new();
            parameters.Add(":in_department_id", departmentId);
            parameters.Add("out_employees_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            return await _dataAccess
                .QueryStoredProcedureAsync<Employee>("DEPARTMENTPROCEDURES.getEmployeesForDepartment", parameters);
        }
    }
}
