using System;
using System.Linq;
using BlastAsia.DigiBook.Domain.Models.Flights;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class FlightService: IFlightService
    {
        private IFlightRepository flightRepository;
        private readonly int fixedLength = 3;
        private int incrementalNumber;

        public FlightService(IFlightRepository flightRepository)
        {
            this.flightRepository = flightRepository;
        }

        public Flight Save(Guid id, Flight flight)
        {
            if (flight.CityOfOrigin == flight.CityOfDestination)
            {
                throw new DestinationErrorException("City of origin should not be the same with City of Destination.");
            }
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
            Flight result = null;
            var found = flightRepository.Retrieve(id);
            incrementalNumber = flightRepository.Retrieve().Count();
            if (found == null)
            {
                flight.DateCreated = DateTime.Now;
                flight.DateModified = DateTime.Now;
                incrementalNumber++;
                flight.FlightCode = flight.CityOfOrigin + flight.CityOfDestination +flight.Etd.ToString("yy")
                    + flight.Etd.ToString("MM") + flight.Etd.ToString("dd") + incrementalNumber.ToString().PadLeft(2, '0');

                var unique = flightRepository.Retrieve(flight.FlightCode);

                if (unique != null)
                {
                    throw new UniqueFlightCodeRequiredException("Flight code is not unique");
                }

                result = flightRepository.Create(flight);
            }
            else
            {
                flight.DateModified = DateTime.Now;
                result = flightRepository.Update(id, flight);
            }
            return result;
        }
    }
}