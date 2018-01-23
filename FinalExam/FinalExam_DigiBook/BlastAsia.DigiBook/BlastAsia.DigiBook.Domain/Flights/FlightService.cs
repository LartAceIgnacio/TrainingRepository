using System;
using System.Linq;
using System.Text.RegularExpressions;
using BlastAsia.DigiBook.Domain.Exceptions;
using BlastAsia.DigiBook.Domain.Flights.Exceptions;
using BlastAsia.DigiBook.Domain.Models.Flights;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class FlightService : IFlightService
    {
        private IFlightRepository flightRepository;
        private int incNum;
        private string strRejex = @"^([A-Z]{3})([A-Z]{3})(\d{2})(\d{2})(\d{2})(\d{2})$";

        public FlightService(IFlightRepository flightRepository)
        {
            this.flightRepository = flightRepository;
        }

        public Flight Save(Guid id, Flight flight)
        {
            if (string.IsNullOrEmpty(flight.CityOfOrigin))
            {
                throw new RequiredFieldException("City of Origin is required");
            }
            if (string.IsNullOrEmpty(flight.CityOfDestination))
            {
                throw new RequiredFieldException("City of Destination is required");
            }
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
            if (flight.ExpectedTimeOfArrival == null)
            {
                throw new DateAndTimeException("ETA is required");
            }
            if (flight.ExpectedTimeOfDeparture == null)
            {
                throw new DateAndTimeException("ETD is required");
            }
            if(flight.ExpectedTimeOfArrival < DateTime.Today)
            {
                throw new DateAndTimeException("ETA should be greater than date today");
            }
            //if (!Regex.IsMatch(flight.FlightCode, strRejex))
            //{
            //    throw new FlightCodeException("Invalid Flight Code Format");
            //}

            //var foundFlightCode = flightRepository
            //    .Retrieve().Where(c => c.FlightCode == flight.FlightCode);
            //if (foundFlightCode != null)
            //{
            //    throw new FlightCodeException("Flight Code is not uniqure");
            //}

            Flight result = null;

            incNum = flightRepository.Retrieve().Count();

            var etd = flight.ExpectedTimeOfDeparture ?? DateTime.Today;

            var found = flightRepository
                .Retrieve(flight.FlightId);

            if (found == null)
            {
                flight.DateCreated = DateTime.Now;
                incNum++;

                flight.FlightCode = string.Concat(flight.CityOfOrigin , flight.CityOfDestination , etd.ToString("yy")
                    , etd.ToString("MM") , etd.ToString("dd")
                    , incNum.ToString().PadLeft(2, '0'));

                result = flightRepository.Create(flight);
            }
            else
            {
                flight.DateModified = DateTime.Now;

                flight.FlightCode = string.Concat(flight.CityOfOrigin, flight.CityOfDestination, etd.ToString("yy")
                    , etd.ToString("MM"), etd.ToString("dd")
                    , incNum.ToString().PadLeft(2, '0'));

                result = flightRepository
                    .Update(flight.FlightId, flight);
            }
            return result;
        }
    }
}