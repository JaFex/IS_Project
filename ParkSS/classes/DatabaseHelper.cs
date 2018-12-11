using ParkDACE.classes;
using ParkSS.exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParkSS.classes
{
    class DatabaseHelper
    {

        private static DirectoryInfo dir = new DirectoryInfo("../../../SmartPark/App_Data");
        private static string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=" + dir.FullName +"\\ParkDB.mdf;Integrated Security = True";

        public static string newRegister(ParkingSpot parkingSpot)
        {
            
            string response = "";

            if (!spotExists(parkingSpot.name))
            {
                int i = insertNewSpot(parkingSpot.id, parkingSpot.name, parkingSpot.latitude, parkingSpot.longitude, parkingSpot.value, parkingSpot.batteryStatus);

                if(i < 1)
                {
                    throw new SpotNotInsertedException("ERROR: Spot could not be inserted");
                }

                response += "\t\tNew spot inserted\n";
            } else
            {
                Thread thread = new Thread(() => updateSpot(parkingSpot));
                thread.Start();
            }
            //Este insert so deve acontecer se o estado for diferente do anterior
            if(!spotValueEquals(parkingSpot.name, parkingSpot.value))
            {
                int i = insertNewSpotRegister(parkingSpot.id, parkingSpot.name, parkingSpot.value, parkingSpot.timestamp);

                if(i < 1)
                {
                    throw new RegisterNotInsertedException("ERROR: Register of spot status could not be inserted");
                }

                response += "\t\tNew register inserted";
            } else
            {
                response += "\t\tDuplicate register";
            }

            //Este insert so deve acontecer se o estado for diferente do anterior
            if (!spotBatteryValueEquals(parkingSpot.name, parkingSpot.batteryStatus))
            {
                int i = insertNewBatteryRegister(parkingSpot.name, parkingSpot.batteryStatus, parkingSpot.timestamp);

                if (i < 1)
                {
                    throw new RegisterNotInsertedException("ERROR: Register of spot battery status could not be inserted");
                }
            }

            return response;
        }

        public static string newParking(ParkingInformation park)
        {
            String response = "";
            try
            {
                if (!parkExists(park.id))
                {
                    response = insertNewPark(park);
                }
                else
                {
                    Thread thread = new Thread(() => updatePark(park));
                    thread.Start();
                    response = "UPDATED";
                }
            } catch(Exception ex)
            {
                throw new ParkNotInsertedException(ex.Message);
            }
            return response;
        }

        private static bool spotBatteryValueEquals(string spotName, string batteryStatus)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string batStatusStr = batteryStatus.Equals("1") ? "Good" : "Low";
            SqlCommand cmd = new SqlCommand("SELECT battery_status FROM Spots WHERE id = @spotName", conn);
            cmd.Parameters.AddWithValue("@spotName", spotName);
            SqlDataReader reader = cmd.ExecuteReader();
            bool response = false;
            if (reader.Read())
            {
                if (batStatusStr.Equals(reader["battery_status"]))
                {
                    response = true;
                }
            }
            reader.Close();
            conn.Close();
            return response;
        }

        private static bool spotValueEquals(string spotName, string status)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT status FROM Spots WHERE id = @spotName", conn);
            cmd.Parameters.AddWithValue("@spotName", spotName);
            SqlDataReader reader = cmd.ExecuteReader();
            bool response = false;
            if (reader.Read())
            {
                if (status.Equals(reader["status"]))
                {
                    response = true;
                }
            }
            reader.Close();
            conn.Close();
            return response;
        }

        private static int insertNewBatteryRegister(string name, string batteryStatus, DateTime timestamp)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string batStatusStr = batteryStatus.Equals("1") ? "Good" : "Low";
            SqlCommand cmd = new SqlCommand("INSERT INTO BatteryRegisters (spot_id, status, timestamp) VALUES (@spotId, @status, @timestamp); UPDATE Spots SET battery_status = @status WHERE id = @spotId", conn);
            cmd.Parameters.AddWithValue("@spotId", name);
            cmd.Parameters.AddWithValue("@status", batStatusStr);
            cmd.Parameters.AddWithValue("@timestamp", timestamp);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            return i;
        }

        private static int insertNewSpotRegister(string id, string name, string value, DateTime timestamp)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Registers (spot_id, park_id, status, timestamp) VALUES (@spotId, @parkId, @status, @timestamp); UPDATE Spots SET status = @status WHERE id = @spotId", conn);
            cmd.Parameters.AddWithValue("@spotId", name);
            cmd.Parameters.AddWithValue("@parkId", id);
            cmd.Parameters.AddWithValue("@status", value);
            cmd.Parameters.AddWithValue("@timestamp", timestamp);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            return i;
        }

        private static int insertNewSpot(string id, string name, string latitude, string longitude, string value, string batteryStatus)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            string batStatusStr = batteryStatus.Equals("1") ? "Good" : "Low";
            SqlCommand cmd = new SqlCommand("INSERT INTO Spots (id, latitude, longitude, park_id) VALUES (@id, @latitude, @longitude, @parkId)", conn);
            cmd.Parameters.AddWithValue("@id", name);
            cmd.Parameters.AddWithValue("@latitude", latitude);
            cmd.Parameters.AddWithValue("@longitude", longitude);
            cmd.Parameters.AddWithValue("@parkId", id);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
            return i;
        }

        private static void updateSpot(ParkingSpot spot)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("Select latitude, longitude From Spots Where id = @id", conn);
            sql.Parameters.AddWithValue("@id", spot.name);
            SqlDataReader reader = sql.ExecuteReader();
            if (reader.Read())
            {
                if (spot.latitude.Equals(reader["latitude"]) && spot.longitude.Equals(reader["longitude"]))
                {
                    reader.Close();
                    conn.Close();
                    return;
                }
            }
            reader.Close();
            SqlCommand cmd = new SqlCommand("UPDATE Spots SET latitude = @latitude, longitude = @longitude  WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@latitude", spot.latitude);
            cmd.Parameters.AddWithValue("@longitude", spot.longitude);
            cmd.Parameters.AddWithValue("@id", spot.name);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
        }

        private static string insertNewPark(ParkingInformation park)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO Parks (id, description, operating_hours, number_spots, number_special_spots) VALUES (@id, @description, @operatingHours, @numberSpots, @numberSpecialSpots)", conn);
            cmd.Parameters.AddWithValue("@id", park.id);
            cmd.Parameters.AddWithValue("@description", park.description);
            cmd.Parameters.AddWithValue("@operatingHours", park.operatingHours);
            cmd.Parameters.AddWithValue("@numberSpots", park.numberOfSpots);
            cmd.Parameters.AddWithValue("@numberSpecialSpots", park.numberOfSpecialSpots);
            int i = cmd.ExecuteNonQuery();
            conn.Close();

            if(i < 1)
            {
                throw new ParkNotInsertedException("ERROR: Park could not be inserted");
            }

            return "INSERTED";
        }

        private static void updatePark(ParkingInformation park)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand sql = new SqlCommand("Select description, operating_hours, number_spots, number_special_spots From Parks Where id = @id", conn);
            sql.Parameters.AddWithValue("@id", park.id);
            SqlDataReader reader = sql.ExecuteReader();
            if (reader.Read())
            {
                if(park.description.Equals(reader["description"]) && park.operatingHours.Equals(reader["operating_hours"]) && park.numberOfSpots == Convert.ToInt32(reader["number_spots"]) && park.numberOfSpecialSpots == Convert.ToInt32(reader["number_special_spots"]))
                {
                    reader.Close();
                    conn.Close();
                    return;
                }
            }
            reader.Close();
            SqlCommand cmd = new SqlCommand("UPDATE Parks SET description = @description, operating_hours = @operatingHours, number_spots = @numberSpots, number_special_spots = @numberSpecialSpots WHERE Id = @id", conn);
            cmd.Parameters.AddWithValue("@description", park.description);
            cmd.Parameters.AddWithValue("@operatingHours", park.operatingHours);
            cmd.Parameters.AddWithValue("@numberSpots", park.numberOfSpots);
            cmd.Parameters.AddWithValue("@numberSpecialSpots", park.numberOfSpecialSpots);
            cmd.Parameters.AddWithValue("@id", park.id);
            int i = cmd.ExecuteNonQuery();
            conn.Close();
        }

        private static Boolean spotExists(string spotName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Spots WHERE id = @spotName", conn);
            cmd.Parameters.AddWithValue("@spotName", spotName);
            SqlDataReader reader = cmd.ExecuteReader();
            bool response = false;
            if (reader.Read())
            {
                response = true;
            }
            reader.Close();
            conn.Close();
            return response;
        }

        private static Boolean parkExists(string parkName)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            conn.Open();
            SqlCommand cmd = new SqlCommand("SELECT Id FROM Parks WHERE id = @parkName", conn);
            cmd.Parameters.AddWithValue("@parkName", parkName);
            SqlDataReader reader = cmd.ExecuteReader();
            bool response = false;
            if (reader.Read())
            {
                response = true;
            }
            reader.Close();
            conn.Close();
            return response;
        }


        //DEPRECATED
        /*private static int insertNewPark(string id)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO Parks (id) VALUES (@id)", conn);
            cmd.Parameters.AddWithValue("@id", id);
            return cmd.ExecuteNonQuery();
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
       
        }*/
    }
}
