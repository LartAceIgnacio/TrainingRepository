using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueNameRequiredException: Exception
    {
        public VenueNameRequiredException(string message)
            :base(message)
        {

        }
    }
}