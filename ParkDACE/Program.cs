﻿using ParkDACE.classes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using ParkDACE.ServiceParkingSpots;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;

namespace ParkDACE
{
    class Program
    { 
        private static List<Provider> providers;
        private static List<LocationExcel> locationCampus;
        private static Timer timer;
        private static MqttClient mClient;
        private static string[] ips = new string[] { "broker.hivemq.com", "127.0.0.1" };

        static void Main(string[] args)
        {
            Console.WriteLine("#################################################################################");
            Console.WriteLine("###################################   INIT   ####################################");
            Console.WriteLine("#################################################################################\n\n");
            providers = new List<Provider>();
            locationCampus = new List<LocationExcel>();
            Console.WriteLine("######################SETTINGS#####################");
            int time = ReadXMLParkingLocation("ParkingLocation.xml");
            string topicsString = "ParkDACE\\all";


            foreach (Provider provider in providers)
            {
                Console.WriteLine("");
                Console.WriteLine("Connection Type: " + provider.connectionType);
                Console.WriteLine("Endpoint: " + provider.endpoint);
                Console.WriteLine("parkInfo: ");
                Console.WriteLine("\tID: " + provider.parkInfoID);
                Console.WriteLine("\tDescription: " + provider.parkInfoDescription);
                Console.WriteLine("\tNumberOfSpots: " + provider.parkInfoNumberOfSpecialSpots);
                Console.WriteLine("\tOperatingHours: " + provider.parkInfoOperatingHours);
                Console.WriteLine("\tNumberOfSpecialSpots: " + provider.parkInfoNumberOfSpecialSpots);
                Console.WriteLine("\tGeoLocationFile: " + provider.parkInfoGeoLocationFile);
                Console.WriteLine("");

                topicsString += ", ParkDACE\\" + provider.parkInfoID;
            }
            Console.WriteLine("####################END-SETTINGS###################\n");

            Console.WriteLine("#####################MOSQUITTO#####################");
            mClient = Mosquitto.connectMosquitto(ips);
            Console.WriteLine("Topics that will be send data "+ topicsString + "!");
            Console.WriteLine("###################END-MOSQUITTO###################\n");

            Console.WriteLine("\n\n\n#################################################################################");
            Console.WriteLine("###########################   Starting Application   ############################");
            Console.WriteLine("#################################################################################\n\n");
            foreach (Provider provider in providers)
            {
                locationCampus.Add(new LocationExcel(provider.parkInfoGeoLocationFile));
                if (provider.connectionType.Equals("DLL") || provider.endpoint.Equals("ParkingSensorNodeDll"))
                {
                    ParkingSensorNodeDll.ParkingSensorNodeDll dll = new ParkingSensorNodeDll.ParkingSensorNodeDll();
                    dll.Initialize(ComputeResponse, time);
                } else if (provider.connectionType.Equals("SOAP"))
                {
                    timer = new Timer(new TimerCallback(timer_SOAP), provider, 1000, time);
                }
            }
            
        }

        private static void timer_SOAP(object stateInfo)
        {
            Console.WriteLine("#######################SOAP########################");
            Provider provider = (Provider)stateInfo;
            Random random = new Random();
            var client = new SpotSensorsClient();
            XmlDocument doc = new XmlDocument();

            Location location = null;
            foreach (LocationExcel le in locationCampus)
            {
                if (le.parkingID.Equals(provider.parkInfoID))
                {
                    location = le.giveLocation(random.Next(0, provider.parkInfoNumberOfSpots));
                }
            }
            if (location == null)
            {
                Console.WriteLine("Erro try to get spot from "+provider.endpoint);
                Console.WriteLine("######################END-SOAP#####################\n\n");
                return;
            }
            string xmlString = client.GetSensorDataString(location.name);
            doc.LoadXml(xmlString);
            XmlNode locationNode = doc.SelectSingleNode("//location");

            locationNode.InnerXml = location.ToXML(doc).InnerXml;

            Console.WriteLine(FunctionHelper.formatXmlToUnminifierString(doc));

            Console.WriteLine("Sending information...");
            Mosquitto.publishMosquitto(mClient, new string[] { "ParkDACE\\all", "ParkDACE\\"+provider.parkInfoID }, doc.OuterXml);
            Console.WriteLine("######################END-SOAP#####################\n\n");
        }

        public static void ComputeResponse(string str)
        {
            Console.WriteLine("########################DLL########################");
            string[] strs = str.Split(';');
            //string[] ids = { "ParkID", "SpotID", "Timestamp", "ParkingSpotStatus", "BatteryStatus" };

            XmlDocument doc = new XmlDocument();
            string xmlString = CreateXMLSpot(strs[0], strs[1], strs[3], strs[2], strs[4]);
            doc.LoadXml(xmlString);

            Console.WriteLine(FunctionHelper.formatXmlToUnminifierString(doc));
            Console.WriteLine("Sending information...");
            Mosquitto.publishMosquitto(mClient, new string[] { "ParkDACE\\all", "ParkDACE\\" +strs[0] }, doc.OuterXml);
            Console.WriteLine("######################END-DLL######################\n\n");
        }

        public static string CreateXMLSpot(string id, string name, string value, string timestamp, string batteryStatus)
        {
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);

            Location location = null;
            foreach (LocationExcel locationExcel in locationCampus)
            {
                if (locationExcel.parkingID.Equals(id))
                {
                    location = locationExcel.giveLocation(name);
                }
            }
            if (location == null)
            {
                Console.WriteLine("Fail to find the location!\n");
                return "";
            }

            XmlElement spot = new ParkingSpot(id, "ParkingSpot", name, value, DateTime.Parse(timestamp), batteryStatus, location).ToXML(doc);

            doc.AppendChild(spot);
            return doc.OuterXml;
        }

        public static int ReadXMLParkingLocation(string filename)
        {
            if (!FunctionHelper.checkIfFileExist(filename))
            {
                return 0;
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(FunctionHelper.givePatch(filename));

            XmlNode root = doc.SelectSingleNode("/parkingLocation");
            foreach(XmlNode provider in root.SelectNodes("//provider"))
            {
                providers.Add(new Provider(provider));
            }

            int refreshRate = Convert.ToInt32(root.Attributes["refreshRate"].Value);
            string unitsString = root.Attributes["units"].Value;
            int units = 0;
            switch (unitsString)
            {
                case "minutes":
                    units = 60000;
                    break;
                case "seconds":
                    units = 1000;
                    break; 
                default:
                    units = 1;
                    break;
            }
            if(refreshRate * units == 0)
            {
                Console.WriteLine("Erro to read the time!");
                return 2000;
            }
            int time = refreshRate*units;
            Console.WriteLine("Was found: " + providers.Count+" providers, they will be update "+ time + " in "+time+ " milliseconds!");
            return time;
        }
    }
}
