using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueNameInvalidException
        : Exception
    {
        public VenueNameInvalidException(string message)
            : base(message)
        {
                
        }
    }
}