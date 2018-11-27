using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkSS.classes
{
    class DatabaseHelper
    {

        private static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ParkSS.Properties.Settings.connStr"].ConnectionString;

        public static int getSpotId(string spotName)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT Id FROM Spots WHERE name = @spotName", conn);
            cmd.Parameters.AddWithValue("@spotName", spotName);

            SqlDataReader reader = cmd.ExecuteReader();

            int spot_id = 0;

            if (reader.Read())
            {
                spot_id = (int)reader["Id"];
            }
            reader.Close();
            conn.Close();

            return spot_id; 
        }

        public static int getParkIdOfSpot(string spotName)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            SqlCommand cmd = new SqlCommand("SELECT park_id FROM Spots WHERE name = @spotName", conn);
            cmd.Parameters.AddWithValue("@spotName", spotName);

            SqlDataReader reader = cmd.ExecuteReader();

            int parkId = 0;

            if (reader.Read())
            {
                parkId = (int)reader["park_id"];
            }
            reader.Close();
            conn.Close();

            return parkId;
        }

        private static int updateSpotStatus(int spotId, string status)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE Spots SET status = @newStatus WHERE Id = @spotId", conn);
            cmd.Parameters.AddWithValue("newStatus", status);
            cmd.Parameters.AddWithValue("spotId", spotId);

            int value = cmd.ExecuteNonQuery();

            conn.Close();

            return value;
        }

        private static int updateSpotBatteryStatus(int spotId, string batteryStatus)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            SqlCommand cmd = new SqlCommand("UPDATE Spots SET battery_status = @newStatus WHERE Id = @spotId", conn);
            cmd.Parameters.AddWithValue("newStatus", batteryStatus);
            cmd.Parameters.AddWithValue("spotId", spotId);

            int value = cmd.ExecuteNonQuery();

            conn.Close();

            return value;
        }

        public static string insertRegister(int spotId, int parkId, string status, int batteryStatus, DateTime timestamp)
        {
            SqlConnection conn = new SqlConnection(connectionString);

            conn.Open();

            string batStatusStr = batteryStatus == 1 ? "Good" : "Low";

            SqlCommand cmd = new SqlCommand("INSERT INTO dbo.registers (spot_id, park_id, status, timestamp) VALUES (@spot_id, @park_id, @status, @timestamp)", conn);
            cmd.Parameters.AddWithValue("@spot_id", spotId);
            cmd.Parameters.AddWithValue("@park_id", parkId);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@timestamp", timestamp);

            int nReg = cmd.ExecuteNonQuery();
            conn.Close();

            conn.Open();

            cmd = new SqlCommand("INSERT INTO batteryregisters (spot_id, battery_status, timestamp) VALUES (@spot_id, @batteryStatus, @timestamp)", conn);
            cmd.Parameters.AddWithValue("@spot_id", spotId);
            cmd.Parameters.AddWithValue("@batteryStatus", batStatusStr);
            cmd.Parameters.AddWithValue("@timestamp", timestamp);

            cmd.ExecuteNonQuery();
            conn.Close();

            int vUpdate = updateSpotStatus(spotId, status);
            int bsUpdate = updateSpotBatteryStatus(spotId, batStatusStr);

            if (nReg > 0 && vUpdate > 0)
            {
                return "*******************************INSERTED****************************";
            }
            else
            {
                return "ERROR: Registering spot data";
            }
       
        }
    }
}
