using DataAccess.Interfaces;
using DataAccess.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DataAccess.Repositories
{
    public class JobRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private readonly ISqlDataAccess _dataAccess;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public virtual async Task<IEnumerable<Job>> GetAllAsync()
        {
            OracleDynamicParameters parameters = new();
            parameters.Add("out_jobs_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            return await _dataAccess
                .QueryStoredProcedureAsync<Job>("JOBPROCEDURES.getJobs", parameters);
        }
    }
}
