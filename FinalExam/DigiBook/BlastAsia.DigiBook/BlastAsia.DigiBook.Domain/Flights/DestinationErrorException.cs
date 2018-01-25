using System;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class DestinationErrorException: Exception
    {
        public DestinationErrorException(string message) : base(message)
        {

        }
    }
}