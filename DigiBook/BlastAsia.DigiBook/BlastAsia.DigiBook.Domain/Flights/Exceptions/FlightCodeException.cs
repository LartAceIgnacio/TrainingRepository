using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Flights.Exceptions
{
    public class FlightCodeException : Exception
    {
        public FlightCodeException(string message)
            : base(message)
        {

        }
    }
}
