using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParkSS.classes
{
    class ParkingSpot
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public DateTime timestamp { get; set; }
        public string batteryStatus { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public ParkingSpot(string stringXML)
        {
            fromXML(stringXML);
        }

        public ParkingSpot fromXML(string stringXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(stringXML);
            XmlNode parkingSpotNode = doc.SelectSingleNode("//parkingSpot");
            XmlNode locationNode = doc.SelectSingleNode("//location");
            XmlNode statusNode = doc.SelectSingleNode("//status");

            this.id = parkingSpotNode["id"].InnerText;
            this.type = parkingSpotNode["type"].InnerText;
            this.name = parkingSpotNode["name"].InnerText;
            this.value = statusNode["value"].InnerText;
            this.timestamp = DateTime.Parse(statusNode["timestamp"].InnerText);
            this.batteryStatus = parkingSpotNode["batteryStatus"].InnerText;
            this.latitude = locationNode["latitude"].InnerText;
            this.longitude = locationNode["longitude"].InnerText;
            return this;
        }

        public void writeOnScreen()
        {
            Console.WriteLine("Parking Spot: ");
            Console.WriteLine("\tID: " + this.id);
            Console.WriteLine("\tType: " + this.type);
            Console.WriteLine("\tName: " + this.name);
            Console.WriteLine("\tLocation: ");
            Console.WriteLine("\t\tLatitude: " + this.latitude);
            Console.WriteLine("\t\tLongitude: " + this.longitude);
            Console.WriteLine("\tStatus: ");
            Console.WriteLine("\t\tValue: " + this.value);
            Console.WriteLine("\t\tTime stamp: " + this.timestamp.ToString("yyyy-MM-dd HH:mm:ss"));
            Console.WriteLine("\tBattery Status: " + this.batteryStatus);
        }
    }
}
