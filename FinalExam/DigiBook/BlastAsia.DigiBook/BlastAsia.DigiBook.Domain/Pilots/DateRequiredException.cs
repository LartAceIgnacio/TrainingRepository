using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class DateRequiredException : ApplicationException
    {
        public DateRequiredException(string message)
        : base(message)
        {

        }
    }
}