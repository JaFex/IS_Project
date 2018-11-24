using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParkDACE.classes
{
    class Provider
    {
        public string connectionType { get; set; }
        public string endpoint { get; set; }
        public string parkInfoID { get; set; }
        public string parkInfoDescription { get; set; }
        public Int32 parkInfoNumberOfSpots { get; set; }
        public string parkInfoOperatingHours { get; set; }
        public Int32 parkInfoNumberOfSpecialSpots { get; set; }
        public string parkInfoGeoLocationFile { get; set; }
        
        public Provider(XmlNode provider)
        {
            connectionType = provider.SelectSingleNode("./connectionType").InnerText;
            endpoint = provider.SelectSingleNode("./endpoint").InnerText;
            XmlNode parkInfo = provider.SelectSingleNode("./parkInfo");
            parkInfoID = parkInfo.SelectSingleNode("./id").InnerText;
            parkInfoDescription = parkInfo.SelectSingleNode("./description").InnerText;
            parkInfoNumberOfSpots = Convert.ToInt32(parkInfo.SelectSingleNode("./numberOfSpots").InnerText);
            parkInfoOperatingHours = parkInfo.SelectSingleNode("./operatingHours").InnerText;
            parkInfoNumberOfSpecialSpots = Convert.ToInt32(parkInfo.SelectSingleNode("./numberOfSpecialSpots").InnerText);
            parkInfoGeoLocationFile = parkInfo.SelectSingleNode("./geoLocationFile").InnerText;
        }
    }
}
