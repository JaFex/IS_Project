using SmartPark.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SmartPark.Controllers
{
    public class ParksController : ApiController
    {
        private string formatDateRecevedByUrl = "dd-MM-yyyy h_mm_ss tt";
        private string formatDateOfBD = "dd/MM/yyyy";
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SmartPark.Properties.Settings.ConnStr"].ConnectionString;
        /*List<Park> parks = new List<Park>
         {
             new Park{ Id = 1, Available=1,ParkingSpots = new List<ParkingSpot>{
                 new ParkingSpot{ IdPark=1,Id=1,Available=1 },
                 new ParkingSpot{ IdPark=1,Id=2,Available=1 },
                 new ParkingSpot{ IdPark=1,Id=3,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=4,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=5,Available=0 },
             } ,Description= "PARQUE DO IPL"},
             new Park{ Id = 2, Available=0,ParkingSpots = new List<ParkingSpot>{
                 new ParkingSpot{ IdPark=1,Id=1,Available=1 },
                 new ParkingSpot{ IdPark=1,Id=2,Available=1 },
                 new ParkingSpot{ IdPark=1,Id=3,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=4,Available=1 },
                 new ParkingSpot{ IdPark=1,Id=5,Available=0 },
             } ,Description= "PARQUE DO ESTG"},
             new Park{ Id = 3, Available=1,ParkingSpots = new List<ParkingSpot>{
                 new ParkingSpot{ IdPark=1,Id=1,Available=1 },
                 new ParkingSpot{ IdPark=1,Id=2,Available=1 },
                 new ParkingSpot{ IdPark=1,Id=3,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=4,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=5,Available=1 },
             } ,Description= "PARQUE DO LEIRIA"},
             new Park{ Id = 4, Available=0,ParkingSpots = new List<ParkingSpot>{
                 new ParkingSpot{ IdPark=1,Id=1,Available=1 },
                 new ParkingSpot{ IdPark=1,Id=2,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=3,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=4,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=5,Available=0 },
             } ,Description= "PARQUE DO Asdasdas"},
             new Park{ Id = 5, Available=1,ParkingSpots = new List<ParkingSpot>{
                 new ParkingSpot{ IdPark=1,Id=1,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=2,Available=1 },
                 new ParkingSpot{ IdPark=1,Id=3,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=4,Available=0 },
                 new ParkingSpot{ IdPark=1,Id=5,Available=0 },
             } ,Description= "PARQUE DO IPsdSAddsadawerqL"}
         };
         */
        //List<Park> parks = null;
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
                if (!reader.HasRows) { return NotFound(); }
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
                return NotFound();
            }
        }
        [Route("api/parks/{str_id}/date/{str_date}")] 
        public IHttpActionResult GetSpotForGivenMoment(string str_id, string str_date)//ex2 http://localhost:51352/api/parks/Campus_2_A_Park1/date/11-27-2018%206_04_01%20PM //FEITOOOOO
        {
            DateTime date;
            try
            {
                date = getDateFromString(str_date);
            }
            catch (Exception ex)
            {
                //return Content(HttpStatusCode.BadRequest,ex.ToString());
                return Content(HttpStatusCode.BadRequest,"ERROR PARSING DATE");
            }
            string query = "SELECT Registers.spot_id ,Registers.status ,Registers.timestamp " +
            "FROM Registers WHERE Registers.park_id = '" + str_id + "'AND Registers.timestamp = '" + date + "';";

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Park park = new Park();
                List<Spot> spots = new List<Spot>();
                Spot spot = new Spot();
                string aux =str_id;
                if (!reader.HasRows) return NotFound();
                while (reader.Read())
                {
                    spot.id = reader.GetString(0);
                    if (!reader.IsDBNull(1)) spot.status = reader.GetString(1);
                    if (!reader.IsDBNull(2)) spot.timestamp = reader.GetDateTime(2);
                    spots.Add(spot);
                }
                park.id = aux;
                park.spots = spots;
                reader.Close();
                return Ok(park);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }


        [Route("api/parks/{str_id}/date1/{str_initial_date}/date2/{str_final_date}")]
        public IHttpActionResult GetListStatusParkForPeriodMoment(string str_id,string str_initial_date, string str_final_date)//ex 3 http://localhost:51352/api/parks/Campus_2_A_Park1/date1/11-27-2018%206_04_01%20PM/date2/11-27-2018%206_20_33%20PM feitoooo
        {
            string initialDateString;
            string finalDateString;
            try
            {
                initialDateString = getDateOfFormateOfDB(str_initial_date);
                finalDateString = getDateOfFormateOfDB(str_final_date);
            }
            catch (Exception ex)
            {
                //return Content(HttpStatusCode.BadRequest,ex.ToString());
                return Content(HttpStatusCode.BadRequest, "ERROR PARSING DATE");
            }
            string query = "SELECT spot_id,status,timestamp " +
            "FROM Registers WHERE park_id = '" + str_id + "';";//' AND timestamp >='" + initialDateString + "' AND timestamp <= '" + finalDateString + "';";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Spot> spots = new List<Spot>();
                if (!reader.HasRows) return NotFound();
                while (reader.Read())
                {
                    if(!reader.IsDBNull(0) && !reader.IsDBNull(1) && !reader.IsDBNull(2))
                    {
                        spots.Add(new Spot{
                            id = reader.GetString(0),
                            status = reader.GetString(1),
                            timestamp = reader.GetDateTime(2)
                        });
                    }
                }
                reader.Close();
                Park park = new Park {
                    id = str_id,
                    spots = spots
                };
                return Ok(park);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }
        
        

        [Route("api/parks/{str_id}/date/{str_date}/free")]
        public IHttpActionResult GetFreeParkForGivenMoment(string str_id, string str_date)//ex4 http://localhost:51352/api/parks/Campus_2_A_Park1/date/11-27-2018%206_16_31%20PM
        {
            DateTime date;
            try
            {
                date = getDateFromString(str_date);
            }
            catch (Exception ex)
            {
                //return Content(HttpStatusCode.BadRequest,ex.ToString());
                return Content(HttpStatusCode.BadRequest, "ERROR PARSING DATE");
            }
            string query = "select Registers.spot_id ,Registers.status ,Registers.timestamp " +
            "from Registers where Registers.park_id = '" + str_id + "'AND Registers.timestamp = '" + date + "' AND Registers.status = 'free';";

            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Park park = new Park();
                List<Spot> spots = new List<Spot>();
                Spot spot = new Spot();
                string aux = str_id;
                if (!reader.HasRows) return NotFound();
                while (reader.Read())
                {
                    spot.id = reader.GetString(0);
                    if (!reader.IsDBNull(1)) spot.status = reader.GetString(1);
                    if (!reader.IsDBNull(2)) spot.timestamp = reader.GetDateTime(2);
                    spots.Add(spot);
                }
                park.id = aux;
                park.spots = spots;
                reader.Close();
                return Ok(park);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }
        
        [Route("api/parks/{str_id}/spots")]
        public IHttpActionResult GetPark(string str_id)//ex5 http://localhost:51352/api/parks/Campus_2_A_Park1/spots //feito
        {
            string query="select Distinct Registers.park_id, Registers.spot_id"+
             " from Registers where Registers.park_id = '"+str_id+"';";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Spot> spots = new List<Spot>();
                Park park = new Park();
                Spot spot;
                if (!reader.HasRows) return NotFound();
                while (reader.Read())
                {
                    if (park.id == null) {
                        park.id = reader.GetString(0);
                    }
                    spot = new Spot();
                    spot.id = reader.GetString(1);
                    spots.Add(spot);
                }
                park.spots = spots;
                reader.Close();
                return Ok(park);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }

        [Route("api/parks/{str_id}/details")]
        public IHttpActionResult GetParkDescription(string str_id)//ex6  http://localhost:51352/api/parks/Campus_2_A_Park1/details /feito
        {
            string query = "SELECT * from Parks where Parks.Id= '"+ str_id+"';";
            try
            {
                //string str_return = " ";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Park park = new Park();
                //List<Park> parks = new List<Park>();
                if (!reader.HasRows) return NotFound();
                while (reader.Read())
                {
                    park.id = reader.GetString(0);
                    if (!reader.IsDBNull(1)) park.number_spot = reader.GetInt32(1);
                    if (!reader.IsDBNull(2)) park.number_special_spot = reader.GetInt32(2);
                    if (!reader.IsDBNull(3)) park.description = reader.GetString(3); ;
                    if (!reader.IsDBNull(4)) park.operating_hours = reader.GetString(4);
                    //parks.Add(park);
                }
                reader.Close();
                return Ok(park);
            }
            catch (Exception ex)
            {
                return NotFound();
            }
        }
        
        [Route("api/parks/{str_id}/details/{str_date}/date")]
        public IHttpActionResult GetPark(string str_id,string str_date)//ex7 http://localhost:51352/api/parks/A-8/details/11-27-2018%206_07_31%20PM/date FEITO
        {
            DateTime date;//
            try
            {
                date = getDateFromString(str_date);
            }
            catch (Exception ex)
            {
                //return Content(HttpStatusCode.BadRequest,ex.ToString());
                return Content(HttpStatusCode.BadRequest, "ERROR PARSING DATE");
            }
            //string query = "select * from Registers where Registers.spot_id = '"+str_id+"' and Registers.timestamp =  '" + date+"'; ";
            /*string query = "select Spots.Id , Spots.latitude, Spots.longitude, Spots.status, Spots.battery_status, Registers.park_id ,Registers.timestamp " +
            "from Spots join Registers on Spots.Id = Registers.spot_id where Spots.Id = '" + str_id + "' and Registers.timestamp = '" + date + "'; ";*/
            string query= " select newtab.spot_id ,Spots.latitude ,Spots.longitude ,Spots.status ,Spots.battery_status,Spots.park_id,newtab.timestamp from Spots join "+
            "(select TOP 1 Registers.park_id , Registers.spot_id , Registers.timestamp "+
            "from Registers where Registers.spot_id = '"+str_id+"' and Registers.timestamp <= '"+date+"' order by Registers.timestamp DEsc) as newtab "+
            "on Spots.Id = newtab.spot_id;";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                Spot spot = new Spot();//para mostrar timestamp
                if (!reader.HasRows) return NotFound();
                while (reader.Read())
                {
                    if (!reader.IsDBNull(0))
                    {
                        spot.id = reader.GetString(0);
                        //return NotFound();
                    }
                    if (!reader.IsDBNull(1)) spot.latitude = reader.GetString(1);
                    if (!reader.IsDBNull(2)) spot.longitude = reader.GetString(2);
                    if (!reader.IsDBNull(3)) spot.status = reader.GetString(3);
                    if (!reader.IsDBNull(4)) spot.battery_status = reader.GetString(4);
                    if (!reader.IsDBNull(5)) spot.park_id = reader.GetString(5);
                    if (!reader.IsDBNull(6)) spot.timestamp = reader.GetDateTime(6);
                    
                }
                reader.Close();
                return Ok(spot);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }

        [Route("api/parks/sensors")]
        public IHttpActionResult GetSpotSensor()//ex8 http://localhost:51352/api/parks/sensors
        {
            string query = "select Spots.Id,Spots.battery_status,Spots.park_id from Spots where Spots.battery_status = 'Low'";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Spot> spots = new List<Spot>();
                if (!reader.HasRows) return NotFound();
                while (reader.Read())
                {
                    Spot spot = new Spot();
                    spot.id = reader.GetString(0);
                    if (!reader.IsDBNull(1)) spot.battery_status = reader.GetString(1);
                    if (!reader.IsDBNull(2)) spot.park_id = reader.GetString(2);
                    spots.Add(spot);
                }
                reader.Close();
                return Ok(spots);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }

        [Route("api/parks/{str_id}/sensors")]
        public IHttpActionResult GetSpotSensorInPark(string str_id)//ex9 http://localhost:51352/api/parks/Campus_2_A_Park1/sensors
        {
            string query = "select Spots.Id,Spots.battery_status,Spots.park_id from Spots where Spots.battery_status = 'Low' and Spots.park_id='" + str_id + "'";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query, connection);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                List<Spot> spots = new List<Spot>();
                if (!reader.HasRows) return NotFound();
                while (reader.Read())
                {
                    Spot spot = new Spot();

                    spot.id = reader.GetString(0);
                    if (!reader.IsDBNull(1)) spot.battery_status = reader.GetString(1);
                    if (!reader.IsDBNull(2)) spot.park_id = reader.GetString(2);
                    spots.Add(spot);
                }
                reader.Close();
                return Ok(spots);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
            }
        }

        [Route("api/parks/{str_id}/rate")]
        public IHttpActionResult GetParkRate(string str_id)//ex10 
        {
            //http://localhost:51352/api/parks/Campus_2_A_Park1/rate
            string query1 = "select count(*) from Registers where Registers.park_id = '"+str_id+"' and Registers.status = 'free'";
            string query2 = "select count(*) from Registers where Registers.park_id = '" + str_id+"' and Registers.status = 'occupied'";
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(query1, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                int freeSpots = 0;
                int occupiedSpots = 0;
                if (!reader.HasRows) return NotFound();
                while (reader.Read())
                {
                    freeSpots = reader.GetInt32(0);
                }
                reader.Close();
                command = new SqlCommand(query2, connection);
                reader = command.ExecuteReader();
                if (!reader.HasRows) return NotFound();
                while (reader.Read())
                {
                    occupiedSpots = reader.GetInt32(0);
                }
                int totalSpots = freeSpots + occupiedSpots;
                float rate = (float)occupiedSpots/totalSpots*100;
                reader.Close();
                return Ok(rate);
            }
            catch (Exception ex)
            {
                return Ok(ex.ToString());
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
