using DataAccess.Models;
using FluentResults;

namespace DataAccess.Repositories
{
    public class JobHistoryRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ISQLDataAccess _dataAccess;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobHistoryRepository(ISQLDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public virtual async Task<IEnumerable<JobHistory>> GetAll()
        {
            return await _dataAccess
                .ExecuteSQLQueryAsync<JobHistory>("SELECT * FROM job_history");
        }

        public virtual async Task<Result> Insert(JobHistory jobHistory)
        {
            string startDate = jobHistory.StartDate.ToString("yyyy-MM-dd");
            string endDate = jobHistory.EndDate.ToString("yyyy-MM-dd");
            string nonQuery = $"INSERT INTO job_history VALUES ({jobHistory.EmployeeId}, '{startDate}'," +
                $"'{endDate}', '{jobHistory.JobId}', {jobHistory.DepartmentId})";

            Result insertionResult = await _dataAccess
                .ExecuteSQLNonQueryAsync(nonQuery);

            return insertionResult;
        }
    }
}
