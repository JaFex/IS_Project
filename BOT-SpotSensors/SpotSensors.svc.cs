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
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class SpotSensors : IService1
    {

        public string GetSensorData(string spotId)
        {
            Random random = new Random();

            XmlDocument doc = new XmlDocument();
            var root = doc.CreateElement("parkingSpot");
            doc.AppendChild(root);

            //ID
            var id = doc.CreateElement("id");
            id.InnerText = "Campus_2_B_Park2";
            root.AppendChild(id);

            //Type
            var type = doc.CreateElement("type");
            type.InnerText = "ParkingSpot";
            root.AppendChild(type);

            //Name
            var name = doc.CreateElement("name");
            name.InnerText = spotId;
            root.AppendChild(name);

            //Location
            var location = doc.CreateElement("location");
            root.AppendChild(location);

            //Status
            var status = doc.CreateElement("status");
            root.AppendChild(status);

            //Status->Value
            var value = doc.CreateElement("value");
            status.InnerText = random.Next(0,2) == 0 ? "occupied" : "free";
            status.AppendChild(value);

            //Status->Timestamp
            var timestamp = doc.CreateElement("timestamp");
            timestamp.InnerText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            status.AppendChild(timestamp);

            //Battery Status
            var battery = doc.CreateElement("batteryStatus");
            battery.InnerText = random.Next(0, 2).ToString();
            root.AppendChild(battery);

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
