﻿using ParkDACE.classes;
using ParkSS.classes;
using ParkSS.exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ParkSS
{
    class Program
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ParkSS.Properties.Settings.connStr"].ConnectionString;
        private static string[] ipsParkDACE = new string[] { "127.0.0.1", "broker.hivemq.com" }; //, "broker.hivemq.com"
        private static string[] topicsParkDACE = new string[] { "ParkDACE\\", "ParkDACE\\all" };
        private static List<string> parkingsNameSubscribed;
        private static MqttClient mClient;
        private static MqttClient mClientPark;
        private static Mutex mutex = new Mutex(true);
        private static Mutex mutexReconnect = new Mutex(true);


        static void Main(string[] args)
        {
            Console.WriteLine("#################################################################################");
            Console.WriteLine("###################################   INIT   ####################################");
            Console.WriteLine("#################################################################################\n\n");
            parkingsNameSubscribed = new List<string>();
            Console.WriteLine("#####################MOSQUITTO#####################");

            Console.WriteLine("####Connect to the topic " + topicsParkDACE[0] + "####");
            mClientPark = Mosquitto.connectMosquittoGuaranteeThatTryToConnect(ipsParkDACE);
            if (mClientPark == null || !mClientPark.IsConnected)
            {
                return;
            }
            mClientPark.ConnectionClosed += TryReconnectMClientPark;
            Console.WriteLine("####Connect to the other topics ####");
            mClient = Mosquitto.connectMosquittoGuaranteeThatTryToConnect(ipsParkDACE);
            if (mClient == null || !mClient.IsConnected)
            {
                return;
            }
            mClient.ConnectionClosed += TryReconnectMClient;
            Console.WriteLine("#####################END-Connection##################\n");

            String topicsSubscribe = "";

            Console.WriteLine("#######Config the mClientPark on topic " + topicsParkDACE[0] + "#######");
            Mosquitto.configFunctionMosquitto(mClientPark, mClientPark_MqttMsgPublishReceived);
            topicsSubscribe += "\n\t\t\t-'"+topicsParkDACE[0]+"'";
            Console.WriteLine("#####################END-Config##################\n");
 
            Console.WriteLine("#######Config the mClient on new topics#######");
            Mosquitto.configFunctionMosquitto(mClient, mClient_MqttMsgPublishReceived);
            Console.WriteLine("#####################END-Config##################\n");

            Console.WriteLine("You will be subscribe to the topic:" + topicsSubscribe);
            Console.WriteLine("#################END-Config-MOSQUITTO###############\n");
            Console.WriteLine("\n\n\n#################################################################################");
            Console.WriteLine("###########################   Starting Application   ############################");
            Console.WriteLine("#################################################################################\n\n");
            Mosquitto.subscribedMosquitto(mClientPark, topicsParkDACE[0]);
            mutexReconnect.ReleaseMutex();
            mutex.ReleaseMutex();
        }

        private static void TryReconnectMClientPark(object sender, EventArgs e)
        {
            mutexReconnect.WaitOne();
            if (mClientPark == null || !mClientPark.IsConnected)
            {
                Console.WriteLine("##############Try to reconnect mClientPark#############");
                mClientPark = Mosquitto.connectMosquittoGuaranteeThatTryToConnect(ipsParkDACE);
                if (mClientPark == null || !mClientPark.IsConnected)
                {
                    System.Environment.Exit(1);
                }
                mClientPark.ConnectionClosed += TryReconnectMClientPark;
                Mosquitto.configFunctionMosquitto(mClientPark, mClientPark_MqttMsgPublishReceived);
                Mosquitto.subscribedMosquitto(mClientPark, topicsParkDACE[0]);
            }
            mutexReconnect.ReleaseMutex();
            mutex.ReleaseMutex();
        }

        private static void TryReconnectMClient(object sender, EventArgs e)
        {
            mutexReconnect.WaitOne();
            if (mClient == null || !mClient.IsConnected)
            {
                Console.WriteLine("##############Try to reconnect mClient#############");
                mClient = Mosquitto.connectMosquittoGuaranteeThatTryToConnect(ipsParkDACE);
                if (mClient == null || !mClient.IsConnected)
                {
                    System.Environment.Exit(1);
                }
                mClient.ConnectionClosed += TryReconnectMClient;
                Mosquitto.configFunctionMosquitto(mClient, mClient_MqttMsgPublishReceived);
                foreach (String parkingInformation in parkingsNameSubscribed)
                {
                    Mosquitto.subscribedMosquitto(mClient, topicsParkDACE[0] + "" + parkingInformation);
                }
            }
            mutexReconnect.ReleaseMutex();
            mutex.ReleaseMutex();
        }

        public static void mClientPark_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) //Recebe tudo independentement que park pretence
        {
            mutex.WaitOne();
            Console.WriteLine("#######################New Park########################################");
            Console.WriteLine("mClientPark-->> Topic: " + e.Topic);
            List<ParkingInformation> parkingsInformationsLocal = new List<ParkingInformation>();
            List<string> parkingsNameLocal = new List<string>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Encoding.UTF8.GetString(e.Message));
            if(!FunctionHelper.ValidXML(doc, "ParkInformation.xsd"))
            {
                Console.WriteLine("##############################END New Park#################################");
                mutex.ReleaseMutex();
                return;
            }
            Console.WriteLine("XML is valid!");
            Boolean newTopics = false;
            foreach (XmlNode parkingInformationNode in doc.SelectNodes("/list/parkingInformation"))
            {
                ParkingInformation parkingInformation = new ParkingInformation(parkingInformationNode.OuterXml);
                parkingsInformationsLocal.Add(parkingInformation);
                if (parkingsNameSubscribed.Count == 0 || !parkingsNameSubscribed.Exists(topicList => topicList.Equals(parkingInformation.id)))
                {
                    Console.Write(newTopics?"": "New topics subscribed: \n");
                    newTopics = true;
                    string topic = topicsParkDACE[0] + "" + parkingInformation.id;
                    Console.WriteLine("\t\t|"+ topic);
                    parkingsNameSubscribed.Add(parkingInformation.id);
                    Mosquitto.subscribedMosquitto(mClient, topic);
                }
            }
            Console.WriteLine();


            //Verificar e unsubscribe os que já não existem
            if (parkingsNameSubscribed.Count > 0)
            {
                List<string> auxList = parkingsNameLocal.Except(parkingsNameSubscribed).ToList();
                if (auxList.Count > 0) {
                    Console.WriteLine("Old topics Unsubscribed: ");
                    foreach (string aux in auxList)
                    {
                        Console.WriteLine("\t\t|" + topicsParkDACE[0] + "" + aux);
                        Mosquitto.unsubscribeMosquitto(mClient, new string[] { topicsParkDACE[0] + "" + aux });
                        parkingsNameSubscribed.Remove(aux);
                    }
                }
            }

            /////// Send data to BD
            string response = "";
            foreach (ParkingInformation park in parkingsInformationsLocal)
            {
                try
                {
                    response = DatabaseHelper.newParking(park);
                }
                catch (ParkNotInsertedException ex)
                {
                    response = ex.Message;
                }
                Console.WriteLine("\t\t\t"+ park.id + ":" + response);
            }

            Console.WriteLine("##############################END New Park#################################");
            mutex.ReleaseMutex();
        }

        public static void mClient_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) //Recebe tudo independentement que park pretence
        {
            mutex.WaitOne();
            Console.WriteLine("##############################" + e.Topic + "#########################################");
            Console.WriteLine("mClient-->> Topic: " + e.Topic);
            if (!FunctionHelper.ValidXML(Encoding.UTF8.GetString(e.Message), "ParkingSpot.xsd"))
            {
                Console.WriteLine("##############################END "+e.Topic+"#################################");
                mutex.ReleaseMutex();
                return;
            }
            Console.WriteLine("XML is valid!");
            ParkingSpot parkingSpot = new ParkingSpot(Encoding.UTF8.GetString(e.Message));
            parkingSpot.writeOnScreen();

            /////// Send data to BD
            string response = "";
            Console.WriteLine("#################################### BD ########################################");
            try
            {
                response = DatabaseHelper.newRegister(parkingSpot);
            }
            catch (ParkNotInsertedException ex)
            {
                response = ex.Message;
            }
            Console.WriteLine(response);
            Console.WriteLine("##############################END " + e.Topic + "#################################");
            mutex.ReleaseMutex();
        }
    }
}
