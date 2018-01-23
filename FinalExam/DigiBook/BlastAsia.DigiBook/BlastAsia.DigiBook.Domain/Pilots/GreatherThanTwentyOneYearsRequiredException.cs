using System;

namespace BlastAsia.DigiBook.Domain.Pilots
{
    public class GreatherThanTwentyOneYearsRequiredException : ApplicationException
    {
        public GreatherThanTwentyOneYearsRequiredException(string message)
        : base(message)
            {

            }
    }
}