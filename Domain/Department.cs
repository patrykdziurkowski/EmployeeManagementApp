using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// The naming of these properties must match the naming inside of the database, including casing.
    /// Property types are strictly defined to match the types in the database.
    /// </summary>
    public class Department
    {
        public Int16? DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public int? MANAGER_ID { get; set; }
        public Int16? LOCATION_ID { get; set; }
    }
}
