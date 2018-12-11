using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using static uPLibrary.Networking.M2Mqtt.MqttClient;

namespace ParkDACE.classes
{
    class Mosquitto
    {
        public Mosquitto()
        {
        }

        public static MqttClient connectMosquitto(string[] ips)
        {
            MqttClient mClient = null;
            foreach (string ip in ips)
            {
                Console.Write("Try to connect to the IP: '" + ip + "'....");
                try
                {
                    mClient = new MqttClient(ip);
                    mClient.Connect(Guid.NewGuid().ToString());
                }
                catch (Exception e)
                {
                    mClient = null;
                }
                if (mClient != null && mClient.IsConnected)
                {
                    Console.WriteLine("Success to connect to the IP: '" + ip + "'!");
                    return mClient;
                }
                else
                {
                    Console.WriteLine("Fail to connect to the IP: '" + ip + "'!");
                }
            }
            return null;
        }

        public static MqttClient connectMosquittoGuaranteeThatTryToConnect(string[] ips)
        {
            MqttClient mClient = null;
            int count = 1;
            int timeToReconnect = 5000;
            do
            {
                mClient = connectMosquitto(ips);
                if (mClient == null || !mClient.IsConnected)
                {
                    if (count > 2)
                    {
                        Console.Write("\nReached the limit of attempts to connect! Do you want to try again? [Y/N]: ");
                        ConsoleKey response = Console.ReadKey(false).Key;
                        Console.WriteLine();
                        if (response == ConsoleKey.N)
                        {
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Try to reconnect in " + timeToReconnect / 1000 + " seconds");
                        Thread.Sleep(timeToReconnect);
                        count++;
                        timeToReconnect = timeToReconnect * 2;
                    }
                }
            } while (mClient == null || !mClient.IsConnected);
            return mClient;
        }

        public static void configFunctionMosquitto(MqttClient mClient, MqttMsgPublishEventHandler client_MqttMsgPublishReceived)
        {
            if (mClient != null && mClient.IsConnected)
            {
                mClient.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                mClient.MqttMsgSubscribed += client_MqttMsgSubscribed;
                mClient.MqttMsgUnsubscribed += client_MqttMsgUnsubscribed;
                Console.WriteLine("Config functions with success!");
            }
            else
            {
                Console.WriteLine("Mosquitto is not connected!");
            }
        }

        public static void unsubscribeAndDisconnectMosquitto(MqttClient mClient, string[] topics)
        {
            unsubscribeMosquitto(mClient, topics);
            disconnectMosquitto(mClient);
        }

        public static void disconnectMosquitto(MqttClient mClient)
        {
            if (mClient != null && mClient.IsConnected)
            {
                mClient.Disconnect();
                Console.WriteLine("Disconnected from mosquitto with success!");
            }
            else
            {
                Console.WriteLine("Mosquitto already not connected!");
            }
        }

        public static void unsubscribeMosquitto(MqttClient mClient, string[] topics)
        {
            if (mClient != null && mClient.IsConnected)
            {
                mClient.Unsubscribe(topics);
                Console.WriteLine("Disconnected from mosquitto with success!");
            }
            else
            {
                Console.WriteLine("Mosquitto is not connected!");
            }
        }

        public static void publishMosquitto(MqttClient mClient, string[] topics, string message)
        {
            if (mClient != null && mClient.IsConnected)
            {
                foreach (string topic in topics)
                {
                    mClient.Publish(topic, Encoding.UTF8.GetBytes(message));
                    Console.WriteLine("\t...information sent (topic: '" + topic + "', message_size: '" + message.Length + "')");
                }
            }
            else
            {
                Console.WriteLine("Mosquitto is not connected!");
            }
        }

        public static void subscribedMosquitto(MqttClient mClient, string topic)
        {
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
            mClient.Subscribe(new string[] { topic }, qosLevels);
        }

        public static void subscribedMosquitto(MqttClient mClient, string[] topics)
        {
            byte[] qosLevels = { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE };
            foreach (string topic in topics)
            {
                mClient.Subscribe(new string[] { topic }, qosLevels);
            }
        }

        private static void client_MqttMsgUnsubscribed(object sender, MqttMsgUnsubscribedEventArgs e)
        {
            Console.WriteLine("Unsubscribed topics with success");
        }

        private static void client_MqttMsgSubscribed(object sender, MqttMsgSubscribedEventArgs e)
        {
            Console.WriteLine("Subscribed topic with success");
        }

    }
}
