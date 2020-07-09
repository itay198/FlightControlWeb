using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models
{
    public class FlightDetails
    {
        public Flight Flight { get; set; }
        public FlightPlan FlightPlan { get; set; }
        public DateTime Landing_time;
    }
}
