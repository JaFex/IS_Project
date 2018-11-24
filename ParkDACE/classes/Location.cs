using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParkDACE.classes
{
    class Location
    {
        public string name { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

        public Location(string latitude, string longitude)
        {
            this.name = "";
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public Location(string name, string latitude, string longitude)
        {
            this.name = name;
            this.latitude = latitude;
            this.longitude = longitude;
        }

        public XmlElement ToXML(XmlDocument doc)
        {
            XmlElement locationNote = doc.CreateElement("location");
            XmlElement xNoteInLocation = doc.CreateElement("latitude");
            xNoteInLocation.InnerText = latitude;
            XmlElement yNoteInLocation = doc.CreateElement("longitude");
            yNoteInLocation.InnerText = longitude;
            locationNote.AppendChild(xNoteInLocation);
            locationNote.AppendChild(yNoteInLocation);
            return locationNote;
        }
    }
}
