using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class DepartmentLocation
    {
        public Int16? DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string STREET_ADDRESS { get; set; }
        public string CITY { get; set; }
        public string STATE_PROVINCE { get; set; }
        public string COUNTRY_NAME { get; set; }
        public string REGION_NAME { get; set; }
    }
}
