using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    /// <summary>
    /// The naming of these properties must match the naming inside of the database, including casing.
    /// Property types are strictly defined to match the types in the database.
    /// </summary>
    public class Location
    {
        public Int16? LOCATION_ID { get; set; }
        public string STREET_ADDRESS { get; set; }
        public string POSTAL_CODE { get; set; }
        public string CITY { get; set; }
        public string STATE_PROVINCE { get; set; }
        public string COUNTRY_ID { get; set; }
    }
}
