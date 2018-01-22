﻿using System;
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

        public Flight Save(Guid id, Flight flight)
        {
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