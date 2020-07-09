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
        public static List<FlightDetails> container = new List<FlightDetails>()
        {
            new FlightDetails
            {
                Flight = new Flight
                {
                    Flight_id = "Flight_1",
                    Longitude = 32.432,
                    Latitude = 54.356,
                    Passengers = 55,
                    Company_name = "El-Al",
                    Date_time = new DateTime(2020,05,07,13,23,0,0),
                    Is_external = false
                } ,
                FlightPlan = new FlightPlan
                {
                    Passengers = 55,
                    Company_name = "El-Al",
                    Initial_location = new FlightLocation
                    {
                        Longitude = 32.432,
                        Latitude = 54.356,
                        Date_time =new DateTime(2020,05,07,13,23,0,0)
                    },
                    Segments = new Segment[]
                    {
                        
                        new Segment
                        {
                            Latitude = 76.654,
                            Longitude = 30.765,
                            Timespan_seconds = 999999999
                        },
                        new Segment
                        {
                            Latitude = 87.5322,
                            Longitude = 98.435,
                            Timespan_seconds = 99999999
                        },
                    }
                },
                Landing_time = new DateTime(2020,07,05,18,24,0,0)

            },
            new FlightDetails
            {
                Flight = new Flight{
                    Flight_id = "Flight_2",
                    Longitude = 32.432,
                    Latitude = 54.356,
                    Passengers = 55,
                    Company_name = "Qatar",
                    Date_time = new DateTime(2020,12,10,23,56,0,0),
                    Is_external = false
                } ,
                FlightPlan = new FlightPlan
                {
                    Passengers = 55,
                    Company_name = "Qatar",
                    Initial_location = new FlightLocation
                    {
                        Longitude = 32.432,
                        Latitude = 54.356,
                        Date_time =new DateTime(2020,12,10,23,56,0,0)
                    },
                    Segments = new Segment[]
                    {
                        new Segment
                        {
                            Latitude = 45.876,
                            Longitude = 30.765,
                            Timespan_seconds = 5
                        },
                        new Segment
                        {
                            Latitude = 87.5322,
                            Longitude = 98.435,
                            Timespan_seconds = 7
                        },
                    }
                },
                Landing_time = new DateTime(2020,12,10,23,56,12,0)
            }
        };
    

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
