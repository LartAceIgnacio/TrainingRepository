using BlastAsia.DigiBook.Domain.Models.Flights;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public interface IFlightService
    {
        Flight Save(Guid id, Flight flight);
    }
}
