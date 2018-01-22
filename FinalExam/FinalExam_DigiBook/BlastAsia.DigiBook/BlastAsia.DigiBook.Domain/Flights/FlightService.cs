using System;
using BlastAsia.DigiBook.Domain.Models.Flights;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class FlightService
    {
        private IFlightRepository flightRepository;

        public FlightService(IFlightRepository flightRepository)
        {
            this.flightRepository = flightRepository;
        }

        public object Save(Guid flightId, Flight flight)
        {
            throw new NotImplementedException();
        }
    }
}