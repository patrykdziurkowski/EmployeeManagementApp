using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Job
    {
        /// <summary>
        /// The naming of these properties must match the naming inside of the database, including casing.
        /// Property types are strictly defined to match the types in the database.
        /// </summary>
        public string JOB_ID { get; set; }
        public string JOB_TITLE { get; set; }
        public int? MIN_SALARY { get; set; }
        public int? MAX_SALARY { get; set; }
    }
}
