using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPark.Models
{
    public class Spot 
    {
        public string id { get; set; }
        public string parkID { get; set; }
        public string status { get; set; }
        public DateTime timestamp { get; set; }
        public string batteryStatus { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
    }
}