using Models.Entities;

namespace Models.Repositories
{
    public class DepartmentRepository
    {
        ////////////////////////////////////////////
        //  Fields and properties
        ////////////////////////////////////////////
        private ISQLDataAccess _dataAccess;


        ////////////////////////////////////////////
        //  Constructors
        ////////////////////////////////////////////
        public DepartmentRepository(ISQLDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }


        ////////////////////////////////////////////
        //  Methods
        ////////////////////////////////////////////
        public IEnumerable<Department> GetAll()
        {
            return _dataAccess
                .ExecuteSQLQuery<Department>("SELECT * FROM departments");
        }

        public Department Get(int departmentId)
        {
            return _dataAccess
                .ExecuteSQLQuery<Department>($"SELECT * FROM departments WHERE department_id = {departmentId}")
                .FirstOrDefault();
        }
    }
}
