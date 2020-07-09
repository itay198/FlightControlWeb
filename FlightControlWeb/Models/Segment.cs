using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FlightControlWeb.Models
{
    public class Segment
    {
        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }
        [JsonPropertyName("timespan_seconds")]
        public int Timespan_seconds { get; set; }

        public Segment() { }
        public Segment(double longit, double latit, int timespan)
        {
            Latitude = latit;
            Longitude = longit;
            Timespan_seconds = timespan;
        }
    }
}
