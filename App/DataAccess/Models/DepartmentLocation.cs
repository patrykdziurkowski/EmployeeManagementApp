﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
#nullable disable
    public class DepartmentLocation
    {
        public short DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string StateProvince { get; set; }
        public string CountryName { get; set; }
        public string RegionName { get; set; }
    }
}
