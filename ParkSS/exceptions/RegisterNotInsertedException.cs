using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSS.exceptions
{
    class RegisterNotInsertedException : Exception
    {
        public RegisterNotInsertedException(string message) : base(message)
        {
        }
    }
}
