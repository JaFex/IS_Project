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

        public IHttpActionResult GetAllParks()//ex 1
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
            return Ok(parks);
        }

        [Route("api/status")]
        /*public IHttpActionResult GetParkForGivenMoment(int id, DateTime date)//ex 2
         {
             var park = parks.FirstOrDefault((p) => p.Id == id);
             if (park == null) return NotFound();
             return Ok(park.ParkingSpots);
         [Route("api/statusStop")]
         }*/

        [Route("api/liststatus")]
        /*public IHttpActionResult GetListStatusParkForPeriodMoment(DateTime date1, DateTime date2)//ex 3
         {
             var spots = Register.Find(
             if (park == null) return NotFound();
             return Ok(park.ParkingSpots);
         }*/

        /*public IHttpActionResult GetFreeParkForGivenMoment(int id, DateTime date)//ex 4
        {
            var park = parks.FirstOrDefault((p) => p.Id == id);
            if (park == null) return NotFound();
            var freeParking = park.ParkingSpots.Find((p) => p.status == "free");
            return Ok(freeParking);
        }*/

        [Route("api/spots/{id}")]
        public IHttpActionResult GetPark(int id)//ex 5
        {
            var park = parks.FirstOrDefault((p) => p.Id == id);
            if (park == null) return NotFound();
            return Ok(park.ParkingSpots);
        }

        /* public IHttpActionResult GetParkDescription(int id)//ex 6
         {
             var park = parks.FirstOrDefault((p) => p.Id == id);
             if (park == null) return NotFound();
             return Ok(park.Description);
         }*/
        /* public IHttpActionResult GetPark(int id,int spot_id,DateTime date)//ex 7
         {
             var park = parks.FirstOrDefault((p) => p.Id == id);
             if (park == null) return NotFound();

             return Ok(park.ParkingSpots);
         }*/
        public IHttpActionResult GetSpotSensor(int id)//ex 8
        {
            var park = parks.FirstOrDefault((p) => p.Id == id);
            if (park == null) return NotFound();
            return Ok(park.ParkingSpots);
        }

    }
}
