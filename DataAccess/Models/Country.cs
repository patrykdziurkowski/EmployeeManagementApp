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
    public class Country
    {
        public string CountryId { get; set; }
        public string CountryName { get; set; }
        public int RegionId { get; set; }
    }
}
