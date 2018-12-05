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

        public ParkingInformation(string id, string description, Int32 numberOfSpots, Int32 numberOfSpecialSpots, string operatingHours)
        {
            this.id = id;
            this.description = description;
            this.numberOfSpots = numberOfSpots;
            this.numberOfSpecialSpots = numberOfSpecialSpots;
            this.operatingHours = operatingHours;
        }

        public XmlElement ToXML(XmlDocument doc)
        {
            XmlElement parkingInformationNote = doc.CreateElement("parkingInformation");

            XmlElement idNote = doc.CreateElement("id");
            idNote.InnerText = this.id;

            XmlElement descriptionNote = doc.CreateElement("description");
            descriptionNote.InnerText = this.description;

            XmlElement numberOfSpotsNote = doc.CreateElement("numberOfSpots");
            numberOfSpotsNote.InnerText = this.numberOfSpots.ToString();

            XmlElement numberOfSpecialSpotsNote = doc.CreateElement("numberOfSpecialSpots");
            numberOfSpecialSpotsNote.InnerText = this.numberOfSpecialSpots.ToString();

            XmlElement operatingHoursNote = doc.CreateElement("operatingHours");
            operatingHoursNote.InnerText = this.operatingHours;

            parkingInformationNote.AppendChild(idNote);
            parkingInformationNote.AppendChild(descriptionNote);
            parkingInformationNote.AppendChild(numberOfSpotsNote);
            parkingInformationNote.AppendChild(numberOfSpecialSpotsNote);
            parkingInformationNote.AppendChild(operatingHoursNote);
            return parkingInformationNote;
        }
    }
}
