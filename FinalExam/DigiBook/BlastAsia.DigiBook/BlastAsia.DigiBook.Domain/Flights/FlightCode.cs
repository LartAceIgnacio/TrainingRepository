using BlastAsia.DigiBook.Domain.Models.Flights;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public static class FlightCode
    {
        public static String GenerateFlightCode(Flight flight)
        {
            return flight.FlightCode = flight.CityOfOrigin + flight.CityOfDestination + flight.Etd.ToString("yy")
                    + flight.Etd.ToString("MM") + flight.Etd.ToString("dd");
        }
    }
}
