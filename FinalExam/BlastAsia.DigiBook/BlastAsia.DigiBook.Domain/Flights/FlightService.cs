using System;
using BlastAsia.DigiBook.Domain.Models.Flights;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class FlightService
    {
        private IFlightRepository flightRepository;
        private readonly int fixedLength = 3;

        public FlightService(IFlightRepository flightRepository)
        {
            this.flightRepository = flightRepository;
        }

        public Flight Save(Guid id, Flight flight)
        {
            if (String.IsNullOrEmpty(flight.CityOfOrigin))
            {
                throw new CityOfOriginRequiredException("City of origin required");
            }
            if (flight.CityOfOrigin.Length < fixedLength)
            {
                throw new CityOriginFixedLengthException("City of origin shoud be 3 characters");
            }
            if (flight.CityOfOrigin.Length > fixedLength)
            {
                throw new CityOriginFixedLengthException("City of origin shoud be 3 characters");
            }
            if (flight.CityOfDestination.Length < fixedLength)
            {
                throw new CityDestinationFixedLengthException("City Destination should be 3 characters");
            }
            if (flight.CityOfDestination.Length > fixedLength)
            {
                throw new CityDestinationFixedLengthException("City Destination should be 3 characters");
            }
            if (flight.Eta == flight.Etd)
            {
                throw new InclusiveArrivalAndDepartureTimeException("Arrival and Departure Time should not be the same");
            }
            if (flight.Eta > flight.Etd)
            {
                throw new InclusiveArrivalAndDepartureTimeException("Arrival time should be earlier than Departure time.");
            }
            if (flight.Etd < flight.Eta)
            {
                throw new InclusiveArrivalAndDepartureTimeException("Departure time should be later than Arrival Time.");
            }
            if (String.IsNullOrEmpty(flight.FlightCode))
            {
                throw new FlightCodeRequiredException("Flight code required");
            }
            Flight result = null;
            var found = flightRepository.Retrieve(id);
            if (found == null)
            {
                result = flightRepository.Create(flight);
            }
            else
            {
                result = flightRepository.Update(id, flight);
            }
            return result;
        }
    }
}