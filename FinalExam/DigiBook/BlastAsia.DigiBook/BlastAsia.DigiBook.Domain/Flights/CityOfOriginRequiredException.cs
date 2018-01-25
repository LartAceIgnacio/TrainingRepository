using System;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class CityOfOriginRequiredException: Exception
    {
        public CityOfOriginRequiredException(string message):base(message)
        {

        }
    }
}