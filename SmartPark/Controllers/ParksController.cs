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

        //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ProductsDatabaseAPI.Properties.Settings.ConnStr"].ConnectionString;
        List<Park> parks = new List<Park>
        {
            new Park{ Id = 1, Available=1,ParkingSpots = new List<ParkingSpot>{
                new ParkingSpot{ IdPark=1,Id=1,Available=1 },
                new ParkingSpot{ IdPark=1,Id=2,Available=1 },
                new ParkingSpot{ IdPark=1,Id=3,Available=0 },
                new ParkingSpot{ IdPark=1,Id=4,Available=0 },
                new ParkingSpot{ IdPark=1,Id=5,Available=0 },
            } },
            new Park{ Id = 2, Available=0,ParkingSpots = new List<ParkingSpot>{
                new ParkingSpot{ IdPark=1,Id=1,Available=1 },
                new ParkingSpot{ IdPark=1,Id=2,Available=1 },
                new ParkingSpot{ IdPark=1,Id=3,Available=0 },
                new ParkingSpot{ IdPark=1,Id=4,Available=1 },
                new ParkingSpot{ IdPark=1,Id=5,Available=0 },
            } },
            new Park{ Id = 3, Available=1,ParkingSpots = new List<ParkingSpot>{
                new ParkingSpot{ IdPark=1,Id=1,Available=1 },
                new ParkingSpot{ IdPark=1,Id=2,Available=1 },
                new ParkingSpot{ IdPark=1,Id=3,Available=0 },
                new ParkingSpot{ IdPark=1,Id=4,Available=0 },
                new ParkingSpot{ IdPark=1,Id=5,Available=1 },
            } },
            new Park{ Id = 4, Available=0,ParkingSpots = new List<ParkingSpot>{
                new ParkingSpot{ IdPark=1,Id=1,Available=1 },
                new ParkingSpot{ IdPark=1,Id=2,Available=0 },
                new ParkingSpot{ IdPark=1,Id=3,Available=0 },
                new ParkingSpot{ IdPark=1,Id=4,Available=0 },
                new ParkingSpot{ IdPark=1,Id=5,Available=0 },
            } },
            new Park{ Id = 5, Available=1,ParkingSpots = new List<ParkingSpot>{
                new ParkingSpot{ IdPark=1,Id=1,Available=0 },
                new ParkingSpot{ IdPark=1,Id=2,Available=1 },
                new ParkingSpot{ IdPark=1,Id=3,Available=0 },
                new ParkingSpot{ IdPark=1,Id=4,Available=0 },
                new ParkingSpot{ IdPark=1,Id=5,Available=0 },
            } }
        };
        public IEnumerable<Park> GetAllParks()
        {
            /*string query = "SELECT Parks from ParkDB where available == true;";
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    
                }
            }
            catch (Exception ex)
            {
                return NotFound();
            }*/
            return parks;
        }

        public IHttpActionResult GetPark(int id)
        {
            var park = parks.FirstOrDefault((p) => p.Id == id);
            if (park == null) return NotFound();
            return Ok(park);
        }
    }
}
