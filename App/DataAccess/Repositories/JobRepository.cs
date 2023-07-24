using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class JobRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ISqlDataAccess _dataAccess;

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
        public async Task<IEnumerable<Job>> GetAllAsync()
        {
            return await _dataAccess
                .ExecuteSqlQueryAsync<Job>("SELECT * FROM jobs");
        }
    }
}
