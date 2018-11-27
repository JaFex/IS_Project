using ParkSS.classes;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ParkSS
{
    class Program
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ParkSS.Properties.Settings.connStr"].ConnectionString;
        private static string[] ips = new string[] { "broker.hivemq.com", "127.0.0.1" };
        private static string[] topics = new string[] { "ParkDACE\\all", "ParkDACE\\Campus_2_A_Park1", "ParkDACE\\Campus_2_B_Park2" };

        static void Main(string[] args)
        {
            //MqttClient mClientALL = Mosquitto.connectMosquitto(ips);
            //Mosquitto.configFunctionMosquitto(mClientALL, mClientALL_MqttMsgPublishReceived);
            
            MqttClient mClientCampus_2_A_Park1 = Mosquitto.connectMosquitto(ips);
            Mosquitto.configFunctionMosquitto(mClientCampus_2_A_Park1, mClientCampus_2_A_Park1_MqttMsgPublishReceived);
            
            MqttClient mClientCampus_2_B_Park2 = Mosquitto.connectMosquitto(ips);
            Mosquitto.configFunctionMosquitto(mClientCampus_2_B_Park2, mClientCampus_2_B_Park2_MqttMsgPublishReceived);

            //Mosquitto.subscribedMosquitto(mClientALL, topics[0]);
            Mosquitto.subscribedMosquitto(mClientCampus_2_A_Park1, topics[1]);
            Mosquitto.subscribedMosquitto(mClientCampus_2_B_Park2, topics[2]);
        }

        /*public static void mClientALL_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) //Recebe tudo independentement que park pretence
        {
            Console.WriteLine("#################################################################################");
            Console.WriteLine("mClientALL-->> Topic: " + e.Topic);
            ParkingSpot parkingSpot = new ParkingSpot(Encoding.UTF8.GetString(e.Message));
            parkingSpot.writeOnScreen();

            /////// Send data to BD
        }*/

        public static void mClientCampus_2_A_Park1_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e) //Recebe tudo do park Campus_2_A_Park1
        {
            Console.WriteLine("#################################################################################");
            Console.WriteLine("mClientCampus_2_A_Park1-->> Topic: " + e.Topic);
            ParkingSpot parkingSpot = new ParkingSpot(Encoding.UTF8.GetString(e.Message));
            parkingSpot.writeOnScreen();

            /////// Send data to BD

            int spotId = DatabaseHelper.getSpotId(parkingSpot.name);
            int parkId = DatabaseHelper.getParkIdOfSpot(parkingSpot.name);

            string result = "";

            //Spot existe logo o parque tambem existe
            //Regista-se a alteração do spot
            if (spotId != 0)
            {
                result = DatabaseHelper.insertRegister(spotId, parkId, parkingSpot.value, int.Parse(parkingSpot.batteryStatus), parkingSpot.timestamp);
            } else
            {
                result = "********************** Spot Not Found **************************";
            }

            Console.WriteLine(result);
        }

        public static void mClientCampus_2_B_Park2_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)  //Recebe tudo do park Campus_2_B_Park2
        {
            Console.WriteLine("#################################################################################");
            Console.WriteLine("mClientCampus_2_B_Park2-->> Topic: " + e.Topic);
            ParkingSpot parkingSpot = new ParkingSpot(Encoding.UTF8.GetString(e.Message));
            parkingSpot.writeOnScreen();

            /////// Send data to BD
        }

        
    }
}
