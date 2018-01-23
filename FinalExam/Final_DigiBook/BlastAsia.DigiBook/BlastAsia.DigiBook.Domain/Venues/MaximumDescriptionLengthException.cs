using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class MaximumDescriptionLengthException : Exception
    {
        public MaximumDescriptionLengthException()
        {
        }

        public MaximumDescriptionLengthException(string message) : base(message)
        {
        }

        public MaximumDescriptionLengthException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}