using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UserCredentials
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }

        public void Clear()
        {
            UserName = null;
            Password = null;
        }

        public bool AreFilledOut()
        {
            return UserName is not null && Password is not null;
        }

        public bool AreNotFilledOut()
        {
            return !AreFilledOut();
        }
    }
}
