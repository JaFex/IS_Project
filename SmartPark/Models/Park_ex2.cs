using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartPark.Models
{
    public class Park_ex2
    {
        public string Id { get; set; }
        public List<Spot_ex2> spots { get; set; }
    }
}