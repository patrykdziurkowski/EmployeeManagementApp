using DataAccess.Models;

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
        public async Task<IEnumerable<JobHistory>> GetAll()
        {
            return await _dataAccess
                .ExecuteSQLQueryAsync<JobHistory>("SELECT * FROM job_history");
        }

        public async Task<bool> Insert(JobHistory jobHistory)
        {
            string startDate = jobHistory.StartDate.Value.ToString("yyyy-MM-dd");
            string endDate = jobHistory.EndDate.Value.ToString("yyyy-MM-dd");
            string nonQuery = $"INSERT INTO job_history VALUES ({jobHistory.EmployeeId}, '{startDate}'," +
                $"'{endDate}', '{jobHistory.JobId}', {jobHistory.DepartmentId})";

            int rowsAffected = await _dataAccess
                .ExecuteSQLNonQueryAsync(nonQuery);

            return rowsAffected > 0;
        }
    }
}
