using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPark.Models
{
    public class Park
    {
        public string id { get; set; }
        public string description { get; set; }
        public Int32 numberOfSpots { get; set; }
        public string operatingHours { get; set; }
        public Int32 numberOfSpecialSpots { get; set; }
        public List<Spot> spots{ get; set; }
    }
}