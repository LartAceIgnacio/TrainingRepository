using System;
using System.Linq;
using BlastAsia.DigiBook.Domain.Exceptions;
using BlastAsia.DigiBook.Domain.Flights.Exceptions;
using BlastAsia.DigiBook.Domain.Models.Flights;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class FlightService : IFlightService
    {
        private IFlightRepository flightRepository;
        private int incNum;

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
            if(flight.ExpectedTimeOfArrival >= flight.ExpectedTimeOfDeparture)
            {
                throw new DateAndTimeException("ETA should be less than ETD");
            }
            //if(flight.ExpectedTimeOfArrival == null)
            //{
            //    throw new DateAndTimeException("ETA is required");
            //}
            //if(flight.ExpectedTimeOfDeparture == null)
            //{
            //    throw new DateAndTimeException("ETD is required");
            //}

            Flight result = null;
            var found = flightRepository
                .Retrieve(flight.FlightId);

            incNum = flightRepository.Retrieve().Count();

            if (found == null)
            {
                flight.DateCreated = DateTime.Now;
                incNum++;

                flight.FlightCode = string.Concat(flight.CityOfOrigin , flight.CityOfDestination , flight.ExpectedTimeOfDeparture.ToString("yy")
                    , flight.ExpectedTimeOfDeparture.ToString("MM") , flight.ExpectedTimeOfDeparture.ToString("dd")
                    , incNum.ToString().PadLeft(2, '0'));

                result = flightRepository.Create(flight);
            }
            else
            {
                flight.DateModified = DateTime.Now;

                result = flightRepository
                    .Update(flight.FlightId, flight);
            }
            return result;
        }
    }
}