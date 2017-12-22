using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueNameLengthRequiredException
        :Exception
    {
        public VenueNameLengthRequiredException(string message)
            :base(message)
        {

        }
    }
}