using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class JobRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ISQLDataAccess _dataAccess;

        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public JobRepository(ISQLDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task<IEnumerable<Job>> GetAll()
        {
            return await _dataAccess
                .ExecuteSQLQueryAsync<Job>("SELECT * FROM jobs");
        }
    }
}
