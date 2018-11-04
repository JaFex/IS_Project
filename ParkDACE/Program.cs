using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkDACE
{
    class Program
    {
        static void Main(string[] args)
        {
            ParkingSensorNodeDll.ParkingSensorNodeDll dll = new ParkingSensorNodeDll.ParkingSensorNodeDll();

            dll.Initialize(ComputeResponse, 10000);
        }

        public static void ComputeResponse(string str)
        {
            string[] strs = str.Split(';');

            string[] ids = { "ParkID", "SpotID", "Timestamp", "ParkingSpotStatus", "BatteryStatus" };

            Console.WriteLine("###################################################");

            for(int i = 0; i < strs.Length; i++)
            {
                Console.WriteLine(ids[i] + ": " + strs[i]);
            }

            Console.WriteLine("###################################################\n");

        }
    }
}
