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
        private static SqlConnection conn = new SqlConnection(connectionString);

        public static string newRegister(ParkingSpot parkingSpot) {

            //Verificar se o spot existe
            //Se não
            //Park existe
            //Se não
            //Criar Park
            //Criar novo spot
            //inserir novo registo
            //inserir novo registo battery

            string response = "INSERTED";

            conn.Open();

            if (!spotExists(parkingSpot.name))
            {
                if (!parkExists(parkingSpot.id))
                {
                    int i = insertNewPark(parkingSpot.id);
                }

                insertNewSpot(parkingSpot.id, parkingSpot.name, parkingSpot.latitude, parkingSpot.longitude, parkingSpot.value, parkingSpot.batteryStatus);
            }
            //Este insert so deve acontecer se o estado for diferente do anterior
            insertNewSpotRegister(parkingSpot.id, parkingSpot.name, parkingSpot.value, parkingSpot.timestamp);
            //Este insert so deve acontecer se o estado for diferente do anterior
            insertNewBatteryRegister(parkingSpot.name, parkingSpot.batteryStatus, parkingSpot.timestamp);

            conn.Close();

            return response;
        }

        private static int insertNewBatteryRegister(string name, string batteryStatus, DateTime timestamp)
        {
            string batStatusStr = batteryStatus.Equals("1") ? "Good" : "Low";
            SqlCommand cmd = new SqlCommand("INSERT INTO BatteryRegisters (spot_id, status, timestamp) VALUES (@spotId, @status, @timestamp); UPDATE Spots SET battery_status = @status WHERE id = @spotId", conn);
            cmd.Parameters.AddWithValue("@spotId", name);
            cmd.Parameters.AddWithValue("@status", batStatusStr);
            cmd.Parameters.AddWithValue("@timestamp", timestamp);
            return cmd.ExecuteNonQuery();
        }

        private static int insertNewSpotRegister(string id, string name, string value, DateTime timestamp)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Registers (spot_id, park_id, status, timestamp) VALUES (@spotId, @parkId, @status, @timestamp); UPDATE Spots SET status = @status WHERE id = @spotId", conn);
            cmd.Parameters.AddWithValue("@spotId", name);
            cmd.Parameters.AddWithValue("@parkId", id);
            cmd.Parameters.AddWithValue("@status", value);
            cmd.Parameters.AddWithValue("@timestamp", timestamp);
            return cmd.ExecuteNonQuery();
        }

        private static int insertNewSpot(string id, string name, string latitude, string longitude, string value, string batteryStatus)
        {
            string batStatusStr = batteryStatus.Equals("1") ? "Good" : "Low";
            SqlCommand cmd = new SqlCommand("INSERT INTO Spots (id, latitude, longitude, park_id) VALUES (@id, @latitude, @longitude, @parkId)", conn);
            cmd.Parameters.AddWithValue("@id", name);
            cmd.Parameters.AddWithValue("@latitude", latitude);
            cmd.Parameters.AddWithValue("@longitude", longitude);
            cmd.Parameters.AddWithValue("@parkId", id);
            return cmd.ExecuteNonQuery();
        }

        private static int insertNewPark(string id)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Parks (id) VALUES (@id)", conn);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery();
        }

        private static Boolean spotExists(string spotName)
        {
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Spots WHERE id = @spotName", conn);
            cmd.Parameters.AddWithValue("@spotName", spotName);
            SqlDataReader reader = cmd.ExecuteReader();
            bool response = false;
            if (reader.Read())
            {
                response = true;
            }
            reader.Close();
            return response;
        }

        private static Boolean parkExists(string parkName)
        {
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Parks WHERE id = @parkName", conn);
            cmd.Parameters.AddWithValue("@parkName", parkName);
            SqlDataReader reader = cmd.ExecuteReader();
            bool response = false;
            if (reader.Read())
            {
                response = true;
            }
            reader.Close();
            return response;
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
