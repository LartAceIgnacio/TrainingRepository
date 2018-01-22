using System;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class FlightCodeRequiredException: Exception
    {
        public FlightCodeRequiredException(string message): base(message)
        {

        }
    }
}