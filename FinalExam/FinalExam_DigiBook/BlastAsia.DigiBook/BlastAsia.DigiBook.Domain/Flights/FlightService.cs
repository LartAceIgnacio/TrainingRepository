using System;
using BlastAsia.DigiBook.Domain.Exceptions;
using BlastAsia.DigiBook.Domain.Flights.Exceptions;
using BlastAsia.DigiBook.Domain.Models.Flights;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class FlightService : IFlightService
    {
        private IFlightRepository flightRepository;

        public FlightService(IFlightRepository flightRepository)
        {
            this.flightRepository = flightRepository;
        }

        public Flight Save(Guid id, Flight flight)
        {
            if(flight.CityOfOrigin.Length != 3)
            {
                throw new MaximumLengthException("City of Origin should be equal to 3");
            }
            if(flight.CityOfDestination.Length != 3)
            {
                throw new MaximumLengthException("City of Destination should be equal to 3");
            }
            if(flight.ExpectedTimeOfArrival <= flight.ExpectedTimeOfDeparture)
            {
                throw new DateAndTimeException("ETA should be greater than ETD");
            }

            Flight result = null;
            var found = flightRepository
                .Retrieve(flight.FlightId);

            if (found == null)
            {
                result = flightRepository.Create(flight);
            }
            else
            {
                result = flightRepository
                    .Update(flight.FlightId, flight);
            }
            return result;
        }
    }
}