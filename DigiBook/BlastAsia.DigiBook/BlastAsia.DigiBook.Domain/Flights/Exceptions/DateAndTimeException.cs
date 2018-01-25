using System;

namespace BlastAsia.DigiBook.Domain.Flights.Exceptions
{
    public class DateAndTimeException : Exception
    {
        public DateAndTimeException(string message)
            : base(message)
        {

        }
    }
}