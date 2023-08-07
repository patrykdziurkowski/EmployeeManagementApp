using DataAccess.Interfaces;
using DataAccess.Models;
using FluentResults;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DataAccess.Repositories
{
    public class JobHistoryRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private readonly ISqlDataAccess _dataAccess;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobHistoryRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public virtual async Task<IEnumerable<JobHistory>> GetAllAsync()
        {
            OracleDynamicParameters parameters = new();
            parameters.Add("out_job_history_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            return await _dataAccess
                .QueryStoredProcedureAsync<JobHistory>("JOBHISTORYPROCEDURES.getJobHistory", parameters);
        }

        public virtual async Task<Result> InsertAsync(JobHistory jobHistory)
        {
            OracleDynamicParameters parameters = new();
            parameters.Add(":in_employee_id", jobHistory.EmployeeId);
            parameters.Add(":in_start_date", jobHistory.StartDate);
            parameters.Add(":in_end_date", jobHistory.EndDate);
            parameters.Add(":in_job_id", jobHistory.JobId);
            parameters.Add(":in_department_id", jobHistory.DepartmentId);

            return await _dataAccess
                .ExecuteStoredProcedureAsync("JOBHISTORYPROCEDURES.createJobHistory", parameters);
        }
    }
}
