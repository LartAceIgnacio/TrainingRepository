using System;
using BlastAsia.DigiBook.Domain.Models.Flights;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public interface IFlightService
    {
        Flight Save(Guid id, Flight flight);
    }
}