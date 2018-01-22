using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueNameMaxLengthException: Exception
    {
        public VenueNameMaxLengthException(string message)
            :base(message)
        {

        }
    }
}