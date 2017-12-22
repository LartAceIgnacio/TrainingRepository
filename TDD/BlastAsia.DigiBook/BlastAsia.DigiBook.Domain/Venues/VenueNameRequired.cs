using System;

namespace BlastAsia.DigiBook.Domain.Venues
{
    public class VenueNameRequired
        : Exception
    {
        public VenueNameRequired(string message)
            :base(message)
        {

        }
    }
}