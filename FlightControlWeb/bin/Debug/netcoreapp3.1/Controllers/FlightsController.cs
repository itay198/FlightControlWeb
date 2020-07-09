using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FlightControlWeb.Models;
using System.IO;
using Newtonsoft.Json;
using System.Net;
using System.Threading.Tasks;

namespace FlightControlWeb.Controllers
{

    

    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {

        private IContainerManager<FlightDetails> flightManager = new FlightDetailsManager();

        

        async Task<List<Flight>> ExternalFlight(Server index, string relative_to, Dictionary<string, Server> dic)
        {
            List<Flight> allFlights = new List<Flight>();
            string strResult;
            string url = index.ServerURL + "/api/Flights?relative_to=" + relative_to;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                strResult = reader.ReadToEnd();
                reader.Close();
            }
            List<Flight> flights = JsonConvert.DeserializeObject<List<Flight>>(strResult);
            foreach (Flight one in flights)
            {

                if (!dic.Contains(new KeyValuePair<string, Server>(one.Flight_id, index)))
                {
                    dic.Add(one.Flight_id, index);
                }
                one.Is_external = true;
                allFlights.Add(one);
            }
            return allFlights;
        }

        async Task<Flight> InternalFlight(DateTime parsedDate, FlightDetails iterator)
        {
                if ((DateTime.Compare(iterator.Flight.Date_time, parsedDate) < 0) &&
                    (DateTime.Compare(iterator.Landing_time, parsedDate) >= 0))
                {
                    TimeSpan timeSpan = parsedDate.Subtract(iterator.Flight.Date_time);
                    string s = timeSpan.TotalSeconds.ToString();
                    double time_span = 0;
                    try
                    {
                        time_span = Double.Parse(s);//.Parse(s);
                        time_span++;

                    }
                    catch (Exception)
                    {
                        Console.WriteLine("TimeSpan not a number\n");
                    
                    }
                    int j = 0;
                    while (true)
                    {
                        if ((time_span - iterator.FlightPlan.Segments[j].Timespan_seconds) >= 0 && j < iterator.FlightPlan.Segments.Length)
                        {
                            time_span = time_span - iterator.FlightPlan.Segments[j].Timespan_seconds;
                            j++;
                            if (j == iterator.FlightPlan.Segments.Length - 1)
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }

                    }
                    double relative_location = time_span / iterator.FlightPlan.Segments[j].Timespan_seconds;
                    if (j <= 1)
                    {
                        iterator.Flight.Longitude = iterator.Flight.Longitude +
                        relative_location * (iterator.FlightPlan.Segments[j].Longitude
                        - iterator.Flight.Longitude);

                        iterator.Flight.Latitude = iterator.Flight.Latitude +
                        relative_location * (iterator.FlightPlan.Segments[j].Latitude
                        - iterator.Flight.Latitude);
                    }
                    else
                    {
                        iterator.Flight.Longitude = iterator.FlightPlan.Segments[j - 1].Longitude +
                        relative_location * (iterator.FlightPlan.Segments[j].Longitude
                        - iterator.FlightPlan.Segments[j - 1].Longitude);

                        iterator.Flight.Latitude = iterator.FlightPlan.Segments[j - 1].Latitude +
                        relative_location * (iterator.FlightPlan.Segments[j].Latitude
                        - iterator.FlightPlan.Segments[j - 1].Latitude);
                    }
                }

            return iterator.Flight;
        }
           
        

        // GET: api/Flights
        public async Task<JsonResult> Get(string relative_to)
        {
             bool sync_all = Request.Query.ContainsKey("sync_all");
            if (sync_all)
            {
                string[] tmp = relative_to.Split('&');
                relative_to = tmp[0];
            }
            DateTime parsedDate;
            parsedDate = DateTime.Parse(relative_to);
            IEnumerable<FlightDetails> allFlightsDetails = flightManager.GetAllEllements();
            List<Flight> allFlights = new List<Flight>();

            int i = 0;
            foreach (FlightDetails iterator in allFlightsDetails)
            {
                if (!sync_all & iterator.Flight.Is_external)
                {
                    continue;
                }
                Flight newFlight = await InternalFlight(parsedDate, iterator);
                allFlights.Add(newFlight);
                i++;

            }
            if (sync_all)
            {
                Dictionary<string, Server> dic = ServerManager.dictionary;
                ServerManager manger = ServerManager.getServerManger();
                foreach(Server index in manger.GetAllEllements())
                {
                    /*string strResult;
                    string url = index.ServerURL + "/api/Flights?relative_to=" + relative_to;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        strResult = reader.ReadToEnd();
                        reader.Close();
                    }
                    List<Flight> flights = JsonConvert.DeserializeObject<List<Flight>>(strResult);
                    foreach(Flight one in flights)
                    {
                        
                        if(!dic.Contains(new KeyValuePair<string, Server>(one.Flight_id,index ))){
                            dic.Add(one.Flight_id, index);
                        }
                        one.Is_external = true;
                        allFlights.Add(one);
                    }*/
                    List<Flight> list = await ExternalFlight(index, relative_to, dic);
                    foreach (Flight iterator in list) 
                    {
                        allFlights.Add(iterator);
                    }
                }
            }
            
            JsonResult jr = new JsonResult(allFlights);
            return jr;
        }



      
        // GET: api/Flights/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Flights
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Flights/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            flightManager.DeleteEllement(id);
        }
    }
}
