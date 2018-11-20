using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPark.Models
{
    public class ParkingSpot
    {
        public int IdPark { get; set; }
        public int Id { get; set; }
        public int Available { get; set; }
        /* public string Type { get; set; }
         public int Name{ get; set; }
         public string Location { get; set; }
         public string Value { get; set; }
         public DateTime TimeStamp { get; set; }
         public int BatteryStatus{ get; set; }
         */
    }
}