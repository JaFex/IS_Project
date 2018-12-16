using ParkDACE.classes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Xml;
using System.Text;
using uPLibrary.Networking.M2Mqtt.Messages;
using uPLibrary.Networking.M2Mqtt;
using System.Security.Permissions;
using System.CodeDom;
using System.Web.Services.Description;
using System.CodeDom.Compiler;
using System.Web.Services.Protocols;
using System.Reflection;
using System.Data;
using System.Web.Services.Discovery;
using System.Xml.Schema;
using System.Linq;
using System.Xml.Serialization;

namespace ParkDACE
{
    class Program
    { 
        private static List<Provider> providers;
        private static List<LocationExcel> locationCampus;
        private static Timer timerParks;
        private static List<Timer> timers;
        private static MqttClient mClient;
        private static string[] ips = new string[] { "127.0.0.1", "broker.hivemq.com" };
        private static Mutex mutex = new Mutex(true);

        static void Main(string[] args)
        {
            Console.WriteLine("#################################################################################");
            Console.WriteLine("###################################   INIT   ####################################");
            Console.WriteLine("#################################################################################\n\n");
            providers = new List<Provider>();
            locationCampus = new List<LocationExcel>();
            timers = new List<Timer>();
            Console.WriteLine("######################SETTINGS#####################");
            if(!FunctionHelper.ValidXMLParkingLocation("ParkingLocation.xml", "ParkingLocation.xsd"))
            {
                Console.WriteLine("####################END-SETTINGS###################\n");
                Console.WriteLine("Press any key to exit!");
                Console.ReadKey();
                return;
            }
            int time = ReadXMLParkingLocation("ParkingLocation.xml");
            if(providers == null || providers.ToArray().Length == 0)
            {
                Console.WriteLine("####################END-SETTINGS###################\n");
                Console.WriteLine("Press any key to exit!");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Xml is valid!");
            string topicsString = "ParkDACE\\, ParkDACE\\all";


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
            mClient = Mosquitto.connectMosquittoGuaranteeThatTryToConnect(ips);
            if (mClient == null || !mClient.IsConnected)
            {
                Console.WriteLine("###################END-MOSQUITTO###################\n");
                Console.WriteLine("Press any key to exit!");
                Console.ReadKey();
                return;
            }
            mClient.ConnectionClosed += TryReconnect;
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
                    timers.Add(new Timer(new TimerCallback(timer_SOAP), provider, 1, time));
                }
            }
            timerParks = new Timer(new TimerCallback(timer_park_information_and_update), providers, 1, time);
            mutex.ReleaseMutex();
        }

        private static void timer_park_information_and_update(object stateInfo)
        {
            mutex.WaitOne();
            Console.WriteLine("######################Update Excel Information###########################");
            foreach (LocationExcel locationExcel in locationCampus)
            {
                locationExcel.updateIfNeedUpdate();
            }
            Console.WriteLine("####################END-Update Excel Information#########################");

            Console.WriteLine("######################Park Information###########################");
            //List<Provider> providersLocal = (List<Provider>)stateInfo;
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);
            XmlElement listNote = doc.CreateElement("list");
            foreach (Provider provider in providers)
            {
                ParkingInformation parkingInformation = new ParkingInformation(provider.parkInfoID, provider.parkInfoDescription, provider.parkInfoNumberOfSpots, provider.parkInfoNumberOfSpecialSpots, provider.parkInfoOperatingHours);
                listNote.AppendChild(parkingInformation.ToXML(doc));
            }
            
            doc.AppendChild(listNote);

            Console.WriteLine(FunctionHelper.formatXmlToUnminifierString(doc));

            Console.WriteLine("Sending information...");
            Mosquitto.publishMosquitto(mClient, new string[] { "ParkDACE\\"}, doc.OuterXml);
            Console.WriteLine("######################END-Park Information#####################\n\n");
            mutex.ReleaseMutex();
        }

        
        private static void timer_SOAP(object stateInfo)
        {
            mutex.WaitOne();
            Console.WriteLine("#######################SOAP########################");
            Provider provider = (Provider)stateInfo;
            Random random = new Random();
            XmlDocument doc = new XmlDocument();
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, null);
            doc.AppendChild(dec);

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
                Console.WriteLine("Erro try to get spot location from " + provider.parkInfoGeoLocationFile);
                Console.WriteLine("######################END-SOAP#####################\n\n");
                return;
            }
            string xmlString = "";
            try
            {
                xmlString = dynamicWebServiceCall(provider.endpoint, "GetSensorDataString", location.name).ToString();
            } catch (Exception e)
            {
                Console.WriteLine("Fail to comunicate wich " + provider.endpoint);
                Console.WriteLine("######################END-SOAP#####################\n\n");
                return;
            }
            doc.LoadXml(xmlString);
            XmlNode locationNode = doc.SelectSingleNode("//location");

            locationNode.InnerXml = location.ToXML(doc).InnerXml;

            Console.WriteLine(FunctionHelper.formatXmlToUnminifierString(doc));

            Console.WriteLine("Sending information...");
            Mosquitto.publishMosquitto(mClient, new string[] { "ParkDACE\\all", "ParkDACE\\" + provider.parkInfoID }, doc.OuterXml);
            Console.WriteLine("######################END-SOAP#####################\n\n");
            mutex.ReleaseMutex();
        }

        public static void ComputeResponse(string str)
        {
            mutex.WaitOne();
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
            mutex.ReleaseMutex();
        }

        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.InheritanceDemand, Name = "FullTrust")]
        [System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.LinkDemand, Name = "FullTrust")]
        private static object dynamicWebServiceCall(string endpoint, string functionName, string locationName)
        {
            string servicoName = endpoint.Split('.')[0].Split('/')[3];
            ServiceDescriptionImporter descriptionImporter = new ServiceDescriptionImporter();
            descriptionImporter.ProtocolName = "Soap";
            descriptionImporter.Style = ServiceDescriptionImportStyle.Client;
            descriptionImporter.CodeGenerationOptions = CodeGenerationOptions.GenerateNewAsync;

            DiscoveryClientProtocol clientProtocol = new DiscoveryClientProtocol();
            clientProtocol.DiscoverAny(endpoint);
            clientProtocol.ResolveAll();
            clientProtocol.Documents.Values.OfType<object>()
                                   .Select(document =>
                                   {
                                       if (document is ServiceDescription)
                                           descriptionImporter.AddServiceDescription(document as ServiceDescription, string.Empty, string.Empty);
                                       else if (document is XmlSchema)
                                           descriptionImporter.Schemas.Add(document as XmlSchema);
                                       return true;
                                   })
                                   .ToList();

            CodeNamespace codeNamemspace = new CodeNamespace();
            CodeCompileUnit codeCompileUnit = new CodeCompileUnit();
            codeCompileUnit.Namespaces.Add(codeNamemspace);
            ServiceDescriptionImportWarnings warning = descriptionImporter.Import(codeNamemspace, codeCompileUnit);
            if (warning == 0)
            {
                // Generate and print the proxy code in C#.
                CodeDomProvider provider1 = CodeDomProvider.CreateProvider("CSharp");
                // Compile the assembly with the appropriate references
                string[] assemblyReferences = new string[] { "System.dll", "System.Web.Services.dll", "System.Web.dll", "System.Xml.dll", "System.Data.dll" };
                CompilerParameters parms = new CompilerParameters(assemblyReferences);
                parms.GenerateInMemory = true;
                CompilerResults results = provider1.CompileAssemblyFromDom(parms, codeCompileUnit);

                foreach (CompilerError oops in results.Errors)
                {
                    Console.WriteLine("======== Compiler error ============");
                    Console.WriteLine(oops.ErrorText);
                }

                //Invoke the web service method
                object o = results.CompiledAssembly.CreateInstance(servicoName);
                MethodInfo mi = o.GetType().GetMethod(functionName);
                return mi.Invoke(o, new String[] { locationName });
            }
            else
            {
                // Print an error message.
                Console.WriteLine("Warning: " + warning);
                return null;
            }
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
            Console.WriteLine("Was found: " + providers.Count+ " providers, the information from the providers is sent every "+time+ " milliseconds!");
            return time;
        }

        private static void TryReconnect(object sender, EventArgs e)
        {
            if (mClient == null || !mClient.IsConnected)
            {
                Console.WriteLine("#####################MOSQUITTO#####################");
                mClient = Mosquitto.connectMosquittoGuaranteeThatTryToConnect(ips);
                if (mClient == null || !mClient.IsConnected)
                {
                    return;
                }
                mClient.ConnectionClosed += TryReconnect;
                Console.WriteLine("###################END-MOSQUITTO###################\n");
            }

            if (mClient == null || !mClient.IsConnected)
            {
                return;
            }
            mutex.ReleaseMutex();
        }
    }
}
