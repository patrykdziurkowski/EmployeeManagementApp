using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime GetNow() => DateTime.Now;
    }
}
