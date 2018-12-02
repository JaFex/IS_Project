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
        private static string[] ipsParkDACE = new string[] { "broker.hivemq.com", "127.0.0.1" };
        private static string[] topicsParkDACE = new string[] { "ParkDACE\\all", "ParkDACE\\Campus_2_A_Park1", "ParkDACE\\Campus_2_B_Park2" };

        static void Main(string[] args)
        {
            Console.WriteLine("#################################################################################");
            Console.WriteLine("###################################   INIT   ####################################");
            Console.WriteLine("#################################################################################\n\n");
            Console.WriteLine("#####################MOSQUITTO#####################");

            /*Console.WriteLine("####Connect to the topic " + topicsParkDACE[0] + "####");
            MqttClient mClientALL = Mosquitto.connectMosquittoGuaranteeThatTryToConnect(ipsParkDACE);
            Console.WriteLine("#####################END-Connection##################\n");*/
            Console.WriteLine("####Connect to the topic " + topicsParkDACE[1] + "####");
            MqttClient mClientCampus_2_A_Park1 = Mosquitto.connectMosquittoGuaranteeThatTryToConnect(ipsParkDACE);
            Console.WriteLine("#####################END-Connection##################");
            Console.WriteLine("####Connect to the topic " + topicsParkDACE[2] + "####\n");
            MqttClient mClientCampus_2_B_Park2 = Mosquitto.connectMosquittoGuaranteeThatTryToConnect(ipsParkDACE);
            Console.WriteLine("#####################END-Connection##################\n");

            String topicsSubscribe = "";

            /*if (mClientALL != null || !mClientALL.IsConnected)
            {
                Console.WriteLine("####Config the topic "+ topicsParkDACE[0] + "####");
                Mosquitto.configFunctionMosquitto(mClientALL, mClientALL_MqttMsgPublishReceived);
                topicsSubscribe += "\n\t\t\t-'"+topicsParkDACE[0]+"'";
                Console.WriteLine("#####################END-Config##################\n");
            }*/
            if (mClientCampus_2_A_Park1 != null || !mClientCampus_2_A_Park1.IsConnected)
            {
                Console.WriteLine("####Config the topic " + topicsParkDACE[0] + "####");
                Mosquitto.configFunctionMosquitto(mClientCampus_2_A_Park1, mClientCampus_2_A_Park1_MqttMsgPublishReceived);
                topicsSubscribe += "\n\t\t\t-'" + topicsParkDACE[1] + "'";
                Console.WriteLine("#####################END-Config##################\n");
            }
            if (mClientCampus_2_B_Park2 != null || !mClientCampus_2_B_Park2.IsConnected)
            {
                Console.WriteLine("####Config the topic " + topicsParkDACE[0] + "####");
                Mosquitto.configFunctionMosquitto(mClientCampus_2_B_Park2, mClientCampus_2_B_Park2_MqttMsgPublishReceived);
                topicsSubscribe += "\n\t\t\t-'" + topicsParkDACE[2] + "'";
                Console.WriteLine("#####################END-Config##################\n");
            }

            Console.WriteLine("You will be subscribe to the topic:" + topicsSubscribe);
            Console.WriteLine("#################END-Config-MOSQUITTO###############\n");
            Console.WriteLine("\n\n\n#################################################################################");
            Console.WriteLine("###########################   Starting Application   ############################");
            Console.WriteLine("#################################################################################\n\n");
            /*if (mClientALL != null || !mClientALL.IsConnected)
            {
                Mosquitto.subscribedMosquitto(mClientALL, topics[0]);
            }*/
            if (mClientCampus_2_A_Park1 != null || !mClientCampus_2_A_Park1.IsConnected)
            {
                Mosquitto.subscribedMosquitto(mClientCampus_2_A_Park1, topicsParkDACE[1]);
            }
            if (mClientCampus_2_B_Park2 != null || !mClientCampus_2_B_Park2.IsConnected)
            {
                Mosquitto.subscribedMosquitto(mClientCampus_2_B_Park2, topicsParkDACE[2]);
            }
        }

        public static void mClientALL_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) //Recebe tudo independentement que park pretence
        {
            Console.WriteLine("#################################################################################");
            Console.WriteLine("mClientALL-->> Topic: " + e.Topic);
            ParkingSpot parkingSpot = new ParkingSpot(Encoding.UTF8.GetString(e.Message));
            parkingSpot.writeOnScreen();

            /////// Send data to BD
        }

        public static void mClientCampus_2_A_Park1_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) //Recebe tudo do park Campus_2_A_Park1
        {
            Console.WriteLine("#################################################################################");
            Console.WriteLine("mClientCampus_2_A_Park1-->> Topic: " + e.Topic);
            ParkingSpot parkingSpot = new ParkingSpot(Encoding.UTF8.GetString(e.Message));
            parkingSpot.writeOnScreen();

            /////// Send data to BD
            string response = null;
            Console.WriteLine("#################################################################################");
            try
            {
                response = DatabaseHelper.newRegister(parkingSpot);
            }
            catch (Exception ex)
            {
                response = ex.Message;
            }
                
            Console.WriteLine("             " + response);
            Console.WriteLine("#################################################################################");

        }

        public static void mClientCampus_2_B_Park2_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)  //Recebe tudo do park Campus_2_B_Park2
        {
            Console.WriteLine("#################################################################################");
            Console.WriteLine("mClientCampus_2_B_Park2-->> Topic: " + e.Topic);
            ParkingSpot parkingSpot = new ParkingSpot(Encoding.UTF8.GetString(e.Message));
            parkingSpot.writeOnScreen();

            /////// Send data to BD
            string response = null;
            Console.WriteLine("#################################################################################");
            try
            {
                response = DatabaseHelper.newRegister(parkingSpot);
            }
            catch (ParkNotInsertedException ex)
            {
                response = ex.Message;
            }
            Console.WriteLine("             " + response);
            Console.WriteLine("#################################################################################");
        }

        
    }
}
