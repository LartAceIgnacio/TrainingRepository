using BlastAsia.DigiBook.Domain.Models.Flights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlastAsia.DigiBook.API.Utils
{
    public static class FlightExtension
    {
        public static Flight ApplyChanges(
            this Flight flight,
            Flight form)
        {
            flight.CityOfOrigin = form.CityOfOrigin;
            flight.CityOfDestination = form.CityOfDestination;
            flight.ExpectedTimeOfArrival = form.ExpectedTimeOfArrival;
            flight.ExpectedTimeOfDeparture = form.ExpectedTimeOfDeparture;
            flight.DateModified = DateTime.Now;

            return flight;
        }
    }
}
