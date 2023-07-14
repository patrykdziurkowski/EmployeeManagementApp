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
    public class Location
    {
#nullable disable
        public short LocationId { get; set; }
#nullable enable
        public string? StreetAddress { get; set; }
        public string? PostalCode { get; set; }
#nullable disable
        public string City { get; set; }
#nullable enable
        public string? StateProvince { get; set; }
        public string? CountryId { get; set; }
    }
}
