using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml;

namespace BOT_SpotSensors
{
    public class ServiceParkingSpots : SpotSensors
    {
        public string GetSensorDataString(string spotId)
        {
            Random random = new Random();

            XmlDocument doc = new XmlDocument();

            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);

            XmlElement parkingSpotNote = doc.CreateElement("parkingSpot");

            XmlElement idNote = doc.CreateElement("id");
            idNote.InnerText = "Campus_2_B_Park2";

            XmlElement typeNote = doc.CreateElement("type");
            typeNote.InnerText = "ParkingSpot";

            XmlElement nameNote = doc.CreateElement("name");
            nameNote.InnerText = spotId;

            XmlElement locationNote = doc.CreateElement("location");

            XmlElement statusNote = doc.CreateElement("status");
            XmlElement valueNoteInStatus = doc.CreateElement("value");
            valueNoteInStatus.InnerText = random.Next(0, 2) == 0 ? "occupied" : "free";
            XmlElement timestampNoteInstatus = doc.CreateElement("timestamp");
            timestampNoteInstatus.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            statusNote.AppendChild(valueNoteInStatus);
            statusNote.AppendChild(timestampNoteInstatus);

            XmlElement batteryStatusNote = doc.CreateElement("batteryStatus");
            batteryStatusNote.InnerText = random.Next(0, 2).ToString();

            parkingSpotNote.AppendChild(idNote);
            parkingSpotNote.AppendChild(typeNote);
            parkingSpotNote.AppendChild(nameNote);
            parkingSpotNote.AppendChild(locationNote);
            parkingSpotNote.AppendChild(statusNote);
            parkingSpotNote.AppendChild(batteryStatusNote);
            doc.AppendChild(parkingSpotNote);

            return doc.OuterXml;
        }

        public ParkinSpot GetSensorDataUsingDataContract(string spotId)
        {
            Random random = new Random();

            ParkinSpot parkingSpot = new ParkinSpot();
            parkingSpot.Id = "Campu_2_B_Park2";
            parkingSpot.Name = spotId;
            parkingSpot.Value = random.Next(0, 2) == 0 ? "occupied" : "free";
            parkingSpot.Timestamp = DateTime.Now;
            parkingSpot.BatteryStatus = random.Next(0, 2);

            return parkingSpot;
        }
    }
}
