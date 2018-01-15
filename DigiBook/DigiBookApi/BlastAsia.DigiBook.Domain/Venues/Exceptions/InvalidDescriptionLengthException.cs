using System;

namespace BlastAsia.DigiBook.Domain.Models.Venues.Exceptions
{
    public class InvalidDescriptionLengthException : ApplicationException
    {
        public InvalidDescriptionLengthException(string message) : base(message)
        {
        }
    }
}