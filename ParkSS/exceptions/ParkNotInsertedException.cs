using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSS.exceptions
{
    class ParkNotInsertedException : Exception
    {
        public ParkNotInsertedException(string message) : base(message)
        {
        }
    }
}
