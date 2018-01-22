using System;

namespace BlastAsia.DigiBook.Domain.Flights
{
    public class InclusiveArrivalAndDepartureTimeException: Exception
    {
        public InclusiveArrivalAndDepartureTimeException(string message):base(message)
        {

        }
    }
}