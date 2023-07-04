using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class DateProvider : IDateProvider
    {
        public DateOnly GetNow() => DateOnly.FromDateTime(DateTime.Now);
    }
}
