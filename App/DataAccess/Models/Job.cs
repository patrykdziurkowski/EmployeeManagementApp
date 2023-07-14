using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
#nullable disable
    public class Job
    {
        /// <summary>
        /// The naming of these properties must match the naming inside of the database, including casing.
        /// Property types are strictly defined to match the types in the database.
        /// </summary>
        public string JobId { get; set; }
        public string JobTitle { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
    }
}
