using System;

namespace BlastAsia.DigiBook.Domain.Venues.Exceptions
{
    public class InvalidVenueNameException : ApplicationException
    {
        public InvalidVenueNameException(string message) : base(message)
        {

        }
    }
}