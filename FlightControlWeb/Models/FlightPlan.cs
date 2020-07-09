using FlightControlWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace FlightControlWeb.Models
{
    
    public class FlightPlan
    { 
        [JsonPropertyName("passengers")]
        public int Passengers{ set; get; }
        [JsonPropertyName("company_name")]
        public string Company_name { set; get; }
        [JsonPropertyName("initial_location")]
        public FlightLocation Initial_location { set; get; }
        [JsonPropertyName("segments")]
        public Segment[] Segments { set; get; }
        public FlightPlan() { }
        public FlightPlan(int pass, string company, FlightLocation ini, Segment[] seg)
        {
            this.Passengers = pass;
            this.Company_name = company;
            this.Initial_location = ini;
            this.Segments = seg;
        }
    }



}
