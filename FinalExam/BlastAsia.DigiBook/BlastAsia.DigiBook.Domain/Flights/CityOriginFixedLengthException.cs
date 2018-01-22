using System;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class CityOriginFixedLengthException: Exception
    {
        public CityOriginFixedLengthException(string message): base(message)
        {

        }
    }
}