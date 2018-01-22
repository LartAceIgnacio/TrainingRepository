using BlastAsia.DigiBook.Domain.Models.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class FlightExtensions
    {
        public static Flight ApplyChanges(this Flight flight, Flight from)
        {
            flight.CityOfOrigin = from.CityOfOrigin;
            flight.CityOfDestination = from.CityOfDestination;
            flight.Eta = from.Eta;
            flight.Etd = from.Etd;

            return flight;
        }
    }
}
