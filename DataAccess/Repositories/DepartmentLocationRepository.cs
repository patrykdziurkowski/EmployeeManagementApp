using DataAccess.Models;

namespace DataAccess.Repositories
{
    public class DepartmentLocationRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ISQLDataAccess _dataAccess;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentLocationRepository(ISQLDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public async Task<IEnumerable<DepartmentLocation>> GetAll()
        {
            string query = "SELECT d.department_id, d.department_name, l.street_address, l.city, l.state_province, c.country_name, r.region_name FROM departments d LEFT JOIN locations l ON l.location_id = d.location_id LEFT JOIN countries c ON c.country_id = l.country_id LEFT JOIN regions r ON r.region_id = c.region_id";
            
            return await _dataAccess
                .ExecuteSQLQueryAsync<DepartmentLocation>(query);
        }
    }
}

