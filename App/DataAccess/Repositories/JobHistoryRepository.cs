using DataAccess.Models;
using FluentResults;

namespace DataAccess.Repositories
{
    public class JobHistoryRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ISqlDataAccess _dataAccess;

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
            return await _dataAccess
                .ExecuteSqlQueryAsync<JobHistory>("SELECT * FROM job_history");
        }

        public virtual async Task<Result> InsertAsync(JobHistory jobHistory)
        {
            string startDate = jobHistory.StartDate.ToString("yyyy-MM-dd");
            string endDate = jobHistory.EndDate.ToString("yyyy-MM-dd");
            string nonQuery = $"INSERT INTO job_history VALUES ({jobHistory.EmployeeId}, '{startDate}'," +
                $"'{endDate}', '{jobHistory.JobId}', {jobHistory.DepartmentId})";

            Result insertionResult = await _dataAccess
                .ExecuteSqlNonQueryAsync(nonQuery);

            return insertionResult;
        }
    }
}
