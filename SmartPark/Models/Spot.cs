using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPark.Models
{
    public class Spot 
    {
        public string Id { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string status { get; set; }
        public string battery_status { get; set; }
        public string park_id { get; set; }
        
    }
}