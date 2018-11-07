using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using Excel = Microsoft.Office.Interop.Excel;

namespace ParkDACE
{
    class Program
    {
        private static string strDocuments = AppDomain.CurrentDomain.BaseDirectory;
        
        private static string fileXmlParkingSpot = "Parking_Spots.xml";

        private static string fileCampus_2_A_Park1Excel = "Campus_2_A_Park1.xlsx";
        private static string[][] locationCampus_2_A_Park1 = new string[2][];

        static void Main(string[] args)
        {
            ParkingSensorNodeDll.ParkingSensorNodeDll dll = new ParkingSensorNodeDll.ParkingSensorNodeDll();
            locationCampus_2_A_Park1 = ReadFromExcelFile(fileCampus_2_A_Park1Excel);
            dll.Initialize(ComputeResponse, 10000);
        }

        public static void ComputeResponse(string str)
        {
            string[] strs = str.Split(';');

            string[] ids = { "ParkID", "SpotID", "Timestamp", "ParkingSpotStatus", "BatteryStatus" };
            Console.WriteLine("###################################################");

            XMLParkingSpot(fileXmlParkingSpot, strs[0], strs[1], strs[3], strs[2], strs[4]);
            Thread.Sleep(800);
            ReadXMLParkingSpot(fileXmlParkingSpot, strs[0], strs[1]);
            Console.WriteLine("############\n");


            for (int i = 0; i < strs.Length; i++)
            {
                Console.WriteLine(ids[i] + ": " + strs[i]);
            }

            Console.WriteLine("###################################################\n");

        }

        public static void ReadXMLParkingSpot(string filename, string id, string name)
        {
            if (!checkIfFileExist(fileXmlParkingSpot))
            {
                return;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(@fileXmlParkingSpot);
            XmlNode root = doc.SelectSingleNode("/parks");
            XmlNode parkingSpotNode = doc.SelectSingleNode("//parkingSpot[id='" + id + "']").SelectSingleNode("//parkingSpot[name='" + name + "']");
            XmlNode statusNode = doc.SelectSingleNode("//parkingSpot[id='" + id + "']").SelectSingleNode("//parkingSpot[name='" + name + "']/status");
            if (parkingSpotNode == null)
            {
                return;
            }
            Console.WriteLine("id: " + parkingSpotNode["id"].InnerText);
            Console.WriteLine("name: " + parkingSpotNode["name"].InnerText);
            Console.WriteLine("location: " + parkingSpotNode["location"].InnerText);
            Console.WriteLine("value: " + statusNode["value"].InnerText);
            Console.WriteLine("timestamp: " + statusNode["timestamp"].InnerText);
            Console.WriteLine("batteryStatus: " + parkingSpotNode["batteryStatus"].InnerText);
        }

        public static string XMLParkingSpot(string filename, string id, string name, string value, string timestamp, string batteryStatus)
        {
            if (!checkIfFileExist(fileXmlParkingSpot))
            {
                return CreateXMLParkingSpot(filename, id, name, value, timestamp, batteryStatus);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(@fileXmlParkingSpot);
            XmlNode root = doc.SelectSingleNode("/parks");
            XmlNode parkingSpotNode = doc.SelectSingleNode("//parkingSpot[id='" + id + "']").SelectSingleNode("//parkingSpot[name='" + name + "']");
            XmlNode statusNode = doc.SelectSingleNode("//parkingSpot[id='" + id + "']").SelectSingleNode("//parkingSpot[name='" + name + "']/status");
            if (parkingSpotNode == null)
            {
                XmlElement parkingSpot = CreateParkingSpot(doc, id, "ParkingSpot", name, value, timestamp, batteryStatus);
                root.AppendChild(parkingSpot);
                doc.Save(@fileXmlParkingSpot);
                return parkingSpot.OuterXml;
            }
            statusNode["value"].InnerText = value;
            statusNode["timestamp"].InnerText = timestamp;
            parkingSpotNode["batteryStatus"].InnerText = batteryStatus;
            doc.Save(@fileXmlParkingSpot);
            return parkingSpotNode.OuterXml;
        }

        public static string CreateXMLParkingSpot(string filename, string id, string name, string value, string timestamp, string batteryStatus)
        {
            XmlDocument doc = new XmlDocument();
            // Create the XML Declaration, and append it to XML document
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            // Create the root element
            XmlElement root = doc.CreateElement("parks");
            doc.AppendChild(root);
            // Create Books
            // Note that to set the text inside the element,
            // you use .InnerText
            // You use SetAttribute to set attribute
            XmlElement parkingSpot = CreateParkingSpot(doc, id, "ParkingSpot", name, value, timestamp, batteryStatus);
            root.AppendChild(parkingSpot);

            doc.Save(@fileXmlParkingSpot);
            //string xmlOutput = doc.OuterXml;

            return parkingSpot.OuterXml;
            //return doc.InnerXml;

        }

        public static XmlElement CreateParkingSpot(XmlDocument doc, string id, string type, string name, string value, string timestamp, string batteryStatus)
        {
            XmlElement parkingSpotNote = doc.CreateElement("parkingSpot");

                XmlElement idNote = doc.CreateElement("id");
                idNote.InnerText = id;

                XmlElement typeNote = doc.CreateElement("type");
                typeNote.InnerText = type;

                XmlElement nameNote = doc.CreateElement("name");
                nameNote.InnerText = name;

                XmlElement locationNote = doc.CreateElement("location");
                string[] location = readLocation(id, name);
                    if(location == null)
                    {
                        location = new string[2];
                        location[0] = "";
                        location[1] = "";
                    }
                    if(location.Length == 4)
                    {
                string[] save = location;
                        location = new string[2];
                        location[0] = "";
                        location[1] = "";
                    }
                    XmlElement xNoteInLocation = doc.CreateElement("x");
                    xNoteInLocation.InnerText = location[0];
                    XmlElement yNoteInLocation = doc.CreateElement("y");
                    yNoteInLocation.InnerText = location[1];
                locationNote.AppendChild(xNoteInLocation);
                locationNote.AppendChild(yNoteInLocation);

                XmlElement statusNote = doc.CreateElement("status");
                    XmlElement valueNoteInstatus = doc.CreateElement("value");
                    valueNoteInstatus.InnerText = value;
                    XmlElement timestampNoteInstatus = doc.CreateElement("timestamp");
                    timestampNoteInstatus.InnerText = timestamp;
                statusNote.AppendChild(valueNoteInstatus);
                statusNote.AppendChild(timestampNoteInstatus);

                XmlElement batteryStatusNote = doc.CreateElement("batteryStatus");
                batteryStatusNote.InnerText = batteryStatus;

            parkingSpotNote.AppendChild(idNote);
            parkingSpotNote.AppendChild(typeNote);
            parkingSpotNote.AppendChild(nameNote);
            parkingSpotNote.AppendChild(locationNote);
            parkingSpotNote.AppendChild(statusNote);
            parkingSpotNote.AppendChild(batteryStatusNote);
            return parkingSpotNote;
        }

        public static string[] readLocation(string id, string spotID)
        {
            string[][] local;
            switch(id)
            {
                case "Campus_2_A_Park1":
                    local = locationCampus_2_A_Park1;
                    break;
                default:
                    return null;
            }
            for(int i=0; i< local[0].Length; i++)
            {
                if (local[0][i] == spotID)
                {
                    return local[1][i].Split(',');
                }
            }
            return null;
        }

        public static string[][] ReadFromExcelFile(string filename)
        {
            checkIfFileExistOrFail(filename);
            var excelAplication = new Excel.Application();
            excelAplication.Visible = false;

            //Opens the excel file
            var excelWorkbook = excelAplication.Workbooks.Open(@"" + strDocuments + "" + filename);
            var excelWorksheet = (Excel.Worksheet)excelWorkbook.ActiveSheet;

            int count = 7;

            while(excelWorksheet.Cells[count, 1].Value != null)
            {
                count++;
            }
            count -= 6;
            string[][] locationParks = new string[2][];
            locationParks[0] = new string[count];
            locationParks[1] = new string[count];

            for (int y = 0, exel = 6; y < count; y++, exel++)
            {
                locationParks[0][y] = excelWorksheet.Cells[exel, 1].Value==null ? string.Empty : excelWorksheet.Cells[exel, 1].Value;
                locationParks[1][y] = excelWorksheet.Cells[exel, 2].Value == null ? string.Empty : excelWorksheet.Cells[exel, 2].Value;
            }

            excelWorkbook.Close();
            excelAplication.Quit();

            //Don't forget to free the memory used by excel objects
            ReleaseComObjects(excelWorksheet);
            ReleaseComObjects(excelWorkbook);
            ReleaseComObjects(excelAplication);
            return locationParks;
        }

        public static void checkIfFileExistOrFail(string filename)
        {
            if (!File.Exists(@""+strDocuments+""+filename))
            {
                throw new FileNotFoundException();
            }
        }

        public static Boolean checkIfFileExist(string filename)
        {
            if (File.Exists(@"" + strDocuments + "" + filename))
            {
                return true;
            }
            return false;
        }

        private static void ReleaseComObjects(object obs)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obs);
                obs = null;
                GC.Collect();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }
    }
}
