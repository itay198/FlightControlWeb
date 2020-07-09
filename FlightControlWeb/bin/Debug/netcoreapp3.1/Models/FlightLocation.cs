using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models
{
    public class FlightLocation
    {
        [JsonPropertyName("longitude")]
        public double Longitude { set; get; }
        [JsonPropertyName("latitude")]
        public double Latitude { set; get; }
        [JsonPropertyName("date_time")]
        public DateTime Date_time { set; get; }
        public FlightLocation()
        {
        }
        public FlightLocation(double longit, double latit, DateTime time)
        {
            this.Latitude = latit;
            this.Longitude = longit; 
            this.Date_time = time;

        }
    }
   
}
