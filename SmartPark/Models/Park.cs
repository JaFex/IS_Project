using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPark.Models
{
    public class Park
    {
        public string Id { get; set; }
        public int number_spot { get; set; }
        public int number_special_spot { get; set; }
        public string description { get; set; }
        public string operating_hours { get; set; }
        public List<Spot> spots{ get; set; }

        /* 
         public string Description { get; set; }
         public int NumberOfSpots{ get; set; }
         public string OperatingHours{ get; set; }
         public int NumberOfSpecialSpots{ get; set; }
         public Xml GeoLocationFile { get; set; } */
    }
}