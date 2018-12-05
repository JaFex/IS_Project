using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParkDACE.classes
{
    class ParkingInformation
    {
        public string id { get; set; }
        public string description { get; set; }
        public Int32 numberOfSpots { get; set; }
        public string operatingHours { get; set; }
        public Int32 numberOfSpecialSpots { get; set; }

        public ParkingInformation(string stringXML)
        {
            fromXML(stringXML);
        }

        public ParkingInformation fromXML(string stringXML)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(stringXML);
            XmlNode parkingInformationNode = doc.SelectSingleNode("//parkingInformation");

            this.id = parkingInformationNode["id"].InnerText;
            this.description = parkingInformationNode["description"].InnerText;
            this.numberOfSpots = Convert.ToInt32(parkingInformationNode["numberOfSpots"].InnerText);
            this.numberOfSpecialSpots = Convert.ToInt32(parkingInformationNode["numberOfSpecialSpots"].InnerText);
            this.operatingHours = parkingInformationNode["operatingHours"].InnerText;
            return this;
        }

        public void writeOnScreen()
        {
            Console.WriteLine("Parking Information: ");
            Console.WriteLine("\tID: " + this.id);
            Console.WriteLine("\tDescription: " + this.description);
            Console.WriteLine("\tNumber Of Spots: " + this.numberOfSpots);
            Console.WriteLine("\tNumber Of Special Spots: " + this.numberOfSpecialSpots);
            Console.WriteLine("\tOperating Hours: " + this.operatingHours);
        }
    }
}
