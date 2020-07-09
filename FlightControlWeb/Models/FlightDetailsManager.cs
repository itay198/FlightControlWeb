using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlightControlWeb.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;


namespace FlightControlWeb.Models
{
    public class FlightDetailsManager : IContainerManager<FlightDetails>
    {
        public static List<FlightDetails> container = new List<FlightDetails>();
        
    

        public void AddEllement(FlightDetails flight)
        {
            container.Add(flight);
        }

        public void DeleteEllement(string flight_id)
        {
            FlightDetails del = container.Where(x => (String.Compare( x.Flight.Flight_id, flight_id)==0)).FirstOrDefault();
            if(del == null)
            {
                throw new Exception("Flight not found");
            }
            container.Remove(del);
        }

        public FlightDetails GetEllement(string flight_id)
        {
            return container.Where(x => String.Compare(x.Flight.Flight_id , flight_id)==0).FirstOrDefault();
        }

        public IEnumerable<FlightDetails> GetAllEllements()
        {
            return container;
        }

        public void UpdateEllement(FlightDetails flight)
        {
            FlightDetails flightToUpdate = container.Where(x => x.Flight.Flight_id == flight.Flight.Flight_id).FirstOrDefault();
            flightToUpdate.Flight.Company_name = flight.Flight.Company_name;
            flightToUpdate.Flight.Date_time = flight.Flight.Date_time;
            flightToUpdate.Flight.Flight_id = flight.Flight.Flight_id;
            flightToUpdate.Flight.Is_external = flight.Flight.Is_external;
            flightToUpdate.Flight.Latitude = flight.Flight.Latitude;
            flightToUpdate.Flight.Longitude = flight.Flight.Longitude;
            flightToUpdate.Flight.Passengers = flight.Flight.Passengers;
        }

        public int GetSize()
        {
            return container.Count;
        }
    }
}
