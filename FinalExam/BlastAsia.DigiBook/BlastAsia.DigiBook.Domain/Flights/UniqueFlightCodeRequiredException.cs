using System;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class UniqueFlightCodeRequiredException: Exception
    {
        public UniqueFlightCodeRequiredException(string message): base(message)
        {

        }
    }
}