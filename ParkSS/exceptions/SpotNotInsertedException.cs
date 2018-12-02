using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSS.exceptions
{
    class SpotNotInsertedException : Exception
    {
        public SpotNotInsertedException(string message) : base(message)
        {
        }
    }
}
