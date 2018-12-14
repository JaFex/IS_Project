using SmartPark.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Serialization;

namespace SmartPark.Controllers
{
    public class ParksController : ApiController
    {
        private string formatDateRecevedByUrl = "dd-MM-yyyy h_mm_ss tt";
        private string formatDateOfBD = "dd/MM/yyyy h:mm:ss tt";
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SmartPark.Properties.Settings.ConnStr"].ConnectionString;

        [Route("api/parks")]
        public IHttpActionResult GetAllParks()//ex1 http://localhost:51352/api/parks FEITOOO
        {
            string query = "SELECT * FROM Parks;";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Park> parks = new List<Park>();
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    parks.Add(new Park{
                        id = reader.GetString(0)
                    });
                }
                reader.Close();
                connection.Close();
                return Ok(parks);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "INTERNAL ERROR");
            }
        }
        [Route("api/parks/{park_id}/date/{str_date}")] 
        public IHttpActionResult GetParkWithSpotsForGivenMoment(string park_id, string str_date)//ex2 http://localhost:51352/api/parks/Campus_2_A_Park1/date/02-12-2018%206_28_22%20PM //FEITOOOOO
        {
            DateTime date;
            try
            {
                date = getDateFromString(str_date);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest,"ERROR PARSING DATE");
            }
            string query = "SELECT r.spot_id,r.status,m.MaxTime FROM" +
            " (SELECT spot_id, MAX(timestamp) as MaxTime FROM Registers WHERE park_id = @ParkID AND timestamp <= @Date GROUP BY spot_id) m" +
            " INNER JOIN Registers r" +
            " ON m.spot_id = r.spot_id AND r.timestamp = m.MaxTime" +
            " WHERE r.park_id = @ParkID ORDER BY r.timestamp DESC;";

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParkID", park_id);
                command.Parameters.AddWithValue("@Date", date);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Spot> spots = new List<Spot>();
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                    {
                        spots.Add(new Spot
                        {
                            id = reader.GetString(0),
                            status = reader.GetString(1),
                            parkID = park_id,
                            timestamp = reader.GetDateTime(2)
                        });
                    }
                }
                reader.Close();
                connection.Close();
                return Ok(
                    new Park
                    {
                        id = park_id,
                        spots = spots
                    }
                );
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "INTERNAL ERROR");
            }
        }


        [Route("api/parks/{park_id}/date/initial/{str_initial_date}/final/{str_final_date}")]
        public IHttpActionResult GetListStatusParkForPeriodMoment(string park_id, string str_initial_date, string str_final_date)//ex3 http://localhost:51352/api/parks/Campus_2_A_Park1/date/initial/02-12-2018%206_28_22%20PM/final/02-12-2018%206_55_22%20PM feitoooo
        {
            DateTime initialDate;
            DateTime finalDate;
            try
            {
                initialDate = getDateFromString(str_initial_date);
                finalDate = getDateFromString(str_final_date);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "ERROR PARSING DATE");
            }
            string query = "SELECT spot_id,status,timestamp FROM Registers WHERE park_id = @ParkID AND (timestamp BETWEEN @InitialDate AND @FinalDate);";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParkID", park_id);
                command.Parameters.AddWithValue("@InitialDate", initialDate);
                command.Parameters.AddWithValue("@FinalDate", finalDate);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Spot> spots = new List<Spot>();
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    if(!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                    {
                        spots.Add(new Spot{
                            id = reader.GetString(0),
                            status = reader.GetString(1),
                            parkID = park_id,
                            timestamp = reader.GetDateTime(2)
                        });
                    }
                }
                reader.Close();
                connection.Close();
                return Ok(
                    new Park
                    {
                        id = park_id,
                        spots = spots
                    }
                );
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "INTERNAL ERROR");
            }
        }
        
        

        [Route("api/parks/{park_id}/date/{str_date}/{str_state}")]
        public IHttpActionResult GetParkForGivenMomentInGivenState(string park_id, string str_date, string str_state)//ex4 http://localhost:51352/api/parks/Campus_2_A_Park1/date/02-12-2018%206_28_22%20PM/free
        {
            DateTime date;
            try
            {
                date = getDateFromString(str_date);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "ERROR PARSING DATE");
            }

            if(!str_state.ToUpper().Equals("FREE") && !str_state.ToUpper().Equals("OCCUPIED"))
            {
                return Content(HttpStatusCode.BadRequest, "ERROR STATE INVALID");
            }

            string query = "SELECT r.spot_id,r.status,m.MaxTime FROM" +
            " (SELECT spot_id, MAX(timestamp) as MaxTime FROM Registers WHERE park_id = @ParkID AND timestamp <= @Date GROUP BY spot_id) m" +
            " INNER JOIN Registers r " +
            "ON m.spot_id = r.spot_id AND r.timestamp = m.MaxTime " +
            "WHERE r.park_id = @ParkID AND UPPER(r.status) = @State " +
            "ORDER BY r.timestamp DESC;";

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParkID", park_id);
                command.Parameters.AddWithValue("@Date", date);
                command.Parameters.AddWithValue("@State", str_state.ToUpper());

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Park park = new Park();
                List<Spot> spots = new List<Spot>();
                Spot spot = new Spot();
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                    {
                        spots.Add(new Spot
                        {
                            id = reader.GetString(0),
                            status = reader.GetString(1),
                            parkID = park_id,
                            timestamp = reader.GetDateTime(2)
                        });
                    }
                }
                reader.Close();
                connection.Close();
                return Ok(
                    new Park
                    {
                        id = park_id,
                        spots = spots
                    }
                );
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);//"INTERNAL ERROR");
            }
        }
        
        [Route("api/parks/{park_id}/spots")]
        public IHttpActionResult GetPark(string park_id)//ex5 http://localhost:51352/api/parks/Campus_2_A_Park1/spots //feito
        {
            string query= "SELECT id,latitude,longitude,status,battery_status FROM Spots WHERE park_id = @ParkID ORDER BY status ASC, battery_status DESC;";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParkID", park_id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Spot> spots = new List<Spot>();
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2) && !reader.IsDBNull(3) && !reader.IsDBNull(4))
                    {
                        spots.Add(new Spot
                        {
                            id = reader.GetString(0),
                            latitude = reader.GetString(1),
                            longitude = reader.GetString(2),
                            status = reader.GetString(3),
                            batteryStatus = reader.GetString(4)
                        });
                    }
                }
                reader.Close();
                connection.Close();
                return Ok(
                    new Park
                    {
                        id = park_id,
                        spots = spots
                    }
                );
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "INTERNAL ERROR");
            }
        }

        [Route("api/parks/{park_id}/details")]
        public IHttpActionResult GetParkDescription(string park_id)//ex6  http://localhost:51352/api/parks/Campus_2_A_Park1/details /feito
        {
            string query = "SELECT id,number_spots,number_special_spots,description,operating_hours FROM Parks WHERE id = @ParkID;";
            try
            {
                //string str_return = " ";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParkID", park_id);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Park park = null;
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2) && !reader.IsDBNull(3) && !reader.IsDBNull(4))
                    {
                        park = new Park
                        {
                            id = reader.GetString(0),
                            numberOfSpots = Convert.ToInt32(reader.GetDecimal(1).ToString()),
                            numberOfSpecialSpots = Convert.ToInt32(reader.GetDecimal(2).ToString()),
                            description = reader.GetString(3),
                            operatingHours = reader.GetString(4)
                        };
                    }
                }
                if(park == null)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                reader.Close();
                connection.Close();
                return Ok(park);
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
                return Content(HttpStatusCode.InternalServerError, "INTERNAL ERROR");
            }
        }
        
        [Route("api/spots/{spot_id}/details/date/{str_date}")]
        public IHttpActionResult GetSpotForGivenMoment(string spot_id, string str_date)//ex7 http://localhost:51352/api/spots/A-8/details/date/02-12-2018%206_28_22%20PM FEITO
        {
            DateTime date;//
            try
            {
                date = getDateFromString(str_date);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, "ERROR PARSING DATE");
            }

            string query = "SELECT s.id,s.latitude,s.longitude,s.park_id,b.battery_status,@Date,n.status FROM Spots s" +
            " JOIN (SELECT TOP 1 r.status,r.spot_id FROM Registers r WHERE r.spot_id = @SpotID AND r.timestamp <= @Date ORDER BY r.timestamp DESC) as n on s.id = n.spot_id" +
            " JOIN (SELECT TOP 1 b.status as battery_status,b.spot_id FROM BatteryRegisters b WHERE b.spot_id = @SpotID AND b.timestamp <= @Date ORDER BY b.timestamp DESC) as b on s.id = b.spot_id;";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SpotID", spot_id);
                command.Parameters.AddWithValue("@Date", date);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Spot spot = null;
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2) && !reader.IsDBNull(3) && !reader.IsDBNull(4) && !reader.IsDBNull(5) && !reader.IsDBNull(6))
                    {
                        spot = new Spot
                        {
                            id = reader.GetString(0),
                            latitude = reader.GetString(1),
                            longitude = reader.GetString(2),
                            parkID = reader.GetString(3),
                            batteryStatus = reader.GetString(4),
                            timestamp = reader.GetDateTime(5),
                            status = reader.GetString(6)
                        };
                    }
                }
                if (spot == null)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                reader.Close();
                connection.Close();
                return Ok(spot);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, ex.Message);// "INTERNAL ERROR");
            }
        }

        [Route("api/spots/sensors/{str_battery_status}")]
        public IHttpActionResult GetSpotsForGivenBatteryState(String str_battery_status)//ex8 http://localhost:51352/api/spots/sensors/low
        {
            if (!str_battery_status.ToUpper().Equals("LOW") && !str_battery_status.ToUpper().Equals("GOOD"))
            {
                return Content(HttpStatusCode.BadRequest, "ERROR BATTERY STATE INVALID");
            }
            string query = "SELECT id,battery_status,park_id FROM Spots WHERE UPPER(battery_status) = @BatteryStatus";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BatteryStatus", str_battery_status.ToUpper());

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Spot> spots = new List<Spot>();
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                    {
                        spots.Add(new Spot
                        {
                            id = reader.GetString(0),
                            batteryStatus = reader.GetString(1),
                            parkID = reader.GetString(2)
                        });
                    }
                }
                if (spots.Count == 0)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                reader.Close();
                connection.Close();
                return Ok(spots);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "INTERNAL ERROR");
            }
        }

        [Route("api/parks/{park_id}/sensors/{str_battery_status}")]
        public IHttpActionResult GetSpotsForGivenBatteryOnGivenPark(string park_id, string str_battery_status)//ex9 http://localhost:51352/api/parks/Campus_2_A_Park1/sensors/low
        {
            if (!str_battery_status.ToUpper().Equals("LOW") && !str_battery_status.ToUpper().Equals("GOOD"))
            {
                return Content(HttpStatusCode.BadRequest, "ERROR BATTERY STATE INVALID");
            }
            string query = "SELECT id,battery_status,park_id from Spots WHERE UPPER(battery_status) = @BatteryStatus AND Spots.park_id=@ParkID";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ParkID", park_id);
                command.Parameters.AddWithValue("@BatteryStatus", str_battery_status.ToUpper());

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Spot> spots = new List<Spot>();
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                    {
                        spots.Add(new Spot
                        {
                            id = reader.GetString(0),
                            batteryStatus = reader.GetString(1),
                            parkID = reader.GetString(2)
                        });
                    }
                }
                if (spots.Count == 0)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                reader.Close();
                connection.Close();
                return Ok(spots);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "INTERNAL ERROR");
            }
        }

        [Route("api/parks/{park_id}/rate/{str_state}")]
        public IHttpActionResult GetRateOfStateOnGivenPark(string park_id, string str_state)//ex10 http://localhost:51352/api/parks/Campus_2_A_Park1/rate/occupied
        {
            if (!str_state.ToUpper().Equals("FREE") && !str_state.ToUpper().Equals("OCCUPIED"))
            {
                return Content(HttpStatusCode.BadRequest, "ERROR STATE INVALID");
            }
            string queryTotal = "SELECT count(*) FROM Spots WHERE park_id = @ParkID";
            string queryTotalState = "SELECT count(*) FROM Spots WHERE park_id = @ParkID AND UPPER(status) = UPPER(@State)";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(queryTotal, connection);
                command.Parameters.AddWithValue("@ParkID", park_id);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                float totalSpots = 0;
                float totalStateSpots = 0;
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    totalSpots = reader.GetInt32(0);
                }
                reader.Close();
                command = new SqlCommand(queryTotalState, connection);
                command.Parameters.AddWithValue("@ParkID", park_id);
                command.Parameters.AddWithValue("@State", str_state);
                reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    reader.Close();
                    connection.Close();
                    return NotFound();
                }
                while (reader.Read())
                {
                    totalStateSpots = reader.GetInt32(0);
                }
                float rate = ((totalStateSpots * 100 )/ totalSpots);
                reader.Close();
                connection.Close();
                return Ok(rate);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.InternalServerError, "INTERNAL ERROR");
            }
        }

        private DateTime getDateFromString(string stringDate)
        {
            return DateTime.ParseExact(stringDate, formatDateRecevedByUrl, System.Globalization.CultureInfo.InvariantCulture);
        }

        private string getStringFromDate(DateTime date)
        {
            return Convert.ToDateTime(date).ToString(formatDateOfBD);
        }

        private string getDateOfFormateOfDB(string stringDate)
        {
            return getStringFromDate(getDateFromString(stringDate));
        }
    }
}
