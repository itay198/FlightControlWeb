using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models;
using System.IO;
using System.Net;
using Newtonsoft.Json;


namespace FlightControlWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightPlanController : ControllerBase
    {

        private IContainerManager<FlightDetails> flightManager = new FlightDetailsManager();

        // GET: api/FlightPlan/5

        [HttpGet("{id}", Name = "GetFlightPlanId")]

        public JsonResult Get(string id)
        {
            JsonResult jr;
            FlightDetails fd = flightManager.GetEllement(id);
            if (fd==null)
            {
                Server server;
                Dictionary<string, Server> dic = ServerManager.dictionary;
                if(dic.TryGetValue(id,out server))
                {
                    
                    string strResult;
                    string url = server.ServerURL + "/api/FlightPlan/" + id;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        strResult = reader.ReadToEnd();
                        reader.Close();
                    }
                    FlightPlan flights = JsonConvert.DeserializeObject<FlightPlan>(strResult);
                    return new JsonResult(flights);
                }
            }
            if (fd !=null)
            {
                jr = new JsonResult(fd.FlightPlan);
            }else
            {
                jr = new JsonResult(null);
            }
           
            return jr;

        }
       

        // POST: api/FlightPlan
        [HttpPost]
        public void Post( FlightPlan flightPlan)
        {
            
            FlightDetails fd = new FlightDetails();

            fd.FlightPlan = flightPlan;
            //build new flight for fd
            Flight temp_f = new Flight
            {
                Flight_id = flightPlan.Company_name + flightManager.GetSize(),
                Longitude = flightPlan.Initial_location.Longitude,
                Latitude = flightPlan.Initial_location.Latitude,
                Passengers = flightPlan.Passengers,
                Company_name = flightPlan.Company_name,
                Date_time = flightPlan.Initial_location.Date_time,
                Is_external = false
            };

            fd.Flight = temp_f;

            //build new dateTime for fd
            DateTime temp_d = fd.FlightPlan.Initial_location.Date_time;

            foreach(Segment seg in fd.FlightPlan.Segments)
            {
                temp_d = temp_d.AddSeconds(seg.Timespan_seconds);
            }

            fd.Landing_time = temp_d;

            flightManager.AddEllement(fd);
            
        }
        
        // PUT: api/FlightPlan/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(long id)
        {

        }
    }
}
