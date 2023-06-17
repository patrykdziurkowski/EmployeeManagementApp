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
    public class Country
    {
        public string COUNTRY_ID { get; set; }
        public string COUNTRY_NAME { get; set; }
        public decimal? REGION_ID { get; set; }
    }
}
