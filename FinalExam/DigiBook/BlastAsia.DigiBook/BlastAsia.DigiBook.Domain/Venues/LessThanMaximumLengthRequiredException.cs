using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class LessThanMaximumLengthRequiredException : Exception
    {
        public LessThanMaximumLengthRequiredException(string message)
        : base(message)
        {
        }
    }
}