using DataAccess.Interfaces;
using DataAccess.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DataAccess.Repositories
{
    public class DepartmentLocationRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private readonly ISqlDataAccess _dataAccess;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentLocationRepository(ISqlDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public virtual async Task<IEnumerable<DepartmentLocation>> GetAllAsync()
        {
            OracleDynamicParameters parameters = new();
            parameters.Add("out_department_locations_cur", OracleDbType.RefCursor, ParameterDirection.Output);

            return await _dataAccess
                .QueryStoredProcedureAsync<DepartmentLocation>("DEPARTMENTLOCATIONPROCEDURES.getDepartmentLocations", parameters);
        }
    }
}

