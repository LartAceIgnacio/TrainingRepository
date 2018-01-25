using System;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class CityDestinationFixedLengthException: Exception
    {
        public CityDestinationFixedLengthException(string message): base(message)
        {

        }
    }
}