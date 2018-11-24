using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParkDACE.classes
{
    class ParkingSpot
    {
        public string id { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public DateTime timestamp { get; set; }
        public string batteryStatus { get; set; }
        public Location location { get; set; }
        
        public ParkingSpot(string id, string type, string name, string value, DateTime timestamp, string batteryStatus, Location location)
        {
            this.id = id;
            this.type = type;
            this.name = name;
            this.value = value.Equals("1")? "occupied" : "free";
            this.timestamp = timestamp;
            this.batteryStatus = batteryStatus;
            this.location = location;
        }

        public XmlElement ToXML(XmlDocument doc)
        {
            XmlElement parkingSpotNote = doc.CreateElement("parkingSpot");

            XmlElement idNote = doc.CreateElement("id");
            idNote.InnerText = this.id;

            XmlElement typeNote = doc.CreateElement("type");
            typeNote.InnerText = this.type;

            XmlElement nameNote = doc.CreateElement("name");
            nameNote.InnerText = this.name;

            XmlElement locationNote = this.location.ToXML(doc);

            XmlElement statusNote = doc.CreateElement("status");
            XmlElement valueNoteInstatus = doc.CreateElement("value");
            valueNoteInstatus.InnerText = value;
            XmlElement timestampNoteInstatus = doc.CreateElement("timestamp");
            timestampNoteInstatus.InnerText = this.timestamp.ToString("yyyy-MM-dd HH:mm:ss");
            statusNote.AppendChild(valueNoteInstatus);
            statusNote.AppendChild(timestampNoteInstatus);

            XmlElement batteryStatusNote = doc.CreateElement("batteryStatus");
            batteryStatusNote.InnerText = this.batteryStatus;

            parkingSpotNote.AppendChild(idNote);
            parkingSpotNote.AppendChild(typeNote);
            parkingSpotNote.AppendChild(nameNote);
            parkingSpotNote.AppendChild(locationNote);
            parkingSpotNote.AppendChild(statusNote);
            parkingSpotNote.AppendChild(batteryStatusNote);
            return parkingSpotNote;
        }
    }
}
