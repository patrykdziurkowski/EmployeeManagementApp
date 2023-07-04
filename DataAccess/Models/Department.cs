using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    /// <summary>
    /// The naming of these properties must match the naming inside of the database, including casing.
    /// Property types are strictly defined to match the types in the database.
    /// </summary>
    public class Department
    {
        public Int16? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? ManagerId { get; set; }
        public Int16? LocationId { get; set; }
    }
}
