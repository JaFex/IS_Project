using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPark.Models
{
    public class Park
    {
        public string id { get; set; }
        public int number_spot { get; set; }
        public int number_special_spot { get; set; }
        public string description { get; set; }
        public string operating_hours { get; set; }
        public List<Spot> spots{ get; set; }
    }
}